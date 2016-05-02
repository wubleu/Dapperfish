using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	SpriteRenderer rend;

	public void init(Vector3 pos) {
		name = "HealthPack";
		transform.position = pos;
		transform.eulerAngles = new Vector3(90, 0, 0);
		transform.localScale = new Vector3 (.75f, .75f, .75f);
		rend = gameObject.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.LoadAll<Sprite>("Textures/Health Pickup Sprite Sheet")[0];
		rend.sortingOrder = 1;
		SphereCollider coll = gameObject.AddComponent<SphereCollider>();
		coll.radius = 0.5f;
		coll.isTrigger = true;

		Animator anim = gameObject.AddComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Health Drop Controller");
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.name == "Necromancer") {
			coll.GetComponent<PlayerController>().Damage(-10);
			Destroy(gameObject);
		}
	}
}
