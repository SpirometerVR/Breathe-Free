using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class mechanics : MonoBehaviour
{

    public float flag = 0;
    public bool playPluck = false;                  // wether or not to play the pluck sound
    public List<GameObject> stones;                 // all the stones
    public List<GameObject> fru;                    // all the fruits
    public static int inhaleTime;
    public static int exhaleTime;
    public static int numOfCycles;


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
    [SerializeField] private List<GameObject> vfx;  // array of particle system attached to stone
    [SerializeField] private GameObject sel;

    Coroutine coroutineInhale, coroutineExhale;

    private void Awake()
    {
    }
    void Start()
    {

        oscGameObject = GameObject.Find("OSC");
        oscScript = oscGameObject.GetComponent<OSC>();
        oscScript.SetAddressHandler("/Spirometer/C", BreathData);
        s = sel.GetComponent<select>();

        inhaleTime = 5;
        exhaleTime = 3;

        stoneHandUpdate = true;
        stoneFruitUpdate = true;


        stoneHandDistance = 0;
        stoneFruitDistance = 0;

        CanvasText.GetComponent<Text>().text = inhaleTime.ToString();
        


    }


    void BreathData(OscMessage message)
    {
        float breath_value = message.GetFloat(0);
        Debug.Log(breath_value + " breath");
        if (breath_value >= 2600)
        {
            flag = 1;
        }
        else if (breath_value < 2600 && breath_value >= 1300)
        {
            flag = 2;
        }
        else
        {
            flag = 3;
        }
    }



    void Update()
    {
        OVRInput.Update();

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
                vfx[count].SetActive(true);
                vfx[count].transform.GetChild(0).gameObject.SetActive(false);

                if (stoneHandUpdate)
                {
                    CanvasText.GetComponent<Text>().text = inhaleTime.ToString();
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
            vfx[count].SetActive(false);
            count++;
            isMovingTowardsPlayer = false;
            
            stoneHandUpdate = true;
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
            

            vfx[count].transform.GetChild(0).gameObject.SetActive(true);

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
                CanvasText.GetComponent<Text>().text = exhaleTime.ToString();
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
        else if (canShoot && !Input.GetKey(KeyCode.D) && Vector3.Distance(stones[count].transform.position, transform.position) >= 1f)
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

        }


        //When stone has hit the fruit
        if (count < stones.Count && stones[count] && Vector3.Distance(stones[count].transform.position, s.go.transform.position) < 1f && fruitCount < fru.Count)
        {
            var particleEffect = stones[count].transform.GetChild(0);
            stones[count].transform.GetChild(0).transform.parent = null;

            stones[count].GetComponent<Rigidbody>().useGravity = true;
            playPluck = true;
            Destroy(stones[count]);
            //toDestory.Add(vfx[count]);
            for (int i = 0; i < 3; i++)
            {
                GameObject.Find("stoneVFX").transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
            }
            StartCoroutine(destory(vfx[count]));
            count++;
            s.go.GetComponent<Rigidbody>().useGravity = true;
            fruitCount++;

            canShoot = false;
            canSummon = true;
            stoneHandUpdate = true;
            stoneFruitUpdate = true;


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
            CanvasText.GetComponent<Text>().text = startVal.ToString();
        }

    }

    IEnumerator countDownExhale(float startVal)
    {
        while (startVal > 0)
        {
            yield return new WaitForSeconds(1.0f);
            startVal--;
            CanvasText.GetComponent<Text>().text = startVal.ToString();
        }

    }

}
