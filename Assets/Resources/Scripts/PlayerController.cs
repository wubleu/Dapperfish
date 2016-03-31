using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// PARAMETERS
	float speed = 2f;

	GameManager parent;
	Material playerMaterial;


	// Use this for initialization
	public void init (GameManager owner) {
		parent = owner;
		parent.MakeSprite (gameObject, Resources.Load<Sprite> ("Textures/Button"), transform, 0, -4, .25f, .25f, 200);
		playerMaterial = GetComponent<SpriteRenderer> ().material;
		playerMaterial.color = new Color (170f/256f, 0f/256f, 150f/256f);
	}
	
	// Update is called once per frame
	void Update () {
		// moves player by speed*Time.deltaTime based on WASD
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (-speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.D)) {
			transform.Translate (speed*Time.deltaTime, 0, 0);
		} if (Input.GetKey (KeyCode.W)) {
			transform.Translate (0, speed*Time.deltaTime, 0);
		} if (Input.GetKey (KeyCode.S)) {
			transform.Translate (0, -speed*Time.deltaTime, 0);
		}
	}
}
