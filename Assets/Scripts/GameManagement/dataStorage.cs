using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataStorage : MonoBehaviour
{
    //List of monsters asked to stay afterHours
    public static string stayingMonster;
    public static int currentDay;

    // List of all monsters for tabsheet
    public static List<Monster> monsters;

    // Start is called before the first frame update
    void Start()
    {
        if(currentDay == 0)
        {
            monsters = new List<Monster>();
            monsters.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int findMonster(string monster)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monster == monsters[i].name)
            {
                return i;
            }
        }
        return -1;
    }
}
