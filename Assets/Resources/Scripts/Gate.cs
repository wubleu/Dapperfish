using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
	public AudioClip open;
		
		// Use this for initialization
		void Start () {
		open = Resources.Load ("Sounds/gateOpen") as AudioClip;
		}

		// Update is called once per frame
		void Update () {

		}

		void OnCollisionEnter(Collision coll){
			if (coll.gameObject.name == "Necromancer") {
				if (coll.gameObject.GetComponent<PlayerController> ().hasKey) {
					Destroy (this.gameObject, 1);
				AudioSource.PlayClipAtPoint (open, GameObject.FindObjectOfType<PlayerController> ().transform.position);

				}
			}
		}
	}
