using UnityEngine;
using System.Collections;

public class DungeonKey : MonoBehaviour {

	public GameManager Gman;
	AudioClip keyGrab;


	// Use this for initialization
	void Start () {
		keyGrab = Resources.Load ("Sounds/keyGrab") as AudioClip;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.name == "Necromancer") {
			AudioSource.PlayClipAtPoint (keyGrab, transform.position);
			Gman.dungeonKeys = Gman.dungeonKeys + 1;
			Destroy (gameObject);
		}
	}
}
