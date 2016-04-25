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

	void OnTriggerStay(Collider other){
		if (other.name == "Necromancer") {
			if (other.GetComponent<PlayerController> ().hasFortKey) {
				anim.SetTrigger ("Closed");
				Destroy (this.gameObject, .5f);
			}
		}
	}
}
