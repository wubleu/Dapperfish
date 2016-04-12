using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {

	int frame;
	CircleCollider2D col;

	void Start () {
		col = gameObject.AddComponent<CircleCollider2D>();
		col.radius = 1.5f;
		col.enabled = false;
	}

	void Update () {
		if (col.enabled) {
			if (frame != Time.frameCount) {
				col.enabled = false;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Enemy") {
			Vector3 direction = other.transform.position - transform.position;
			if (Vector3.Angle(direction, transform.right) < 45 * 0.5f) {
				//damage
			}
		}
	}

	public void Enable() {
		col.enabled = true;
		frame = Time.frameCount;
	}
}
