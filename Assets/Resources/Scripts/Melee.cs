using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {

	int frame;
	SphereCollider col;

	void Start () {
		name = "Melee";
		col = gameObject.AddComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = 1.5f;
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
		GameObject mel = new GameObject ();
		mel.transform.position = col.transform.position;
		mel.transform.localEulerAngles = new Vector3 (90, 0, 0);
		mel.transform.localScale = new Vector3 (1.5f, 1.25f, 1.5f);
		SpriteRenderer rend = mel.AddComponent<SpriteRenderer> ();
		rend.sortingLayerName = "PlayerController";
		rend.sortingOrder = 5;
		Animator anim = mel.AddComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Melee Controller");
		Destroy (mel.gameObject, .5f);
		frame = Time.frameCount;
		return true;
	}
}
