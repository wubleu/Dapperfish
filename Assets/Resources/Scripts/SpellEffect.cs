using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	// PARAMETERS
	float lifetime = .5f;
	float damageSpellSpeed = .2f;

	float clock = 0, radius;
	public Vector3 angle;

	void Start() {
		radius = transform.localScale.x;
		transform.parent = GameObject.Find ("Game Manager").transform;
		transform.rotation = transform.parent.rotation;
	}

	void Update() {
		if (name == "Damage") {
			if ((clock += Time.deltaTime) > lifetime * 2) {
				Destroy(gameObject);
			}
			transform.Translate(angle * damageSpellSpeed, Space.World);
		} else if ((clock += Time.deltaTime) > lifetime) {
			Destroy(gameObject);
		} else {
			radius -= radius * (Time.deltaTime / lifetime);
			gameObject.transform.localScale = new Vector3(radius, radius, radius);
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "AI") {
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
