using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class mechanics : MonoBehaviour
{

    public float flag = 0;
    public GameObject flowerLight;
    public bool playPluck = false;                  // wether or not to play the pluck sound
    public List<GameObject> stones;                 // all the stones
    public List<GameObject> fru;                    // all the fruits
    public static int inhaleTime = 3;
    public static int exhaleTime = 3;
    public static int numOfCycles = 1;
    public static string userName;
    public int cycleCounter = 0;
    public bool gameOver = false;
    public int score = 0;

    // For scoreboard;
    public GameObject leader;
    dreamloLeaderBoard fwLeaderBoard;
    private bool topScoresReceived = false;
    private Text topNameList;
    private Text topScoreList;
    private Text topRankList;

    private bool stoneHandUpdate;                   // aids in calculating the distance of stone-hand just once.
    private bool stoneFruitUpdate;                  // aids in calculating the distance of stone-fruit just once.
    private bool isMovingTowardsPlayer = false;                   // can the stone move from its position towards the hand.
    private bool canShoot = false;                  // determines wether or not the stone can be launched based on its position
    private bool canSummon = true;                  // can summon the stone or not

    private int count = 0;                          // to keep track of stones count
    private int fruitCount = 0;                     // to keep track of furits count
    private ParabolaController cont;
    private OSC oscScript;
    private GameObject oscGameObject;

    private select s;
    private float stoneHandDistance;                // for distance between stone and hand
    private float stoneFruitDistance;               // for distance between stone and fruit

    [SerializeField] private GameObject CanvasText;
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private GameObject FinalScoreText;
    [SerializeField] private List<GameObject> vfx;  // array of particle system attached to stone
    [SerializeField] private GameObject sel;
    [SerializeField] private GameObject myCamera;

    Coroutine coroutineInhale, coroutineExhale;

    private void Awake()
    {
    }
    void Start()
    {

        gameOver = false;

        oscGameObject = GameObject.Find("OSC");
        oscScript = oscGameObject.GetComponent<OSC>();
        oscScript.SetAddressHandler("/Spirometer/C", BreathData);
        s = sel.GetComponent<select>();

        //inhaleTime = 3;
        //exhaleTime = 3;
        //numOfCycles = 5;
        cycleCounter = 0;

        stoneHandUpdate = true;
        stoneFruitUpdate = true;

        this.fwLeaderBoard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();

        topNameList = GameObject.Find("Top Names List").GetComponent<Text>();
        topScoreList = GameObject.Find("Top Scores List").GetComponent<Text>();
        topRankList = GameObject.Find("Top Ranks List").GetComponent<Text>();


        stoneHandDistance = 0;
        stoneFruitDistance = 0;

        CanvasText.GetComponent<Text>().text = "Inhale Target Time: " + inhaleTime.ToString();
        ScoreText.GetComponent<Text>().text = "Points: " + score.ToString();



    }


    void BreathData(OscMessage message)
    {
        float breath_value = message.GetFloat(0);
        Debug.Log(breath_value + " breath");
        // Turn flower light off if spirometer is not connected.
        if (breath_value > 0)
        {
            MeshRenderer flowerLightRend = flowerLight.GetComponent<MeshRenderer>();
            Material newColor = (Material)Resources.Load("Materials-SQ/Green - SQ", typeof(Material));
            flowerLightRend.material = newColor;
        }
        // Turn flower light off if spirometer is not connected.
        else
        {
            MeshRenderer flowerLightRend = flowerLight.GetComponent<MeshRenderer>();
            Material newColor = (Material)Resources.Load("Materials-SQ/Red - SQ", typeof(Material));
            flowerLightRend.material = newColor;
        }
        if (breath_value <= 1170)
        {
            flag = 1;
        }
        else if (breath_value >= 1480)
        {
            flag = 3;
        }
        else
        {
            flag = 2;
        }
    }



    void Update()
    {
        OVRInput.Update();

        ScoreText.GetComponent<Text>().text = "Cycles Left: " + (numOfCycles - cycleCounter).ToString();

        if (cycleCounter == numOfCycles)
		{
            gameOver = true;
		}

        if (!gameOver)
        {
            leader.SetActive(false);
            // inhale
            if (canSummon && Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || flag == 1)
            {
                if (count == stones.Count)
                {
                    Debug.Log("No more stones left");
                }
                else
                {
                    cont = stones[count].GetComponent<ParabolaController>();
                    isMovingTowardsPlayer = true;
                    //vfx[count].transform.GetChild(0).gameObject.SetActive(false);

                    if (stoneHandUpdate)
                    {
                        CanvasText.GetComponent<Text>().text = "Inhale Target Time: " + inhaleTime.ToString();
                        coroutineInhale = StartCoroutine(countDownInhale(inhaleTime));

                        stoneHandDistance = Vector3.Distance(stones[count].transform.position, transform.position);
                        stoneHandDistance -= 0.4f;
                        stoneHandUpdate = false;
                    }
                    if (Vector3.Distance(stones[count].transform.position, transform.position) > 0.45f)
                    {
                        stones[count].transform.position = Vector3.MoveTowards(stones[count].transform.position, transform.position + transform.forward * 0.4f - transform.up * 0.1f, Time.deltaTime * (stoneHandDistance / inhaleTime));

                    }

                }

            }

            // called if player stops inhaling before stone reaches the player

            //else if (move && flag!=1 && Vector3.Distance(stones[count].transform.position, transform.position) > 0.2f)
            else if (isMovingTowardsPlayer && !Input.GetKey(KeyCode.Space) && Vector3.Distance(stones[count].transform.position, transform.position) > 0.45f)
            {
                stones[count].GetComponent<Rigidbody>().useGravity = true;
                StopCoroutine(coroutineInhale);
                //vfx[count].SetActive(false);
                count++;
                isMovingTowardsPlayer = false;

                stoneHandUpdate = true;

                cycleCounter++;
            }


            // for the stone to always be in front of the camera
            if (Vector3.Distance(stones[count].transform.position, this.transform.position) <= 0.45f)
            {
                stones[count].transform.position = transform.position + transform.forward * 0.4f - transform.up * 0.1f;

                //stones[count].transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
            }


            // When stone has arrived
            if ((Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.RawButton.A) || flag == 3) && Vector3.Distance(stones[count].transform.position, this.transform.position) <= 0.45f && fruitCount < fru.Count && s.stay)
            {

                vfx[count].SetActive(true);
                //vfx[count].transform.GetChild(0).gameObject.SetActive(true);

                isMovingTowardsPlayer = false;
                canShoot = true;
                canSummon = false;

                GameObject.Find("Trails").GetComponent<ParticleSystem>().Play();
                GameObject point1 = new GameObject();
                GameObject point2 = new GameObject();
                GameObject point3 = new GameObject();
                GameObject root = new GameObject();
                point1.name = "child1";
                point2.name = "child2";
                point3.name = "child3";
                root.name = "parent";
                point1.transform.parent = root.transform;
                point2.transform.parent = root.transform;
                point3.transform.parent = root.transform;
                point1.transform.position = stones[count].transform.position;
                point3.transform.position = s.go.transform.position;
                point2.transform.position = new Vector3((point1.transform.position.x + point3.transform.position.x) / 2, point3.transform.position.y + 0.3f, (point1.transform.position.z + point3.transform.position.z) / 2);



                if (!cont.enabled)
                {
                    cont.enabled = true;
                }
                if (stoneFruitUpdate)
                {
                    CanvasText.GetComponent<Text>().text = "Exhale Target Time: " + exhaleTime.ToString();
                    coroutineExhale = StartCoroutine(countDownExhale(exhaleTime));

                    stoneFruitDistance = Vector3.Distance(stones[count].transform.position, s.go.transform.position);
                    stoneFruitDistance -= 1f;
                    cont.Speed = stoneFruitDistance / exhaleTime;

                    stoneFruitUpdate = false;
                }
                cont.ParabolaRoot = root;
                cont.Autostart = true;
                cont.Animation = true;
                cont.Speed = stoneFruitDistance / exhaleTime;
            }


			// when player stops exhaling before stone hits the fruit
			//else if (canShoot && flag!=3 && Vector3.Distance(stones[count].transform.position, transform.position) >= 1f)
			// For keyboard playability, uncomment else if below and comment out the else if on line 221.
			//else if (canShoot && !Input.GetKey(KeyCode.D) && Vector3.Distance(stones[count].transform.position, transform.position) >= 1f)
			else if (canShoot && flag != 3 && Vector3.Distance(stones[count].transform.position, transform.position) >= 1f)
					{
                stones[count].GetComponent<Rigidbody>().useGravity = true;
                vfx[count].SetActive(false);
                count++;
                StopCoroutine(coroutineExhale);


                cont.enabled = false;
                canSummon = true;
                canShoot = false;
                stoneHandUpdate = true;
                stoneFruitUpdate = true;

                cycleCounter++;

            }


            //When stone has hit the fruit
            if (s.go && count < stones.Count && stones[count] && Vector3.Distance(stones[count].transform.position, s.go.transform.position) < 1f && fruitCount < fru.Count)
            {
                var particleEffect = stones[count].transform.GetChild(0);
                stones[count].transform.GetChild(0).transform.parent = null;

                stones[count].GetComponent<Rigidbody>().useGravity = true;
                playPluck = true;
                CanvasText.GetComponent<Text>().text = "Inhale Target Time: " + inhaleTime.ToString();
                Destroy(stones[count]);
                //toDestory.Add(vfx[count]);
                for (int i = 0; i < 3; i++)
                {
                    GameObject.Find("stoneVFX").transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                }
                StartCoroutine(destory(vfx[count]));
                count++;
                s.go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                s.go.GetComponent<Rigidbody>().isKinematic = false;
                s.go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                s.go.GetComponent<Rigidbody>().useGravity = true;

                ParticleSystem points = s.go.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
                points.Play();

                score += 5;

                fruitCount++;

                canShoot = false;
                canSummon = true;
                stoneHandUpdate = true;
                stoneFruitUpdate = true;

                cycleCounter++;


            }
        }
		else
        {
            ScoreText.GetComponent<Text>().text = "";
            CanvasText.GetComponent<Text>().text = "";
            FinalScoreText.GetComponent<Text>().text = "Final Score: " + score + "/" + (numOfCycles * 5);

            leader.SetActive(true);

            if (fwLeaderBoard.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
            if (fwLeaderBoard.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");

            fwLeaderBoard.AddScore(userName, (int)(100 * score / (numOfCycles * 5)));

            List<dreamloLeaderBoard.Score> scoreList = fwLeaderBoard.ToListHighToLow();

            if (scoreList == null)
            {
                Debug.Log("(loading...)");
            }
            else
            {
                int maxToDisplay = 6;
                int countScr = 0;
                foreach (dreamloLeaderBoard.Score currentScore in scoreList)
                {
                    countScr++;

                    //Debug.Log(currentScore.score.ToString());
                    topRankList.text += countScr + "\n";
                    topScoreList.text += currentScore.score.ToString() + "%\n";
                    topNameList.text += currentScore.playerName.Replace("+", " ") + "\n";

                    if (countScr >= maxToDisplay) break;
                }
            }
        }
    }
    IEnumerator destory(GameObject go)
    {
        yield return new WaitForSeconds(3);
        Destroy(go);
    }


    IEnumerator countDownInhale(float startVal)
    {
        while (startVal > 0)
        {
            yield return new WaitForSeconds(1.0f);
            startVal--;
            CanvasText.GetComponent<Text>().text = "Inhale Target Time: " + startVal.ToString();
        }

    }

    IEnumerator countDownExhale(float startVal)
    {
        while (startVal > 0)
        {
            yield return new WaitForSeconds(1.0f);
            startVal--;
            CanvasText.GetComponent<Text>().text = "Exhale Target Time: " + startVal.ToString();
        }

    }

}