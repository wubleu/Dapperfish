using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Key : MonoBehaviour {

	AudioClip keyGrab;
	PlayerController p;
	bool made;
	// Use this for initialization
	void Start () {
		made = false;
	}

	void Update(){
		if (!made) {
			made = true;
			this.name = "key";
			keyGrab = Resources.Load ("Sounds/keyGrab") as AudioClip;
			p = GameObject.Find("Necromancer").GetComponent<PlayerController>();
		}
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.name == "Necromancer") {
			coll.GetComponent<PlayerController> ().HasKey ();
			if (GameObject.FindObjectOfType<GameManager> ().level == 1) {
				GameObject.FindObjectOfType<GameManager>().checkpoint = true;
			}
			AudioSource.PlayClipAtPoint (keyGrab, transform.position);
			foreach (KeyInfo k in p.gManager.keys) {
				print (k.location);
				print (transform.position);
				if (k.location == transform.position) {
					foreach (Link l in p.gManager.links) {
						if (l.name == k.name) {
							l.unlocked = true;
							print ("here");
						}
					}
				}
			}
			Destroy (gameObject);
		}
	}
}
