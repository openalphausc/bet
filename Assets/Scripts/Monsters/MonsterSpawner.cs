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
            Monster instantiatedMonster = Instantiate(monstersToSpawn[0], new Vector3(0, 0, 0), Quaternion.identity);
            instantiatedMonster.name = monstersToSpawn[0].name;
            readyToSpawn = false;

            monstersToSpawn.RemoveAt(0);
            monstersOnScreen.Add(instantiatedMonster);
        }

        
    }
}
