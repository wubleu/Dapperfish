using UnityEngine;
using System.Collections;

public class NecromancerBoss : MonoBehaviour {

	// PARAMETERS
	float teleportCooldown = 2f;

	float teleportCooldownTimer = 0;
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
		print(cSprites = Resources.LoadAll<Sprite> ("Textures/Boss Sprite Sheet"));
		rend = GetComponentInChildren<SpriteRenderer>();
		rend.gameObject.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		rend.sprite = cSprites [0];
	}
	
	// Update is called once per frame
	void Update () {
		if ((teleportCooldownTimer += Time.deltaTime) > teleportCooldown) {
			Abilities.Summon (transform.position.x + Random.Range (-3f, 3f), transform.position.y, transform.position.z + Random.Range (-3f, 3f));
			teleportCooldownTimer = 0;
		}
	}
}
