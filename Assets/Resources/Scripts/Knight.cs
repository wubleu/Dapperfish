using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {

	float chargespeed = 85, normalspeed = 6, wait = 1f, charge = 0.3f, caggro = 4, timer = 0, chargecd = .3f;
	int mode = 0;

	public void initKnight(GameManager gMan, EnemyManager owner, PlayerController necro) {

		// PARAMETERS
		allyColor = new Color(0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = normalspeed;
		maxHP = hp = 6;
		meleeThreshold = 1;
		meleeDamage = 25;
		aggroRange = 7;
		necroAggroModifier = 2;
		immune = true;
		animmax = .3f;
		animcount = animmax;
		base.init(gMan, owner, necro);
		//this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 0, 0);
	}

	protected override void Update() {
		if (target != null) {
			if (mode == 0) { // walking
				SwitchTargets ();
				base.Update ();
				if (target != null && target.name == "Necromancer" && Vector3.Distance (transform.position, target.transform.position) < caggro) {
					mode = 1;
					timer = wait;
					rend.sprite = cSprites [3];
					agent.speed = 0;
				}
			} else if (mode == 1) { // waiting
				timer -= Time.deltaTime;
				if (timer < wait - (3 * (wait / 4))) {
					rend.sprite = cSprites [4];
				} else if (timer < wait - (2 * (wait / 4))) {
					rend.sprite = cSprites [5];
				} else if (timer < wait - (wait / 4)) {
					rend.sprite = cSprites [6];
				}
				if ((timer -= Time.deltaTime) <= 0) {
					mode = 2;
					timer = charge;
					agent.speed = chargespeed;
					float angle = Mathf.PI / 2 - Mathf.Atan2 (target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
					agent.SetDestination (transform.position + chargespeed * charge * new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle)));
					rend.sprite = cSprites [7];
				}
			} else if ((timer -= Time.deltaTime) <= 0) { // charging
				rend.sprite = cSprites [8];
				mode = 3;
				timer = chargecd;
				agent.speed = 0;
			} else if (mode == 3) {
				if ((timer-= Time.deltaTime) <= 0){
					mode = 0;
					agent.speed = normalspeed;
					rend.sprite = cSprites [0];
				}
			}
		} else {
			SwitchTargets();
			base.Update();
			mode = 0;
			agent.speed = normalspeed;
		}
	}

	void OnCollisionStay(Collision coll) {
		if (coll.gameObject == target ) {
			agent.speed = normalspeed;
		}
		base.OnCollision (coll);
	}
}