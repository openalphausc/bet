using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabsheetGroupMonsters : MonoBehaviour
{
    // A Monster with 0 / 0 points will just be ignored.
    public static List<Monster> allMonsters = dataStorage.monsters;

    public List<Monster> oneStarMonsters = new List<Monster>();
    public List<Monster> twoStarMonsters = new List<Monster>();
    public List<Monster> threeStarMonsters = new List<Monster>();

    private List<Monster> tempTwoThreeStarMonsters = new List<Monster>();

    public TextMeshProUGUI threeStarMonstersText;
    public TextMeshProUGUI twoStarMonstersText;
    public TextMeshProUGUI oneStarMonstersText;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Length of monsters array: " + dataStorage.monsters.Count);
        allMonsters = dataStorage.monsters;
        foreach (Monster monster in allMonsters)
        {
            // If the total points are 0, then the monster never ordered
            if (monster.totalPoints == 0)
            {
                //Debug.Log("Monster " + monster.name + " has 0 points");
                continue;
            }
            Debug.Log(monster.name);
            // One star condition. < 60%
            if ((monster.pointsEarned + 0.0) / monster.totalPoints < 0.6)
                oneStarMonsters.Add(monster);
            // Else, it's either two or three stars. This will be decided after the for loop
            else
                tempTwoThreeStarMonsters.Add(monster);
        }

        // epic math
        int numTwoStar = (int) Mathf.Ceil(tempTwoThreeStarMonsters.Count / 2.0f);
        int numThreeStar = (int) Mathf.Floor(tempTwoThreeStarMonsters.Count / 2.0f);

        //Debug.Log("Num two star: " + numTwoStar);
        //Debug.Log("Num three star: " + numThreeStar);

        PointsComp comp = new PointsComp();
        tempTwoThreeStarMonsters.Sort(comp);

        // copy the two star monsters into the array
        for (int i = 0; i < numTwoStar; ++i)
        {
            twoStarMonsters.Add(tempTwoThreeStarMonsters[i]);
        }
        // copy the three star monsters into the array
        for (int i = numTwoStar; i < numTwoStar + numThreeStar; ++i)
        {
            threeStarMonsters.Add(tempTwoThreeStarMonsters[i]);
        }

        oneStarMonsters.Sort(comp);
        twoStarMonsters.Sort(comp);
        threeStarMonsters.Sort(comp);

        // Display monsters text
        oneStarMonstersText.text = ListToString(oneStarMonsters);
        twoStarMonstersText.text = ListToString(twoStarMonsters);
        threeStarMonstersText.text = ListToString(threeStarMonsters);

    }

    // A comparer for sorting the monsters arrays by percentage of points earned
    public class PointsComp : IComparer<Monster>
    {
        public int Compare(Monster x, Monster y)
        {
            // Calculate the percentage of points earned for each monster.
            // If either's total points is 0, then the percent is also 0.
            double xPercent;
            double yPercent;
            if (x.totalPoints == 0)
            {
                xPercent = 0;
            }
            else
            {
                xPercent = (x.pointsEarned + 0.0) / x.totalPoints;
            }
            if (y.totalPoints == 0)
            {
                yPercent = 0;
            }
            else
            {
                yPercent = (y.pointsEarned + 0.0) / y.totalPoints;
            }
            return xPercent.CompareTo(yPercent);
        }
    }

    public class NameComp : IComparer<Monster>
    {
        public int Compare(Monster x, Monster y)
        {
            return x.name.CompareTo(y.name);
        }
    }

    // print list function for monster list
    void printList(List<Monster> monsters)
    {
        foreach (Monster m in monsters)
        {
            Debug.Log(m.name);
        }
    }

    // function that converts a list into a string like this: Element 1, Element 2, Element 3...
    string ListToString(List<Monster> monsters)
    {
        string s = "";
        foreach (Monster m in monsters)
        {
            s = s + m.name + ", ";
        }
        if (s.Length > 0)
            s = s.Substring(0, s.Length - 2); // remove the last ,
        return s;
    }
}
