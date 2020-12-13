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

}
