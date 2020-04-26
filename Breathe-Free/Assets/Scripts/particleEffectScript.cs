using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem psRed;
    public ParticleSystem psYellow;
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        //ps.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("fruits"))
        {
            Debug.Log(collision.collider.GetComponent<Renderer>().material.name+ "name");
            if(collision.collider.gameObject.GetComponent<Renderer>().material.name=="redMat (Instance)")
            {
                Debug.Log("Red");
                Instantiate(psRed, collision.collider.transform.position, Quaternion.Euler(-90f, 0, 0));
                psRed.Play();
            }
            else if (collision.collider.gameObject.GetComponent<Renderer>().material.name == "yellowMat (Instance)")
            {

                Instantiate(psYellow, collision.collider.transform.position, Quaternion.Euler(-90f, 0, 0));
                psYellow.Play();
            }
            //ps.transform.position = collision.collider.transform.position;

            collision.collider.GetComponent<Rigidbody>().isKinematic = true;
            collision.collider.tag = "hunted";

        }



    }
}
