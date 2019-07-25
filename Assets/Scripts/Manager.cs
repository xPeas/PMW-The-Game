using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    GameManager theGame;
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        theGame = new GameManager();
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}