﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monstersToSpawn = new List<Monster>();
    public List<Monster> monstersOnScreen = new List<Monster>();
    public int totalMonstersSpawned = 0;
    public int maxTotalMonsters = 2;
    public int maxMonstersOnScreen;

    public static List<Seat> barSeats;
    public float timeUntilNextSpawn = -1; // will be set randomly

    public LightFadeUp fadeUpScript;

    // Start is called before the first frame update
    void Start()
    {
        CreateBarSeats();
    }

    // Update is called once per frame
    void Update()
    {
        // increment time until next monster spawn
        if (monstersOnScreen.Count < maxMonstersOnScreen && totalMonstersSpawned < maxTotalMonsters)
        {
            if (timeUntilNextSpawn > Time.deltaTime)
            {
                timeUntilNextSpawn -= Time.deltaTime;
            }
            else if(fadeUpScript.DoneFadingUp())
            {
                SpawnMonster();
                timeUntilNextSpawn = GetSpawnTime();
            }
        }

        // cycle through on screen monsters and despawn them if they're offscreen
        // also count how many are on screen
        for (int i = 0; i < monstersOnScreen.Count; i++)
        {
            Monster monster = monstersOnScreen[i];
            if (monster.state == Monster.MonsterState.offscreen)
            {
                // remove monster
                monstersOnScreen.RemoveAt(i);
                i--;
                Destroy(monster.gameObject);
                // start spawn timer for next monster
                timeUntilNextSpawn = GetSpawnTime();
                
                // if already served all monsters, go to after hours
                if (totalMonstersSpawned >= maxTotalMonsters)
                {
                    SceneManager.LoadScene("AfterHours");
                    return;
                }
            }
        }
    }

    float GetSpawnTime()
    {
        return 2 * UnityEngine.Random.Range(1, 5);
    }

    void SpawnMonster()
    {
        // no monsters to spawn
        if (monstersToSpawn.Count == 0)
        {
            Debug.Log("Tried to spawn a monster, but there isn't a monster in the list to spawn.");
            return;
        }

        // no available seats
        if (GetAvailableSeat() == null)
        {
            Debug.Log("No available seat to spawn monster, restarting spawn timer.");
            timeUntilNextSpawn = GetSpawnTime();
            return;
        }

        // pick a random monster to spawn
        int randomIndex = UnityEngine.Random.Range(0, monstersToSpawn.Count);
        Monster instantiatedMonster = Instantiate(monstersToSpawn[randomIndex]);
        instantiatedMonster.name = monstersToSpawn[randomIndex].name;
        instantiatedMonster.prefab = monstersToSpawn[randomIndex];
        instantiatedMonster.seat = GetAvailableSeat();
        instantiatedMonster.seat.SetOccupancy(true);

        monstersToSpawn.RemoveAt(randomIndex);
        monstersOnScreen.Add(instantiatedMonster);
        totalMonstersSpawned++;
        //Debug.Log(totalMonstersSpawned);
    }

    // Creates the locations of the bar seats
    void CreateBarSeats()
    {
        barSeats = new List<Seat>();
        Vector3 leftSeat = new Vector3(-30, 0, 0);
        Vector3 middleSeat = new Vector3(0, 0, 0);
        Vector3 rightSeat = new Vector3(30, 0, 0);

        barSeats.Add(new Seat(leftSeat, false));
        barSeats.Add(new Seat(middleSeat, false));
        barSeats.Add(new Seat(rightSeat, false));
    }

    Seat GetAvailableSeat()
    {
        // First, check if there's an available seat
        bool availableSeat = false;
        foreach (Seat seat in barSeats)
        {
            if (seat.occupied == false)
            {
                availableSeat = true;
                break;
            }
        }
        // if there's no available seat, return null
        if (availableSeat == false) return null;

        // now keep randomly selecting seats until you find an empty one
        while (true)
        {
            int randomIndex = UnityEngine.Random.Range(0, barSeats.Count);
            // if that seat is occupied, just try again
            if (barSeats[randomIndex].occupied) continue;
            // otherwise, that's the target seat
            return MonsterSpawner.barSeats[randomIndex];
        }
    }
}

// Just encompasses a seat location and whether or not it's occupied
public class Seat
{
    public Vector3 seatLocation;
    public Boolean occupied;

    public Seat(Vector3 loc, Boolean occ)
    {
        seatLocation = loc;
        occupied = occ;
    }

    public void SetOccupancy(Boolean occ)
    {
        occupied = occ;
    }

    public Boolean GetOccupancy()
    {
        return occupied;
    }
}