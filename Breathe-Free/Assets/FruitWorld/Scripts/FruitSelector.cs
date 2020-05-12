using UnityEngine;
using System.Collections.Generic;

public class FruitSelector : MonoBehaviour
{

    public bool stay = false;
    public GameObject go;

    private GameObject previousGo;
    private float fruitDistance = Mathf.Infinity;
    private Vector3 tempDir;
    private List<Collider> triggerList;


    [SerializeField] private FruitWorldController mechanics;

    Vector3 dir;

    /**
     * Start is called once before the first frame.
     */
    void Start()
    {
        triggerList = new List<Collider>();
        previousGo = null;
        go = null;
        tempDir = new Vector3(0, 1, 0);
    }

    /**
     * Update is called once per frame.
     */
    void Update()
    {
        // Use a raycast based on the camera's direction to target the fruits.
        dir = transform.forward;
        Ray ray = new Ray(transform.position, dir);

        if (tempDir != ray.direction)
        {
            fruitDistance = Mathf.Infinity;
        }
        tempDir = ray.direction;
        foreach (Collider targetFruit in triggerList)
        {
            if (targetFruit != null)
            {
                float dist = Vector3.Cross(ray.direction, targetFruit.gameObject.transform.position - ray.origin).magnitude;
                if (dist < fruitDistance)
                {
                    fruitDistance = dist;
                    go = targetFruit.gameObject;
                    if (previousGo)
                    {
                        previousGo.transform.GetChild(0).gameObject.SetActive(false);

                    }
                    go.transform.GetChild(0).gameObject.SetActive(true);
                    previousGo = go;
                    Debug.Log(dist + " from " + go);

                }
            }

        }

        if (go != null)
        {
            Debug.DrawLine(ray.origin, go.transform.position, Color.red, 2f);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
        }
	}

    /**
     * Determine the actions on collision. The stones will collide with pebbles
     * and initially, the fruit will stay at its position and be added to the trigger list.
     * @param other - the other collision object.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fruits"))
        {
            stay = true;
            if (!triggerList.Contains(other))
            {
                triggerList.Add(other);
            }
        }
    }

    /**
     * After being added to the trigger list, the fruit will no longer glow and will fall from
     * the tree.
     * @param: other - the other object in collision.
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("fruits"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);       //glow of the fruit
            stay = false;
            if (triggerList.Contains(other))
            {
                triggerList.Remove(other);
            }
        }
    }
}
