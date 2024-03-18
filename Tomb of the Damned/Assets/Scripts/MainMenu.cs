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
    Index 0: Main Menu
    Index 1: Main Game (what we coded all last quarter)
    Index 2: Game Over
    Index 3: Foilage
    Index 4: Inventory User Interface

    
*/


public class MainMenu : MonoBehaviour
{
    void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Entered Scene " + currentSceneIndex);

        // Check if the current scene is the Main Game scene
        if (currentSceneIndex == 1)
        {
            Debug.Log("Cursor Not Visible");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        }
        else
        {
            Debug.Log("Cursor Visible");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Allow the cursor to move freely
        }
    }

    public void playGame()
    {
        // Load the next scene in the build settings
        Cursor.visible = false; // Make cursor invisible when transitioning to the next scene
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Additively load the foliage scene
        SceneManager.LoadScene("Foliage", LoadSceneMode.Additive);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void playGameAgain()
    {
        // Load the previous scene in the build settings
        Cursor.visible = true; // Make cursor visible when transitioning back to the previous scene
        Cursor.lockState = CursorLockMode.None; // Allow the cursor to move freely
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        // Additively load the foliage scene
        SceneManager.LoadScene("Foliage", LoadSceneMode.Additive);
    }
}