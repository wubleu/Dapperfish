using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	// PARAMETERS
	float maxHealth;
	GameObject healthbar;

	PlayerController necromancer;
	SpriteRenderer bar;
	protected Sprite[] cSprites;

	public void init (float maxHP) {
		necromancer = GameObject.Find("Necromancer").GetComponent<PlayerController>();

		cSprites = Resources.LoadAll<Sprite>("Textures/Health Bar");
		gameObject.AddComponent<SpriteRenderer> ().sprite = cSprites [1];
		name = "Health";
		transform.parent = necromancer.transform;
		transform.localPosition = new Vector3(-12, 10, 9);
		transform.localScale = new Vector3(1, 1, 1);
		transform.localEulerAngles = new Vector3(90, 0, 0);
		gameObject.layer = 5;

		bar = new GameObject().AddComponent<SpriteRenderer>();
		bar.name = "HealthBar";
		bar.transform.parent = transform;
		bar.transform.localPosition = Vector3.zero;
		bar.transform.localEulerAngles = new Vector3(0, 0, 0);
		bar.sprite = cSprites [0];
		bar.color = Color.red; //new Color(0, 100/256, 0, 0.5f);

		maxHealth = maxHP;

	}

	void Update () {
		bar.transform.localScale = new Vector3( necromancer.hp / maxHealth ,1, 1);
	}
}
