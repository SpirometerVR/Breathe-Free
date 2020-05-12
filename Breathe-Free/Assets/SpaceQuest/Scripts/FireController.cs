using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject player;
    private ParticleSystem flames;
    private RocketController playerScript;
    public Vector3 offset = new Vector3(0f, -0.6f, -16.1f);

    /**
     * Start is called before the first frame update
     */
    void Start()
    {
        // Find the flame GameObjects.
        playerScript = player.GetComponent<RocketController>();
        flames = GetComponent<ParticleSystem>();

        // Manually stop the flames at the beginning of the game.
        flames.Stop();
    }

    /**
     * Update is called once per frame.
     */
    void Update()
    {
        // Only play flames when player is exhaling.
        if (playerScript.exhaleIsOn && playerScript.exhalePhase)
        {
            flames.Play();
        }
        else
        {
            flames.Stop();
        }
    }
}
