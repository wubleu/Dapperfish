using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {

	int chargespeed = 2;
	float normalspeed = 1.3f;
	float chargetimer = .5f;
	float chargedist = 2.5f;
	bool charging = false;


	// Use this for initialization
	public void initKnight(GameManager gMan, EnemyManager owner) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = normalspeed;
		maxHP = 8f;
		switchDirThreshold = .5f;
		meleeThreshold = 1f;
		meleeDamage = 5f;
		aggroRange = 4f;
		necroAggroModifier = 1.3f;

		base.init(gMan, owner);
	}


	// Update is called once per frame
	new void Update() {
		if (target != null) {
			float TargDist = Vector3.Distance (target.transform.position, transform.position);
			if (TargDist <= chargedist) {
				speed = chargespeed;
				charging = true;
			}
		}

		if (charging) {
			chargetimer -= Time.deltaTime;
		}
		if (chargetimer <= 0) {
			charging = false;
			chargetimer = .5f;
			speed = normalspeed;
		}
		base.Update();
		base.MoveToward();
	}

	void Attack(){
	}
	void OnCollisionStay(Collision coll) {
		if (coll.gameObject == target) {
			charging = false;
			chargetimer = .5f;
			speed = normalspeed;
		}
		base.OnCollision (coll);
	}


	void OnTriggerEnter(Collider coll) {
		//		base.TakeHit (coll.gameObject);
	}
}