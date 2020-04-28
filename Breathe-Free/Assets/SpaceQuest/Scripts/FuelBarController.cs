using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour
{
    private Slider fuelSlider;
    private GameObject player;
    private RocketController playerScript;
    private BreathObjectGenerator breathGen;

    private float fuelLevel;
    private float timer = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        // Create a Rocket object & Breath Gen
        player = GameObject.FindGameObjectWithTag("Rocket");
        playerScript = player.GetComponent<RocketController>();
        breathGen = player.GetComponent<BreathObjectGenerator>();

        fuelSlider = GameObject.FindGameObjectWithTag("Fuel Bar").GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        // If the player is inhaling, then increase the fuel bar as they inhale.
        if (playerScript.inhalePhase && playerScript.inhaleIsOn && breathGen.inhaleSpawned)
        {
            timer = 1f;
            if (fuelSlider.value <= 1)
            {
                // Adjust the speed based on the inhaleTargetTime
                IncreaseFuel(playerScript.inhaleDuration/playerScript.inhaleTargetTime);
                fuelLevel = GetFuelLevel();
            }
        }
        // If the player just exhaled and it switched to inhalePhase, wait 1 second before resetting the fuel bar
        // for the next inhale cycle.
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
        // While exhaling, gradually reduce the fuel bar.
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

    // Method to decrease fuel bar while exhaling.
    private void DecreaseFuel()
    {
        fuelSlider.value = fuelLevel - (playerScript.exhaleDuration / playerScript.exhaleTargetTime);
    }

    private float GetFuelLevel()
    {
        return fuelSlider.value;
    }
}
