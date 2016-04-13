using UnityEngine;
using System.Collections;

public class AIBehavior : MonoBehaviour {

	// PARAMETERS
	protected Color allyColor, enemyColor;
	public float maxHP;
	protected float speed, infectionCost, switchDirThreshold, meleeThreshold, meleeDamage, switchDirTimer = 0, meleeTimer = 0, root = 0, hp;

	protected GameObject necromancer, target;
	protected GameManager gManager;
	protected EnemyManager eManager;
	protected Material material;
	protected NavMeshAgent agent;
	public bool isEnemy = true;


	// Use this for initialization
	protected void init(GameManager gMan, EnemyManager owner, string textureName, float x, float y, float xScale, float yScale) {
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
		necromancer = target = GameObject.Find("Necromancer");
		hp = maxHP;
		agent.speed = speed;
		agent.radius = .1f;
	}


	protected void Update() {
		meleeTimer += Time.deltaTime;
		switchDirTimer += Time.deltaTime;
		if (root > 0) {
			root -= Time.deltaTime;
			if (root <= 0) {
				agent.speed = 1.5f;
			}
		} else {
			MoveToward ();
		}
	}


	void LateUpdate() {
		transform.position = new Vector3 (transform.position.x, .01f, transform.position.z);
		gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		print (necromancer.GetComponent<PlayerController> ().minionCount + "   " + eManager.peasantCount);
	}


	protected void MoveToward() {
		if ((isEnemy && eManager.necromancerController.minionCount <= 0) || (!isEnemy && eManager.peasantCount <= 0)) {
			agent.destination = necromancer.transform.position;
			target = necromancer;
		} else if (meleeTimer >= meleeThreshold) {
			SwitchTargets ();
		} else {
			SwitchTargets ();
		}

		//Vector3 direction = new Vector3 (target.transform.position.x-transform.position.x, 0, target.transform.position.z-transform.position.z);
		//float directionMagnitude = Mathf.Sqrt (Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.z, 2));
		//direction = new Vector3 (direction.x / directionMagnitude, direction.z / directionMagnitude);
		//transform.Translate (direction.x*speed*Time.deltaTime, direction.y*speed*Time.deltaTime, 0);
	}


	protected virtual void SwitchTargets() {
		float targetDist = 100;
		if (target == null && isEnemy) {
			agent.destination = necromancer.transform.position;
			target = necromancer;
		}
		if (target != null) {
			targetDist = Vector3.Distance(target.transform.position, transform.position);
			// Mathf.Sqrt(
			// Mathf.Pow ((target.gameObject.transform.position.x - transform.position.x), 2) + 
			// Mathf.Pow ((target.gameObject.transform.position.z - transform.position.z), 2));
		} 
		foreach (AIBehavior AI in FindObjectsOfType<AIBehavior>()) {
			if (AI.isEnemy != isEnemy) {
				float AIDist = Vector3.Distance(AI.transform.position, transform.position);
//				Mathf.Sqrt (
//				Mathf.Pow ((AI.gameObject.transform.position.x - transform.position.x), 2) +
//				Mathf.Pow ((AI.gameObject.transform.position.z - transform.position.z), 2));
				if (AIDist < targetDist) {
					targetDist = AIDist;
					agent.destination = AI.transform.position;
				}
			}
		}
	}

	protected void Melee(Collision coll) {
		if (coll == null || coll.gameObject.name == "Terrain") {
			return;
		}
		AIBehavior unit = coll.gameObject.GetComponent<AIBehavior> ();
		if (coll.gameObject.name == "Necromancer") {
			if (isEnemy) {
				coll.gameObject.GetComponent<PlayerController> ().TakeHit (coll.gameObject);
				meleeTimer = 0;
			}
		} else if (unit.isEnemy != isEnemy) {
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


	public void Infect() {
		if (isEnemy) {
			eManager.peasantCount--;
			necromancer.GetComponent<PlayerController> ().minionCount++;
			isEnemy = false;
			SwitchTargets ();
			material.color = allyColor;
			target = null;
			hp = maxHP;
		}
	}


	protected void OnCollision(Collision coll) {
		if (meleeTimer > meleeThreshold) {
			Melee (coll);
		}
	}

	public void Root() {
		root = 1.5f;
		speed = 0;
	}

	public void Damage(float damage) {
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
