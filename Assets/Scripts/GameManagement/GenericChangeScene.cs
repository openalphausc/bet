using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericChangeScene : MonoBehaviour
{
    public string sceneToChangeTo;

    // very advanced and hard to understand changescene function
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
