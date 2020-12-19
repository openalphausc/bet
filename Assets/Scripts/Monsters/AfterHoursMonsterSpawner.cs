using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class AfterHoursMonsterSpawner : MonoBehaviour
{
    public static Vector3 monsterLocation = new Vector3(-15f, -5f, 0f);
    public static GameObject currentMonster = null;
    public static bool active = false;
    public static bool tutorialOver = false;
    private static bool khepriSpawned = false;
    public static ArrayList monsterList = new ArrayList();
    public static ArrayList monsterAnswers = new ArrayList();
    void Start()
    {
        // Instantiate ghost if first day, otherwise regular monster
        if (dataStorage.currentDay == 0)
        {
            currentMonster = Instantiate(
                Resources.Load<GameObject>("Prefabs/Monsters/Ghost"),
                monsterLocation,
                Quaternion.identity);

            currentMonster.name = "Ghost";
            currentMonster.GetComponent<Monster>().inAfterHours = true;
            currentMonster.GetComponent<SpriteRenderer>().sprite = currentMonster.GetComponent<Monster>().emotions[2];
        }
        else
        {
            currentMonster = Instantiate(
                Resources.Load<GameObject>("Prefabs/Monsters/" + dataStorage.stayingMonster),
                monsterLocation,
                Quaternion.identity);

            currentMonster.name = dataStorage.stayingMonster;
            currentMonster.GetComponent<Monster>().inAfterHours = true;

            
        }

        active = true;
    }

    void Update()
    {
        //spawn khepri after ghost disappears from scene
        if (tutorialOver && !khepriSpawned)
        {
            currentMonster = Instantiate(
                Resources.Load<GameObject>("Prefabs/Monsters/" + dataStorage.stayingMonster),
                monsterLocation,
                Quaternion.identity);

            currentMonster.name = dataStorage.stayingMonster;
            currentMonster.GetComponent<Monster>().inAfterHours = true;
            khepriSpawned = true;
        }
    }

    //finds if monster has been in afterhours, if it has, then it will return the index of its answer. Otherwise it returns -1.
    public static int findMonster(string monster)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (monster == (string)monsterList[i])
            {
                return i;
            }
        }
        return -1;
    }

}
