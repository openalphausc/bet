using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class AfterHoursMonsterSpawner : MonoBehaviour
{

    private static ArrayList monsterList;
    private static Vector3 monsterLocation = new Vector3(-25f, -5f, 0f);

    public static GameObject currentMonster = null;
    public DialogueRunner dRunner;  // DialogueRunner

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(dataStorage.stayingMonsters.Count);
        monsterList = dataStorage.stayingMonsters;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMonster == null)
        {
            currentMonster = GetFirstMonster();
            currentMonster.GetComponent<Monster>().inAfterHours = true;
        }
    }

    GameObject GetFirstMonster()
    {
        // Get monster name
        if (monsterList.Count > 0)
        {
            string monsterName = monsterList[0] as string;
            monsterList.RemoveAt(0);

            // Set node to the current monster's
            dRunner.StartDialogue(monsterName + "AH");

            // Instantiate it
            return Instantiate(Resources.Load<GameObject>("Prefabs/Monsters/" + monsterName), monsterLocation, Quaternion.identity);
        }

        return null;

    }

}
