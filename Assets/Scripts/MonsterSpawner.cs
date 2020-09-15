using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monstersToSpawn;

    private float spawnerTimer;

    public GameObject birdPerson;
    private bool birdPersonSpawned;

    // Start is called before the first frame update
    void Start()
    {
        monstersToSpawn = new List<GameObject>();
        monstersToSpawn.Add(birdPerson);

        spawnerTimer = 0;

        birdPersonSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        spawnerTimer += Time.deltaTime;

        //Debug.Log("spawner's timer is " + spawnerTimer);

        // spawn birdPerson after certain amount of time
        if(spawnerTimer >= 1.0f && !birdPersonSpawned)
        {
            Instantiate(birdPerson, new Vector3(0, 0, 0), Quaternion.identity);
            birdPersonSpawned = true;
        }
    }
}
