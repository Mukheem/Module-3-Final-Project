using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    /*
     * Method to Start the game scene
     */
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    /*
     * Method to Re-Start the game scene after the player finishes the play.
     */
    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }
}
