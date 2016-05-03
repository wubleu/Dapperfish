using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {

	float chargespeed = 100, wait = 0.5f, charge = 0.3f, caggro = 4, timer = 0, chargecd = 1f;
	public int mode = 0;
	Vector3 start;
	KnightMelee melee;

	public void initKnight(GameManager gMan, EnemyManager owner, PlayerController necro, params bool[] isElite) {
		allyColor = new Color(0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 4.5f;
		maxHP = hp = 20;
		meleeThreshold = 2;
		meleeDamage = 25;
		aggroRange = 9;
		necroAggroModifier = 2;
		immune = true;
		animmax = .3f;
		animcount = animmax;
		if (isElite.Length == 0) {
			base.init (gMan, owner, necro);
		} else if (isElite.Length == 1) {
			base.init (gMan, owner, necro, isElite[0]);
		} else {
			base.init (gMan, owner, necro, isElite[0], isElite[1]);
		} 
		gManager = gMan;

		melee = new GameObject().AddComponent<KnightMelee>();
		melee.transform.parent = transform;
		melee.transform.localPosition = new Vector3(0, 0, 0.75f);
	}

	protected override void Update() {
		if (paused) {
			return;
		}
		if (meleeTimer > 0) {
			if ((meleeTimer -= Time.deltaTime) <= meleeThreshold - 0.5f) {
				rend.sprite = cSprites[3];
			} else {
				rend.sprite = cSprites[2];
			}
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
		if (target != null) {
			if (mode == 0) { // walking
				SwitchTargets();
				if (timer > 0) {
					timer -= Time.deltaTime;
				}
				if (target != null && target.name == "Necromancer" && timer <= 0 && Vector3.Distance(transform.position, target.transform.position) < caggro) {
					start = target.transform.position;
					mode = 1;
					timer = wait;
					agent.speed = 0;
				}
			} else if (mode == 1) { // waiting
				if ((timer -= Time.deltaTime) <= 0){
					mode = 2;
					timer = charge;
					agent.speed = chargespeed;
					rend.sprite = cSprites [8];

					float speed = Vector3.Distance(start, target.transform.position) / wait, angle = Mathf.PI / 2 - Mathf.Atan2(target.transform.position.x - start.x, target.transform.position.z - start.z);
					start = target.transform.position + speed * 0.25f * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
					angle = Mathf.PI / 2 - Mathf.Atan2(start.x - transform.position.x, start.z - transform.position.z);
					agent.SetDestination(transform.position + chargespeed * charge * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));

					melee.Charge();
				}

				if (timer < wait / 4) {
					rend.sprite = cSprites [4];
				} else if (timer < wait / 2) {
					rend.sprite = cSprites [5];
				} else if (timer < 3 * wait / 4) {
					rend.sprite = cSprites [6];
				}
			} else if ((timer -= Time.deltaTime) <= 0) { // charging
				rend.sprite = cSprites [3];
				mode = 0;
				timer = chargecd;
				agent.speed = speed;
				melee.Disable();
			}
		} else {
			SwitchTargets();
			mode = 0;
		}
		if (root > 0 && (root -= Time.deltaTime) <= 0) {
			agent.speed = speed;
		}
	}

	protected override void OnCollisionStay(Collision coll) {
		if (mode == 2 || root > 0) {
			return;
		}
		if (meleeTimer <= 0 && (coll.collider.gameObject.name == "Necromancer" || 
			(coll.gameObject.tag == "AI" && !coll.collider.GetComponent<AIBehavior>().isEnemy))) {
			rend.sprite = cSprites [1];
			melee.Enable();
			meleeTimer = meleeThreshold;
		}
	}

	public override void Root() {
		root = rootPersistence;
		agent.speed = 0;
		melee.Disable();
	}

	protected override void Death() {
		new GameObject().AddComponent<HealthPack>().init(transform.position);
	}
}