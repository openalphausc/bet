using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSpawner : MonoBehaviour
{
    public GameObject emptyGlassPrefab;

    public void SpawnGlass()
    {
        GameObject newGlass = Instantiate(emptyGlassPrefab);
        newGlass.transform.position = new Vector2(-4.5f, -10.67f);
    }
}
