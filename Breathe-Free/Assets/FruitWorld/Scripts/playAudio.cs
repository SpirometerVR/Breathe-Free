using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour
{
    [SerializeField] public List<AudioSource> audio;
    public FruitWorldController m;
    public pauseMenu p;
    void FixedUpdate()
    {

        if (!audio[0].isPlaying)
        {
            audio[0].Play();
        }
        //Debug.Log(m.playPluck+"pluck");
        if (m.playPluck)
        {

            audio[0].volume = 0.5f;
            if (!audio[1].isPlaying)
            {
                audio[1].PlayOneShot(audio[1].clip);
                StartCoroutine(change());
            }

        }
    }
    IEnumerator change()
    {
        yield return new WaitForSeconds(audio[1].clip.length);

        m.playPluck = false;
        yield return new WaitForSeconds(0.5f);
        audio[0].volume = 1f;
    }

}