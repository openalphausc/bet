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
    public Queue<string>[] monsterQueue = new Queue<string>[6];
    public Queue<string> monstersOfTheDay = new Queue<string>();
    public int maxTotalMonsters = 10;
    public int maxMonstersOnScreen;

    public static List<Seat> barSeats;
    public static float timeUntilNextSpawn = -1; // will be set randomly

    public LightFadeUp fadeUpScript;

    public static bool inTutorial = true;
    public static int currDay = 0;
    public static bool tutorialHasRun = false;
    public static GameObject SkipTutorialButton;
    public static Monster bob = null;

    // Start is called before the first frame update
    void Start()
    {
        CreateMonsterQueue();
        monstersOfTheDay = monsterQueue[currDay];

        CreateBarSeats();
        //GameObject.Find("SkipTutorialButton").SetActive(false);
        SkipTutorialButton = GameObject.Find("SkipTutorialButton");
        SkipTutorialButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((currDay >= 0 && currDay <= 2) && !tutorialHasRun)
        {
            inTutorial = true;
            RunTutorial();
            SpawnMonster();
            tutorialHasRun = true;
        }

        if (!inTutorial)
        {
            // increment time until next monster spawn
            if (monstersOnScreen.Count < maxMonstersOnScreen)
            {
                if (timeUntilNextSpawn > Time.deltaTime)
                {
                    timeUntilNextSpawn -= Time.deltaTime;
                }
                else if (fadeUpScript.DoneFadingUp())
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
                    if (monstersOfTheDay.Count == 0 && monstersOnScreen.Count == 0)
                    {
                        SceneManager.LoadScene("AfterHours");
                        currDay++;
                        monstersOfTheDay = monsterQueue[currDay];
                        tutorialHasRun = false;

                        return;
                    }
                }
            }
        }

    }

    public static float GetSpawnTime()
    {
        return 2 * UnityEngine.Random.Range(1, 5);
    }

    void SpawnMonster()
    {
        if (monstersOfTheDay.Count == 0)
        {
            //Debug.Log("Tried to spawn a monster, but there isn't a monster in the list to spawn.");
            return;
        }

        // no available seats
        if (GetAvailableSeat() == null)
        {
            timeUntilNextSpawn = GetSpawnTime();
            return;
        }

        if (monstersOfTheDay.Count != 0)
        {
            string currMonster = monstersOfTheDay.Dequeue();

            int monsterIndex = -1;

            for (int i = 0; i < monstersToSpawn.Count; ++i)
            {
                if (monstersToSpawn[i].name.Equals(currMonster) == true)
                {
                    monsterIndex = i;
                    break;
                }
            }

            if (monsterIndex == -1)
            {
                return;
            }

            Monster instantiatedMonster = Instantiate(monstersToSpawn[monsterIndex]);
            instantiatedMonster.name = monstersToSpawn[monsterIndex].name;
            instantiatedMonster.prefab = monstersToSpawn[monsterIndex];
            instantiatedMonster.seat = GetAvailableSeat();
            instantiatedMonster.seat.SetOccupancy(true);

            if(inTutorial)
            {
                bob = instantiatedMonster;
            }
            else
            {
                bob = null;
            }
            monstersOnScreen.Add(instantiatedMonster);
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
    }

    private void CreateMonsterQueue()
    {
        string path = "Assets/Text/MonsterOrderNoGenerals.txt";

        StreamReader reader = new StreamReader(path);
        int currIndex = 0;

        while (!reader.EndOfStream)
        {
            Queue<string> currMonsters = new Queue<string>();
            string line = reader.ReadLine();

            string[] monsters = line.Split(',');

            for (int i = 0; i < monsters.Length; ++i)
            {
                currMonsters.Enqueue(monsters[i]);
            }

            monsterQueue[currIndex] = currMonsters;
            currIndex++;
        }

        reader.Close();
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