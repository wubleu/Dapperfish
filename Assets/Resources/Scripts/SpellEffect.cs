using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	int frame;

	void Start() {
		frame = Time.frameCount;
	}

	void Update() {
		if (frame != Time.frameCount) {
			Destroy(gameObject);
		}
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
