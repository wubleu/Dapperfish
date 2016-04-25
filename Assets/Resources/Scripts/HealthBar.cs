using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	// PARAMETERS
	float maxHealth;

	PlayerController necromancer;
	SpriteRenderer bar;

	public void init (float maxHP) {
		necromancer = GameObject.Find("Necromancer").GetComponent<PlayerController>();

		gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/BarOutline");
		name = "Health";
		transform.parent = necromancer.transform;
		transform.localPosition = new Vector3(11, 10, -9);
		transform.localScale = new Vector3(2, 2, 1);
		transform.localEulerAngles = new Vector3(90, 0, 0);

		bar = new GameObject().AddComponent<SpriteRenderer>();
		bar.name = "HealthBar";
		bar.transform.parent = transform;
		bar.transform.localPosition = Vector3.zero;
		bar.transform.localEulerAngles = new Vector3(0, 0, 0);
		bar.sprite = Resources.Load<Sprite>("Textures/Bar");
		bar.color = Color.green; //new Color(0, 100/256, 0, 0.5f);

		maxHealth = maxHP;
	}

	void Update () {
		bar.transform.localScale = new Vector3(1, necromancer.hp / maxHealth, 1);
	}
}
