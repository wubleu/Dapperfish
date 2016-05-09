using UnityEngine;
using System.Collections;

public class Gate3 : MonoBehaviour {

	public GameManager Gman;
	public bool finalfight = false;
	public AudioClip open;
	// Use this for initialization
	void Start () {
		open = Resources.Load ("Sounds/gateOpen") as AudioClip;
	}

	// Update is called once per frame
	void Update () {
		if (Gman.dungeonKeys == 2 && !finalfight) {
			gameObject.SetActive (false);
			AudioSource.PlayClipAtPoint (open, GameObject.FindObjectOfType<PlayerController> ().transform.position);

		}
		if (finalfight) {
			gameObject.SetActive (true);
			Gman.dungeonKeys = 0;
		}
	}
}