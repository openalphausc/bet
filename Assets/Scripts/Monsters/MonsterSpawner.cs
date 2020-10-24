using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monstersToSpawn = new List<Monster>();
    public List<Monster> monstersOnScreen = new List<Monster>();
    private int spawnerCount = 0;
    public int maxSpawn = 3;
    public int maxMonstersOnScreen;

    private float spawnerTimer = 0.0f;

    private bool readyToSpawn = true;

    public static List<Seat> barSeats;
    private float timeSinceLastSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnerCount = 0;
        createBarSeats();
        maxMonstersOnScreen = barSeats.Count;

        // SET maxMonstersOnScreen HERE. Make sure it's less than or equal to 3.
        maxMonstersOnScreen = 2;
    }

    // Update is called once per frame
    void Update()
    {
        spawnerTimer += Time.deltaTime;
        timeSinceLastSpawn += Time.deltaTime;

        // cycle through on screen monsters and despawn them if they're offscreen
        // also count how many are on screen
        int currentlyOnScreen = 0;
        for (int i = 0; i < monstersOnScreen.Count; i++)
        {
            Monster monster = monstersOnScreen[i];
            if (monster.state == Monster.MonsterState.offscreen)
            {
                monstersOnScreen.RemoveAt(i);
                i--;
                Destroy(monster.gameObject);
                spawnerCount++;
                if (spawnerCount >= maxSpawn)
                {
                    SceneManager.LoadScene("AfterHours");
                    return;
                }
                readyToSpawn = true;
                spawnerTimer = 0.0f;
            }
            else
                currentlyOnScreen++;
        }
        if (currentlyOnScreen < maxMonstersOnScreen && timeSinceLastSpawn > 2.0f)
            readyToSpawn = true;
            

        // spawn monsters after certain amount of time
        if (spawnerTimer >= 1.0f && readyToSpawn && monstersToSpawn.Count > 0)
        {
            // pick a random monster to spawn
            int randomIndex = UnityEngine.Random.Range(0, monstersToSpawn.Count);
            // randomIndex = 10; // TEMP REMOVE
            Monster instantiatedMonster = Instantiate(monstersToSpawn[randomIndex]);
            instantiatedMonster.name = monstersToSpawn[randomIndex].name;
            instantiatedMonster.prefab = monstersToSpawn[randomIndex];
            readyToSpawn = false;
            timeSinceLastSpawn = 0;

            // monstersToSpawn.RemoveAt(randomIndex);
            monstersOnScreen.Add(instantiatedMonster);
        }


    }

    // Creates the locations of the bar seats
    void createBarSeats()
    {
        barSeats = new List<Seat>();
        Vector3 leftSeat = new Vector3(-35, 0, 0);
        Vector3 middleSeat = new Vector3(0, 0, 0);
        Vector3 rightSeat = new Vector3(35, 0, 0);

        barSeats.Add(new Seat(leftSeat, false));
        barSeats.Add(new Seat(middleSeat, false));
        barSeats.Add(new Seat(rightSeat, false));
    }
}

public class Seat
{
    public Vector3 seatLocation;
    public Boolean occupied;

    public Seat(Vector3 loc, Boolean occ)
    {
        seatLocation = loc;
        occupied = occ;
    }

    public void setOccupancy(Boolean occ)
    {
        occupied = occ;
    }

    public Boolean getOccupancy()
    {
        return occupied;
    }
}