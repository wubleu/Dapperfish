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
		if (isElite.Length == 0) {
			base.init (gMan, owner, necro);
		} else if (isElite.Length == 1) {
			base.init (gMan, owner, necro, isElite[0]);
		} else {
			base.init (gMan, owner, necro, isElite[0], isElite[1]);
		} 
	}
		
	protected override void Update() {
		if (paused) {
			return;
		}
		if (root > 0 && (root -= Time.deltaTime) <= 0) {
			agent.speed = speed;
			meleeTimer = 0;
		} else if (root > 0) {
			return;
		}
		if (inWave) {
			if (Vector3.Distance (transform.position, necromancer.transform.position) < aggroRange) {
				inWave = false;
			} else {
				transform.LookAt (necromancer.transform);
				agent.destination = necromancer.transform.position;
				return;
			}
		}
		SwitchTargets();
		base.Update();
	}
}
