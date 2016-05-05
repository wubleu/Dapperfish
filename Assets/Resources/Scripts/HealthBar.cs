using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	// PARAMETERS
	float maxHealth, type;
	GameObject healthbar;

	PlayerController necromancer;
	NecromancerBoss boss;
	SpriteRenderer bar;
	protected Sprite[] cSprites;

	public void init (float maxHP, GameObject person) {
		necromancer = GameObject.Find("Necromancer").GetComponent<PlayerController>();
		transform.parent = necromancer.transform;
		transform.localScale = new Vector3(1, 1, 1);
		transform.localEulerAngles = new Vector3(90, 0, 0);
		cSprites = Resources.LoadAll<Sprite>("Textures/Health Bar");
		gameObject.AddComponent<SpriteRenderer> ().sprite = cSprites [1];
		gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 1;

		bar = new GameObject().AddComponent<SpriteRenderer>();
		bar.name = "HealthBar";
		bar.transform.parent = transform;
		bar.transform.localPosition = Vector3.zero;
		bar.transform.localEulerAngles = new Vector3(0, 0, 0);
		bar.sprite = cSprites [0];
		bar.GetComponent<SpriteRenderer>().sortingOrder = 1;
		maxHealth = maxHP;

		if (person.name == "Necromancer") {
			type = 0;
			name = "Health";
			bar.color = Color.red;
			transform.localPosition = new Vector3(-12, 15, 9);
		} else {
			type = 1;
			name = "Boss Health";
			bar.color = Color.green;
			boss = GameObject.Find("Necromancer Boss").GetComponent<NecromancerBoss>();
			transform.localPosition = new Vector3(0, 15, 9);
		}
	}

	void Update () {
		if (type == 0) {
			bar.transform.localScale = new Vector3( necromancer.hp / maxHealth, 1, 1);
		} else {
			bar.transform.localScale = new Vector3( boss.hp / maxHealth, 1, 1);
		}
	}
}
