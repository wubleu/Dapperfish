using UnityEngine;
using System.Collections;

public class NecromancerBoss : MonoBehaviour {

	// PARAMETERS
	float teleportCooldown = 5f, summonCooldown = 10f, damageCooldown = 20f, rootDuration = 1f, maxHp = 10f, aggroToAIRange = 4;

	float rootTimer = 0f, teleportCooldownTimer = 0f, summonCooldownTimer = 9.5f, damageCooldownTimer = 0f, hp;
	int minionCount = 0;
	bool paused = false;
	GameObject target;
	GameManager gManager;
	EnemyManager eManager;
	PlayerController necromancer;
	SpriteRenderer rend;
	Sprite[] cSprites;


	public void initNecroBoss (GameManager gMan, EnemyManager owner, PlayerController necro) {
		gManager = gMan;
		eManager = owner;
		necromancer = necro;
		tag = "Untagged";
		hp = maxHp;
		cSprites = Resources.LoadAll<Sprite> ("Textures/Boss Sprite Sheet");
		rend = GetComponentInChildren<SpriteRenderer>();
		rend.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		rend.sprite = cSprites [0];
		Rigidbody rbody = gameObject.GetComponent<Rigidbody> ();
		rbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (paused || (rootTimer < rootDuration && (rootTimer += Time.deltaTime) < rootDuration)) {
			return;
		}

		// JUST BEING USED FOR TESTING PURPOSES NOW- not at all reflective of final behavior. just tweaking summon a bit.
		if ((teleportCooldownTimer += Time.deltaTime) > teleportCooldown) {
			minionCount += Abilities.Summon (transform.position.x + Random.Range (-6f, 6f), transform.position.y, transform.position.z + Random.Range (-6f, 6f));
			teleportCooldownTimer = 0;
		}

	}

//	void SetTarget() {
//		float targetDist = Vector3.Distance (transform.position, target.transform.position);
//		if (targetDist > aggroToAIRange * 1.5f || (target != necromancer.gameObject && targetDist > aggroToAIRange)) {
//			int unitGridX = ((int)transform.position.x - gManager.xGridOrigin) / 10;
//			int unitGridY = ((int)transform.position.z - gManager.yGridOrigin) / 10;
//			int checkingX, checkingY;
//			for (int x = -1; x < 2; x++) {
//				for (int y = -1; y < 2; y++) {
//					checkingX = x + unitGridX;
//					checkingY = y + unitGridY;
//					if ((checkingX >= 0 && checkingX < gManager.xDimension) && (checkingY >= 0 && checkingY < gManager.yDimension)) {
//						foreach (AIBehavior AI in gManager.enemyGrid[checkingX, checkingY]) {
//							if (AI != null && !AI.isEnemy) {
//								float AIDist = Vector3.Distance (AI.transform.position, transform.position);
//								if (AIDist < aggroToAIRange) {
//									target = AI.gameObject;
//									targetDist = AIDist;
//								}
//							}
//						}
//					}
//				}
//			}
//		}
//		return targetDist;
//	}

	public void Die() {
		Destroy (gameObject);
	}

	public void Damage(float damage) {
		hp -= damage;
		if (hp <= 0) {
			Die ();
		}
	}

	public void Root() {
		rootTimer = 0;
	}

	public void PauseBoss() {
		paused = true;
	}

	public void UnPauseBoss() {
		paused = false;
	}
}
