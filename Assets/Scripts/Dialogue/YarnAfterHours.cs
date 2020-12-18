using System.Collections;
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
            if(dataStorage.currentDay == 0 && !AfterHoursMonsterSpawner.tutorialOver)
            {
                FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue("TutorialGhostDay1AH");
            }
            else
            {
                FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(gameObject.name + "AH1");
            }
        }
    }
    [YarnCommand("finishedTalkingWith")]
    public void deleteMonster()
    {
        gameObject.SetActive(false);
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
        Debug.Log("node is running");
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
        }
        else
        {
            Debug.Log("Unknown monster in After Hours, not in points array");
        }
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
        }
        else
        {
            Debug.Log("Unknown monster in After Hours, not in points array");
        }
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
