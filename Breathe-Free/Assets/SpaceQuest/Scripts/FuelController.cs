using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    private float speed = 28.85f;
    private GameObject player;
	private RocketController playerScript;

    private GameObject rightThruster;
    private GameObject middleThruster;
    private GameObject leftThruster;

    private GameObject rightFuel;
    private GameObject middleFuel;
    private GameObject leftFuel;

    private AudioClip fuelSound;

    private float numFuels = 3;

    /**
     * Start is called before the first frame update
     */
    void Start()
    {
        // Create a Rocket object
        player = GameObject.FindGameObjectWithTag("Rocket");
        playerScript = player.GetComponent<RocketController>();

        // Find the thrusters
        rightThruster = GameObject.FindGameObjectWithTag("Right Thruster");
        middleThruster = GameObject.FindGameObjectWithTag("Middle Thruster");
        leftThruster = GameObject.FindGameObjectWithTag("Left Thruster");

        // Find the fuel objects.
        rightFuel = GameObject.FindGameObjectWithTag("Right Fuel");
        middleFuel = GameObject.FindGameObjectWithTag("Middle Fuel");
        leftFuel = GameObject.FindGameObjectWithTag("Left Fuel");

        // Find fuel sound.
        fuelSound = playerScript.fuel;
    }

    /**
     * Update is called once per frame
     */
    void Update()
    {
        // Move the fuel object towards the player when inhaling
        if (playerScript.inhalePhase && playerScript.inhaleIsOn)
        {
            // Move the right fuel towards the engine for the first portion of inhaling.
            if (playerScript.inhaleDuration > 0 && playerScript.inhaleDuration <= RocketController.inhaleTargetTime / numFuels)
            {
                rightFuel.transform.position = Vector3.MoveTowards(rightFuel.transform.position, rightThruster.transform.position, (speed / RocketController.inhaleTargetTime) * Time.deltaTime);
                // Play fuel sound when it reaches the thruster.
                if(playerScript.inhaleDuration == RocketController.inhaleTargetTime / numFuels)
				{
                    playerScript.audio.PlayOneShot(fuelSound, 0.5f);
                }
            }
            // Move the left fuel towards the engine for the second portion of inhaling.
            else if (playerScript.inhaleDuration > RocketController.inhaleTargetTime / numFuels && playerScript.inhaleDuration <= 2 * (RocketController.inhaleTargetTime / numFuels))
			{
                leftFuel.transform.position = Vector3.MoveTowards(leftFuel.transform.position, leftThruster.transform.position, (speed / RocketController.inhaleTargetTime) * Time.deltaTime);
                // Play fuel sound when it reaches the thruster.
                if (playerScript.inhaleDuration == 2 * (RocketController.inhaleTargetTime / numFuels))
                {
                    playerScript.audio.PlayOneShot(fuelSound, 0.5f);
                }
            }
            // Move the middle fuel towards the engine for the last portion of inhaling.
            else 
			{
                middleFuel.transform.position = Vector3.MoveTowards(middleFuel.transform.position, middleThruster.transform.position, (speed / (RocketController.inhaleTargetTime * 1.95f)) * Time.deltaTime);
                // Play fuel sound when it reaches the thruster.
                if (playerScript.inhaleDuration == RocketController.inhaleTargetTime)
                {
                    playerScript.audio.PlayOneShot(fuelSound, 0.5f);
                }
            } 
		}
    }
}
