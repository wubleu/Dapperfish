using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	// PARAMETERS
	Color healthBarColor = new Color ((0f/256f), (100f/256f), (0f/256f));

	GameManager gManager;
	PlayerController necromancer;
	float maxHealth;

	// Use this for initialization
	public void init (GameManager gameManager, PlayerController owner, float maxHP) {
		gManager = gameManager;
		necromancer = owner;
		GameObject outline = new GameObject ();
		gManager.MakeSprite (outline, "BarOutline", necromancer.transform, 
			7/necromancer.transform.localScale.x, -3.93f/necromancer.transform.localScale.y, 
			1.3725f/necromancer.transform.localScale.x, 2.5f/necromancer.transform.localScale.y, 200);
		gameObject.name = "InfectionBarOutline";
		gManager.MakeSprite (gameObject, "Bar", outline.transform, 0, -.425f, 1f, 0f, 200, .5f, 0);
		gameObject.GetComponent<SpriteRenderer> ().material.color = healthBarColor;
		maxHealth = maxHP;
	}


	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3 (1, necromancer.hp / maxHealth, 0);
	}
}
