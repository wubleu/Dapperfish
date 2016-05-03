﻿using UnityEngine;
using System.Collections;

public class SpellEffect : MonoBehaviour {

	// PARAMETERS
	float lifetime, spellSpeed, radius;
	bool enemy;
	bool paused = false;
	float arrowPower;
	Vector3 angle;

	public void init(float life, bool bad = false, float speed = 0, Vector3 dir = default(Vector3), params float[] arrowDamage) {
		radius = transform.localScale.x;
		lifetime = life;
		spellSpeed = speed;
		angle = dir;
		enemy = bad;
		if (name == "Arrow") {
			lifetime = life / speed;
			arrowPower = arrowDamage[0];
		}
	}

	void Update() {
		if (paused) {
			return;
		}
		if ((lifetime -= Time.deltaTime) <= 0) {
			Destroy(gameObject);
		}
		if (name == "Damage" || name == "Bullet" || name == "Arrow") {
			transform.Translate(angle * spellSpeed * Time.deltaTime, Space.World);
			transform.position = new Vector3 (transform.position.x, .5f, transform.position.z);
		}
	}

	void OnTriggerEnter(Collider col) {
		if ((name == "Arrow" || name == "Bullet") && col.gameObject.tag == "Obstacle") {
			Destroy (gameObject);
		}
		AIBehavior AI = col.GetComponent<AIBehavior> ();
		switch (name) {
		case "Blight":
			if (col.tag == "AI" && AI.isEnemy) {
				AI.Infect ();
			}
			break;
		case "Root":
			if (col.tag == "AI" && enemy != AI.isEnemy) {
				AI.Root ();
			} else if (enemy && col.name == "Necromancer") {
				col.GetComponent<PlayerController> ().Root ();
			} else if (!enemy && col.name == "Necromancer Boss") {
				col.GetComponent<NecromancerBoss> ().Root ();
			}
			break;
		case "Damage":
			if (col.tag == "AI" && enemy != AI.isEnemy) {
				AI.Damage (10);
			} else if (enemy && col.name == "Necromancer") {
				col.GetComponent<PlayerController> ().Damage (10);
			} else if (!enemy && col.name == "Necromancer Boss") {
				col.GetComponent<NecromancerBoss> ().Damage (10);
			}
			break;
		case "Bullet":
			if (col.tag == "AI" && enemy != AI.isEnemy) {
				AI.Damage (2);
				Destroy (gameObject);
			} else if (enemy && col.name == "Necromancer") {
				col.GetComponent<PlayerController> ().Damage (1);
				Destroy (gameObject);
			} else if (!enemy && col.name == "Necromancer Boss") {
				col.GetComponent<NecromancerBoss> ().Damage (2);
				Destroy (this.gameObject);
			}
			break;
		case "Arrow":
			if (col.tag == "AI" && enemy != AI.isEnemy) {
				AI.Damage (arrowPower);
				Destroy (gameObject);
			} else if (enemy && col.name == "Necromancer") {
				col.GetComponent<PlayerController> ().Damage (arrowPower);
				Destroy (gameObject);
			} else if (!enemy && col.name == "Necromancer Boss") {
				col.GetComponent<NecromancerBoss> ().Damage (arrowPower);
				Destroy (gameObject);
			}
			break;
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "AI") {
			AIBehavior AI = col.GetComponent<AIBehavior> ();
			if (name == "Root" && enemy != AI.isEnemy) {
				AI.Root ();
			}
		} else if (name == "Root" && enemy && col.name == "Necromancer") {
			col.GetComponent<PlayerController> ().Root ();
		} else if (name == "Root" && !enemy && col.name == "Necromancer Boss") {
			col.GetComponent<NecromancerBoss> ().Root ();
		}
	}

	public void PauseSpell() {
		paused = true;
	}

	public void UnPauseSpell() {
		paused = false;
	}
}
