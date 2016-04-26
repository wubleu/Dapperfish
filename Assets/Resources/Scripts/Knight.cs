using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {

	float chargespeed = 30, normalspeed = 4, wait = 0.75f, charge = 0.25f, caggro = 4, timer = 0;
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
		base.init(gMan, owner, necro);
		this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 0, 0);
	}

	// Update is called once per frame
	new void Update() {
		if (target != null) {
			if (mode == 0) { // walking
				base.Update();
				if (target != null && target.name == "Necromancer" && Vector3.Distance(transform.position, target.transform.position) < caggro) {
					mode = 1;
					timer = wait;
					agent.speed = 0;
				}
			} else if (mode == 1) { // waiting
				if ((timer -= Time.deltaTime) <= 0){
					mode = 2;
					timer = charge;
					agent.speed = chargespeed;
					float angle = Mathf.PI / 2 - Mathf.Atan2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
					agent.SetDestination(transform.position + chargespeed * charge * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
				}
			} else if ((timer -= Time.deltaTime) <= 0) { // charging
				mode = 0;
				agent.speed = normalspeed;
			}
		} else {
			base.Update();
			mode = 0;
			agent.speed = normalspeed;
		}
	}

	void OnCollisionStay(Collision coll) {
		if (coll.gameObject == target) {
			agent.speed = speed;
		}
		base.OnCollision (coll);
	}
}