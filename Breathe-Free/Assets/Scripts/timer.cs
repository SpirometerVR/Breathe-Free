using UnityEngine.UI;
using UnityEngine;

public class timer : MonoBehaviour
{
    Text text;
    string sec = "00";
    [SerializeField] float minutes=1;
    [SerializeField] float seconds=60;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "0" + minutes+1 + ":" + "00";

    }

    // Update is called once per frame
    void Update()
    {


        //time -= Time.deltaTime*speed;
        //string seconds = "00";   
        if (minutes > -1)
        {
            text.text = "0" + minutes + ":" + sec;
            seconds = seconds - Time.deltaTime*3;
            sec = Mathf.Ceil(seconds).ToString();
            if (seconds == 0)
            {
                sec = "00";
                minutes--;
                seconds = 59;
            }
        }
        
    }
}
