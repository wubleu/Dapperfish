using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {

	int frame;
	SphereCollider col;

	void Start () {
		col = gameObject.AddComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = 1;
	}

	void Update () {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		transform.localEulerAngles = new Vector3(0, 0, 90 + Mathf.Rad2Deg * Mathf.Atan2(transform.position.x - mouse.x, mouse.z - transform.position.z));

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
				other.GetComponent<Peasant>().Damage(10);
			}
		}
	}

	public void Enable() {
		col.enabled = true;
		frame = Time.frameCount;
	}
}
