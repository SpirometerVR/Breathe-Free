using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class testing : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    private Animator animator;
    private Animator lastAnimator;
    private MenuAnimatorFunctions animatorFunctions;
    public static AudioSource audio;
    private int inhaleAmount;
    private int exhaleAmount;
    private int numOfCycles;
    public int gameIndex = -1;



    [SerializeField] Animator MenuAnimator;


    private void Start()
    {
        lastAnimator = null;
        audio = GetComponent<AudioSource>();
        inhaleAmount = 1;
        exhaleAmount = 1;
        numOfCycles = 1;
    }

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity))
        {
            //print(hit.collider.name);
            if (hit.collider.tag=="button")
            {
                //Debug.Log("selected");
                animator = hit.collider.gameObject.GetComponent<Animator>();
                animatorFunctions = hit.collider.gameObject.GetComponent<MenuAnimatorFunctions>();
                if (lastAnimator && lastAnimator != animator)
                {
                    lastAnimator.SetBool("selected", false);
                }
                lastAnimator = animator;

                animator.SetBool("selected", true);
                if (Input.GetMouseButtonDown(0))
                {
                    animator.SetBool("pressedButton", true);
                    animator.SetTrigger("pressed");
                    

                    Debug.Log("pressed "+animatorFunctions.index);
                    if(animatorFunctions.index==0 || animatorFunctions.index == 1)
                    {
                        gameIndex = animatorFunctions.index;
                        StartCoroutine(toInputsMenu());
                    }
                    else if(animatorFunctions.index>=3 && animatorFunctions.index <= 8)
                    {
                        StartCoroutine(increment_decrement(animatorFunctions.index, hit.collider.gameObject));
                    }
                    else if(animatorFunctions.index == 9)
                    {
                        StartCoroutine(backToStartenu());
                    }
                    else if (animatorFunctions.index == 10)
                    {
                        StartCoroutine(startGame());
                    }
                    else
                    {
                        Debug.Log("Quit Game");
                        //StartCoroutine(quit());                     
                    }
                }
            }
            else
            {
                //Debug.Log("not selected");
                animator.SetBool("selected", false);

            }
        }
        else
        {
            //Debug.Log("not selected");
            if (animator)
            {
                animator.SetBool("selected", false);

            }
        }

        if (animator && animator.GetBool("pressedButton"))
        {
            animator.SetBool("pressedButton", false);
            animatorFunctions.disableOnce = true;
        }
    }

    IEnumerator toInputsMenu()
    {
        audio.PlayOneShot(audio.clip);
        yield return new WaitForSeconds(0.1f);
        MenuAnimator.SetTrigger("toInputMenu");
    }
    IEnumerator startGame()
    {
        audio.PlayOneShot(audio.clip);
        yield return new WaitForSeconds(1f);
        if (gameIndex == 0)
        {
            SceneManager.LoadScene("FruitWorld");

        }
        else if (gameIndex == 1)
        {
            SceneManager.LoadScene("SpaceQuest");
        }
    }
    IEnumerator backToStartenu()
    {
        audio.PlayOneShot(audio.clip);
        yield return new WaitForSeconds(0.1f);
        MenuAnimator.SetTrigger("toStartMenu");

    }
    IEnumerator increment_decrement(int index, GameObject go)
    {
        print(go.transform.parent.GetChild(2));
        yield return new WaitForSeconds(0);
        if(index==3)
        {
            if (inhaleAmount > 1)
            {
                inhaleAmount -= 1;
            }
            go.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = inhaleAmount.ToString();

        }
        else if (index == 5)
        {
            if(exhaleAmount > 1)
            {
                exhaleAmount -= 1;
            }
            go.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = exhaleAmount.ToString();

        }
        else if (index == 7)
        {
            if (numOfCycles > 1)
            {
                numOfCycles -= 1;
            }
            go.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = numOfCycles.ToString();

        }
        else if (index == 4)
        {
            if (inhaleAmount <= 7)
            {
                inhaleAmount += 1;
            }
            go.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = inhaleAmount.ToString();

        }
        else if (index == 6)
        {
            if (exhaleAmount <= 7)
            {
                exhaleAmount += 1;
            }
            go.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = exhaleAmount.ToString();

        }
        else if (index == 8)
        {
            if (numOfCycles <= 9)
            {
                numOfCycles += 1;
            }
            go.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = numOfCycles.ToString();

        }
    }
    IEnumerator quit()
    {
        audio.PlayOneShot(audio.clip);
        yield return new WaitForSeconds(0.1f);
        Application.Quit();
    }
}