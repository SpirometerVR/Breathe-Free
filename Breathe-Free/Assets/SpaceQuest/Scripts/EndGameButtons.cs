using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class EndGameButtons : MonoBehaviour
{
    private GameObject game;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void buttonPress()
	{
        game = EventSystem.current.currentSelectedGameObject;
        if (game.name == "RetryButton")
        {
            SceneManager.LoadScene("SpaceQuest");
        }
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
