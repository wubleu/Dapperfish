using UnityEngine;
using System.Collections;

public class Gate2 : MonoBehaviour {

	public GameManager Gman;
	public AudioClip open;

	// Use this for initialization
	void Start () {
		open = Resources.Load ("Sounds/gateOpen") as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
		if (Gman.waveclear) {
			Destroy (this.gameObject);
			AudioSource.PlayClipAtPoint (open, GameObject.FindObjectOfType<PlayerController> ().transform.position);

		}
	}
}
