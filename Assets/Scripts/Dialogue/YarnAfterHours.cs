﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnAfterHours : MonoBehaviour
{
    
    [YarnCommand("finishedTalkingWith")]
    public void deleteMonster()
    {
        // Next monster gets loaded in in the AfterHoursMonsterSpawner
        gameObject.SetActive(false);
        AfterHoursMonsterSpawner.currentMonster = null;
    }

}
