using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {

	int frame;
	SphereCollider col;

	void Start () {
		name = "Melee";
		col = gameObject.AddComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = 1;
	}

	void Update () {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		transform.localEulerAngles = new Vector3(0, 270 - Mathf.Rad2Deg * Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z), 0);

		if (col.enabled) {
			if (frame != Time.frameCount) {
				col.enabled = false;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "AI") {
			Vector3 direction = other.transform.position - transform.position;
			if (Vector3.Angle(direction, transform.right) < 45) {
				other.GetComponent<AIBehavior>().Damage(4);
			}
		}
	}

	public bool Enable() {
		col.enabled = true;
		frame = Time.frameCount;
		return true;
	}
}
