using UnityEngine;
using System.Collections;

public class Archer : AIBehavior {

	// ENEMY SPECIFIC PARAMETERS
	float range = 7f;
	float moveThreshold = .4f;
	float firingWaitThreshold = .7f;
	protected float preferredRange = 4.5f;

	public bool moving = false;
	public bool hasFired = true;
	protected bool strafing = false;
	bool strafeLeft = false;
	float moveTimer = 0;
	float firingWaitTimer = 0;

	public void initArcher(GameManager gMan, EnemyManager owner, PlayerController necro, params bool[] isElite) {
		gManager = gMan;
		eManager = owner;
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 6f;
		maxHP = hp = 3f;
		meleeThreshold = 1f;
		meleeDamage = 2f;
		aggroRange = 10f;
		necroAggroModifier = 1.2f;
		chaseDist = 2f;
		if (isElite.Length == 0) {
			base.init (gMan, owner, necro);
		} else if (isElite.Length == 1) {
			base.init (gMan, owner, necro, isElite[0]);
		} else {
			base.init (gMan, owner, necro, isElite[0], isElite[1]);
		} 
		GetComponent<NavMeshAgent>().stoppingDistance = 2;
	}
		
	protected override void Update() {
		if (paused) {
			return;
		}
		if (root > 0 && (root -= Time.deltaTime) <= 0) {
			SpriteRenderer[] x = GetComponentsInChildren<SpriteRenderer> ();
			for (int i = 0; i < x.Length; i++) {
				if (x[i].name == "Stun") {
					Destroy (x[i].gameObject);
				}
			}
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
		if (moving) {
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
			if ((moveTimer += Time.deltaTime) > moveThreshold && (
					(target == null) ||
					(target != null  &&  Vector3.Distance (target.transform.position, transform.position) <= range-.5f))) {
				agent.speed = 0;
				moving = false;
				strafing = false;
				moveTimer = 0;
			} if (strafing && target != null) {
				
				if (strafeLeft) {
					agent.destination = transform.position + new Vector3 (transform.position.z - target.transform.position.z, 0, target.transform.position.x - transform.position.x);
				} else {
					agent.destination = transform.position + new Vector3 (target.transform.position.z - transform.position.z, 0, transform.position.x - target.transform.position.x);
				}
				agent.speed = speed/2;
			}
		} else {
			if ((firingWaitTimer += Time.deltaTime) > firingWaitThreshold / 2f && target != null && !hasFired) {
				if (isEnemy) {
					rend.sprite = cSprites [1];
				} else {
					rend.sprite = cSprites [4];
				}
				Fire ();
				hasFired = true;
				firingWaitTimer = firingWaitThreshold / 1.9f;
			} else if (hasFired && firingWaitTimer > firingWaitThreshold) {
				if (isEnemy) {
					rend.sprite = cSprites [0];
				} else {
					rend.sprite = cSprites [3];
				}
				if (target != null && Vector3.Distance(target.transform.position, transform.position) < preferredRange) {
					strafing = true;
					if (Random.Range (0, 2) == 0) {
						strafeLeft = true;
					} else {
						strafeLeft = false;
					}
				}
				moving = true;
				hasFired = false;
				firingWaitTimer = 0;
			}
		}
	}

	protected override float CheckAITargetsInSquare(float targetDist) {
		int unitGridX = ((int)transform.position.x - gManager.xGridOrigin) / 10;
		int unitGridY = ((int)transform.position.z - gManager.yGridOrigin) / 10;
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

	protected override void CheckNecro(float targetDist) {
		if (isEnemy) {
			float rawNecroDist = Vector3.Distance (necromancer.transform.position, transform.position);
			float necroDist = Mathf.Abs (aggroRange - rawNecroDist);
			if (necroDist < targetDist * necroAggroModifier && rawNecroDist < aggroRange) {
				agent.enabled = true;
				targetDist = necroDist;
				target = necromancer;
				agent.destination = necromancer.transform.position;
			}
		}
		NecromancerBoss nBoss = GameObject.FindObjectOfType<NecromancerBoss> ();
		if (nBoss != null && !isEnemy) {
			GameObject necroBoss = nBoss.gameObject;
			float rawNecroDist = Vector3.Distance (necroBoss.transform.position, transform.position);
			float necroDist = Mathf.Abs (aggroRange - rawNecroDist);
			if (necroDist < targetDist * necroAggroModifier && rawNecroDist < aggroRange) {
				agent.enabled = true;
				targetDist = necroDist;
				target = necroBoss;
				agent.destination = necroBoss.transform.position;
			}
		}
	}


	void Fire() {
		Abilities.Arrow(transform.position, Mathf.PI / 2 - Mathf.Atan2(target.transform.position.x - transform.position.x,
			target.transform.position.z - transform.position.z), isEnemy, range+1, meleeDamage);
	}
}
