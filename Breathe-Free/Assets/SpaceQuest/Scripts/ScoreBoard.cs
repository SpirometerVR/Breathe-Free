using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public float diamondScore;
    public float totalDiamonds;

    private RocketController player;

    private Text inhaleScore;
    private Text exhaleScore;
    private Text finalScore;

    private ScoreBoard exhaleScoreCard;
    private ScoreBoard inhaleScoreCard;
    private ScoreBoard finalScoreCard;

    // Start is called before the first frame update
    void Start()
    {
        // Find all of the score boards under the Canvas object.
        exhaleScore = GameObject.FindGameObjectWithTag("Diamond Score").GetComponent<Text>();
        finalScore = GameObject.FindGameObjectWithTag("Final Score").GetComponent<Text>();

        // Find the player.
        player = GameObject.FindGameObjectWithTag("Rocket").GetComponent<RocketController>();

        // Initialize scoreboard objects.
        exhaleScoreCard = GameObject.FindGameObjectWithTag("Diamond Score").GetComponent<ScoreBoard>();
        finalScoreCard = GameObject.FindGameObjectWithTag("Final Score").GetComponent<ScoreBoard>();

        // Set the target score based on the exhale cycles.
        totalDiamonds = RocketController.exhaleTargetTime * RocketController.cycles;
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is over, print the final score alone.
        if (player.gameOver)
        {
            finalScore.text = "Final Score: " + (diamondScore) + "/" + (totalDiamonds);
            exhaleScore.text = "";
        }

        // Otherwise, print the current score.
        else
        {
            finalScore.text = "";
            exhaleScore.text = "Diamonds: " + diamondScore;
        }
    }
}
