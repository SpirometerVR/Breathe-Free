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

    /**
     * Start is called before the first frame update.
     */
    private void Start()
    {
        // Create a Rocket object & Breath Gen
        player = GameObject.FindGameObjectWithTag("Rocket");
        playerScript = player.GetComponent<RocketController>();
        breathGen = player.GetComponent<BreathObjectGenerator>();

        fuelSlider = GameObject.FindGameObjectWithTag("Fuel Bar").GetComponent<Slider>();
    }

    /**
     * Update is called once per frame
     */
    private void Update()
    {
        // If the player is inhaling, then increase the fuel bar as they inhale.
        if (playerScript.inhalePhase && playerScript.inhaleIsOn && breathGen.inhaleSpawned)
        {
            // Reset timer.
            timer = 1f;
            if (fuelSlider.value <= 1)
            {
                // Adjust the speed based on the inhaleTargetTime
                IncreaseFuel(playerScript.inhaleDuration / RocketController.inhaleTargetTime);
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

    /**
     * Reset the fuel bar to zero when the cycle is over.
     */
    private void ResetFuel()
    {
        fuelSlider.value = 0;
    }

    /**
     * Increase the fuel bar fill as the player inhales.
     */
    private void IncreaseFuel(float fuel)
    {
        fuelSlider.value = fuel;
    }

    /**
     * Decrease the fuel bar fill as the player exhales.
     */
    private void DecreaseFuel()
    {
        fuelSlider.value = fuelLevel - (playerScript.exhaleDuration / RocketController.exhaleTargetTime);
    }

    /**
     * Return the current fuel level.
     * @return: feul slider value
     */
    private float GetFuelLevel()
    {
        return fuelSlider.value;
    }
}
