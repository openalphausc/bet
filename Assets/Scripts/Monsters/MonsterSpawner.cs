using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monstersToSpawn = new List<Monster>();
    public List<Monster> monstersOnScreen = new List<Monster>();

    private float spawnerTimer = 0.0f;

    private bool readyToSpawn = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnerTimer += Time.deltaTime;

        // cycle through on screen monsters and despawn them if they're offscreen
        for(int i = 0; i < monstersOnScreen.Count; i++)
        {
            Monster monster = monstersOnScreen[i];
            if (monster.state == Monster.MonsterState.offscreen)
            {
                monstersOnScreen.RemoveAt(i);
                i--;
                Destroy(monster.gameObject);

                readyToSpawn = true;
                spawnerTimer = 0.0f;
            }
        }

        // spawn monsters after certain amount of time
        if(spawnerTimer >= 1.0f && readyToSpawn && monstersToSpawn.Count > 0)
        {
            // pick a random monster to spawn
            int randomIndex = Random.Range(0, monstersToSpawn.Count);
            // randomIndex = 10; // TEMP REMOVE
            Monster instantiatedMonster = Instantiate(monstersToSpawn[randomIndex]);
            instantiatedMonster.name = monstersToSpawn[randomIndex].name;
            instantiatedMonster.prefab = monstersToSpawn[randomIndex];
            readyToSpawn = false;

            // monstersToSpawn.RemoveAt(randomIndex);
            monstersOnScreen.Add(instantiatedMonster);
        }

        
    }
}
