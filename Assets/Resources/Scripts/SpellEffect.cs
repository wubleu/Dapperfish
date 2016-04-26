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
		if (name == "Arrow") {
			lifetime = life / speed;
		}
	}

	void Update() {
		if ((clock += Time.deltaTime) > lifetime) {
			Destroy(gameObject);
		}
		if (name == "Damage" || name == "Bullet" || name == "Arrow") {
			transform.Translate(angle * spellSpeed * Time.deltaTime, Space.World);
			transform.position = new Vector3 (transform.position.x, .5f, transform.position.z);
		} /*else {
			radius -= radius * (Time.deltaTime / lifetime);
			gameObject.transform.localScale = new Vector3(radius, radius, radius);
		}*/
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "AI") {
			AIBehavior AI = col.GetComponent<AIBehavior> ();
			switch (name) {
			case "Blight":
				if (AI != null && infectPower >= AI.infectionCost) {
					AI.Infect ();
					infectPower -= AI.infectionCost;
				}
				return;
			case "Root":
				if (AI.isEnemy) {
					AI.Root ();
				}
				return;
			case "Damage":
				if (AI.isEnemy) {
					AI.Damage (10);
				}
				return;
			case "Bullet":
				AI.Damage (1);
				Destroy (gameObject);
				return;
			case "Arrow":
				print (col.gameObject.name );
				if (enemy != AI.isEnemy) {
					AI.Damage (1);
					Destroy (gameObject);
				}
				return;
			}
		} else if (col.name == "Necromancer" && enemy) {
			col.GetComponent<PlayerController>().Damage(1);
			Destroy(gameObject);
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "AI") {
			AIBehavior AI = col.GetComponent<AIBehavior> ();
			if (col.name == "Root" && AI.isEnemy) {
				AI.Root ();
			}
		}
	}
}
