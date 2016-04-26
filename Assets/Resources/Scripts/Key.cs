﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.name = "key";
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.name == "Necromancer") {
			if (transform.position.z<-25) {
				coll.gameObject.GetComponent<PlayerController> ().hasFortKey = true;
				GameObject.Find ("Text").GetComponent<Text> ().text = "Fort Key Found.\n Get East Gate Key From Fort";
				Destroy (gameObject);
			} else {
				coll.gameObject.GetComponent<PlayerController> ().HasKey ();
				GameObject.Find ("Text").GetComponent<Text> ().text = "Gate Key Found.\n Proceed East Gate!\n Stand by gate 3 seconds to unlock.";
				Destroy (gameObject);
			}
		}
	}
}
