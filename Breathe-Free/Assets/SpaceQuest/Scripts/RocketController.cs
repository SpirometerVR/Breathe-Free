using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    // Adjusted all private GameObjects & Script Objects to be Public due to Private access errors.

    // Flags for exhale and inhale phases.
    public bool exhalePhase = false;
    public bool inhalePhase = true;

    public GameObject miniDiamond;
    public GameObject miniDiamondTwo;
    public GameObject engineLight;

    public bool inBounds = true;

    // Public variables that are adjusted based on the startMenu input.
    public static float exhaleTargetTime = 3f;
    public static float inhaleTargetTime = 3f;
    public static string userName;

    public float exhaleDuration;
    public float inhaleDuration;
	public float breakDuration;

	// Public cycles variable can be adjusted by doctor/patient.
	public static float cycles = 5f;
    public float cycleCounter = 0f;
    public bool gameOver = false;
    public bool submitNewScore = true;

    // Music that will be played when items are collected.
    public AudioClip diamond;
    public AudioClip crash;
    public AudioClip fuel;

    // Variables to track inhale & exhale duration.
    private float downTime = 0f;
    private float upTime = 0f;
	private float breakTime = 0f;
	private float exhaleStart = 0f;
    private float inhaleStart = 0f;
	private float breakStart = 0f;
    private float tempInhale = 0f;

    public bool exhaleIsOn = false;
    public bool inhaleIsOn = false;
    public bool breakIsOn = false;

    // Target threshold values for inhale and exhale.
    private float exhaleThresh = 1480f;
    private float inhaleTresh = 1170f;
    private float steadyThresh = 1340f;
	private float speedMultiplier = 200;

	public AudioSource audio;
    public Renderer gameRocket;

    // Create GameObject to find OSC
    public GameObject OSC;
    // Hold OSC data in spirometer object
    public OSC spirometer;
    // Get the rocket as a rigidbody
    public Rigidbody rocketBody;

    public ScoreBoard diamondScores;
    public ScoreBoard finalScores;

    public GameObject leaderBoard;
    public GameObject ovrGazePointer;

    public BreathObjectGenerator breathGen;

    // Reference to the dreamloLeaderboard prefab in the scene
    dreamloLeaderBoard sqLeaderBoard;
    private bool topScoresReceived = false;
    private Text topNameList;
    private Text topScoreList;
    private Text topRankList;

    enum gameState
    {
        waiting,
        running,
        enterscore,
        leaderboard,
        gameOver
    };

    gameState gState;

    /**
     * Start is called before the first frame update
     */
    void Start()
    {
        // Find the OSC Game Object.
        OSC = GameObject.Find("OSC");
        spirometer = OSC.GetComponent<OSC>();
        // Read input data from the M5 Stick on start.
        spirometer.SetAddressHandler("/Spirometer/C", ReceiveSpirometerData);

        // Get game renderer for the Rocket
        gameRocket = GetComponent<Renderer>();
        gameRocket.enabled = true;

        // Get rigid body and audio components for the rocket.
        audio = GetComponent<AudioSource>();

        // Find the score board objects for each respective scoreboard.
        diamondScores = GameObject.FindGameObjectWithTag("Diamond Score").GetComponent<ScoreBoard>();
        finalScores = GameObject.FindGameObjectWithTag("Final Score").GetComponent<ScoreBoard>();

        // Find objects for the leaderboard.
        topNameList = GameObject.Find("Top Names List").GetComponent<Text>();
        topScoreList = GameObject.Find("Top Scores List").GetComponent<Text>();
        topRankList = GameObject.Find("Top Ranks List").GetComponent<Text>();

        // Manually set inhale phase to true at start of game.
        inhalePhase = true;

        // Initialzie the leaderboard objects.
        this.sqLeaderBoard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        leaderBoard = GameObject.FindGameObjectWithTag("Leader Board");
        ovrGazePointer = GameObject.FindGameObjectWithTag("OVRGazePointer");
        leaderBoard.SetActive(false);
        ovrGazePointer.SetActive(false);

        this.gState = gameState.running;
    }


    /**
     * Update is called once per frame.     
     */
    void Update()
    {
        // If the number of cycles is greater than the target, end the game.
        if (cycleCounter > cycles && gState == gameState.running)
        {
            this.gState = gameState.gameOver;
            gameOver = true;
        }
    }

    /**
     * FixedUpdate is called once per frame. Use FixedUpdate instead of Update to minimize the
     * ship's shaking when moving on exhale.
     */
    private void FixedUpdate()
    {
        // Once the player has completed the number of cycles, set gameOver to true and destroy all existing gameObjects.
        if (gState == gameState.gameOver)
        {
            GameOver();
        }

        // Otherwise, if the game is not over:
        if (gState == gameState.running)
        {
			// Unfreeze restrictions so that ship moves normally when not in collision mode.
			rocketBody.constraints = RigidbodyConstraints.None;
			rocketBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
			rocketBody.isKinematic = false;

			// Change rocket direction based on camera in VR.
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Vector3 cameraVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);

			// Accelerate rocket when player is exhaling or using upArrow input.
			// Only allow exhale for as long as previous cycle was inhaled as soon as diamonds spawn.
			if (exhalePhase && cameraBounds() && exhaleDuration <= tempInhale && breathGen.exhaleSpawned)
            {
                ExhaleControls(cameraVector);
            }

            // If the player has exhaled the max amount and is now out of fuel, reset the flags.
            if (exhaleDuration > tempInhale)
            {
                // Reset all of the flags.
                exhalePhase = false;
                inhalePhase = true;
                exhaleDuration = 0;
                inhaleDuration = 0;
                breakDuration = 0;
                exhaleIsOn = false;
                inhaleIsOn = false;
                breakIsOn = true;
            }

            // If the player is in the inhale phase and the fuel has been generated, start the cycle.
            if (inhalePhase && cameraBounds() && breathGen.inhaleSpawned)
            {
                InhaleControls();
            }

            // If the player is neither exhaling nor inhaling:
            if (!exhaleIsOn && !inhaleIsOn)
            {
                // If the player is using VR there will be no keys, so add logic here:
                if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.UpArrow))
                {
                    inhaleIsOn = false;
                    exhaleIsOn = false;
                    breakIsOn = true;

                    SteadyStateControls(cameraVector);
				}

                // Otherwise, just run the logic normally.
                SteadyStateControls(cameraVector);
			}

        }
    }

    /**
     * Receive data from the spirometer and use the breath data to determine the game state.
     * The digital spirometer is connected here.
     * @param: message - the data read in from the spirometer.
     */
    private void ReceiveSpirometerData(OscMessage message)
    {
        float breathVal = message.GetFloat(0);

        // Debugging purposes.
        Debug.Log(breathVal);

        // Turn on engine light if the spirometer is connected.
        if(breathVal > 0)
        {
            MeshRenderer engineRenderer = engineLight.GetComponent<MeshRenderer>();
            Material newColor = (Material)Resources.Load("Materials-SQ/Green - SQ", typeof(Material));
            engineRenderer.material = newColor;
        }
        // Turn engine light off if spirometer is not connected.
        else
        {
            MeshRenderer engineRenderer = engineLight.GetComponent<MeshRenderer>();
            Material newColor = (Material)Resources.Load("Materials-SQ/Red - SQ", typeof(Material));
            engineRenderer.material = newColor;
        }

        // Determine the state based on the spirometer data.
        if (breathVal >= exhaleThresh)
        {
            exhaleIsOn = true;
            inhaleIsOn = false;
            breakIsOn = false;
        }
        if (breathVal < exhaleThresh && breathVal > inhaleTresh)
        {
            exhaleIsOn = false;
            inhaleIsOn = false;
            breakIsOn = true;
        }
        if (breathVal <= inhaleTresh)
        {
            inhaleIsOn = true;
            exhaleIsOn = false;
            breakIsOn = false;
        }
    }

    /**
      * Determine the correct action when the space ship collides with objects. Each object has a unique
      * collision action based on its tag.
      * @param: other - the other object colliding with the ship.
      */
    private void OnTriggerEnter(Collider other)
    {
        // If it collides with a diamond.
        if (other.gameObject.CompareTag("Diamond") || other.gameObject.CompareTag("Diamond Two"))
        {
            // Add constraints so that ship does not float randomly on collision
            rocketBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
			rocketBody.isKinematic = true;
			transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            //Destroy(other.gameObject.);
            Destroy(other.gameObject);
            audio.PlayOneShot(diamond, 5f);

            // Create mini diamonds for score UI. See DiamondController for controller script.
            Instantiate(miniDiamond, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), Quaternion.Euler(90, 180, 0));

            // Update all instances of diamondScore so there is data consistency
            finalScores.diamondScore += 1;
        }
        // Destroy rigidBody within Diamonds so as not to cause physics issues.
        else if (other.gameObject.CompareTag("Asteroid Destroyer"))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Engine Light"))
        {
            // Do nothing on collision with Engine Light.
        }
        // If it collides with any other object.
        else
        {
			// Add constraints so that ship does not float randomly on collision
			rocketBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
			rocketBody.isKinematic = true;
			transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            Destroy(other.gameObject);
            audio.PlayOneShot(crash, 0.5f);

            // Blink ship on crash.
            StartCoroutine(BlinkTime(2f));

            // Create mini diamonds for score UI. See DiamondFall for controller script.
            GameObject board = GameObject.FindGameObjectWithTag("Diamond Score");

            // Do not let score be negative.
            if (diamondScores.diamondScore >= 1)
            {
                Instantiate(miniDiamondTwo, new Vector3(board.transform.position.x, board.transform.position.y - 3, board.transform.position.z), Quaternion.Euler(90, 180, 0));
            }
        } 
    }

    /**
     * If the ship collides with certain objects, start a coroutine that blinks the ship on
     * and off to make it seem like a collision occurred. This will also be related to point loss.
     * @param: blinkDuration - the amount of time the ship will blink for in seconds.
     */
    private IEnumerator BlinkTime(float blinkDuration)
    {
        float timeCounter = 0;
        MeshRenderer engineRenderer = engineLight.GetComponent<MeshRenderer>();
        while (timeCounter < blinkDuration)
        {
            // make the rocket blink off and on.
            gameRocket.enabled = !gameRocket.enabled;
            engineRenderer.enabled = !engineRenderer.enabled;
            //wait 0.3 seconds per interval
            yield return new WaitForSeconds(0.3f);
            timeCounter += (1f / 3f);
        }
        gameRocket.enabled = true;
        engineRenderer.enabled = true;
    }

    /**
     * In order to guide the player, the ship will only accelerate when the player is looking in the
     * forward direction. If the player is looking outside of the bounds, the ship will not move when the
     * player exhales or inhales.
     * @return a boolean if the ship direction is in or out of bounds.
     */
    private bool cameraBounds()
	{
		if ((transform.rotation.eulerAngles.y >= 0 && transform.rotation.eulerAngles.y <= 60) || (transform.rotation.eulerAngles.y >= 300 && transform.rotation.eulerAngles.y <= 360))
        {
			inBounds = true;
		}
		else
		{
			inBounds = false;
		}
        return inBounds;
	}

    /**
     * Once the player completes the therapy cycles, set the game state to GameOver and destroy
     * all existing objects in the game. Turn off the out of bounds indicator and make the leaderboard
     * appear. The leaderboard will then be updated based on the player's score.
     */
    private void GameOver()
	{
        // Display the leader board.
        leaderBoard.SetActive(true);
        ovrGazePointer.SetActive(true);

        // Debugging purposes.
        if (sqLeaderBoard.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
        if (sqLeaderBoard.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");

        // Add the player's score to the leaderboard.
        sqLeaderBoard.AddScore(userName, (int)(100 * diamondScores.diamondScore / diamondScores.totalDiamonds));

        // Destroy all remaining objects in the game.
        Destroy(GameObject.FindGameObjectWithTag("Right Fuel"));
        Destroy(GameObject.FindGameObjectWithTag("Left Fuel"));
        Destroy(GameObject.FindGameObjectWithTag("Middle Fuel"));
        Destroy(GameObject.FindGameObjectWithTag("Fuel"));

        List<dreamloLeaderBoard.Score> scoreList = sqLeaderBoard.ToListHighToLow();

        if (scoreList == null)
        {
            Debug.Log("(loading...)");
        }
        // Display the top scores on the leaderboard.
        else
        {
            int maxToDisplay = 6;
            int count = 0;
            foreach (dreamloLeaderBoard.Score currentScore in scoreList)
            {
                count++;
                // Update the name, score, and rank list.
                topRankList.text += count + "\n";
                topScoreList.text += currentScore.score.ToString() + "%\n";
                topNameList.text += currentScore.playerName.Replace("+", " ") + "\n";

                if (count >= maxToDisplay) break;
            }

            if (count > 0) { this.gState = gameState.waiting; }
        }
    }

    /**
     * If the player is exhaling or pressing the up arrow, then accelerate the ship forwards through space.
     * The player will push the spaceship forward for as long as they exhale.
     * @param: cameraVector - Vector3 with the camera's direction
     */
    private void ExhaleControls(Vector3 cameraVector)
	{
        if (exhaleIsOn || Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                exhaleIsOn = true;
                breakIsOn = false;
            }
            // reset inhaleDuration timer.
            inhaleDuration = 0;
            breakDuration = 0;
            // Start timer to determine how long the breath is exhaled.
            downTime = Time.time;
            // Use transform.translate so that space ship does not stop on collisions.
            transform.Translate(new Vector3(cameraVector.x, 0, cameraVector.z) * speedMultiplier * Time.deltaTime);
            // Determine how long the exhale is or how long upArrow is being held down for.
            exhaleDuration = downTime - exhaleStart;
            // Start counting the break time
            breakStart = Time.time;
        }

        //TO ALLOW KEY BOARD PLAYABILITY, UNCOMMENT THE FUNCTION BELOW:
        KeyBoardExhale();
    }

    /**
     * If the player is inhaling or pressing the space bar, then collect fuel to
     * refuel the ship. 
     */
    private void InhaleControls()
	{
        // Pull fuel towards the rocket when inhaling or using Space key.
        if (inhaleIsOn || Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                inhaleIsOn = true;
                breakIsOn = false;
            }
            // reset exhaleDuration & break duration timer.
            exhaleDuration = 0;
            breakDuration = 0;
            // Start timer to determine how long the breath is inhaled.
            upTime = Time.time;
            // Determine how long inhale was held for.
            inhaleDuration = upTime - inhaleStart;
            // Start counting the break time
            breakStart = Time.time;
            // Set tempInhale to be the time inhaled.
            tempInhale = inhaleDuration;
        }

        //TO ALLOW KEY BOARD PLAYABILITY, UNCOMMENT THE FUNCTION BELOW:
        KeyBoardInhale();
    }

    /**
     * If the player is neither inhaling nor exhaling, then keep the ship steady.
     * Additionally, right after the exhale phase, negate the added force to stop the ship
     * and reset all of the flags.
     * @param: cameraVector - Vector3 with the camera's direction
     */
    private void SteadyStateControls(Vector3 cameraVector)
	{
        // Snapshot this time. This time will be compared with the amount of time exhale/inhale is held to
        // determine how long the inhale or exhale was.
        exhaleStart = Time.time;
        inhaleStart = Time.time;

        // Count how long the break is
        breakTime = Time.time;

        // Let the spaceship float for a duration of time after exhale to make it a less immediate
        // stop. Do not do this on start cycle.
        if (breakDuration <= 0.3f && exhaleDuration > 0)
        {
            rocketBody.AddRelativeForce(new Vector3(cameraVector.x, 0, cameraVector.z) * 4.5f, ForceMode.VelocityChange);
        }
        // Negate the force added to the rocket via exhalation.
        else
        {
            var oppositeDirX = -rocketBody.velocity;
            rocketBody.AddForce(oppositeDirX);
        }
        // Only count exhale and inhales that are longer than 0.3 second to remove erroneous air flow data.
        // Once inhale or exhale is conducted and completed, switch cycles.
        if (inhalePhase && inhaleDuration >= 0.3)
        {
            inhalePhase = false;
            exhalePhase = true;
        }
        if (exhalePhase && exhaleDuration >= 0.3)
        {
            inhalePhase = true;
            exhalePhase = false;
        }

        // Determine how long the break was
        breakDuration = breakTime - breakStart;
    }

    /**
     * Method that allows for manual (keyboard) play for the inhale cycle.
     */
    private void KeyBoardInhale()
	{
        if (!Input.GetKey(KeyCode.Space))
        {
            inhaleIsOn = false;
        }
    }

    /**
     * Method that allows for manual (keyboard) play for the exhale cycle.
     */
    private void KeyBoardExhale()
	{
        if (!Input.GetKey(KeyCode.UpArrow))
        {
            exhaleIsOn = false;
        }
    }
}


