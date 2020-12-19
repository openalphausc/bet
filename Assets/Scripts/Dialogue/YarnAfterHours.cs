﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class YarnAfterHours : MonoBehaviour
{
    public static GameObject currentMonster = null;
    public static Vector3 monsterLocation = new Vector3(-25f, -5f, 0f);

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "AfterHours")
        {
            if (dataStorage.currentDay == 0 && !AfterHoursMonsterSpawner.tutorialOver)
            {
                FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostDay1AH");
            }
            else
            {

                /*NameComp nameComp = new NameComp();
                int index = dataStorage.monsters.BinarySearch(AfterHoursMonsterSpawner.currentMonster.GetComponent<Monster>(), nameComp);
                if (index >= 0)
                {*/
                int index = AfterHoursMonsterSpawner.findMonster(gameObject.name);

                if (index != -1)
                {
                    if (index == 0)
                    {
                        FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(gameObject.name + "AH2WrongAnswer");
                    }
                    else if (index == 1)
                    {
                        FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(gameObject.name + "AH2RightAnswer");
                    }
                }
                else
                {
                    FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(gameObject.name + "AH1");
                }
            }
        }
    }
    [YarnCommand("finishedTalkingWith")]
    public void deleteMonster()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<Monster>().emotions[2];
        AfterHoursMonsterSpawner.currentMonster.GetComponent<Monster>().inAfterHours = false;
        AfterHoursMonsterSpawner.currentMonster = null;
        AfterHoursMonsterSpawner.active = false;
        dataStorage.currentDay++;
        dataStorage.stayingMonster = null;
        SceneManager.LoadScene("Tabsheet");
    }

    [YarnCommand("running")]
    public void running()
    {
        // Debug.Log("node is running");
    }

    // Update monster points for right answer
    [YarnCommand("rightAnswer")]
    public void updatePointsRight()
    {
        string talkingMonster = dataStorage.stayingMonster;
        // find the talking monster in the points array
        NameComp nameComp = new NameComp();
        int index = dataStorage.monsters.BinarySearch(AfterHoursMonsterSpawner.currentMonster.GetComponent<Monster>(), nameComp);
        // if the monster is in the data storage array, update its points
        if (index >= 0)
        {
            dataStorage.monsters[index].pointsEarned += 50;
            dataStorage.monsters[index].totalPoints += 50;
            Debug.Log("+50 points to " + dataStorage.monsters[index].name);
            if (AfterHoursMonsterSpawner.findMonster(currentMonster.name) == -1)
            {
                AfterHoursMonsterSpawner.monsterList.Add(currentMonster.name);
            }
            AfterHoursMonsterSpawner.monsterAnswers.Add(1);
        }
        else
        {
            // Debug.Log("Unknown monster in After Hours, not in points array");
        }
        //gameObject.GetComponent<Monster>().rightAnswer = 1;
        Debug.Log("right Answer");
    }

    // Update monster points for right answer
    [YarnCommand("wrongAnswer")]
    public void updatePointsWrong()
    {
        string talkingMonster = dataStorage.stayingMonster;
        // find the talking monster in the points array
        NameComp nameComp = new NameComp();
        int index = dataStorage.monsters.BinarySearch(AfterHoursMonsterSpawner.currentMonster.GetComponent<Monster>(), nameComp);
        // if the monster is in the data storage array, update its points
        if (index >= 0)
        {
            dataStorage.monsters[index].pointsEarned += 25;
            dataStorage.monsters[index].totalPoints += 50;
            Debug.Log("+25 points to " + dataStorage.monsters[index].name);
            if (AfterHoursMonsterSpawner.findMonster(currentMonster.name) == -1)
            {
               AfterHoursMonsterSpawner.monsterList.Add(currentMonster.name);
            }
            AfterHoursMonsterSpawner.monsterAnswers.Add(0);
        }
        else
        {
            // Debug.Log("Unknown monster in After Hours, not in points array");
        }

        Debug.Log("wrong Answer");
    }

    // name comparison class, don't mind
    public class NameComp : IComparer<Monster>
    {
        public int Compare(Monster x, Monster y)
        {
            return x.name.CompareTo(y.name);
        }
    }

    [YarnCommand("TutorialOver")]
    public void endTutorial()
    {
        AfterHoursMonsterSpawner.tutorialOver = true;
        AfterHoursMonsterSpawner.currentMonster.GetComponent<Monster>().inAfterHours = false;
        AfterHoursMonsterSpawner.currentMonster = null;
        gameObject.SetActive(false);
    }
}
