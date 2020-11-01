using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class AfterHoursMonsterSpawner : MonoBehaviour
{

    private static string monsterStay;
    private static Vector3 monsterLocation = new Vector3(-25f, -5f, 0f);

    public static GameObject currentMonster = null;
    public DialogueRunner dRunner;  // DialogueRunner

    // Start is called before the first frame update
    void Start()
    {
        monsterStay = dataStorage.stayingMonster;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMonster != null)
        {
            currentMonster = GetFirstMonster();
            currentMonster.GetComponent<Monster>().inAfterHours = true;
            currentMonster = null;
        }
    }

    GameObject GetFirstMonster()
    {
        // Get monster name
        string monsterName = monsterStay;

        // Set node to the current monster's
        dRunner.StartDialogue(monsterName + "AH1");

        // Instantiate it
        GameObject afterHoursMonster = Instantiate(Resources.Load<GameObject>("Prefabs/Monsters/" + monsterName), monsterLocation, Quaternion.identity);
        afterHoursMonster.name = monsterName;
        return afterHoursMonster;

    }

}
