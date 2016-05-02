using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	SpriteRenderer rend;

	public void init(Vector3 pos) {
		name = "HealthPack";
		transform.position = pos;
		transform.eulerAngles = new Vector3(90, 0, 0);
		SphereCollider coll = gameObject.AddComponent<SphereCollider>();
		coll.radius = 0.5f;
		coll.isTrigger = true;

		rend = gameObject.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/BenCircle");
		rend.color = Color.green;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.name == "Necromancer") {
			coll.GetComponent<PlayerController>().Damage(-10);
			Destroy(gameObject);
		}
	}
}
