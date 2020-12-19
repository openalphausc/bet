using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataStorage : MonoBehaviour
{
    //List of monsters asked to stay afterHours
    public static string stayingMonster;
    public static int currentDay;

    // List of all monsters for tabsheet
    public static List<Monster> monsters;
    public static int allMonsterPoints = 0;
    public static List<Monster> threeStarMonsters = new List<Monster>();

    // Array to keep track of how many times monster has visited
    public static List<string> monstersVisited;
    public static int[] timesVisited;
    public static int[] monsterPoints;

    public static int totalPointsOverall = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(currentDay == 0)
        {
            monsters = new List<Monster>();
            monsters.Clear();
        }
        monstersVisited = new List<string>(new string[]
            {"3Gremlins", "CowboyAlien", "EldritchMonster", "GalahadTheDragonborn", "KatetheCockatrice", "Khepri", "KnifeUnicorn", "Shapeshifter", "Utsuro", "XandartheEboy", "Ghost"});
        timesVisited = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0};
        monsterPoints = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int findMonster(string monster)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monster == monsters[i].name)
            {
                return i;
            }
        }
        return -1;
    }

    public static int incrementVisited(string monster)
    {
        for(int i = 0; i < monstersVisited.Count; i++)
        {
            if(monster == monstersVisited[i])
            {
                int val = timesVisited[i];
                timesVisited[i]++;
                return val;
            }
        }

        return 0;
    }

    public static void incrementPoints(string monster, int points)
    {
        for (int i = 0; i < monstersVisited.Count; i++)
        {
            if (monster == monstersVisited[i])
            {
                monsterPoints[i] += points;
            }
        }
    }
}
