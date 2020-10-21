using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataStorage : MonoBehaviour
{
    //List of monsters asked to stay afterHours
    public static ArrayList stayingMonsters;

    // Start is called before the first frame update
    void Start()
    {
        stayingMonsters = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
