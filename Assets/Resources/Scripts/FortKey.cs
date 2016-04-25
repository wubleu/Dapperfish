using UnityEngine;
using System.Collections;

public class FortKey : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "Necromancer") {
			if (other.GetComponent<PlayerController> ().hasFortKey) {
				anim.SetTrigger ("Close");
				Destroy (this.gameObject, 1);
			}
		}
	}
}
