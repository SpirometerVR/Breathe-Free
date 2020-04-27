using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour
{
    [SerializeField] public List<AudioSource> audio;
    private mechanics m;
    private pauseMenu p;

    // Start is called before the first frame update
    void Start()
    {
        //audio = GetComponent<AudioSource>();
        //audio = GetComponent<AudioSource>();
        //audio.Play();   
        m = GameObject.Find("hand").GetComponent<mechanics>();

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!audio[0].isPlaying)
        {
            audio[0].Play();           
        }
        //Debug.Log(m.playPluck+"pluck");
        if(m.playPluck)
        {
            
            audio[0].volume = 0.5f;
            if(!audio[1].isPlaying)
            {
                audio[1].PlayOneShot(audio[1].clip);
                StartCoroutine(change());
            }
                      
        }
        Debug.Log(pauseMenu.isPaused);
    }
    IEnumerator change()
    {
        yield return new WaitForSeconds(audio[1].clip.length);
        
        m.playPluck = false;
        yield return new WaitForSeconds(0.5f);
        audio[0].volume = 1f;
    }

}
