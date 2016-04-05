﻿using UnityEngine;
using System.Collections;

public class Peasant : AIBehavior {


	// Use this for initialization
	public void init (GameManager gMan, EnemyManager owner) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 1.5f;
		maxHP = 3f;
		infectionCost = 9;
		switchDirThreshold = .5f;
		meleeThreshold = 1f;
		meleeDamage = 1f;

		isEnemy = true;
		base.init (gMan, owner, "Circle", 0, 0, .3f, .3f);
		name = "Peasant";
		gameObject.AddComponent<CircleCollider2D> ();
	}

	
	// Update is called once per frame
	void Update () {
		base.Update ();
		base.MoveToward ();
	}
		

	void OnCollisionStay2D(Collision2D coll) {
		base.OnCollision (coll);
	}


	void OnTriggerEnter2D(Collider2D coll) {
		base.TakeHit (coll.gameObject);
	}
}
