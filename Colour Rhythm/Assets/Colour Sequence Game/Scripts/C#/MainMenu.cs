using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lose : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip buttonClickSound;

	// Use this for initialization
	void Start () {
        audioSource = GameObject.Find("Camera").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	//void Update ()
    
	
    public void ButtonSound() //When called, a button click sound will be played through the audioSource variable component
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

}

