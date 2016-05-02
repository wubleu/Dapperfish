using UnityEngine;
using System.Collections;

public class Peasant : AIBehavior {

	public void initPeasant(GameManager gMan, EnemyManager owner, PlayerController necro, params bool[] isElite) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 5.2f;
		maxHP = hp = 4f;
		meleeThreshold = 1f;
		meleeDamage = .3f;
		aggroRange = 9f;
		necroAggroModifier = 1.2f;
		chaseThreshold = 1f;
		chaseDist = 1f;
		hoverRadius = 1.5f;
		animmax = .2f;
		animcount = animmax;
		if (isElite.Length > 0) {
			if (isElite [0] == true) {
				base.init (gMan, owner, necro, true);
			} else {
				base.init (gMan, owner, necro);
			}
			if (isElite.Length > 1) {
				inWave = isElite[1];
				agent.destination = necromancer.transform.position;
			}
		} else {
			base.init (gMan, owner, necro);
		}
	}
		
	new void Update() {
		if (paused) {
			return;
		}
		if (inWave) {
			if (Vector3.Distance (transform.position, necromancer.transform.position) < aggroRange) {
				inWave = false;
			} else {
				return;
			}
		}
		base.Update();
		SwitchTargets ();
	}

	void OnCollisionStay(Collision coll) {
		base.OnCollision (coll);
	}

}
