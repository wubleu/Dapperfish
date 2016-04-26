using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	public float hp = 50, size = 1f, speed = 1.8f, castcd = .25f;
	bool isMelee = true, casted = false;
	public float[] cd = new float[5] {0.5f, 5, 3, 50, 2}, timers = new float[5] {0, 0, 0, 0, 0};
	Melee melee;
	protected Sprite[] cSprites;
	public Image[] icons = new Image[4];
	
	// CONTROLS PARAMETERS
	KeyCode[] controls = new KeyCode[6] {
		KeyCode.Mouse0, // auto-attack
		KeyCode.Mouse1, // blight
		KeyCode.Tab, // root
		KeyCode.F, // damage
		KeyCode.Space, // blink
		KeyCode.Q // switch weapon
	};

	public int minionCount = 0;
	GameManager gManager;
	public EnemyManager eManager;
	public bool hasKey = false;
	public bool hasFortKey = false;
	public bool needsNav = false;
	public bool destined = false;
	public float currentY = 0;
	public float unlockTime = 0;

	//Sounds

	public AudioClip BlightClip;
	public AudioClip RootClip;
	public AudioClip DamageClip;
	public AudioClip BlinkClip;


	GameObject necromodel, rightarm, leftarm, body, shooter;
	SpriteRenderer lamodel, bodymodel, ramodel;

	public void init (GameManager owner, EnemyManager eMan) {
		eManager = eMan;
		gManager = owner;
		gameObject.name = "Necromancer";

		cSprites = Resources.LoadAll<Sprite> ("Textures/Necromancer Sprite Sheet 0");

		gameObject.AddComponent<SphereCollider>().radius = size*.75f;
		NavMeshAgent nav = gameObject.AddComponent<NavMeshAgent>();
		nav.updateRotation = false;
		nav.radius = .25f;
		nav.height = 1;

		Rigidbody rigid = gameObject.AddComponent<Rigidbody>();
		rigid.isKinematic = true;
		rigid.freezeRotation = true;
		gameObject.GetComponent<SphereCollider> ().isTrigger = true;

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

		new GameObject().AddComponent<HealthBar>().init(hp);
		for (int i = 1; i <= 4; i++) {
			icons[i - 1] = GameObject.Find("CD" + i).GetComponent<Image>();
		}

		GameObject.Find("Main Camera").transform.parent = transform;

		melee = new GameObject().AddComponent<Melee>();
		melee.transform.parent = transform;
		melee.transform.localPosition = new Vector3(0, 1, 0);

		DamageClip = Resources.Load ("Sounds/Damage") as AudioClip;
		BlightClip = Resources.Load ("Sounds/Blight") as AudioClip;
		RootClip = Resources.Load ("Sounds/Root") as AudioClip;
		BlinkClip = Resources.Load ("Sounds/Blink") as AudioClip;

	}

	void Update () {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		if (timers[0] > 0) {
			if ((timers[0] -= Time.deltaTime) <= .25f) {
				ramodel.sprite = cSprites [19];
				ramodel.transform.localPosition = new Vector3(0.12f, 1, 0.29f);
				if (!casted) {
					lamodel.transform.localPosition = new Vector3 (-0.136f, 1, 0.382f);
				}
			}
		}
		for (int i = 1; i <= 4; i++) {
			if (timers[i] > 0 && (timers[i] -= Time.deltaTime) < 0) {
				timers[i] = 0;
			}
			icons[i - 1].fillAmount = 1 - (timers[i] / cd[i]);
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
		if (timers[0] <= 0) {
			if (Input.GetKeyDown(controls[0]) && isMelee) {
				melee.Enable();
				timers[0] = cd[0];
			}
			if (Input.GetKey(controls[0]) && !isMelee) {
				Abilities.Bullet(shooter.transform.position, Mathf.PI / 2 + Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z));
				ramodel.sprite = cSprites [17];
				if (!casted) {
					lamodel.transform.localPosition = new Vector3 (-0.126f, 1, 0.347f);
				}
				ramodel.transform.localPosition = new Vector3(0.103f, 1, 0.266f);
				timers[0] = cd[0];
			}
		}
		if (Input.GetKeyDown(controls[5])) {
			isMelee = !isMelee;
		}
		for (int i = 1; i <= 4; i++) {
			if (Input.GetKeyDown(controls[i]) && timers[i] == 0) {
				timers[i] = cd[i];
				switch (i) {
					case 1: // blight
						Abilities.Blight (mouse);
					AudioSource.PlayClipAtPoint (BlightClip, transform.position);	

						casted = true;
						lamodel.sprite = cSprites [13];
						lamodel.transform.localPosition = new Vector3(-0.3f, 1, 0.45f);
						break;
					case 2: // root
						Abilities.Root(mouse);
					AudioSource.PlayClipAtPoint (RootClip, transform.position);	

						casted = true;
						lamodel.sprite = cSprites [12];
						lamodel.transform.localPosition = new Vector3(-0.3f, 1, 0.45f);
						break;
				case 3: // damage
					Abilities.Damage (transform.position, Mathf.PI / 2 + Mathf.Atan2 (transform.position.x - mouse.x, mouse.z - transform.position.z));
					AudioSource.PlayClipAtPoint (DamageClip, transform.position);	
					casted = true;
						lamodel.sprite = cSprites [11];
						lamodel.transform.localPosition = new Vector3(-0.3f, 1, 0.45f);
						break;
					case 4: // blink
						Abilities.Blink(transform, Mathf.PI / 2 - Mathf.Atan2(mouse.x - transform.position.x, mouse.z - transform.position.z));
					AudioSource.PlayClipAtPoint (BlinkClip, transform.position);	

						break;
				}
			}
		}
		int[] b = new int[2] {0, 0};
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
				gManager.Finish();
			}
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
		if (hasFortKey && 39.5f<transform.position.x && transform.position.x<40.3f && -20>transform.position.z && transform.position.z>-21 && Input.GetKey(KeyCode.W)) {
			agent.destination = new Vector3(40f,1.0f,-18f);
			destined = true;
		}
		if (Mathf.Abs(currentY-transform.position.y)>.6||(destined && transform.position.x>(agent.destination.x-.05f) && transform.position.x<(agent.destination.x+.05f) && transform.position.y>(agent.destination.y-.05f) && transform.position.y<(agent.destination.y+.05f) && transform.position.z>(agent.destination.z-.05f) && transform.position.z<(agent.destination.z+.05f))) {
			Destroy (agent);
			needsNav = true;
			destined = false;
		}
		if (hasFortKey && 39.5f<transform.position.x && transform.position.x<40.3f && -18.2f<transform.position.z && transform.position.z<-17.7f && Input.GetKey(KeyCode.S)) {
			gameObject.GetComponent<NavMeshAgent>().destination = new Vector3(40f,0.0f,-20.5f);
			destined = true;
		}
		//handling off-mesh east gate link
		if (hasKey && 102.5f<transform.position.x && transform.position.x<104f && -20>transform.position.z && transform.position.z>-24) {
			unlockTime = unlockTime + Time.deltaTime;
			if (unlockTime > 3) {
				agent.destination = new Vector3 (106f, 1.0f, -22f);
				destined = true;
			}
		}
		currentY = transform.position.y;
	}

	void LateUpdate(){
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		necromodel.transform.LookAt (mouse);
	}

	public void TakeHit(GameObject collObj) {
		hp -= 1;
		if (hp <= 0) {
			gManager.Death ();
		}
	}

	public void HasKey(){
		hasKey = true;
		eManager.delayedSpawn ("fort");
	}

	public void Damage(float damage) {
		hp -= 1;
		if (hp <= 0) {
			gManager.Death();
		}
	}
}
