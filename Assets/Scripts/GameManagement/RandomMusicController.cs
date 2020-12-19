using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMusicController : MonoBehaviour
{

    public AudioClip dayMusic1;
    public AudioClip dayMusic2;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.clip = Random.value >= 0.5 ? dayMusic1 : dayMusic2; // 50/50 between songs
        
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
