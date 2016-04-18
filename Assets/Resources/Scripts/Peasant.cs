﻿using UnityEngine;
using System.Collections;

public class Peasant : AIBehavior {

	public void init (GameManager gMan, EnemyManager owner) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 1.5f;
		maxHP = 3f;
		infectionCost = 15;
		switchDirThreshold = .5f;
		meleeThreshold = 1f;
		meleeDamage = 1f;
		aggroRange = 4f;
		necroAggroModifier = 1.2f;
		chaseThreshold = 1f;
		hoverRadius = 3f;

		isEnemy = true;
		base.init (gMan, owner, "Circle", 0, 0, .3f, .3f);
		name = "Peasant";
		gameObject.AddComponent<SphereCollider> ().radius = .17f;
		gameObject.GetComponent<NavMeshAgent> ().radius = .17f;
	}

	new void Update() {
		base.Update();
		base.MoveToward();
	}

	void OnCollisionStay(Collision coll) {
		base.OnCollision (coll);
	}

	void OnTriggerEnter(Collider coll) {
//		base.TakeHit (coll.gameObject);
	}
}
