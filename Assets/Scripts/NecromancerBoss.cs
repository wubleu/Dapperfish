using UnityEngine;
using System.Collections;

public class NecromancerBoss : MonoBehaviour {

	// PARAMETERS
	float teleportCooldown = 10f, rootDuration = 1f, teleportCooldownTimer = 9.5f, maxHp = 10f;

	float rootTimer = 0, hp;
	bool paused = false;
	GameManager gManager;
	EnemyManager eManager;
	PlayerController necromancer;
	SpriteRenderer rend;
	Sprite[] cSprites;


	public void initNecroBoss (GameManager gMan, EnemyManager owner, PlayerController necro) {
		gManager = gMan;
		eManager = owner;
		necromancer = necro;
		tag = "Untagged";
		hp = maxHp;
		cSprites = Resources.LoadAll<Sprite> ("Textures/Boss Sprite Sheet");
		rend = GetComponentInChildren<SpriteRenderer>();
		rend.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		rend.sprite = cSprites [0];
		Rigidbody rbody = gameObject.GetComponent<Rigidbody> ();
		rbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (paused || (rootTimer < rootDuration && (rootTimer += Time.deltaTime) > rootDuration)) {
			return;
		}
		// JUST BEING USED FOR TESTING PURPOSES NOW- not at all reflective of final behavior. just tweaking summon a bit.
		if ((teleportCooldownTimer += Time.deltaTime) > teleportCooldown) {
			Abilities.Summon (transform.position.x + Random.Range (-6f, 6f), transform.position.y, transform.position.z + Random.Range (-6f, 6f));
			teleportCooldownTimer = 0;
		}
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
