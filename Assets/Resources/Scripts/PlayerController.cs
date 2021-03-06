﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	public float maxhp = 50, hp, size = 1f, speed = 4f, castcd = .25f, currentY = 0, unlockTime = 0, rootDuration = 1f;
	public bool isMelee = false, casted = false, hasKey = false, hasFortKey = false, needsNav = false, destined = false;
	public float[] cd = new float[5] {5, 5, 15, 2, 0.5f}, timers = new float[5] {0, 0, 0, 0, 0}, ranges = new float[4] {10, 10, 10, 100}, area = new float[3] {3.4f, 3.56f, 2.43f};
	bool[] unlock = new bool[4] {false, false, false, false};
	protected Sprite[] cSprites;
	public Image[] icons = new Image[4];
	
	// CONTROLS PARAMETERS
	KeyCode[] controls = new KeyCode[5] {
		KeyCode.Mouse1, // blight
		KeyCode.LeftShift, // root
		KeyCode.F, // damage
		KeyCode.Space, // blink
		KeyCode.Mouse0 // auto-attack
	};

	public int minionCount = 0;
	float rootTimer = 0;
	public GameManager gManager;
	Melee melee;
	public EnemyManager eManager;
	bool paused = false;
	public AudioClip gun;


	//Sounds
	public AudioClip BlightClip, RootClip, AudioClip, DamageClip, BlinkClip, DeathClip;

	GameObject necromodel, rightarm, leftarm, body, shooter;
	SpriteRenderer lamodel, bodymodel, ramodel, spellRange, AOE;

	public void init (GameManager owner, EnemyManager eMan) {
		eManager = eMan;
		gManager = owner;
		gameObject.name = "Necromancer";

		hp = maxhp;

		cSprites = Resources.LoadAll<Sprite> ("Textures/Necromancer Sprite Sheet 0");

		gameObject.AddComponent<SphereCollider>().radius = size*.75f;
		NavMeshAgent nav = gameObject.AddComponent<NavMeshAgent>();
		nav.updateRotation = false;
		nav.radius = .25f;
		nav.height = 1;

		Rigidbody rigid = gameObject.AddComponent<Rigidbody>();
		rigid.isKinematic = true;
		rigid.freezeRotation = true;

		// Necromancer Model
		necromodel = new GameObject();
		necromodel.transform.parent = transform;
		necromodel.transform.localPosition = transform.position;

		SpriteRenderer model = new GameObject().AddComponent<SpriteRenderer>();
		model.name = "Necro Model";
		model.transform.parent = necromodel.transform;
		model.transform.localPosition = new Vector3(0, 1, 0);
		model.transform.localEulerAngles = new Vector3(90, 180, 0);
		model.transform.localScale = new Vector3(size, size, size);
		model.sortingLayerName = "PlayerController";
		model.sortingOrder = 4;
		model.sprite = cSprites [9];

		body = new GameObject ();
		bodymodel = body.AddComponent<SpriteRenderer> ();
		bodymodel.name = "Necro Body Model";
		bodymodel.transform.parent = necromodel.transform;
		bodymodel.transform.localPosition = new Vector3(0.03f, 1, 0.157f);
		bodymodel.transform.localEulerAngles = new Vector3(90, 180, 0);
		bodymodel.transform.localScale = new Vector3(size, size, size);
		bodymodel.sortingLayerName = "PlayerController";
		bodymodel.sortingOrder = 1;
		bodymodel.sprite = cSprites [10];

		leftarm = new GameObject ();
		lamodel = leftarm.AddComponent<SpriteRenderer> ();
		lamodel.name = "Left Arm Model";
		lamodel.transform.parent = necromodel.transform;
		lamodel.transform.localPosition = new Vector3(-0.136f, 1, 0.382f);
		lamodel.transform.localEulerAngles = new Vector3(90, 180, 0);
		lamodel.transform.localScale = new Vector3(size, size, size);
		lamodel.sortingLayerName = "PlayerController";
		lamodel.sortingOrder = 3;
		lamodel.sprite = cSprites [14];

		rightarm = new GameObject ();
		ramodel = rightarm.AddComponent<SpriteRenderer> ();
		ramodel.name = "Necro Right Arm Model";
		ramodel.transform.parent = necromodel.transform;
		ramodel.transform.localPosition = new Vector3(0.12f, 1, 0.29f);
		ramodel.transform.localEulerAngles = new Vector3(90, 180, 0);
		ramodel.transform.localScale = new Vector3(size, size, size);
		ramodel.sortingLayerName = "PlayerController";
		ramodel.sortingOrder = 2;
		ramodel.sprite = cSprites [19];

		shooter = new GameObject ();
		shooter.transform.parent = ramodel.transform;

		new GameObject().AddComponent<HealthBar>().init(hp, gameObject);
		for (int i = 1; i <= 4; i++) {
			icons[i - 1] = GameObject.Find("CD" + i).GetComponent<Image>();
		}

		GameObject.Find("Main Camera").transform.parent = transform;

		melee = new GameObject().AddComponent<Melee>();
		melee.transform.parent = transform;
		melee.transform.localPosition = new Vector3(0, 1, 0);

		spellRange = new GameObject().AddComponent<SpriteRenderer>();
		spellRange.color = Color.clear;
		spellRange.sprite = Resources.Load<Sprite>("Textures/SmoothHollowCircle");
		spellRange.transform.parent = transform;
		spellRange.transform.localPosition = new Vector3(0, 1, 0);
		spellRange.transform.eulerAngles = new Vector3(90, 0, 0);
		spellRange.name = "SpellRange";

		AOE = new GameObject().AddComponent<SpriteRenderer>();
		AOE.color = Color.clear;
		AOE.sprite = Resources.Load<Sprite>("Textures/BenCircle");
		AOE.transform.eulerAngles = new Vector3(90, 0, 0);
		AOE.name = "AOE";

		DamageClip = Resources.Load ("Sounds/Damage") as AudioClip;
		BlightClip = Resources.Load ("Sounds/Blight") as AudioClip;
		RootClip = Resources.Load ("Sounds/Root") as AudioClip;
		BlinkClip = Resources.Load ("Sounds/Blink") as AudioClip;
		DeathClip = Resources.Load ("Sounds/death") as AudioClip;
		gun = Resources.Load ("Sounds/gun") as AudioClip;

	}

	void Update () {
		if (paused || (rootTimer < rootDuration && (rootTimer += Time.deltaTime) > rootDuration)) {
			return;
		}
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		if (timers[4] > 0) {
			if ((timers[4] -= Time.deltaTime) <= .25f) {
				ramodel.sprite = cSprites [19];
				ramodel.transform.localPosition = new Vector3(0.12f, 1, 0.29f);
				if (!casted) {
					lamodel.transform.localPosition = new Vector3 (-0.136f, 1, 0.382f);
				}
			}
		}
		for (int i = 0; i <= 3; i++) {
			if (timers[i] > 0 && (timers[i] -= Time.deltaTime) < 0) {
				timers[i] = 0;
			}
			icons[i].fillAmount = 1 - (timers[i] / cd[i]);
		}
		if (casted) {
			castcd -= Time.deltaTime;
		}
		if (castcd <= 0) {
			casted = false;
			castcd = .25f;
			lamodel.transform.localPosition = new Vector3(-0.136f, 1, 0.382f);
			lamodel.sprite = cSprites [14];
		}
		if (timers[4] <= 0 && Input.GetKey(controls[4])) {
			Abilities.Bullet(shooter.transform.position, Mathf.PI / 2 + Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z));
			AudioSource.PlayClipAtPoint (gun, transform.position);

			ramodel.sprite = cSprites [17];
			if (!casted) {
				lamodel.transform.localPosition = new Vector3 (-0.126f, 1, 0.347f);
			}
			ramodel.transform.localPosition = new Vector3(0.103f, 1, 0.266f);
			timers[4] = cd[4];
		}
		int[] b = new int[2] {0, 0};
		if (Input.GetKey(KeyCode.A)) {
			b[0] = -1;
		} else if (Input.GetKey(KeyCode.D)) {
			b[0] = 1;
		}
		if (Input.GetKey(KeyCode.W)) {
			b[1] = 1;
		} else if (Input.GetKey(KeyCode.S) ) {
			b[1] = -1;
		}
		if (b[0] != 0 && b[1] != 0) {
			transform.Translate(b[0] * speed * Time.deltaTime / Mathf.Sqrt(2), 0, b[1] * speed * Time.deltaTime / Mathf.Sqrt(2));
		} else {
			transform.Translate(b[0] * speed * Time.deltaTime, 0, b[1] * speed * Time.deltaTime);
		}

		for (int i = 0; i <= 3; i++) {
			if (!unlock[i]) {
				continue;
			}
			if (Input.GetKeyDown(controls[i]) && i <= 2 && timers[i] == 0) {
				spellRange.transform.localScale = new Vector3(ranges[i] / 3.2f, ranges[i] / 3.2f, 1);
				spellRange.color = Color.black;
				AOE.transform.localScale = new Vector3(area[i], area[i], 1);
				AOE.color = new Color(1, 0, 0, 0.25f);
			}
			if (Input.GetKey(controls[i]) && i <= 2) {
				AOE.transform.position = mouse + new Vector3(0, 1, 0);
			}
			if (Input.GetKeyUp(controls[i])) {
				spellRange.color = AOE.color = Color.clear;
			}
			if (Input.GetKeyUp(controls[i]) && timers[i] == 0 && Vector3.Distance(mouse, transform.position) <= ranges[i]) {
				timers[i] = cd[i];
				switch (i) {
					case 0: // blight
						Abilities.Blight (mouse);
						AudioSource.PlayClipAtPoint (BlightClip, transform.position);
						casted = true;
						lamodel.sprite = cSprites [13];
						lamodel.transform.localPosition = new Vector3(-0.3f, 1, 0.45f);
						break;
					case 1: // root
						Abilities.Root(mouse);
						AudioSource.PlayClipAtPoint (RootClip, transform.position);	
						casted = true;
						lamodel.sprite = cSprites [12];
						lamodel.transform.localPosition = new Vector3(-0.3f, 1, 0.45f);
						break;
					case 2: // damage
						Abilities.Damage (transform.position, Mathf.PI / 2 + Mathf.Atan2 (transform.position.x - mouse.x, mouse.z - transform.position.z));
						AudioSource.PlayClipAtPoint (DamageClip, transform.position);	
						casted = true;
						lamodel.sprite = cSprites [11];
						lamodel.transform.localPosition = new Vector3(-0.3f, 1, 0.45f);
						break;
					case 3: // blink
//						float angle = 15;
//						if (b[0] != 0 && b[1] != 0) {
//							angle = 90 - b[0] * 90 + b[0] * b[1] * 45;
//						} else if (b[0] != 0) {
//							angle = 90 - b[0] * 90;
//						} else if (b[1] != 0) {
//							angle = b[1] * 90;
//						}
//						Abilities.Blink(transform, angle);
						Abilities.Blink(transform, Mathf.PI / 2 - Mathf.Atan2(mouse.x - transform.position.x, mouse.z - transform.position.z));
						AudioSource.PlayClipAtPoint (BlinkClip, transform.position);	
						break;
				}
			}
		}

		if (transform.position.x > 105) {
			gManager.Finish();
		}
		//Handling off-mesh fort link
		if (needsNav){
			NavMeshAgent nav = gameObject.AddComponent<NavMeshAgent>();
			nav.updateRotation = false;
			nav.radius = .25f;
			nav.height = 1;
			needsNav = false;
		}
		NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
		foreach (Link l in gManager.links) {
			if (Vector3.Distance (l.begin, transform.position) < .5 && Input.GetKey (l.into) && l.unlocked) {
				if (l.time <= 0) {
					agent.destination = l.end;
					destined = true;
				} else {
					l.time = l.time - Time.deltaTime;
				}
			} else if (Vector3.Distance (l.begin, transform.position) < .5 && l.unlocked){
				if (l.time > 0) {
					l.time = l.time - Time.deltaTime;
				}
			} else if (Vector3.Distance (l.end, transform.position) < .5 && Input.GetKey (l.outof)){
				if (l.time <= 0) {
					agent.destination = l.begin;
					destined = true;
				} else {
					l.time = l.time - Time.deltaTime;
				}
			}
		}

//		if (hasFortKey && 39.5f<transform.position.x && transform.position.x<40.3f && -20>transform.position.z && transform.position.z>-21 && Input.GetKey(KeyCode.W)) {
//			agent.destination = new Vector3(40f,1.0f,-18f);
//			destined = true;
//		}
		if (destined && Vector3.Distance(agent.destination,transform.position)<.2) {
			Destroy (agent);
			needsNav = true;
			destined = false;
		}
//		if (hasFortKey && 39.5f<transform.position.x && transform.position.x<40.3f && -18.2f<transform.position.z && transform.position.z<-17.7f && Input.GetKey(KeyCode.S)) {
//			gameObject.GetComponent<NavMeshAgent>().destination = new Vector3(40f,0.0f,-20.5f);
//			destined = true;
//		}
//		//handling off-mesh east gate link
//		if (hasKey && 102.5f<transform.position.x && transform.position.x<104f && -20>transform.position.z && transform.position.z>-24) {
//			unlockTime = unlockTime + Time.deltaTime;
//			if (unlockTime > 3) {
//				agent.destination = new Vector3 (106f, 1.0f, -22f);
//				destined = true;
//			}
//		}
		currentY = transform.position.y;
	}

	void LateUpdate(){
		if (paused) {
			return;
		}
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		necromodel.transform.LookAt (mouse);
	}

	public void TakeHit(GameObject collObj) {
		/*if (collObj.name == "Peasant") {
			hp -= 2;
		}
		if (collObj.name == "Arrow") {
			hp -= 5;
		}
		if (collObj.name == "Knight") {
			hp -= 10;
		}*/
		hp -= 1;
		if (hp <= 0) {
			gManager.Death ();
			AudioSource.PlayClipAtPoint (DeathClip, transform.position);

		}
	}

	public void HasKey(){
		if (!hasFortKey) {
			hasFortKey = true;
			gManager.ChangeObjective("Open the Fort gate.");
		} else {
			hasKey = true;
			//gManager.objectives.text = "Get through the East Gate. \nGate will take 3 seconds to open.";
			eManager.delayedSpawn ("fort", false);
			foreach (Link l in gManager.links) {
				if (l.name == "gate") {
					l.unlocked = true;
				}
			}
		}
	}
//
//	public void HasFortKey(){
//		hasFortKey = true;
//		foreach (Link l in gManager.links) {
//			if (l.name == "fort"){
//				l.unlocked = true;
//			}
//		}
//	}
//
	public void Damage(float damage) {
		hp -= damage;
		if (hp <= 0) {
			gManager.Death();
		}
		if (hp > maxhp) {
			hp = maxhp;
		}
	}

	public void Root() {
		rootTimer = 0;
	}

	public void PausePlayer () {
		paused = true;
	}

	public void UnPausePlayer () {
		paused = false;
	}

	public void EnableBlight() {
		unlock[0] = true;
		GameObject.Find("CD1").GetComponent<Image>().color = Color.black;
	}

	public void EnableRoot() {
		unlock[1] = true;
		GameObject.Find("CD2").GetComponent<Image>().color = Color.white;
	}

	public void EnableDamage() {
		unlock[2] = true;
		GameObject.Find("CD3").GetComponent<Image>().color = Color.red;
	}

	public void EnableBlink() {
		unlock[3] = true;
		GameObject.Find("CD4").GetComponent<Image>().color = Color.blue;
	}
}
