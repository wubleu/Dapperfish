using UnityEngine;
using System.Collections;

public class AIBehavior : MonoBehaviour {

	// PARAMETERS
	protected Color allyColor, enemyColor;
	public float maxHP;
	public float hoverRadius, chaseThreshold, chaseClock, aggroRange, necroAggroModifier, speed, infectionCost, switchDirThreshold,
					meleeThreshold, meleeDamage, switchDirTimer = 0, meleeTimer = 0, root = 0, hp;

	public GameObject target = null;
	protected GameObject necromancer;
	protected GameManager gManager;
	protected EnemyManager eManager;
	protected Material material;
	protected NavMeshAgent agent;
	protected float hoverRads;
	public bool hovering = false;
	public bool isEnemy = true;


	// Use this for initialization
	protected void init(GameManager gMan, EnemyManager owner, string textureName, float x, float y, float xScale, float yScale) {
		tag = "AI";
		eManager = owner;
		gManager = gMan;
		gManager.MakeSprite(gameObject, textureName, eManager.transform, x, y, xScale, yScale, 200);
		material = GetComponent<SpriteRenderer> ().material;
		agent = gameObject.AddComponent<NavMeshAgent> ();
		agent.updateRotation = false;
		Rigidbody rbody = gameObject.AddComponent<Rigidbody> ();
		rbody.useGravity = false;
		rbody.constraints = RigidbodyConstraints.FreezeRotation;
		material.color = enemyColor;
		necromancer = GameObject.Find("Necromancer");
		hp = maxHP;
		agent.speed = speed;
		agent.radius = .1f;
	}


	protected void Update() {
		meleeTimer += Time.deltaTime;
		switchDirTimer += Time.deltaTime;
		if (hovering) {
			Hover ();
		}
		if (root > 0) {
			root -= Time.deltaTime;
			if (root <= 0) {
				agent.speed = 1.5f;
			}
		} if (switchDirTimer > switchDirThreshold) {
			SwitchTargets ();
			switchDirTimer = 0;
		}
	}

	// makes sure our units stay on a level plane and don't get bounced by the physics engine
	void LateUpdate() {
		transform.position = new Vector3 (transform.position.x, .01f, transform.position.z);
		gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}


	protected virtual void MoveToward() {
		if (meleeTimer >= meleeThreshold) {
			SwitchTargets ();
		}
	}

	 
	protected virtual void SwitchTargets() {
		float targetDist = aggroRange;
		// if this AI has had a target for chaseThreshold or more seconds, and the target is outside of aggro distance, attempts to switch targets
		if (target != null) {
			if (!isEnemy && !target.GetComponent<AIBehavior> ().isEnemy) {
				target = null;
			} else {
				float currTargetDist = Vector3.Distance (target.transform.position, transform.position);
				agent.destination = target.transform.position;
				if ((chaseClock += Time.deltaTime) > chaseThreshold) {
					chaseClock = 0;
					if (currTargetDist < aggroRange) {
						targetDist = 0;	
					} else {
						target = null;
					}
				} 
			}
		}
		// if the old target has moved out of range or did not exist, looks for new target
		if (target == null) {
			// checks all AI's whose allegiance is different from this AI's
			foreach (AIBehavior AI in FindObjectsOfType<AIBehavior>()) {
				if (AI.isEnemy != isEnemy) {
					float AIDist = Vector3.Distance (AI.transform.position, transform.position);
					if (AIDist < targetDist) {
						target = AI.gameObject;
						targetDist = AIDist;
						agent.destination = AI.transform.position;
						agent.speed = speed;
					}
				}
			}
			// enemy AIs check the necro. necro is a bit more intimidating than other targets.
			if (isEnemy) {
				float necroDist = Vector3.Distance (necromancer.transform.position, transform.position);
				if (necroDist < targetDist * necroAggroModifier) {
					target = necromancer;
					agent.destination = necromancer.transform.position;
					agent.speed = speed;
				}
			}
		} 
		// if a target still has not been set, initiates hovering behavior for allied AI's and stops enemy AI's
		if (target == null) {
			if (isEnemy) {
				agent.speed = 0;
			} else {
				if (!hovering) {
					hovering = true;
					agent.destination = necromancer.transform.position;
				}
			}
		} else if (!isEnemy) {
			hovering = false;
			transform.parent = eManager.transform;
		}
	}

	// when friendly AI has no target:
	// if it is not yet in orbiting range of the necro, moves toward him with eManager as parent
	// if it is in range, orbits at hoverRadius units away with necro as parent
	protected virtual void Hover() {
		// moving toward the necro
		if (transform.parent != necromancer.transform) {
			// if ready to start orbiting, starts orbiting
			if (Vector3.Distance (transform.position, necromancer.transform.position) < hoverRadius/2f) {
				transform.parent = necromancer.transform;
				hoverRads = Mathf.Atan (transform.localPosition.y / transform.localPosition.x);
				agent.speed = 0;
			} else {
				agent.destination = necromancer.transform.position;
			}
		}
		// orbiting the necro
		if (transform.parent == necromancer.transform) {
			hoverRads += speed * Time.deltaTime;
			Vector3 hoverPos = Abilities.NormalizeVector (new Vector3 (1, Mathf.Tan(hoverRads), 0));
			if (((Mathf.PI/2) + hoverRads)%(2*Mathf.PI) < Mathf.PI) {
				transform.localPosition = new Vector3 (hoverRadius * hoverPos.x, hoverRadius * hoverPos.y);
			} else {
				transform.localPosition = new Vector3 (-hoverRadius * hoverPos.x, -hoverRadius * hoverPos.y);
			}
		}
		SwitchTargets ();
	}

	protected void Melee(Collision coll) {
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
			unit.TakeHit (coll.gameObject);
			meleeTimer = 0;
		}
	}


	public virtual void TakeHit(GameObject collObj) {
		if (collObj.name == "Blight") {
			collObj.GetComponent<Blight> ().Infect (infectionCost, this);
			return;
		} else if (collObj.name == "SpellShot") {
			hp -= 3;
		} else {
			hp -= 1;
		}
		if (hp <= 0) { 
			if (isEnemy) {
				eManager.peasantCount -= 1;
			} else {
				necromancer.GetComponent<PlayerController> ().minionCount -= 1;
			}
			Destroy (gameObject);
		}
	}


	protected virtual void OnCollision(Collision coll) {
		if (meleeTimer > meleeThreshold) {
			Melee (coll);
		}
	}


	public virtual void Infect() {
		if (isEnemy) {
			eManager.peasantCount--;
			necromancer.GetComponent<PlayerController> ().minionCount++;
			SwitchTargets ();
			material.color = allyColor;
			isEnemy = false;
			target = null;
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
			Destroy (gameObject);
		}
	}
}
