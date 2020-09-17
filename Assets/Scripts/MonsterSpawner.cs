using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monstersToSpawn = new List<Monster>();
    public List<Monster> monstersOnScreen = new List<Monster>();

    private float spawnerTimer;

    private bool birdPersonSpawned;

    // Start is called before the first frame update
    void Start()
    {
        spawnerTimer = 0;

        birdPersonSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        spawnerTimer += Time.deltaTime;

        // spawn birdPerson after certain amount of time
        if(spawnerTimer >= 1.0f && !birdPersonSpawned)
        {
            Monster instantiatedMonster = Instantiate(monstersToSpawn[0], new Vector3(0, 0, 0), Quaternion.identity);
            birdPersonSpawned = true;

            monstersToSpawn.RemoveAt(0);
            monstersOnScreen.Add(instantiatedMonster);
        }

        // cycle through on screen monsters and despawn them if they're offscreen
        for(int i = 0; i < monstersOnScreen.Count; i++)
        {
            Monster monster = monstersOnScreen[i];
            if (monster.state == Monster.MonsterState.offscreen)
            {
                monstersOnScreen.RemoveAt(i);
                i--;
                Destroy(monster.gameObject);
            }
        }
    }
}
