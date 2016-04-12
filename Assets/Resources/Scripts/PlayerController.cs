using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	public float hp = 12;
	float speed = 1.1f;
	float spellShotInterval = .2f;
	Color necroColor = new Color (120f/256f, 0f/256f, 100f/256f);

	// CONTROLS PARAMETERS
	KeyCode blightKey = KeyCode.Mouse1;
	KeyCode graspingKey = KeyCode.Mouse0;
	KeyCode beamKey = KeyCode.Mouse0;
	KeyCode blinkKey = KeyCode.Mouse0;
	KeyCode meleeKey = KeyCode.Mouse0;
	KeyCode shootKey = KeyCode.Mouse0;

	public int minionCount;
	float shotClock;
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
		playerMaterial = GetComponent<SpriteRenderer> ().material;
		playerMaterial.color = necroColor;
		gameObject.AddComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<Rigidbody> ().freezeRotation = true;
		gameObject.AddComponent<SphereCollider> ().radius = .3f;

		GameObject infectBar = new GameObject();
		infectionBar = infectBar.AddComponent<InfectionBar> ();
		infectionBar.init (gManager, this);
		GameObject hpBar = new GameObject();
		healthBar = hpBar.AddComponent<HealthBar> ();
		healthBar.init (gManager, this, hp);

		// TEMPORARY CAMERA FOLLOW
		GameObject.Find("Main Camera").transform.parent = transform;

		shotClock = 0;
		minionCount = 0;
	}


	// Update is called once per frame
	void Update () {
		// moves player by speed*Time.deltaTime based on WASD
		if (Input.GetKey (KeyCode.A) && !(transform.position.x <= -85)) {
			transform.Translate (-speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.D) && !(transform.position.x >= 85)) {
			transform.Translate (speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.W) && !(transform.position.z >= 46.5)) {
			transform.Translate (0, speed*Time.deltaTime, 0);
		} if (Input.GetKey (KeyCode.S) && !(transform.position.z <= -46.5)) {
			transform.Translate (0, -speed*Time.deltaTime, 0);
		}
		// if space key is down and enough time has passed, fires spellShot
		shotClock += Time.deltaTime;
		if (Input.GetKey (KeyCode.Space) && shotClock > spellShotInterval) {
			GameObject shot = new GameObject ();
			shot.AddComponent<SpellShot> ().init (this, gManager);
			shotClock = 0;
		}
		if (Input.GetMouseButton(0) && infectionBar.infectionCharge > 50) {
			GameObject blight = new GameObject ();
			blight.AddComponent<Blight> ().init (this, gManager, infectionBar.infectionCharge);
			infectionBar.infectionCharge = 0;
		}
	}


	public void TakeHit(GameObject collObj) {
		print ("taking hit");
		hp -= 1;
		if (hp <= 0) {
			gManager.Death ();
		}
	}
}
