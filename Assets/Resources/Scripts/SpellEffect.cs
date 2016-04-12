using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	void Update() {
		Destroy(gameObject);
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.tag == "Enemy") {
			switch (name) {
				case "Blight":
					return;
				case "Root":
					return;
				case "Damage":
					return;
			}
		}
	}
}
