using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	// PARAMETERS
	float lifetime, spellSpeed, radius, clock = 0, infectPower = 250;
	bool enemy;
	Vector3 angle;

	public void init(float life, float speed = 0, Vector3 dir = default(Vector3), bool bad = false) {
		radius = transform.localScale.x;
		lifetime = life;
		spellSpeed = speed;
		angle = dir;
		enemy = bad;
	}

	void Update() {
		if ((clock += Time.deltaTime) > lifetime) {
			Destroy(gameObject);
		}
		if (name == "Damage" || name == "Bullet" || name == "Arrow") {
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
					AIBehavior AI = col.GetComponent<AIBehavior> ();
					if (AI != null && infectPower >= AI.infectionCost) {
						AI.Infect ();
						infectPower -= AI.infectionCost;
					}
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
				case "Arrow":
					if (enemy != col.GetComponent<AIBehavior>().isEnemy) {
						col.GetComponent<AIBehavior>().Damage(1);
						Destroy(gameObject);
					}
					return;
			}
		} else if (col.name == "Necromancer" && enemy) {
			col.GetComponent<PlayerController>().Damage(1);
			Destroy(gameObject);
		}
	}
}
