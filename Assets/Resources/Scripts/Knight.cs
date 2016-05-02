using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {

	float chargespeed = 100, normalspeed = 6, wait = 0.5f, charge = 0.4f, caggro = 4, timer = 0, chargecd = .5f;
	int mode = 0;
	Vector3 start;
	KnightMelee melee;

	public void initKnight(GameManager gMan, EnemyManager owner, PlayerController necro, params bool[] isElite) {
		allyColor = new Color(0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = normalspeed;
		maxHP = hp = 25;
		meleeThreshold = 1;
		meleeDamage = 25;
		aggroRange = 9;
		necroAggroModifier = 2;
		immune = true;
		animmax = .3f;
		animcount = animmax;
		if (isElite.Length > 0) {
			if (isElite [0] == true) {
				base.init (gMan, owner, necro, true);
			} else {
				base.init (gMan, owner, necro);
			}
			if (isElite.Length > 1) {
				inWave = isElite[1];
				agent.destination = necromancer.transform.position;
			}
		} else {
			base.init (gMan, owner, necro);
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
		if (inWave) {
			if (Vector3.Distance (transform.position, necromancer.transform.position) < aggroRange) {
				inWave = false;
			} else {
				return;
			}
		}
		if (target != null) {
			if (mode == 0) { // walking
				SwitchTargets ();
				base.Update ();
				if (target != null && target.name == "Necromancer" && Vector3.Distance(transform.position, target.transform.position) < caggro) {
					start = target.transform.position;
					mode = 1;
					timer = wait;
					rend.sprite = cSprites [3];
					agent.speed = 0;
				}
			} else if (mode == 1) { // waiting
				if ((timer -= Time.deltaTime) <= 0){
					mode = 2;
					timer = charge;
					agent.speed = chargespeed;
					rend.sprite = cSprites [7];

					float speed = Vector3.Distance(start, target.transform.position) / wait, angle = Mathf.PI / 2 - Mathf.Atan2(target.transform.position.x - start.x, target.transform.position.z - start.z);
					start = target.transform.position + speed * 0.25f * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
					angle = Mathf.PI / 2 - Mathf.Atan2(start.x - transform.position.x, start.z - transform.position.z);
					agent.SetDestination(transform.position + chargespeed * charge * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
				}

				if (timer < wait / 4) {
					rend.sprite = cSprites [4];
				} else if (timer < wait / 2) {
					rend.sprite = cSprites [5];
				} else if (timer < 3 * wait / 4) {
					rend.sprite = cSprites [6];
				}
			} else if ((timer -= Time.deltaTime) <= 0) { // charging
				rend.sprite = cSprites [8];
				mode = 3;
				timer = chargecd;
				agent.speed = 0;
				melee.Charge();
			} else if (mode == 3) {
				if ((timer-= Time.deltaTime) <= 0) {
					melee.Disable();
					mode = 0;
					agent.speed = normalspeed;
					rend.sprite = cSprites [0];
				}
			}
		} else {
			SwitchTargets();
			base.Update();
			mode = 0;
			agent.speed = normalspeed;
		}
	}

	void OnCollisionStay(Collision coll) {
		if (mode == 3) {
			return;
		}
		if (meleeTimer >= meleeThreshold && (coll.collider.gameObject.name == "Necromancer" || 
			(coll.gameObject.gameObject.tag == "AI" && !coll.collider.GetComponent<AIBehavior>().isEnemy))) {
			rend.sprite = cSprites [0];
			melee.Enable();
			meleeTimer = 0;
		}
	}
}