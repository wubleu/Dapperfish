using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	public float hp = 500, mana = 100, recharge = 20, size = 0.3f;
	float speed = 1.1f;
	Color necroColor = new Color(120f / 256f, 0f / 256f, 100f / 256f);
	float[] clocks = new float[5] {0, 0, 0, 0, 0}, timers = new float[5] {0.5f, 0.5f, 0.5f, 0.5f, 0.5f}, mcosts = new float[5] {0, 60, 30, 80, 30};
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

	public int minionCount = 0;
	GameManager gManager;
	ManaBar manaBar;
	HealthBar healthBar;
	public EnemyManager eManager;


	// Use this for initialization
	public void init (GameManager owner, EnemyManager eMan) {
		eManager = eMan;
		gManager = owner;
		gameObject.name = "Necromancer";

		gameObject.AddComponent<SphereCollider>().radius = size;
		NavMeshAgent nav = gameObject.AddComponent<NavMeshAgent>();
		nav.updateRotation = false;
		nav.radius = .25f;
		nav.height = 1;

		Rigidbody rigid = gameObject.AddComponent<Rigidbody>();
		rigid.isKinematic = true;
		rigid.freezeRotation = true;

		// Necromancer Model
		SpriteRenderer model = new GameObject().AddComponent<SpriteRenderer>();
		model.name = "Necro Model";
		model.transform.parent = transform;
		model.transform.localPosition = new Vector3(0, 1, 0);
		model.transform.localEulerAngles = new Vector3(90, 0, 0);
		model.transform.localScale = new Vector3(size, size, size);
		model.sortingLayerName = "PlayerController";
		model.sprite = Resources.Load<Sprite>("Textures/Circle");
		model.color = necroColor;

		// HealthBar + ManaBar
		new GameObject().AddComponent<HealthBar>().init(hp);
		new GameObject().AddComponent<ManaBar>().init(this.mana, recharge);

		// TEMPORARY CAMERA FOLLOW
		GameObject.Find("Main Camera").transform.parent = transform;

		melee = new GameObject().AddComponent<Melee>();
		melee.transform.parent = transform;
		melee.transform.localPosition = new Vector3(0, 0, 0);
	}

	void Update () {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;

		for (int i = 0; i < 5; i++) {
			if (clocks[i] > 0) {
				clocks[i] -= Time.deltaTime;
			} else if (Input.GetKeyDown(controls[i]) && mana > mcosts[i]) {
				clocks[i] = timers[i];
				mana -= mcosts[i];
				switch (i) {
					case 0: // auto-attack
						melee.Enable();
						break;
					case 1: // blight
						Abilities.Blight(mouse);
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
		int[] b = new int[2] {0, 0};
		// moves player by speed*Time.deltaTime based on WASD
		if (Input.GetKey(KeyCode.A)) {
			b[0] = -1;
			transform.Translate(-speed*Time.deltaTime, 0, 0);
		} else if (Input.GetKey(KeyCode.D)) {
			b[0] = 1;
			transform.Translate(speed*Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.W)) {
			b[1] = 1;
			transform.Translate(0, 0, speed*Time.deltaTime);
		} else if (Input.GetKey(KeyCode.S) ) {
			b[1] = -1;
			transform.Translate(0, 0, -speed*Time.deltaTime);
		}
		if (b[0] != 0 && b[1] != 0) {
			transform.Translate(b[0] * speed * Time.deltaTime / Mathf.Sqrt(2), 0, b[1] * speed * Time.deltaTime / Mathf.Sqrt(2));
		} else {
			transform.Translate(b[0] * speed * Time.deltaTime, 0, b[1] * speed * Time.deltaTime);
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
