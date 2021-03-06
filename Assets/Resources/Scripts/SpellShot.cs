﻿using UnityEngine;
using System.Collections;

public class SpellShot : MonoBehaviour {

	// PARAMETERS
	float speed = .07f;
	Color spellShotColor = new Color (75f / 256f, 105f / 256f, 66f / 256f);

	PlayerController necromancer;
	GameManager gManager;
	SpriteRenderer spellShot;
	Vector3 direction;
	Animator anim;
	public AudioClip gun;
	public AudioClip impact;

	// Use this for initialization
	public void init (PlayerController owner, GameManager gMan) {
		necromancer = owner;
		gManager = gMan;

//		gManager.MakeSprite (gameObject, "Circle", necromancer.transform, 0, 0, .25f, .25f, 200);
		transform.parent = gManager.transform;
		gameObject.name = "SpellShot";
		spellShot = GetComponent<SpriteRenderer> ();
		anim = gameObject.AddComponent<Animator> ();
		gameObject.AddComponent<SphereCollider> ().isTrigger = true;
		gameObject.GetComponent<SphereCollider> ().radius = .07f;


		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction = new Vector3 (mousePos.x-transform.position.x, mousePos.z-transform.position.z, 0);
		float directionMagnitude = Mathf.Sqrt (Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
		direction = new Vector3 (direction.x / directionMagnitude, direction.y / directionMagnitude, 0);
		transform.Translate (direction.x*.3f, direction.y*.3f, 0);

		gun = Resources.Load ("Sounds/gun") as AudioClip;
		impact = Resources.Load ("Sounds/impact") as AudioClip;
		AudioSource.PlayClipAtPoint (gun, transform.position);
	}


	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(transform.position.x-necromancer.transform.position.x) > 7 || Mathf.Abs(transform.position.y-necromancer.transform.position.y) > 4) {
			Destroy (gameObject);
		}
		transform.Translate (direction.x*speed, direction.y*speed, 0);
	}


	void OnTriggerEnter(Collider coll) {
		if (coll.name != "Necromancer") {
			AudioSource.PlayClipAtPoint (impact, transform.position);

			Destroy (gameObject);
		}
	}
}
