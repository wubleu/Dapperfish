using UnityEngine;
using System.Collections;

public class Archer : AIBehavior {

	float range = 4.5f;

	public void initArcher(GameManager gMan, EnemyManager owner, PlayerController necro) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 1.5f;
		maxHP = hp = 3f;
		switchDirThreshold = .5f;
		meleeThreshold = 1f;
		meleeDamage = 1f;
		aggroRange = 8f;
		necroAggroModifier = 1.2f;
		GetComponent<NavMeshAgent>().stoppingDistance = 4;
		infectionCost = 35;

		base.init(gMan, owner, necro);
	}

	new void Update() {
		base.Update();
		if (target != null) {
			if (Vector3.Distance(transform.position, target.transform.position) < range) {
				if (meleeTimer > meleeThreshold) {
					Abilities.Arrow(transform.position, Mathf.PI / 2 - Mathf.Atan2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z), isEnemy);
					meleeTimer = 0;
				}
			}
		}
	}
}
