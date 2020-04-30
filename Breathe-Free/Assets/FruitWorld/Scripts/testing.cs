using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class testing : MonoBehaviour
{

    public static AudioSource audio;
    private int inhaleTime;
    private int exhaleTime;
    private int numOfCycles;
    public int gameIndex = -1;
    private GameObject game;

    public GameObject buttons;
    public GameObject inputs;



    [SerializeField] Animator MenuAnimator;
    [SerializeField] TextMeshProUGUI inhaleText;
    [SerializeField] TextMeshProUGUI exhaleText;
    [SerializeField] TextMeshProUGUI cyclesText;


    private void Start()
    {
        audio = GetComponent<AudioSource>();
        inhaleTime = 1;
        exhaleTime = 1;
        numOfCycles = 1;
    }

    

    public void toInputs()
    {
        buttons.SetActive(false);
        inputs.SetActive(true);
        audio.PlayOneShot(audio.clip);
        game=EventSystem.current.currentSelectedGameObject;

        Debug.Log(game);
    }

    public void toStart()
    {
        buttons.SetActive(true);
        inputs.SetActive(false);
        audio.PlayOneShot(audio.clip);

    }

    public void incrementInhale()
    {
        if (inhaleTime < 15)
            inhaleTime++;
        inhaleText.text = inhaleTime.ToString();
        audio.PlayOneShot(audio.clip);
    }
    public void incrementExhale()
    {
        if (exhaleTime < 15)
            exhaleTime++;
        exhaleText.text = exhaleTime.ToString();
        audio.PlayOneShot(audio.clip);
    }
    public void incrementCycles()
    {
        if (numOfCycles < 20)
            numOfCycles++;
        cyclesText.text = numOfCycles.ToString();
        audio.PlayOneShot(audio.clip);
    }

    public void decrementInhale()
    {
        if (inhaleTime > 1)
            inhaleTime--;
        inhaleText.text = inhaleTime.ToString();
        audio.PlayOneShot(audio.clip);
    }
    public void decrementExhale()
    {
        if (exhaleTime >1)
            exhaleTime--;
        exhaleText.text = exhaleTime.ToString();
        audio.PlayOneShot(audio.clip);
    }
    public void decrementCycles()
    {
        if (numOfCycles >1)
            numOfCycles--;
        cyclesText.text = numOfCycles.ToString();
        audio.PlayOneShot(audio.clip);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        audio.PlayOneShot(audio.clip);
    }

    public void startGame()
    {
        Debug.Log(game);
        mechanics.inhaleTime = inhaleTime;
        mechanics.exhaleTime = exhaleTime;
        mechanics.numOfCycles = numOfCycles;
        RocketController.inhaleTargetTime = inhaleTime;
        RocketController.exhaleTargetTime = exhaleTime;
        RocketController.cycles = numOfCycles;


        if (game.name == "FruitWorldButton")
            SceneManager.LoadScene("FruitWorld");
        else
            SceneManager.LoadScene("SpaceQuest");
        audio.PlayOneShot(audio.clip);
    }

}