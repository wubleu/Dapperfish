﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	public float hp = 120;
	float speed = 1.1f;
	Color necroColor = new Color(120f / 256f, 0f / 256f, 100f / 256f);
	float[] clocks = new float[5] {0, 0, 0, 0, 0}, timers = new float[5] {0.5f, 3, 3, 3, 3};
	Melee melee;
	// spellShotInterval = .2f

	// CONTROLS PARAMETERS
	KeyCode[] controls = new KeyCode[5] {
		KeyCode.Mouse0, // auto-attack
		KeyCode.Mouse1, // blight
		KeyCode.LeftShift, // root
		KeyCode.LeftControl, // damage
		KeyCode.Space // blink
	};

	GameObject background;

	public int minionCount;
//	float shotClock;
	GameManager gManager;
	InfectionBar infectionBar;
	HealthBar healthBar;
	public EnemyManager eManager;
	Material playerMaterial;


	// Use this for initialization
	public void init (GameManager owner, EnemyManager eMan) {
		eManager = eMan;
		gManager = owner;
		gManager.MakeSprite (gameObject, "Circle", gManager.transform, 0, 0, .5f, .5f, 200);
		gameObject.name = "Necromancer";
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerController";
		playerMaterial = GetComponent<SpriteRenderer>().material;
		playerMaterial.color = necroColor;
		gameObject.AddComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<Rigidbody> ().freezeRotation = true;
		gameObject.AddComponent<SphereCollider> ().radius = .3f;
		gameObject.AddComponent<NavMeshAgent> ().updateRotation = false;

		GameObject infectBar = new GameObject();
		infectionBar = infectBar.AddComponent<InfectionBar> ();
		infectionBar.init (gManager, this);
		GameObject hpBar = new GameObject();
		healthBar = hpBar.AddComponent<HealthBar>();
		healthBar.init (gManager, this, hp);

		// TEMPORARY CAMERA FOLLOW
		GameObject.Find("Main Camera").transform.parent = transform;

//		shotClock = 0;
		minionCount = 0;

		melee = new GameObject().AddComponent<Melee>();
		melee.transform.parent = transform;
		melee.transform.position = new Vector3(0, 0, 0);
		background = GameObject.Find ("Background");
	}


	// Update is called once per frame
	void Update () {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;

		for (int i = 0; i < 5; i++) {
			if (clocks[i] > 0) {
				clocks[i] -= Time.deltaTime;
			} else if (Input.GetKeyDown(controls[i])) {
				clocks[i] = timers[i];
				print("Ability " + i);
				switch (i) {
					case 0: // auto-attack
						melee.Enable();
						break;
					case 1: // blight
						Abilities.Blight(mouse);
						infectionBar.infectionCharge = 0;
						break;
					case 2: // root
						Abilities.Root(mouse);
						break;
					case 3: // damage
						Abilities.Damage(transform.position, Mathf.PI / 2 + Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z));
						break;
					case 4: // blink
						float a = Mathf.PI / 2 + Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z);
						Abilities.Blink(a, transform);
						break;
				}
				    
			}
		}
		// moves player by speed*Time.deltaTime based on WASD
		if (Input.GetKey (KeyCode.A) ) {
			transform.Translate (-speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.D) ) {
			transform.Translate (speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.W) ) {
			transform.Translate (0, speed*Time.deltaTime, 0);
		} if (Input.GetKey (KeyCode.S) ) {
			transform.Translate (0, -speed*Time.deltaTime, 0);
		}
		// if space key is down and enough time has passed, fires spellShot
//		shotClock += Time.deltaTime;
//		if (Input.GetKey (KeyCode.Space) && shotClock > spellShotInterval) {
//			GameObject shot = new GameObject ();
//			shot.AddComponent<SpellShot> ().init (this, gManager);
//			shotClock = 0;
//		}
//		if (Input.GetMouseButton(0) && infectionBar.infectionCharge > 50) {
//			GameObject blight = new GameObject ();
//			blight.AddComponent<Blight>().init (this, gManager, infectionBar.infectionCharge);
//			infectionBar.infectionCharge = 0;
//		}
	}


	public void TakeHit(GameObject collObj) {
		print ("taking hit");
		hp -= 1;
		if (hp <= 0) {
			gManager.Death ();
		}
	}
}
