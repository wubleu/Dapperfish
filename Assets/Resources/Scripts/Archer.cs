using UnityEngine;
using System.Collections;

public class Archer : AIBehavior {

	// ENEMY SPECIFIC PARAMETERS
	float range = 5f;
	float moveThreshold = .4f;
	float firingWaitThreshold = .7f;
	protected float preferredRange = 2.5f;

	bool moving = false;
	bool hasFired = true;
	protected bool backtracking = false;
	float moveTimer = 0;
	float firingWaitTimer = 0;
	

	public void initArcher(GameManager gMan, EnemyManager owner, PlayerController necro) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 6f;
		maxHP = hp = 3f;
		meleeThreshold = 1f;
		meleeDamage = 1f;
		aggroRange = 10f;
		necroAggroModifier = 1.2f;
		chaseDist = 2f;
		GetComponent<NavMeshAgent>().stoppingDistance = 4;
		infectionCost = 35;

		base.init(gMan, owner, necro);
	}
		
	new void Update() {
		base.Update();
		if (moving) {
			backtracking = false;
			SwitchTargets ();
			if (target != null) {
				float targetDist = Vector3.Distance (necromancer.transform.position, transform.position);
				if (targetDist < preferredRange) {
					agent.speed = 0;
				} else {
					agent.speed = speed * (Vector3.Distance (transform.position, target.transform.position) / aggroRange);
				}
			} else {
				agent.speed = 0;
			}
			if ((moveTimer += Time.deltaTime) > moveThreshold) {
				agent.speed = 0;
				moving = false;
				moveTimer = 0;
			}
		} else if (!moving) {
			SwitchTargets ();
			if ((firingWaitTimer += Time.deltaTime) > firingWaitThreshold / 2f && target != null && !hasFired) {
				Fire ();
				hasFired = true;
				firingWaitTimer = firingWaitThreshold / 1.9f;
			} else if (hasFired && firingWaitTimer > firingWaitThreshold) {
				moving = true;
				hasFired = false;
				firingWaitTimer = 0;
			}
		}
	}

	protected override float CheckAITargetsInSquare(float targetDist) {
		int unitGridX = ((int)transform.position.x - gManager.xGridOrigin) / 10;
		int unitGridY = ((int)transform.position.y - gManager.yGridOrigin) / 10;
		int checkingX;
		int checkingY;
		for (int x = -1; x < 2; x++) {
			for (int y = -1; y < 2; y++) {
				checkingX = x + unitGridX;
				checkingY = y + unitGridY;
				if ((checkingX>=0 && checkingX<gManager.xDimension) && (checkingY>=0 && checkingY<gManager.yDimension)) {
					foreach (AIBehavior AI in gManager.enemyGrid[checkingX, checkingY]) {
						if (AI != null && AI.isEnemy != isEnemy) {
							float rawAIDist = Vector3.Distance (AI.transform.position, transform.position);
							float AIDist = Mathf.Abs (aggroRange - rawAIDist);
							if (AIDist < targetDist && rawAIDist < aggroRange) {
								agent.enabled = true;
								target = AI.gameObject;
								targetDist = AIDist;
								agent.destination = AI.transform.position;
							}
						}
					}
				}
			}
		}
		return targetDist;
	}

	protected override float CheckNecro(float targetDist) {
		if (isEnemy) {
			float rawNecroDist = Vector3.Distance (necromancer.transform.position, transform.position);
			float necroDist = Mathf.Abs (aggroRange - rawNecroDist);
			if (necroDist < targetDist * necroAggroModifier && rawNecroDist < aggroRange) {
				targetDist = necroDist;
				target = necromancer;
				agent.destination = necromancer.transform.position;
			}
		}
		return targetDist;
	}


	void Fire() {
		Abilities.Arrow(transform.position, Mathf.PI / 2 - Mathf.Atan2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z), isEnemy);
	}
}
