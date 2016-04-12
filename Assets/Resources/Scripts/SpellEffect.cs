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
		if (col.gameObject.name == "Peasant") {
			switch (name) {
				case "Blight":
					col.GetComponent<Peasant>().Infect();
					return;
				case "Root":
					col.GetComponent<Peasant>().Root();
					return;
				case "Damage":
					col.GetComponent<Peasant>().Damage(10);
					return;
			}
		}
	}
}
