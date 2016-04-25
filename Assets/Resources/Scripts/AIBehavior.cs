using UnityEngine;
using System.Collections;

public class AIBehavior : MonoBehaviour {

	// PARAMETERS
	protected Color allyColor, enemyColor;
	public float hoverRadius, chaseDist, chaseThreshold, chaseClock, aggroRange, necroAggroModifier, speed, switchDirThreshold = Random.Range(.3f, .4f),
					meleeThreshold, meleeDamage, switchDirTimer = 0, meleeTimer = 0, root = 0, hp, maxHP, infectionCost;
	public GameObject target = null;
	protected GameObject necromancer;
	public GameManager gManager;
	protected EnemyManager eManager;
	protected SpriteRenderer rend;
	protected NavMeshAgent agent;
	protected float hoverRads;
	public bool hovering = false, isEnemy = true, immune = false;

	protected virtual void init(GameManager gMan, EnemyManager owner, PlayerController necro) {
		eManager = owner;
		gManager = gMan;
		necromancer = necro.gameObject;
		agent = GetComponent<NavMeshAgent>();
		rend = GetComponentInChildren<SpriteRenderer>();
		transform.parent = eManager.transform;
		agent.speed = speed;
		agent.stoppingDistance = .1f;
	}

	protected virtual void Update() {
		meleeTimer += Time.deltaTime;
		switchDirTimer += Time.deltaTime;
		if (root > 0 && (root -= Time.deltaTime) <= 0) {
			agent.speed = speed;
			meleeTimer = 0;
		}
//		if (switchDirTimer > switchDirThreshold) {
			SwitchTargets();
			switchDirTimer = 0;
//		}
		if (hovering) {
			Hover();
		} else if (target != null){
			agent.SetDestination(target.transform.position);
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
		// if the target is within aggroRange, keeps target
		if (target != null) {
			if (target.name != "Necromancer" && !isEnemy && !target.GetComponent<AIBehavior>().isEnemy) {
				target = null;
			} else {
				float currTargetDist = Vector3.Distance(target.transform.position, transform.position);
				if (currTargetDist < aggroRange) {
					targetDist = currTargetDist/2f;
				} else {
					target = null;
				}
			}
		}
		targetDist = CheckAITargetsInSquare(targetDist);
//		foreach (AIBehavior AI in FindObjectsOfType<AIBehavior>()) {
//			if (AI.isEnemy != isEnemy) {
//				float AIDist = Vector3.Distance (AI.transform.position, transform.position);
//				if (AIDist < targetDist) {
//					target = AI.gameObject;
//					targetDist = AIDist;
//					agent.destination = AI.transform.position;
//					agent.speed = speed;
//				}
//			}
//		}
		if (isEnemy) { // check necro distance
			float necroDist = Vector3.Distance (necromancer.transform.position, transform.position);
			if (necroDist / necroAggroModifier < targetDist && necroDist < aggroRange) {
				target = necromancer;
			}
		}
		// if a target still has not been set, initiates hovering behavior for allied AI's and stops enemy AI's
		if (target == null) {
			if (!isEnemy) {
				hovering = true;
			}
		}
	}

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

	// when friendly AI has no target:
	// if it is not yet in orbiting range of the necro, moves toward him with eManager as parent
	// if it is in range, orbits at hoverRadius units away with necro as parent
	protected virtual void Hover() {
		// moving toward the necro
		if (transform.parent != necromancer.transform) {
			// if ready to start orbiting, starts orbiting
			if (Vector3.Distance (transform.position, necromancer.transform.position) < hoverRadius) {
				transform.parent = necromancer.transform;
				hoverRads = Mathf.Atan (transform.localPosition.z / transform.localPosition.x);
				if (transform.localPosition.x > 0) {
					hoverRads += Mathf.PI;
				}
				agent.enabled = false;
			} else {
				agent.destination = necromancer.transform.position;
				agent.enabled = true;
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
		SwitchTargets ();
	}

	protected void Melee(Collision coll, float damage = 1) {
		if (coll.gameObject.tag != "AI" && coll.gameObject.name != "Necromancer") {
			return;
		}
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
		if (meleeTimer > meleeThreshold && coll.gameObject == target) {
			SwitchTargets ();
			Melee (coll);
		}
	}

	public virtual void Infect() {
		if (isEnemy && !immune) {
			isEnemy = false;
			transform.parent = eManager.transform;
			GetComponent<NavMeshAgent>().stoppingDistance = 0;
			eManager.peasantCount--;
			necromancer.GetComponent<PlayerController>().minionCount++;
			target = null;
			SwitchTargets();
			rend.color = allyColor;
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
			Destroy(gameObject);
		}
	}
}
