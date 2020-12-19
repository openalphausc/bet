using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericChangeScene : MonoBehaviour
{
    public string sceneToChangeTo;
    public GameObject buttonSounds;

    // very advanced and hard to understand changescene function
    public void ChangeScene()
    {
        buttonSounds.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
