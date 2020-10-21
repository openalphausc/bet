using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnBarTending : MonoBehaviour
{
    [YarnCommand("stayAfter")]
    public void stayAfter()
    {
        Debug.Log(dataStorage.stayingMonsters.Count);
        dataStorage.stayingMonsters.Add(gameObject);
        Debug.Log(dataStorage.stayingMonsters.Count);
        //dataStorage.GetComponent("stayingMonsters") = stayingMonsters;
    }

    void Start()
    {

    }
}
