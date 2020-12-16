using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class YarnAfterHours : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "AfterHours")
        {
            FindObjectOfType<Yarn.Unity.DialogueRunner>().StartDialogue(gameObject.name + "AH1");
        }
    }
    [YarnCommand("finishedTalkingWith")]
    public void deleteMonster()
    {
        gameObject.SetActive(false);
        AfterHoursMonsterSpawner.currentMonster.GetComponent<Monster>().inAfterHours = false;
        AfterHoursMonsterSpawner.currentMonster = null;
        AfterHoursMonsterSpawner.active = false;
        SceneManager.LoadScene("Tabsheet");
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
}
