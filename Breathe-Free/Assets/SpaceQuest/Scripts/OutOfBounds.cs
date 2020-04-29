using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfBounds : MonoBehaviour
{
    public GameObject player;
    public RocketController playerScript;

    public GameObject arrowOne;
    public GameObject arrowTwo;
    public GameObject arrowThree;
    public GameObject arrowFour;

    public float timer = 2.5f;

    private GameObject OutOfBoundsCanvas;
    private Text outOfBoundsText;

    // Start is called before the first frame update
    void Start()
    {
        // Find objects.
        player = GameObject.FindGameObjectWithTag("Rocket");
        playerScript = player.GetComponent<RocketController>();
        outOfBoundsText = GameObject.FindGameObjectWithTag("Out Of Bounds").GetComponent<Text>();
        OutOfBoundsCanvas = GameObject.FindGameObjectWithTag("Out Of Bounds Canvas");

        // Set all arrows to invisible.
        arrowOne.GetComponent<Renderer>().enabled = false;
        arrowTwo.GetComponent<Renderer>().enabled = false;
        arrowThree.GetComponent<Renderer>().enabled = false;
        arrowFour.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScript.gameOver)
        {
            // Only print error when the camera view is out of bounds.
            if (playerScript.inBounds && (Camera.main.transform.rotation.eulerAngles.y >= 0 && Camera.main.transform.rotation.eulerAngles.y <= 60) || (Camera.main.transform.rotation.eulerAngles.y >= 300 && Camera.main.transform.rotation.eulerAngles.y <= 360))
            {
                // Lock Rotation on X and Z Axis.
                OutOfBoundsCanvas.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                outOfBoundsText.text = "";

                // Turn off arrows.
                arrowOne.GetComponent<Renderer>().enabled = false;
                arrowTwo.GetComponent<Renderer>().enabled = false;
                arrowThree.GetComponent<Renderer>().enabled = false;
                arrowFour.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                // Lock Rotation on X and Z Axis.
                OutOfBoundsCanvas.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                outOfBoundsText.text = "RETURN TO SHIP";

                timer -= Time.deltaTime;

                // If the player is looking left, put the right arrows on.
                if (player.transform.rotation.eulerAngles.y < 300 && player.transform.rotation.eulerAngles.y >= 180)
                {
                    // Stagger arrow visbility
                    if (timer >= 2f)
                    {
                        arrowOne.GetComponent<Renderer>().enabled = false;
                        arrowTwo.GetComponent<Renderer>().enabled = false;
                        arrowThree.GetComponent<Renderer>().enabled = false;
                        arrowFour.GetComponent<Renderer>().enabled = false;
                    }
                    else if (timer < 2f && timer >= 1f)
                    {
                        arrowOne.GetComponent<Renderer>().enabled = true;
                        arrowTwo.GetComponent<Renderer>().enabled = false;
                        arrowThree.GetComponent<Renderer>().enabled = false;
                        arrowFour.GetComponent<Renderer>().enabled = false;
                    }
                    else if (timer < 1f && timer >= 0)
                    {
                        arrowOne.GetComponent<Renderer>().enabled = false;
                        arrowTwo.GetComponent<Renderer>().enabled = true;
                        arrowThree.GetComponent<Renderer>().enabled = false;
                        arrowFour.GetComponent<Renderer>().enabled = false;
                    }
                    // Reset timer.
                    else
                    {
                        timer = 2.5f;
                    }
                }
                // If the player is looking right, put the left arrows on.
                else //(player.transform.rotation.eulerAngles.y < 300 && player.transform.rotation.eulerAngles.y >= 180)
                {
                    // Stagger arrow visbility
                    if (timer >= 2f)
                    {
                        arrowOne.GetComponent<Renderer>().enabled = false;
                        arrowTwo.GetComponent<Renderer>().enabled = false;
                        arrowThree.GetComponent<Renderer>().enabled = false;
                        arrowFour.GetComponent<Renderer>().enabled = false;
                    }
                    else if (timer < 2f && timer >= 1f)
                    {
                        arrowOne.GetComponent<Renderer>().enabled = false;
                        arrowTwo.GetComponent<Renderer>().enabled = false;
                        arrowThree.GetComponent<Renderer>().enabled = true;
                        arrowFour.GetComponent<Renderer>().enabled = false;
                    }
                    else if (timer < 1f && timer >= 0)
                    {
                        arrowOne.GetComponent<Renderer>().enabled = false;
                        arrowTwo.GetComponent<Renderer>().enabled = false;
                        arrowThree.GetComponent<Renderer>().enabled = false;
                        arrowFour.GetComponent<Renderer>().enabled = true;
                    }
                    // Reset timer.
                    else
                    {
                        timer = 2.5f;
                    }
                }
            }
        }
        else
        {
            // Lock Rotation on X and Z Axis.
            OutOfBoundsCanvas.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            outOfBoundsText.text = "";
            arrowOne.GetComponent<Renderer>().enabled = false;
            arrowTwo.GetComponent<Renderer>().enabled = false;
            arrowThree.GetComponent<Renderer>().enabled = false;
            arrowFour.GetComponent<Renderer>().enabled = false;
        }
    }
}
