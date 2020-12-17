using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Controller : MonoBehaviour
{

    public float secondsToFadeOut = 1.0f;

    public void NextScene()
    {
        Initiate.Fade("Menu", Color.black, secondsToFadeOut);
    }


}
