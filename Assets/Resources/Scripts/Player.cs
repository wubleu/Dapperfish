using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	float hp = 12, speed = 1.1f;
	float[] clocks = new float[5] {0, 0, 0, 0, 0}, timers = new float[5] {1, 1, 1, 1, 1};
	Melee melee;

	public void init() {
		gameObject.name = "Necromancer";
		gameObject.AddComponent<CapsuleCollider>();
		gameObject.AddComponent<Rigidbody>().isKinematic = true;
		SpriteRenderer rend = gameObject.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/Circle2");
		rend.color = new Color (120f/256f, 0f/256f, 100f/256f);

		melee = new GameObject().AddComponent<Melee>();
		melee.transform.parent = transform;
		melee.transform.position = new Vector3(0, 0, 0);
	}

	void Update () {
		for (int i = 0; i < 5; i++) {
			if (clocks[i] > 0) {
				clocks[i] -= Time.deltaTime;
			}
		}

		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.z = 0;
		transform.localEulerAngles = new Vector3(0, 0, Vector3.Angle(transform.position, mouse));

		if (Input.GetKey("a")) {
			transform.Translate (-speed*Time.deltaTime, 0, 0);
		}
		if (Input.GetKey("d")) {
			transform.Translate (speed*Time.deltaTime, 0, 0);
		}
		if (Input.GetKey("w")) {
			transform.Translate (0, speed*Time.deltaTime, 0);
		}
		if (Input.GetKey("s")) {
			transform.Translate (0, -speed*Time.deltaTime, 0);
		}
		if (Input.GetMouseButtonDown(0) && clocks[0] <= 0) {
			melee.Enable();
			clocks[0] = timers[0];
		}
		if (Input.GetMouseButtonDown(1) && clocks[1] <= 0) {
			Abilities.Blight(mouse);
			clocks[1] = timers[1];
		}
		if (Input.GetKeyDown("left shift") && clocks[2] <= 0) {
			Abilities.Root(mouse);
			clocks[2] = timers[2];
		}
		if (Input.GetKeyDown("left ctrl") && clocks[3] <= 0) {
			Abilities.Damage(mouse);
			clocks[3] = timers[3];
		}
//		if (Input.GetKeyDown("space") && clocks[4] <= 0) {
//			Abilities.Blink(mouse, transform);
//			clocks[4] = timers[4];
//		}
	}

	public void Damage(float damage) {
		hp -= damage;
		if (hp <= 0) {
			// GameOver
		}
	}
}
