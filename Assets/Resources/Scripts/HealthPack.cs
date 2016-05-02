using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	SpriteRenderer rend;

	public void init(Vector3 pos) {
		name = "HealthPack";
		transform.position = pos;
		SphereCollider coll = gameObject.AddComponent<SphereCollider>();
		coll.radius = 0.5f;
		coll.isTrigger = true;

		rend = gameObject.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/BenCircle");
		rend.color = Color.green;
	}

	void OnTrigger(Collider coll) {
		if (coll.name == "Necromancer") {
			coll.GetComponent<PlayerController>().Damage(-10);
			Destroy(gameObject);
		}
	}
}
