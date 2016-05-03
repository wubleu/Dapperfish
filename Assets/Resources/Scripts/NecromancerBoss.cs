using UnityEngine;
using System.Collections;

public class NecromancerBoss : MonoBehaviour {

	// PARAMETERS
	float teleportCooldown = 5f, minSummonCooldown = 5f, maxSummonCooldown = 15f, summonRandomizeRange = 4f, damageCooldown = 20f,
		rootDuration = 1f, maxHp = 500f, aggroToAIRange = 4;

	float rootTimer = 0f, teleportCooldownTimer = 0f, summonCooldownTimer = 0f, summonCooldown = 5f, damageCooldownTimer = 0f,
			teleportGridXLoc, teleportGridYLoc, hp;
	int minionCount = 0;
	bool paused = false, waiting = true;
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
		target = necromancer.gameObject;
		tag = "Untagged";
		hp = maxHp;
		cSprites = Resources.LoadAll<Sprite> ("Textures/Boss Sprite Sheet");
		rend = GetComponentInChildren<SpriteRenderer>();
		rend.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		rend.sprite = cSprites [0];
		Rigidbody rbody = gameObject.GetComponent<Rigidbody> ();
		rbody.isKinematic = true;

		teleportGridXLoc = ((int)transform.position.x - 15) / 5;
		teleportGridYLoc = ((int)transform.position.z + 10) / 5;
	}
	
	// Update is called once per frame
	void Update () {
		if (paused || (rootTimer < rootDuration && (rootTimer += Time.deltaTime) < rootDuration)) {
			return;
		}
		if (waiting) {
			if (transform.position.x - necromancer.transform.position.x < 10) {
				waiting = false;
			} else {
				return;
			}
		}

		if ((teleportCooldownTimer += Time.deltaTime) > teleportCooldown && 
			Vector3.Distance(transform.position, target.transform.position) < aggroToAIRange) {
			Teleport ();
			teleportCooldownTimer = 0;
		}
		if ((summonCooldownTimer += Time.deltaTime) > summonCooldown) {
			Vector3 summonCenter = FindCenterOfMinions ();
			float xOffset, yOffset, modifiedRandRange;
			if (Random.Range (0, 2) == 0) {
				xOffset = Random.Range (-summonRandomizeRange, summonRandomizeRange);
				modifiedRandRange = summonRandomizeRange - Mathf.Abs (xOffset / 2);
				yOffset = Random.Range (-modifiedRandRange, modifiedRandRange);
			} else {
				yOffset = Random.Range (-summonRandomizeRange, summonRandomizeRange);
				modifiedRandRange = summonRandomizeRange - Mathf.Abs (yOffset / 2);
				xOffset = Random.Range (-modifiedRandRange, modifiedRandRange);
			}
			Vector3 summonLoc = new Vector3(summonCenter.x + xOffset, transform.position.y, summonCenter.z + yOffset);
			Abilities.Summon (summonLoc.x, summonLoc.y, summonLoc.z);
			if (Vector3.Distance (summonLoc, transform.position) < 3) {
				Teleport ();
				teleportCooldownTimer = 0;
			}
			summonCooldown = Random.Range (minSummonCooldown, maxSummonCooldown);
			summonCooldownTimer = 0;
		}
		SetTarget ();
		transform.LookAt (target.transform);
	}

	void SetTarget() {
		float targetDist = Vector3.Distance (transform.position, target.transform.position);
		if (target == null || Vector3.Distance (transform.position, necromancer.transform.position) < targetDist) {
			target = necromancer.gameObject;
		} else if (targetDist > aggroToAIRange*1.5 || (target != necromancer.gameObject && targetDist > aggroToAIRange)) {
			int unitGridX = ((int)transform.position.x - gManager.xGridOrigin) / 10;
			int unitGridY = ((int)transform.position.z - gManager.yGridOrigin) / 10;
			int checkingX, checkingY;
			for (int x = -1; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					checkingX = x + unitGridX;
					checkingY = y + unitGridY;
					if ((checkingX >= 0 && checkingX < gManager.xDimension) && (checkingY >= 0 && checkingY < gManager.yDimension)) {
						foreach (AIBehavior AI in gManager.enemyGrid[checkingX, checkingY]) {
							if (AI != null && !AI.isEnemy) {
								float AIDist = Vector3.Distance (AI.transform.position, transform.position);
								if (AIDist < aggroToAIRange) {
									target = AI.gameObject;
									targetDist = AIDist;
								}
							}
						}
					}
				}
			}
		}
	}

	void Teleport() {
		float newX = Random.Range(0, 4), newY = Random.Range(0, 4);
		while ((newX == teleportGridXLoc && (newY <= teleportGridYLoc + 1 && newY >= teleportGridYLoc - 1)) || 
			(newY == teleportGridYLoc && (newX <= teleportGridXLoc + 1 && newX >= teleportGridXLoc - 1)) || 
			(newX == 0 && (newY == 0 || newY == 3)) || (newY == 0 && (newX == 0 || newX == 3))  ) {
			newX = Random.Range (0, 4);
			newY = Random.Range(0, 4);
		}
		Abilities.Blink(transform, 0, new Vector3 (((newX * 5f) + 15f) + Random.Range (0f, 5f), transform.position.y, 
			((newY * 5f) - 10f) + Random.Range (0f, 5f)));
		teleportGridXLoc = newX;
		teleportGridYLoc = newY;
	}
					
	Vector3 FindCenterOfMinions() {
		float xSum = necromancer.transform.position.x, ySum = necromancer.transform.position.z, xDivisor = 1, yDivisor = 1;
		foreach (AIBehavior AI in FindObjectsOfType<AIBehavior>()) {
			if (!AI.isEnemy) {
				xDivisor++;
				yDivisor++;
				xSum += AI.transform.position.x;
				ySum += AI.transform.position.z;
			}
		}
		return new Vector3 (xSum/xDivisor, necromancer.transform.position.y, ySum/yDivisor);
	}

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
