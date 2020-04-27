using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {

	// Use this for initialization
	public int index;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	public AudioSource audioSource;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
	}
	


}


