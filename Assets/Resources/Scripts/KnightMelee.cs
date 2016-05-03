using UnityEngine;
using System.Collections;

public class KnightMelee : MonoBehaviour {

	int frame;
	SphereCollider col;
	bool charging = false;

	void Start () {
		name = "KnightMelee";
		col = gameObject.AddComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = 0.8f;
	}

	void Update () {
		if (col.enabled && !charging) {
			if (frame != Time.frameCount) {
				col.enabled = false;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Necromancer") {
			other.GetComponent<PlayerController>().Damage(5);
			if (charging) {
				col.enabled = false;
				charging = false;
			}
		} else if (other.tag == "AI" && !other.GetComponent<AIBehavior>().isEnemy) {
			other.GetComponent<AIBehavior>().Damage(5);
		}
	}

	public void Enable() {
		col.enabled = true;
		frame = Time.frameCount;
	}

	public void Charge() {
		col.enabled = true;
		charging = true;
	}

	public void Disable() {
		col.enabled = false;
	}
}
