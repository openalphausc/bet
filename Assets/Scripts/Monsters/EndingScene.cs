using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class EndingScene : MonoBehaviour
{
    public string ending;
    public bool ghostIntro = false;
    public static int monstersSpoken = 0;
    public static Vector3 monsterLocation = new Vector3(-7f, -2f, 0f);
    public static GameObject currentMonster = null;
    public static bool active = false;
    public string monster1;
    public string monster2;
    public string monster3;
    private bool monster1Spoken = false;
    private bool monster2Spoken = false;
    private bool monster3Spoken = false;
    private bool monster4Spoken = false;
    public static bool introEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        monster1 = "CowboyAlien";
        monster2 = "Utsuro";
        monster3 = "Khepri";
        //FindBestSupports();
        
        currentMonster = Instantiate(
                Resources.Load<GameObject>("Prefabs/Monsters/Ghost"),
                monsterLocation,
                Quaternion.identity);

        currentMonster.name = "TutorialGhost";
        currentMonster.GetComponent<Monster>().inEnding = true;
        currentMonster.GetComponent<SpriteRenderer>().sprite = currentMonster.GetComponent<Monster>().emotions[2];
        currentMonster.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.9f, .9f, .9f);
        
        if (dataStorage.totalPointsOverall < 6500)
        {
            ending = "GoodEnding";
            GameObject.Find("BadEnding").SetActive(false);
            GameObject.Find("GoodEnding").SetActive(true);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostGoodEnding");
        }
        else
        {
            ending = "BadEnding";
            GameObject.Find("BadEnding").SetActive(true);
            GameObject.Find("GoodEnding").SetActive(false);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostBadEnding");
        }
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(monstersSpoken == 0 && introEnded && !monster3Spoken)
        {
            currentMonster = Instantiate(
               Resources.Load<GameObject>("Prefabs/Monsters/"+monster3),
               monsterLocation,
               Quaternion.identity);

            currentMonster.name = monster3;
            currentMonster.GetComponent<Monster>().inEnding = true;
            currentMonster.GetComponent<SpriteRenderer>().sprite = currentMonster.GetComponent<Monster>().emotions[2];
            currentMonster.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.9f, .9f, .9f);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(monster3 + ending);
            monster3Spoken = true;
        }
        if(monstersSpoken == 1 && !monster2Spoken)
        {
            currentMonster = Instantiate(
               Resources.Load<GameObject>("Prefabs/Monsters/" + monster2),
               monsterLocation,
               Quaternion.identity);

            currentMonster.name = monster2;
            currentMonster.GetComponent<Monster>().inEnding = true;
            currentMonster.GetComponent<SpriteRenderer>().sprite = currentMonster.GetComponent<Monster>().emotions[2];
            currentMonster.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.9f, .9f, .9f);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(monster2 + ending);
            monster2Spoken = true;
        }
        if(monstersSpoken == 2 && !monster1Spoken)
        {
            currentMonster = Instantiate(
               Resources.Load<GameObject>("Prefabs/Monsters/" + monster1),
               monsterLocation,
               Quaternion.identity);

            currentMonster.name = monster1;
            currentMonster.GetComponent<Monster>().inEnding = true;
            currentMonster.GetComponent<SpriteRenderer>().sprite = currentMonster.GetComponent<Monster>().emotions[2];
            currentMonster.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.9f, .9f, .9f);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(monster1 + ending);
            monster1Spoken = true;
        }
        if(monstersSpoken == 3 && !monster4Spoken)
        {
            currentMonster = Instantiate(
                Resources.Load<GameObject>("Prefabs/Monsters/Ghost"),
                monsterLocation,
                Quaternion.identity);

            currentMonster.name = "TutorialGhost";
            currentMonster.GetComponent<Monster>().inEnding = true;
            currentMonster.GetComponent<SpriteRenderer>().sprite = currentMonster.GetComponent<Monster>().emotions[2];
            currentMonster.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.9f, .9f, .9f);
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhost" + ending + "B");
            monster4Spoken = true;
        }
    }

    void FindBestSupports()
    {
        int first = 0;
        int second = 0;
        int third = 0;

        for(int i = 0; i < dataStorage.monstersVisited.Count-1; ++i)
        {
            if(dataStorage.monsterPoints[i] > dataStorage.monsterPoints[first])
            {
                third = second;
                second = first;
                first = i;
            }
            else if(dataStorage.monsterPoints[i] > dataStorage.monsterPoints[second])
            {
                third = second;
                second = i;
            }
            else if(dataStorage.monsterPoints[i] > dataStorage.monsterPoints[third])
            {
                third = i;
            }
        }

        monster1 = dataStorage.monstersVisited[first];
        monster2 = dataStorage.monstersVisited[second];
        monster3 = dataStorage.monstersVisited[third];
    }

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
}
