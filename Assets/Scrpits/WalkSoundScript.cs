using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip WalkSound;
    private AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D))
	    {
	    	if(!audio.isPlaying)
	    	{
	    		audio.clip = WalkSound;
	    		audio.Play();
	    	}
	    }
	    else
	    {
	    	audio.Stop();			
	    }
    }
}
