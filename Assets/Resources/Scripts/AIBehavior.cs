using UnityEngine;
using System.Collections;

public class AIBehavior : MonoBehaviour {

	// PARAMETERS
	protected Color allyColor, enemyColor;
	public float hoverRadius, chaseDist, chaseThreshold, chaseClock, aggroRange, necroAggroModifier, speed,
		meleeThreshold, meleeDamage, switchDirTimer = 0, meleeTimer = 0, root = 0, hp, maxHP, infectionCost, animcount, animmax, meleecd = .2f;
	public GameObject target = null;
	protected GameObject necromancer;
	public GameManager gManager;
	protected EnemyManager eManager;
	protected SpriteRenderer rend;
	protected NavMeshAgent agent;
	protected float hoverRads;
	public bool hovering = false, isEnemy = true, immune = false, hoverPaused = false, windup = false, attacked = false;
	protected Sprite[] cSprites;

	protected virtual void init(GameManager gMan, EnemyManager owner, PlayerController necro) {
		eManager = owner;
		gManager = gMan;
		necromancer = necro.gameObject;
		agent = GetComponent<NavMeshAgent>();
		rend = GetComponentInChildren<SpriteRenderer>();
		transform.parent = eManager.transform;
		agent.speed = speed;
		agent.stoppingDistance = .2f;
		agent.acceleration = 60;
		agent.autoTraverseOffMeshLink = false;
		SwitchTargets ();
		if (name == "Archer") {
			cSprites = Resources.LoadAll<Sprite> ("Textures/Skeleton Archer Sprite Sheet");
			rend.sprite = cSprites [0];
		}
		if (name == "Peasant") {
			cSprites = Resources.LoadAll<Sprite> ("Textures/Zombie Sprite Sheet");
			rend.sprite = cSprites [7];
		}
		if (name == "Knight") {
			cSprites = Resources.LoadAll<Sprite> ("Textures/Knight Sprite Sheet");
			rend.sprite = cSprites [0];
		}
	}

	protected virtual void Update() {
		if (root > 0 && (root -= Time.deltaTime) <= 0) {
			agent.speed = speed;
			meleeTimer = 0;
		} else if (root > 0) {
			return;
		}
		meleeTimer += Time.deltaTime;
		if (meleeTimer >= meleeThreshold / 5) {
			if (name == "Peasant") {
				if (isEnemy) {
					rend.sprite = cSprites [7];
				} else {
					rend.sprite = cSprites [3];
				}
			}
		}
		if (windup) {
			animcount -= Time.deltaTime;
		}
		if (animcount <= 0) {
			if (name == "Peasant") {
				if (isEnemy) {
					rend.sprite = cSprites [5];
				} else {
					rend.sprite = cSprites [1];
				}
				animcount = animmax;
				windup = false;
			}
			if (name == "Knight") {
				rend.sprite = cSprites [2];
				animcount = animmax;
				windup = false;
				attacked = true;
			}
		}
		if (attacked) {
			meleecd -= Time.deltaTime;
			if (meleecd <= 0) {
				attacked = false;
				meleecd = .2f;
				rend.sprite = cSprites [0];
			}
		}
		if (attacked == false) {
			switchDirTimer += Time.deltaTime;
			if (hovering) {
				agent.enabled = true;
				transform.LookAt (necromancer.transform);
				Hover ();
			} else if (target != null) {
				transform.LookAt (target.transform);
				agent.enabled = true;
				agent.SetDestination (target.transform.position);
			} else {
				agent.enabled = false;
			}
		}
	}

	// makes sure our units stay on a level plane and don't get bounced by the physics engine
	void LateUpdate() {
		transform.position = new Vector3 (transform.position.x, .01f, transform.position.z);
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	protected virtual void SwitchTargets() {
		hovering = false;
		float targetDist = aggroRange;
		bool keepingTarget = false;
		// if the target is within aggroRange, keeps target
		if (target != null) {
			agent.enabled = true;
			if (target.name != "Necromancer" && !isEnemy && !target.GetComponent<AIBehavior> ().isEnemy) {
				target = null;
			} else {
				float currTargetDist = Vector3.Distance (target.transform.position, transform.position);
				if (currTargetDist < aggroRange || (target == necromancer && currTargetDist < aggroRange*necroAggroModifier)) {
					targetDist = currTargetDist/2f;	
					agent.destination = target.transform.position;
					keepingTarget = true;
					transform.LookAt (target.transform);
				} else {
					target = null;
				}
			}
		}
		// if the old target has moved out of range or did not exist, looks for new target
		if (target == null || keepingTarget) {
			// checks all AI's whose allegiance is different from this AI's
			targetDist = CheckAITargetsInSquare(targetDist);

			// enemy AIs check the necro. necro is a bit more intimidating than other targets.
			CheckNecro(targetDist);
		}
		if (target == null) {
			if (!isEnemy) {
				hovering = true;
			}
		} else if (!isEnemy) {
			transform.parent = eManager.transform;
		}
	}

	// runs a targeting check on all AI in my square, all adjacent/diagonally adjacent squares
	// takes in the distance to the current target and returns the distance to whatever the target is at the end of the function
	protected virtual float CheckAITargetsInSquare(float targetDist) {
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
							float AIDist = Vector3.Distance (AI.transform.position, transform.position);
							if (AIDist < targetDist) {
								agent.enabled = true;
								target = AI.gameObject;
								targetDist = AIDist;
							}
						}
					}
				}
			}
		}
		return targetDist;
	}

	// runs a targeting check on the necromancer- never targets the necromancer if I am not an enemy
	// returns the targetDist passed in or the modified necromancer targetDist (distance * necroAggroModifier) if the necromancer is targeted
	protected virtual float CheckNecro(float targetDist) {
		if (isEnemy) {
			float necroDist = Vector3.Distance (necromancer.transform.position, transform.position);
			if (necroDist < targetDist * necroAggroModifier && necroDist < aggroRange) {
				target = necromancer;
				agent.destination = necromancer.transform.position;
				agent.speed = speed;
			}
		}
		return targetDist*necroAggroModifier;
	}

	// when friendly AI has no target:
	// if it is not yet in orbiting range of the necro, moves toward hover area with eManager as parent
	// if it is in range, orbits at hoverRadius units away with necro as parent
	protected virtual void Hover() {
		// moving toward the necro
		if (transform.parent != necromancer.transform) {
			// if ready to start orbiting, starts orbiting
			float distToNecro = Vector3.Distance (transform.position, necromancer.transform.position);
			if (distToNecro < hoverRadius+.2f && distToNecro > hoverRadius-.2f) {
				transform.parent = necromancer.transform;
				hoverRads = Mathf.Atan (transform.localPosition.z / transform.localPosition.x);
				if (transform.localPosition.x > 0) {
					hoverRads += Mathf.PI;
				}
				agent.enabled = false;
			} else {
				agent.enabled = true;
				agent.speed = speed;
				if (distToNecro < hoverRadius - .2f) {
					agent.destination = transform.position-necromancer.transform.position;
				} else {
					agent.destination = necromancer.transform.position;
				}
			}
		}
		// orbiting the necro
		if (transform.parent == necromancer.transform) {
			hoverRads += speed * Time.deltaTime;
			Vector3 hoverPos = Abilities.NormalizeVector (new Vector3 (1, Mathf.Tan(hoverRads)));
			if (((Mathf.PI/2) + hoverRads)%(2*Mathf.PI) < Mathf.PI) {
				transform.localPosition = new Vector3 (-hoverRadius * hoverPos.x, 0, -hoverRadius * hoverPos.y);
			} else {
				transform.localPosition = new Vector3 (hoverRadius * hoverPos.x, 0, hoverRadius * hoverPos.y);
			}
		}
	}

	protected void Melee(Collision coll, float damage = 1) {
		if (coll.gameObject.tag != "AI" && coll.gameObject.name != "Necromancer") {
			return;
		}
		AudioSource.PlayClipAtPoint (gManager.scratch, transform.position);
		if (coll.gameObject.name == "Necromancer") {
			if (isEnemy) {
				coll.gameObject.GetComponent<PlayerController> ().TakeHit (coll.gameObject);
				meleeTimer = 0;
			}
			return;
		}
		AIBehavior unit = coll.gameObject.GetComponent<AIBehavior> ();
		if (unit.isEnemy != isEnemy) {
			unit.Damage(damage);
			meleeTimer = 0;
		}
	}

	protected virtual void OnCollision(Collision coll) {
		if (coll.gameObject == target) {
				agent.speed = speed / 5f;
				if (meleeTimer > meleeThreshold) {
					SwitchTargets ();
					if (name == "Peasant") {
						if (isEnemy) {
							rend.sprite = cSprites [4];
							windup = true;
						} else {
							rend.sprite = cSprites [0];
						}
					}
					if (name == "Knight") {
						rend.sprite = cSprites [1];
						windup = true;
					}
					Melee (coll);
				}
		} 
	}

	void OnCollisionExit(Collision coll) {
		if (coll.gameObject == target) {
			agent.enabled = true;
			agent.speed = speed;
		}
	}

	public virtual void Infect() {
		if (isEnemy && !immune) {
			isEnemy = false;
			transform.parent = eManager.transform;
			eManager.peasantCount--;
			necromancer.GetComponent<PlayerController>().minionCount++;
			target = null;
			SwitchTargets();
			if (name == "Archer") {
				rend.sprite = cSprites [3];
			}
			if (name == "Peasant") {
				rend.sprite = cSprites [3];
			}
			hp = maxHP;
		}
	}

	public virtual void Root() {
		root = 1.5f;
		agent.speed = 0;
	}

	public virtual void Damage(float damage) {
		hp -= damage;
		if (hp <= 0) { 
			if (isEnemy) {
				eManager.peasantCount -= 1;
			} else {
				necromancer.GetComponent<PlayerController>().minionCount -= 1;
			}
			/*if (name == "Archer") {
				if (isEnemy == true) {
					rend.sprite = cSprites [2];
				} else {
					rend.sprite = cSprites [5];
				}
				//this.GetComponent<Archer> ().enabled = false;
				Destroy (this.gameObject,2);
			}*/
			Destroy(gameObject);
		}
	}
}
