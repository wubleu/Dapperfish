using UnityEngine;
using System.Collections;

public class Peasant : AIBehavior {

	public void initPeasant(GameManager gMan, EnemyManager owner, PlayerController necro) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 4f;
		maxHP = hp = 3f;
		meleeThreshold = 1f;
		meleeDamage = 1f;
		aggroRange = 7f;
		necroAggroModifier = 1.2f;
		chaseThreshold = 1f;
		chaseDist = 1f;
		hoverRadius = 1.5f;
		infectionCost = 25;

		base.init(gMan, owner, necro);
	}

	protected override void Update() {
		base.Update();
	}

	void OnCollisionStay(Collision coll) {
		base.OnCollision (coll);
	}

}
