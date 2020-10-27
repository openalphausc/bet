using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monstersToSpawn = new List<Monster>();
    public List<Monster> monstersOnScreen = new List<Monster>();
    public int totalMonstersSpawned = 0;
    public int maxTotalMonsters = 10;
    public int maxMonstersOnScreen;

    public static List<Seat> barSeats;
    public float timeUntilNextSpawn = -1; // will be set randomly

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
                Debug.Log("decremented time ");
            }
            else
            {
                Debug.Log("time reached 0, gonna spawn a monster");
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
        Debug.Log("started SpawnMonster()");
        if (monstersToSpawn.Count == 0)
        {
            Debug.Log("Tried to spawn a monster, but there isn't a monster in the list to spawn.");
            return;
        }

        Debug.Log("SPAWNED A MONSTER " + timeUntilNextSpawn);
        // pick a random monster to spawn
        int randomIndex = UnityEngine.Random.Range(0, monstersToSpawn.Count);
        // randomIndex = 10; // TEMP REMOVE
        Monster instantiatedMonster = Instantiate(monstersToSpawn[randomIndex]);
        instantiatedMonster.name = monstersToSpawn[randomIndex].name;
        instantiatedMonster.prefab = monstersToSpawn[randomIndex];

        // monstersToSpawn.RemoveAt(randomIndex);
        monstersOnScreen.Add(instantiatedMonster);
        totalMonstersSpawned++;
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