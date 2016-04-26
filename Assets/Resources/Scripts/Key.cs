using UnityEngine;
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
				Destroy (gameObject);
			} else {
				coll.gameObject.GetComponent<PlayerController> ().HasKey ();
				GameObject.Find ("Text").GetComponent<Text> ().text = "Get Key Found.\n Proceed East Gate!";
				Destroy (gameObject);
			}
		}
	}
}
