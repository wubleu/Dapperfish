using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	// PARAMETERS
	float lifetime, spellSpeed, radius, clock = 0;
	Vector3 angle;

	public void init(float life, float speed = 0, Vector3 dir = default(Vector3)) {
		radius = transform.localScale.x;
		lifetime = life;
		spellSpeed = speed;
		angle = dir;
	}

	void Update() {
		if ((clock += Time.deltaTime) > lifetime) {
			Destroy(gameObject);
		}
		if (name == "Damage" || name == "Bullet") {
			transform.Translate(angle * spellSpeed * Time.deltaTime, Space.World);
		} else {
			radius -= radius * (Time.deltaTime / lifetime);
			gameObject.transform.localScale = new Vector3(radius, radius, radius);
		}
	}

	void OnTriggerEnter(Collider col) {
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
				case "Bullet":
					col.GetComponent<AIBehavior>().Damage(1);
					Destroy(gameObject);
					return;
			}
		}
	}
}
