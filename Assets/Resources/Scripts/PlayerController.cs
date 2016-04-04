using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	float speed = 2f;
	float spellShotInterval = .5f;
	Color necroColor = new Color (140f/256f, 0f/256f, 120f/256f);

	float shotClock;
	GameObject infectionBar;
	GameManager gManager;
	Material playerMaterial;


	// Use this for initialization
	public void init (GameManager owner) {
		gManager = owner;
		gManager.MakeSprite (gameObject, "Circle", gManager.transform, 0, 0, .25f, .25f, 200);
		gameObject.name = "Necromancer";
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerController";
		playerMaterial = GetComponent<SpriteRenderer> ().material;
		playerMaterial.color = necroColor;

		infectionBar = new GameObject();
		infectionBar.AddComponent<InfectionBar> ().init (gManager, this);

		// TEMPORARY CAMERA FOLLOW
		GameObject.Find("Main Camera").transform.parent = transform;

		shotClock = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// moves player by speed*Time.deltaTime based on WASD
		if (Input.GetKey (KeyCode.A) && !(transform.position.x <= -85)) {
			transform.Translate (-speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.D) && !(transform.position.x >= 85)) {
			transform.Translate (speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.W) && !(transform.position.y >= 46.5)) {
			transform.Translate (0, speed*Time.deltaTime, 0);
		} if (Input.GetKey (KeyCode.S) && !(transform.position.y <= -46.5)) {
			transform.Translate (0, -speed*Time.deltaTime, 0);
		}
		print (gameObject.GetComponent<SpriteRenderer> ().sprite.texture);
		// if space key is down and enough time has passed, fires spellShot
		shotClock += Time.deltaTime;
		if (Input.GetKey (KeyCode.Space) && shotClock > spellShotInterval) {
			GameObject shot = new GameObject ();
			shot.AddComponent<SpellShot> ().init (this, gManager);
			shotClock = 0;
		}
	}
}
