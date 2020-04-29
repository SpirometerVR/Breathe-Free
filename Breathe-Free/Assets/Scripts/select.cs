using UnityEngine;
using System.Collections.Generic;

public class select : MonoBehaviour
{

    public bool stay = false;
    public GameObject go;

    private GameObject previousGo;
    private List<GameObject> fruits;
    private float fruitDistance = Mathf.Infinity;
    private Vector3 tempDir;


    [SerializeField] private GameObject myCamera;
    [SerializeField] private mechanics mechanics;
  
    Vector3 dir;
    Vector3 screenPos;
    Camera cam;



    void Start()
    {
        fruits = mechanics.fru;
        //cam = Camera.main.GetComponent<Camera>();
        //screenPos = cam.WorldToScreenPoint(target.position);
        //Debug.Log("target is " + screenPos.x + " pixels from the left");
        //Debug.Log(Screen.width/2);
        //Debug.Log(Screen.width*3 / 4);
        previousGo = null;
        go = null;
        tempDir = new Vector3(0, 1, 0);
    }

    void Update()
    {
        //dir = transform.forward;
        //Ray ray = new Ray(transform.position, dir);

        //if (tempDir != ray.direction)
        //{
        //    fruitDistance = Mathf.Infinity;
        //}
        //tempDir = ray.direction;
        //foreach (GameObject targetFruit in fruits)
        //{
            
            
        //    float dist = Vector3.Cross(ray.direction, targetFruit.transform.position - ray.origin).magnitude;
        //    if (dist < fruitDistance)
        //    {
        //        fruitDistance = dist;
        //        go = targetFruit;
        //        if (previousGo)
        //        {
        //            previousGo.transform.GetChild(0).gameObject.SetActive(false);

        //        }
        //        go.transform.GetChild(0).gameObject.SetActive(true);
        //        previousGo = go;
        //        Debug.Log(dist + "yoooo" + go);

        //    }
            
        //}
    

        //Debug.DrawLine(ray.origin, myCamera.transform.position, Color.red, 2f);
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
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
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);         // glow of the fruit  
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("fruits"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);       //glow of the fruit
            stay = false;
        }
    }
}
   