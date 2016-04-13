using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	// PARAMETERS
	float lifetime = .5f;
	float damageSpellSpeed = .2f;

	float clock;
	public Vector3 angle;

	void Start() {
		clock = 0;
	}

	void Update() {
		if (name == "Damage") {
			transform.Translate(angle*damageSpellSpeed);
			if ((clock += Time.deltaTime) > lifetime*2) {
				Destroy (gameObject);
			}
		}
		else if ((clock += Time.deltaTime) > lifetime) {
			Destroy (gameObject);
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.GetComponent<AIBehavior>() != null) {
			switch (name) {
				case "Blight":
					col.GetComponent<AIBehavior>().Infect();
					return;
				case "Root":
					col.GetComponent<AIBehavior>().Root();
					return;
				case "Damage":
					col.GetComponent<AIBehavior>().Damage(10);
					return;
			}
		}
	}
}
