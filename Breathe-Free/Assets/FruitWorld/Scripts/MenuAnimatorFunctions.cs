using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimatorFunctions : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	public bool disableOnce;
	public int index;

	void PlaySound(AudioClip whichSound){
		if(!disableOnce && !testing.audio.isPlaying){
			menuButtonController.audioSource.PlayOneShot (whichSound);
		}else{
			disableOnce = false;
		}
	}
}	
