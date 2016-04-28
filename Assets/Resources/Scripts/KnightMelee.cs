using UnityEngine;
using System.Collections;

public class KnightMelee : MonoBehaviour {

	int frame;
	SphereCollider col;

	void Start () {
		name = "KnightMelee";
		col = gameObject.AddComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = 0.8f;
	}

	void Update () {
		if (col.enabled) {
			if (frame != Time.frameCount) {
				col.enabled = false;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Necromancer") {
			print("KnightMelee hit necro");
			Vector3 direction = other.transform.position - transform.position;
			if (Vector3.Angle(direction, transform.right) < 45) {
				other.GetComponent<PlayerController>().Damage(5);
			}
		} else if (other.tag == "AI" && !other.GetComponent<AIBehavior>().isEnemy) {
			print("KnightMelee hit ai");
			Vector3 direction = other.transform.position - transform.position;
			if (Vector3.Angle(direction, transform.right) < 45) {
				other.GetComponent<AIBehavior>().Damage(5);
			}
		}
	}

	public void Enable() {
		print("KnightMelee enabled");
		col.enabled = true;
		frame = Time.frameCount;
	}
}
