using UnityEngine;
using System.Collections;

public class AIBehavior : MonoBehaviour {

	// PARAMETERS
	protected Color allyColor, enemyColor;
	public float hoverRadius, chaseDist, chaseThreshold, chaseClock, aggroRange, necroAggroModifier, speed,
		meleeThreshold, meleeDamage, meleeTimer = 0, root = 0, hp, maxHP, infectionCost, talert = 0,
		animcount, animmax, rootPersistence = .5f, convertedHp = .8f, convertedStrength = .6f, meleecd = 1;
	protected bool paused = false;
	public bool inWave = false;
	protected float resumeSpeed;
	protected float resumeAcceleration;
	public GameObject target = null;
	protected GameObject necromancer;
	public GameManager gManager;
	protected EnemyManager eManager;
	protected SpriteRenderer rend;
	protected NavMeshAgent agent;
	protected float hoverRads;
	public bool isEnemy = true, immune = false, hoverPaused = false, windup = false, attacked = false, isElite = false;
	protected Sprite[] cSprites;
	Vector3 origin;

	protected virtual void init(GameManager gMan, EnemyManager owner, PlayerController necro, params bool[] isElite) {
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
		if (isElite.Length >= 1 && isElite[0] == true) {
			SetToElite();
		}
		if (isElite.Length >= 2 && (inWave = isElite [1]) == true) {
			agent.destination = necromancer.transform.position;
			target = necromancer;
		}
		resumeSpeed = speed;
		origin = transform.position;
	}

	protected virtual void Update() {
		if (talert > 0 && (talert -= Time.deltaTime) <= 0) {
			aggroRange -= 100;
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
				meleecd = 1f;
				rend.sprite = cSprites [0];
			}
		} else if (target != null) {
			transform.LookAt (target.transform);
			agent.SetDestination(target.transform.position);
		}
	}

	// makes sure our units stay on a level plane and don't get bounced by the physics engine
	void LateUpdate() {
		transform.position = new Vector3 (transform.position.x, .01f, transform.position.z);
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	protected void SwitchTargets() {
		float targetDist = aggroRange;
		bool aggro = false;
		if (target != null) { // if the target is within aggroRange, keeps target
			if (target.name != "Necromancer" && target.name != "Necromancer Boss" && !isEnemy && !target.GetComponent<AIBehavior> ().isEnemy) {
				target = null;
			} else {
				float currTargetDist = Vector3.Distance (target.transform.position, transform.position);
				if (currTargetDist < aggroRange || (target == necromancer && currTargetDist < aggroRange*necroAggroModifier)) {
//					targetDist = currTargetDist/2f;	
					agent.destination = target.transform.position;
					transform.LookAt (target.transform);
				} else {
					target = null;
				}
			}
		} else {
			aggro = true;
		}
		targetDist = CheckAITargetsInSquare(targetDist); // checks all AI's whose allegiance is different from this AI's
		CheckNecro(targetDist); // enemy AIs check the necro. necro is a bit more intimidating than other targets.
		if (target == null) {
			if (!isEnemy) {
				agent.SetDestination(necromancer.transform.position);
				agent.stoppingDistance = 2;
				agent.speed = speed;
				transform.LookAt(necromancer.transform);
			} else if (agent.enabled == true && !inWave) {
				if (Vector3.Distance(transform.position, origin) > 1) {
					agent.SetDestination(origin);
					agent.speed = speed;
					transform.LookAt(origin);
					agent.stoppingDistance = 0;
				} else {
					agent.enabled = false;
				}
			}
		} else {
			agent.stoppingDistance = .2f;
			if (!isEnemy) {
				transform.parent = eManager.transform;
			} else if (aggro) {
				Alarm();
			}
		}
	}

	// runs a targeting check on all AI in my square, all adjacent/diagonally adjacent squares
	// takes in the distance to the current target and returns the distance to whatever the target is at the end of the function
	protected virtual float CheckAITargetsInSquare(float targetDist) {
		int unitGridX = ((int)transform.position.x - gManager.xGridOrigin) / 10;
		int unitGridY = ((int)transform.position.z - gManager.yGridOrigin) / 10;
		int checkingX, checkingY;
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
	protected virtual void CheckNecro(float targetDist) {
		if (isEnemy) {
			float necroDist = Vector3.Distance (necromancer.transform.position, transform.position);
			if (necroDist < targetDist * necroAggroModifier && necroDist < aggroRange) {
				agent.enabled = true;
				target = necromancer;
				agent.destination = necromancer.transform.position;
			}
		}
		NecromancerBoss nBoss = GameObject.FindObjectOfType<NecromancerBoss> ();
		if (nBoss != null && !isEnemy) {
			GameObject necroBoss = nBoss.gameObject;
			float necroDist = Vector3.Distance (necroBoss.transform.position, transform.position);
			if (necroDist < targetDist * necroAggroModifier && necroDist < aggroRange) {
				agent.enabled = true;
				target = necroBoss;
				agent.destination = necroBoss.transform.position;
			}
		}
	}

	protected void Melee(Collision coll, float damage = 1) {
		if (coll.gameObject.tag != "AI" && coll.gameObject.name != "Necromancer" && coll.gameObject.name != "Necromancer Boss") {
			return;
		}
		AudioSource.PlayClipAtPoint (gManager.scratch, transform.position);
		if ((coll.gameObject.name == "Necromancer" && isEnemy) || (coll.gameObject.name == "Necromancer Boss")) {
			if (isEnemy) {
				coll.gameObject.GetComponent<PlayerController> ().TakeHit (coll.gameObject);
				meleeTimer = 0;
			} else {
				coll.gameObject.GetComponent<NecromancerBoss> ().TakeHit ();
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

	protected virtual void OnCollisionStay(Collision coll) {
		if (coll.gameObject == target) {
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
			if (paused) {
				resumeSpeed = agent.speed;
			} else {
				agent.speed = resumeSpeed;
			}
		}
	}

	public virtual void Infect() {
		if (isEnemy && !immune) {
			root = 0;
			isEnemy = false;
			transform.parent = eManager.transform;
			eManager.peasantCount--;
			necromancer.GetComponent<PlayerController>().minionCount++;
			target = null;
			SwitchTargets();
			SpriteRenderer[] x = GetComponentsInChildren<SpriteRenderer> ();
			for (int i = 0; i < x.Length; i++) {
				if (x[i].name == "Stun") {
					Destroy (x[i].gameObject);
				}
			}
			if (name == "Archer") {
				rend.sprite = cSprites [3];
			}
			if (name == "Peasant") {
				rend.sprite = cSprites [3];
			}
			hp *= convertedHp;
			meleeDamage *= convertedStrength;
		}
	}

	public virtual void Root() {
		if (isEnemy) {
			root = rootPersistence;
			agent.speed = 0;
		}
	}

	public virtual void Damage(float damage) {
		hp -= damage;
		if (hp <= 0) { 
			if (isEnemy) {
				eManager.peasantCount -= 1;
			} else {
				necromancer.GetComponent<PlayerController>().minionCount -= 1;
			}
			gManager.EnemyDeath (transform.position, name, isEnemy);
			Death();
			Destroy (this.gameObject);
		}
	}

	protected virtual void Death() {}

	protected void SetToElite() {
		isElite = true;
		immune = true;
		GameObject elite = new GameObject ();
		elite.transform.parent = this.transform;
		elite.transform.localPosition = new Vector3 (0, 0, 0);
		elite.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Textures/Elite Symbol");
		elite.transform.localEulerAngles = new Vector3 (90, 0, 0);
		elite.name = "Elite Symbol";
	}

	public void PauseAI() {
		paused = true;
		resumeAcceleration = agent.acceleration;
		agent.acceleration = 1000;
		resumeSpeed = agent.speed;
		agent.speed = 0;
	}

	public void UnPauseAI () {
		paused = false;
		agent.speed = resumeSpeed;
		agent.acceleration = resumeAcceleration;
		resumeSpeed = speed;
	}

	private void Alarm() {
		new GameObject().AddComponent<Alarm>().init(transform.position);
	}

	public void Alert() {
		if (target == null) {
			agent.enabled = true;
			agent.destination = necromancer.transform.position;
			target = necromancer;
			aggroRange += 100;
			talert = 1;
		}
	}
}
