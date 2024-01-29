using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
    How to get Queue of Scenes 
    1. Click on File 
    2. Click On Build Settings
    at the top you should see the queue 

    the current queue is as follows 
    0: Main Menu
    1: Main Game (what we coded all last quarter)
*/


public class MainMenu : MonoBehaviour
{
    

    // this is how we will change to our main game from the main menu 
    public void playGame()
    {
        // this will load the next level in the queue 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        //Application.Quit();
    }

    // this is where we load our main game from the gameOver Screen
    public void playGameAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
