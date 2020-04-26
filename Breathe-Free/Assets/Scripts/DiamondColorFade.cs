using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondColorFade : MonoBehaviour
{
    private Color diamondColor;
    // Start is called before the first frame update
    void Start()
    {
        diamondColor = this.GetComponent<Renderer>().material.color;
        //diamondColor.a = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(diamondColor);
        while (diamondColor.a > 0)
        {
            Color tempColor = diamondColor;
            tempColor.a -= 0.01f;
            diamondColor = tempColor;
        }

    }
}
