using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {

	int chargespeed = 8;
	float normalspeed = 1.3f;
	float chargetimer = .5f;
	float chargedist = 2.5f;
	float chargecd= 1;
	bool cd = false;
	bool charging = false;


	// Use this for initialization
	public void initKnight(GameManager gMan, EnemyManager owner, PlayerController necro) {

		// PARAMETERS
		allyColor = new Color(0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = normalspeed;
		maxHP = 24f;
		meleeThreshold = 1f;
		meleeDamage = 25f;
		aggroRange = 7f;
		necroAggroModifier = 2f;
		immune = true;
		base.init(gMan, owner, necro);
		this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 0, 0);
	}


	// Update is called once per frame
	new void Update() {
		if (target != null) {
			float TargDist = Vector3.Distance (target.transform.position, transform.position);
			if (TargDist <= chargedist && charging == false && cd == false) {
				agent.speed = chargespeed;
				charging = true;
			}
		}
		if (charging) {
			chargetimer -= Time.deltaTime;
		}
		if (chargetimer <= 0) {
			charging = false;
			cd = true;
			chargetimer = .5f;
			agent.speed = speed;
		}
		if (cd) {
			chargecd -= Time.deltaTime;
		}
		if (chargecd <= 0) {
			cd = false;
			chargecd = 1;
		}
		base.Update();
	}

	void Attack(){
	}
	void OnCollisionStay(Collision coll) {
		if (coll.gameObject == target) {
			charging = false;
			chargetimer = .3f;
			agent.speed = speed;
		}
		base.OnCollision (coll);
	}


	void OnTriggerEnter(Collider coll) {
		//		base.TakeHit (coll.gameObject);
	}
}