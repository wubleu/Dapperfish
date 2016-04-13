using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	int frame;
	float clock = 0;
	public Vector3 angle;

	void Start() {
		frame = Time.frameCount;
	}

	void Update() {
		if (name == "Damage") {
			clock += Time.deltaTime;
			if (clock < 3) {
				transform.Translate(angle);
			} else {
				Destroy(gameObject);
			}
		} else if (frame != Time.frameCount) {
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
