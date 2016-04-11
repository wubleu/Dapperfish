using UnityEngine;
using System.Collections;

public class SpellShot : MonoBehaviour {

	// PARAMETERS
	float speed = .07f;
	Color spellShotColor = new Color (75f / 256f, 105f / 256f, 66f / 256f);

	PlayerController necromancer;
	GameManager gManager;
	Material spellShotMaterial;
	Vector3 direction;


	// Use this for initialization
	public void init (PlayerController owner, GameManager gMan) {
		necromancer = owner;
		gManager = gMan;

		gManager.MakeSprite (gameObject, "Circle", necromancer.transform, 0, 0, .25f, .25f, 200);
		transform.parent = gManager.transform;
		gameObject.name = "SpellShot";
		gameObject.AddComponent<SphereCollider> ().isTrigger = true;
		spellShotMaterial = GetComponent<SpriteRenderer> ().material;
		spellShotMaterial.color = spellShotColor;

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction = new Vector3 (mousePos.x-transform.position.x, mousePos.y-transform.position.y, 0);
		float directionMagnitude = Mathf.Sqrt (Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
		direction = new Vector3 (direction.x / directionMagnitude, direction.y / directionMagnitude, 0);
		transform.Translate (direction.x*.3f, direction.y*.3f, 0);
	}


	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(transform.position.x-necromancer.transform.position.x) > 9 || Mathf.Abs(transform.position.y-necromancer.transform.position.y) > 5) {
			Destroy (gameObject);
		}
		transform.Translate (direction.x*speed, direction.y*speed, 0);
	}


	void OnTriggerEnter(Collider coll) {
		if (coll.name != "Necromancer") {
			Destroy (gameObject);
		}
	}
}
