using UnityEngine;
using System.Collections;

public class NecromancerBoss : MonoBehaviour {

	// PARAMETERS
	float teleportCooldown = 3f, minSummonCooldown = 10f, maxSummonCooldown = 18f, summonRandomizeRange = 4f, 
	damageCooldown = 20f, rootDuration = 1f, maxHp = 150f, aggroToAIRange = 4, shootingPeriod = 2f, shootingFreq = .26f, 
	shootingAngle = 30f, shootingPause = .6f, shootChance = 40, castleftcd = .5f, castrightcd = .5f, 
	damageExplosionWait = .2f, damageExplosionCount = 8, damageExplosionTriggerIntervals = 30;

	float rootTimer = 0f, teleportCooldownTimer = 0f, summonCooldownTimer = 0f, summonCooldown = 5f, damageCooldownTimer = 0f, 
		damageExplosionTimer = 0, damageExplosionsSoFar = 0, shootingTimer, nextShotTimer, teleportGridXLoc, teleportGridYLoc, hp;
	float damageExplosionAngle;
	float damageExplosionStartAngle;
	int minionCount = 0;
	bool paused = false;
	bool exploding = false;
	bool castleft = false;
	bool castright = false;
	bool begin = false;
	public bool waiting = true;
	GameObject target;
	GameManager gManager;
	EnemyManager eManager;
	PlayerController necromancer;
	SpriteRenderer rend;
	Sprite[] cSprites;
	public bool dead = false;
	GameObject body;
	SpriteRenderer bodymodel;
	GameObject leftarm;
	SpriteRenderer lamodel;
	GameObject rightarm;
	SpriteRenderer ramodel;

	public void initNecroBoss (GameManager gMan, EnemyManager owner, PlayerController necro) {
		gManager = gMan;
		eManager = owner;
		necromancer = necro;
		target = necromancer.gameObject;
		tag = "Untagged";
		hp = maxHp;
		shootingTimer = shootingPeriod;
		nextShotTimer = shootingFreq;
		cSprites = Resources.LoadAll<Sprite> ("Textures/Boss Sprite Sheet");
		rend = GetComponentInChildren<SpriteRenderer>();
		rend.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		rend.sprite = cSprites [0];
		rend.sortingOrder = 3;

		body = new GameObject ();
		bodymodel = body.AddComponent<SpriteRenderer> ();
		bodymodel.name = "Boss Body Model";
		bodymodel.transform.parent = rend.gameObject.transform;
		bodymodel.transform.localPosition = new Vector3(0, -0.16f, 0);
		bodymodel.transform.localEulerAngles = new Vector3(0, 0, 0);
		bodymodel.transform.localScale = new Vector3(1, 1, 1);
		//bodymodel.sortingLayerName = "PlayerController";
		bodymodel.sortingOrder = 0;

		leftarm = new GameObject ();
		lamodel = leftarm.AddComponent<SpriteRenderer> ();
		lamodel.name = "Left Arm Model";
		lamodel.transform.parent = rend.gameObject.transform;
		lamodel.transform.localPosition = new Vector3(0.25f,-0.331f,0);
		lamodel.transform.localEulerAngles = new Vector3(0, 0, 0);
		lamodel.transform.localScale = new Vector3(1, 1, 1);
		lamodel.sortingOrder = 2;

		rightarm = new GameObject ();
		ramodel = rightarm.AddComponent<SpriteRenderer> ();
		ramodel.name = "Right Arm Model";
		ramodel.transform.parent = rend.gameObject.transform;
		ramodel.transform.localPosition = new Vector3(-0.319f,-0.111f,0);
		ramodel.transform.localEulerAngles = new Vector3(0, 0, 310);
		ramodel.transform.localScale = new Vector3(1, 1, 1);
		ramodel.sortingOrder = 1;
		ramodel.flipX = true;


		Rigidbody rbody = gameObject.GetComponent<Rigidbody> ();
		rbody.isKinematic = true;

		teleportGridXLoc = ((int)transform.position.x - 15) / 5;
		teleportGridYLoc = ((int)transform.position.z + 10) / 5;
	}
	
	// Update is called once per frame
	void Update () {
		if (castleft) {
			castleftcd -= Time.deltaTime;
		}
		if (castleftcd <= 0) {
			castleft = false;
			lamodel.sprite = cSprites [14];
			lamodel.transform.localPosition = new Vector3(0.25f,-0.331f,0);
			castleftcd = .5f;
		}
		if (castright) {
			castrightcd -= Time.deltaTime;
		}
		if (castrightcd <= 0) {
			castright = false;
			ramodel.sprite = cSprites [14];
			ramodel.transform.localPosition = new Vector3(-0.319f,-0.111f,0);
			ramodel.transform.localEulerAngles = new Vector3(0, 0, 310);
			castrightcd = .5f;
		}

		if (paused || (rootTimer < rootDuration && (rootTimer += Time.deltaTime) < rootDuration)) {
			return;
		}
		if (rootTimer > rootDuration) {
			SpriteRenderer[] x = GetComponentsInChildren<SpriteRenderer> ();
			for (int i = 0; i < x.Length; i++) {
				if (x[i].name == "Stun") {
					Destroy (x[i].gameObject);
				}
			}
		}
		if (waiting) {
			return;
		}
		if (!waiting && !begin) {
			Activate ();
			begin = true;
		}

		if (exploding == true) {
			DamageExplosion ();
			ramodel.sprite = cSprites [11];
			castright = true;
			castleft = true;
			castrightcd = .5f;
			castleftcd = .5f;
			ramodel.transform.localPosition = new Vector3(-0.292f,-0.339f,0);
			ramodel.transform.localEulerAngles = new Vector3(0, 0, 3);
			lamodel.sprite = cSprites [11];
			lamodel.transform.localPosition = new Vector3(0.286f,-0.498f,0);
		}

		if (shootingTimer < shootingPeriod && (shootingTimer += Time.deltaTime) > 0) { 
			if ((nextShotTimer += Time.deltaTime) > shootingFreq) {
				ramodel.sprite = cSprites [11];
				castright = true;
				castrightcd = .5f;
				ramodel.transform.localPosition = new Vector3(-0.292f,-0.339f,0);
				ramodel.transform.localEulerAngles = new Vector3(0, 0, 3);
				print("shot");
				nextShotTimer -= shootingFreq;
				Abilities.Bullet (transform.position, 
					-Mathf.Deg2Rad * (transform.eulerAngles.y + Random.Range (-shootingAngle, shootingAngle)) + Mathf.PI / 2, true);
			}
		}

		if (shootingTimer > 0) {
			if (target == null) {
				SetTarget ();
			}
			if (((teleportCooldownTimer += Time.deltaTime) > teleportCooldown &&
			   Vector3.Distance (transform.position, target.transform.position) < aggroToAIRange) ||
			   teleportCooldownTimer > teleportCooldown * 2) {
				Teleport ();
				teleportCooldownTimer = 0;
				if (Random.Range (0, 100) < shootChance) {
					shootingTimer = -shootingPause;
				}
			}

			if ((summonCooldownTimer += Time.deltaTime) > summonCooldown) {
				CastSummon ();
				lamodel.sprite = cSprites [13];
				lamodel.transform.localPosition = new Vector3(0.286f,-0.498f,0);
				castleft = true;
				castleftcd = .5f;
				if (Random.Range (0, 100) < shootChance) {
					shootingTimer = -shootingPause;
				}
			}
		}

		SetTarget ();
		transform.LookAt (target.transform);
	}

	void SetTarget() {
		if (target == null) {
			target = necromancer.gameObject;
		}
		float targetDist = Vector3.Distance (transform.position, target.transform.position);
		if (Vector3.Distance (transform.position, necromancer.transform.position) < targetDist) {
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

	void CastSummon() {
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

	void DamageExplosion() {
		if ((damageExplosionTimer += Time.deltaTime) > damageExplosionWait) {
			damageExplosionTimer = 0;
			damageExplosionsSoFar++;
			Abilities.Damage (transform.position, transform.eulerAngles.y * Mathf.PI, true);
			damageExplosionAngle += damageExplosionAngle + 360 / damageExplosionCount;
		}
		if (damageExplosionsSoFar == damageExplosionCount) {
			damageExplosionsSoFar = 0;
			exploding = false;
		}
	}

	public void Die() {
		GameObject death = new GameObject();
		death.transform.position = transform.position;
		death.transform.localScale = transform.localScale;
		death.transform.localEulerAngles = new Vector3 (90, 0, 0);
		death.AddComponent<SpriteRenderer>();
		Animator anim = death.AddComponent<Animator>();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Boss Death Controller");
		Destroy(this.gameObject);
		Destroy(death, 1.5f);
	}

	public void Damage(float damage) {
		float prevHP = hp%damageExplosionTriggerIntervals;
		hp -= damage;
		if (prevHP <= 10 && hp % damageExplosionTriggerIntervals >= damageExplosionTriggerIntervals - 10) {
			exploding = true;
			damageExplosionStartAngle = transform.eulerAngles.y;
			damageExplosionTimer = damageExplosionWait;
		}
		if (hp <= 0) {
			dead = true;
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

	public void Activate(){
		rend.sprite = cSprites [9];
		bodymodel.sprite = cSprites [10];
		ramodel.sprite = cSprites [14];
		lamodel.sprite = cSprites [14];
	}
}
