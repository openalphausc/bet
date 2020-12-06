using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class AfterHoursMonsterSpawner : MonoBehaviour
{
    private static Vector3 monsterLocation = new Vector3(-25f, -5f, 0f);
    public static GameObject currentMonster = null;
    public static bool active = false;
    
    void Start()
    {
        // Instantiate it
        currentMonster = Instantiate(
            Resources.Load<GameObject>("Prefabs/Monsters/" + dataStorage.stayingMonster),
            monsterLocation, 
            Quaternion.identity);
        
        currentMonster.name = dataStorage.stayingMonster;
        currentMonster.GetComponent<Monster>().inAfterHours = true;
        
        active = true;
    }

}
