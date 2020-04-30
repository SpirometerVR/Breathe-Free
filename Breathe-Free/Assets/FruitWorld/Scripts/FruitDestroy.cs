using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 22.5)
		{
            Destroy(this.gameObject);
		}
        
    }
}
