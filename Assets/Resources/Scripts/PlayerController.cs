using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	public float hp = 50, mana = 100, recharge = 20, size = 0.3f, speed = 1.1f, mclock = 0, mtime = 0.5f;
	bool isMelee = true;
	Color necroColor = new Color(120f / 256f, 0f / 256f, 100f / 256f);
	float[] mcosts = new float[6] {0, 60, 30, 80, 30, 0};
	Melee melee;


	protected Sprite[] cSprites;
	// spellShotInterval = .2f

	// CONTROLS PARAMETERS
	KeyCode[] controls = new KeyCode[6] {
		KeyCode.Mouse0, // auto-attack
		KeyCode.Mouse1, // blight
		KeyCode.LeftShift, // root
		KeyCode.LeftControl, // damage
		KeyCode.Space, // blink
		KeyCode.Tab // switch weapon
	};

	public int minionCount = 0;
	GameManager gManager;
	ManaBar manaBar;
	HealthBar healthBar;
	public EnemyManager eManager;
	public bool hasKey = false;

	GameObject necromodel;
	GameObject rightarm;
	GameObject leftarm;
	GameObject body;
	SpriteRenderer lamodel;
	SpriteRenderer bodymodel;
	SpriteRenderer ramodel;

	// Use this for initialization
	public void init (GameManager owner, EnemyManager eMan) {
		eManager = eMan;
		gManager = owner;
		gameObject.name = "Necromancer";

		cSprites = Resources.LoadAll<Sprite> ("Textures/Necromancer Sprite Sheet 0");

		gameObject.AddComponent<SphereCollider>().radius = size;
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


		//model.color = necroColor;

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
		if (mclock > 0) {
			mclock -= Time.deltaTime;
		}
		for (int i = 0; i < mcosts.Length; i++) {
			if (Input.GetKeyDown(controls[i]) && mana > mcosts[i]) {
				mana -= mcosts[i];
				switch (i) {
					case 0: // auto-attack
						if (mclock <= 0) {
							mclock = mtime;
							if (isMelee) melee.Enable();
							else Abilities.Bullet(transform.position, Mathf.PI / 2 + Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z));
						}
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
					case 5: // switch weapons
						isMelee = !isMelee;
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

		if (transform.position.x > 105) {
			if (!hasKey) {
				transform.position = new Vector3 (105, transform.position.y, transform.position.z);
			} else {
				print ("winner winner chicken dinner");
			}
		}
	}

	void LateUpdate(){
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		necromodel.transform.LookAt (mouse);
	}

	public void TakeHit(GameObject collObj) {
		print ("taking hit");
		hp -= 1;
		if (hp <= 0) {
			gManager.Death ();
		}
	}

	public void HasKey(){
		hasKey = true;
	}
}
