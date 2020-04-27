using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour
{
    private Slider fuelSlider;
    private GameObject player;
    private RocketController playerScript;

    private float fuelLevel;
    private float timer = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        // Create a Rocket object
        player = GameObject.FindGameObjectWithTag("Rocket");
        playerScript = player.GetComponent<RocketController>();

        fuelSlider = GameObject.FindGameObjectWithTag("Fuel Bar").GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerScript.inhalePhase && playerScript.inhaleIsOn)
        {
            timer = 1f;
            if (fuelSlider.value <= 1)
            {
                IncreaseFuel(playerScript.inhaleDuration/playerScript.inhaleTargetTime);
                fuelLevel = GetFuelLevel();
            }
        }
        else if (playerScript.inhalePhase)
        {
            if (timer <= 0)
            {
                ResetFuel();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else if (playerScript.exhalePhase && playerScript.exhaleIsOn)
        {
            if (fuelSlider.value >= 0)
            {
                DecreaseFuel();
            }
        }
    }

    // Method to set fuel to 0 at the start of each inhale.
    private void ResetFuel()
    {
        fuelSlider.value = 0;
    }

    // Method to adjust fuel bar as inhaling.
    private void IncreaseFuel(float fuel)
    {
        fuelSlider.value = fuel;
    }

    private void DecreaseFuel()
    {
        fuelSlider.value = fuelLevel - (playerScript.exhaleDuration / playerScript.exhaleTargetTime);
    }

    private float GetFuelLevel()
    {
        return fuelSlider.value;
    }
}
