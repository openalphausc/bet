using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monstersToSpawn = new List<Monster>();
    public List<Monster> monstersOnScreen = new List<Monster>();
    public int totalMonstersSpawned = 0;
    public Queue<Monster>[] MonsterQueue = new Queue<Monster>[9];
    public Queue<Monster> MonstersOfTheDay = new Queue<Monster>();
    public Queue<string>[] MonsterQueueS = new Queue<string>[9];
    public Queue<string> MonstersOfTheDayS = new Queue<string>();
    public int maxTotalMonsters = 10;
    public int maxMonstersOnScreen;

    public static List<Seat> barSeats;
    public float timeUntilNextSpawn = -1; // will be set randomly

    public LightFadeUp fadeUpScript;
    
    public static bool inTutorial = true;
    public static int currDay = 0;
    public bool bobishere = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateMonsterQueue();
        MonstersOfTheDay = MonsterQueue[currDay];
        MonstersOfTheDayS = MonsterQueueS[currDay];
       
        CreateBarSeats();
        /*for(int i = 0; i < monstersToSpawn.Count; ++i)
        {
            Debug.Log(monstersToSpawn[i]);
        }*/
        if (inTutorial) RunTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currDay);
        if(currDay == 0)
        {
            bobishere = true;
            //inTutorial == true;
        }
        else
        {
            bobishere = false;
            
        }
        if (!inTutorial)
        {
            // increment time until next monster spawn
            if (monstersOnScreen.Count < maxMonstersOnScreen /*&& totalMonstersSpawned < maxTotalMonsters*/)
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
                    Debug.Log(MonstersOfTheDayS.Count);
                    if (MonstersOfTheDayS.Count == 0 && monstersOnScreen.Count == 0 && !bobishere )//totalMonstersSpawned >= maxTotalMonsters)
                    {
                        Debug.Log("GoingtoAfterHours");
                        SceneManager.LoadScene("AfterHours");
                        currDay++;
                        MonstersOfTheDayS = MonsterQueueS[currDay];
                        
                        return;
                    }
                }
            }
        }
        else
        {
            if (bobishere && MonstersOfTheDayS.Count == 0 && monstersOnScreen.Count == 0)
            {
                Debug.Log("Bob left");
                currDay++;
                MonstersOfTheDayS = MonsterQueueS[currDay];
            }
        }
        
    }

    float GetSpawnTime()
    {
        return 2 * UnityEngine.Random.Range(1, 5);
    }

    void SpawnMonster()
    {

        if (inTutorial)
        {
            /*Monster instantiatedMonster = Instantiate(monstersToSpawn[4]);  // Ghost is at index 4
            instantiatedMonster.name = monstersToSpawn[4].name;
            Debug.Log(instantiatedMonster.name);
            instantiatedMonster.prefab = monstersToSpawn[4];
            instantiatedMonster.seat = GetAvailableSeat();
            instantiatedMonster.seat.SetOccupancy(true);*/

            Debug.Log("bob cmere you lil binch");
            string currMonster = MonstersOfTheDayS.Dequeue();
            Debug.Log("Should be bob: ");
            Monster instantiatedMonster = Instantiate(findMonster(currMonster));
            instantiatedMonster.name = findMonster(currMonster).name;
            instantiatedMonster.prefab = findMonster(currMonster);
            instantiatedMonster.seat = GetAvailableSeat();
            instantiatedMonster.seat.SetOccupancy(true);
        }
        else
        {
            // no monsters to spawn
            /*if (monstersToSpawn.Count == 0)
            {
                Debug.Log("Tried to spawn a monster, but there isn't a monster in the list to spawn.");
                return;
            }*/
            if (MonstersOfTheDayS.Count == 0)
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
            /*int randomIndex = UnityEngine.Random.Range(0, monstersToSpawn.Count);
            Monster instantiatedMonster = Instantiate(monstersToSpawn[randomIndex]);
            instantiatedMonster.name = monstersToSpawn[randomIndex].name;
            instantiatedMonster.prefab = monstersToSpawn[randomIndex];
            instantiatedMonster.seat = GetAvailableSeat();
            instantiatedMonster.seat.SetOccupancy(true);
    
            monstersToSpawn.RemoveAt(randomIndex);
            monstersOnScreen.Add(instantiatedMonster);
            totalMonstersSpawned++;*/
            if (MonstersOfTheDayS.Count != 0)
            {
                string currMonster = MonstersOfTheDayS.Dequeue();
                Debug.Log(currMonster);
                Debug.Log(currDay);
                Monster instantiatedMonster = Instantiate(findMonster(currMonster));
                instantiatedMonster.name = findMonster(currMonster).name;
                instantiatedMonster.prefab = findMonster(currMonster);
                instantiatedMonster.seat = GetAvailableSeat();
                instantiatedMonster.seat.SetOccupancy(true);

                //monstersToSpawn.RemoveAt(randomIndex);
                monstersOnScreen.Add(instantiatedMonster);
            }

        }
        
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

        if (inTutorial)
        {
            return MonsterSpawner.barSeats[1];
        }
        else
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

    private void RunTutorial()
    {
        // Disable cup movement
        GlassMove.cupCanMove = false;
        
        // Disabling items
        // buttons
        GameObject.Find("ClearGlassButton").GetComponent<Button>().interactable = false;
        GameObject.Find("CloseBarButton").GetComponent<Button>().interactable = false;
        
        // ingredients
        foreach (Transform ingredient in GameObject.Find("Ingredients").transform)
        {
            ingredient.GetComponent<HoverHighlight>().isEnabled = false; // Disable hover highlighting
            ingredient.GetComponent<ClickIngredient>().isEnabled = false; // Disable clicking
        }
        
        // toppings
        GameObject.Find("nightmareFuel").SetActive(true);
        GameObject.Find("goldenDust").SetActive(true);
        GameObject.Find("mud").SetActive(true);
        GameObject.Find("zombieFlesh").SetActive(true);
        
        // misc
        GameObject.Find("Blender").SetActive(true); // TEMP SET BACK TO FALSE

        // Spawn ghost in
        SpawnMonster();
    }

    private void CreateMonsterQueue()
    {
        
        string path = "Assets/Text/MonsterOrderNoGenerals.txt";

        StreamReader reader = new StreamReader(path);
        int currIndex = 0;

        while(!reader.EndOfStream)
        {
            Queue<Monster> currMonsters = new Queue<Monster>();
            Queue<string> currMonstersS = new Queue<string>();
            string line = reader.ReadLine();

            string[] monsters = line.Split(',');

            for (int i = 0; i < monsters.Length; ++i)
            {
                Debug.Log(currIndex+ " adding: " + monsters[i]);
                
                currMonstersS.Enqueue(monsters[i]);
                Monster adding = findMonster(monsters[i]);
                currMonsters.Enqueue(adding);
            }

            MonsterQueue[currIndex] = currMonsters;
            MonsterQueueS[currIndex] = currMonstersS;
            currIndex++;
        }

       /* for(int i = 0; i < 9; ++i)
        {
            for(int j = 0; j < MonsterQueue[i].Count; ++j)
            {
                Monster curr = MonsterQueue[i].Dequeue();
                Debug.Log(i + curr.name);
            }
        }*/

        reader.Close();

    }

    private Monster findMonster(string name)
    {
        for(int i = 0; i < monstersToSpawn.Count; ++i)
        {
            if(monstersToSpawn[i].name == name)
            {
                return monstersToSpawn[i];
            }
        }

        return null;
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