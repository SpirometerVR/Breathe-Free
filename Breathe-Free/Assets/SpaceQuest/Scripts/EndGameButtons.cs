using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class EndGameButtons : MonoBehaviour
{
    private GameObject game;

    /**
     * Make the buttons at the end of the game clickable. The buttons will either
     * lead to the start menu or restart the SpaceQuest game with the same cycle parameters.
     */
    public void buttonPress()
	{
        game = EventSystem.current.currentSelectedGameObject;

        // If the Restart button is clicked, restart the game.
        if (game.name == "RetryButton")
        {
            SceneManager.LoadScene("SpaceQuest");
        }
        // Otherwise go to the start menu if the Menu button is clicked.
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
