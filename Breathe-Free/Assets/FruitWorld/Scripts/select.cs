using UnityEngine;
using System.Collections;

public class select : MonoBehaviour
{
    //public Transform target;
    Vector3 screenPos;
    Camera cam;
    public bool stay = false;
    public GameObject go;


    void Start()
    {
        //cam = Camera.main.GetComponent<Camera>();
        //screenPos = cam.WorldToScreenPoint(target.position);
        //Debug.Log("target is " + screenPos.x + " pixels from the left");
        //Debug.Log(Screen.width/2);
        //Debug.Log(Screen.width*3 / 4);
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fruits"))
        {
            stay = true;
            go = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("fruits"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);           
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("fruits"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            stay = false;
        }
    }
}
   