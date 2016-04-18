using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.name = "key";
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.name == "Necromancer") {
			coll.gameObject.GetComponent<PlayerController> ().HasKey ();
			Destroy (gameObject);
		}
	}
}
