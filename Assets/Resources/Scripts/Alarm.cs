using UnityEngine;
using System.Collections;

public class Alarm : MonoBehaviour {

	float clock = 0.3f;

	public void init(Vector3 pos) {
		SphereCollider coll = gameObject.AddComponent<SphereCollider>();
		coll.radius = 2;
		coll.isTrigger = true;
		transform.position = pos;
	}

	void Update() {
		if ((clock -= Time.deltaTime) <= 0) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "AI") {
			coll.GetComponent<AIBehavior>().Alert();
		}
	}
}
