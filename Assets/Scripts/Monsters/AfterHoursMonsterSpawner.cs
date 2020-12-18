using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class AfterHoursMonsterSpawner : MonoBehaviour
{
    public static Vector3 monsterLocation = new Vector3(-25f, -5f, 0f);
    public static GameObject currentMonster = null;
    public static bool active = false;
    public static bool tutorialOver = false;
    private static bool khepriSpawned = false;
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

}
