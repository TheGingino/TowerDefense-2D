using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Level");
    } 
}