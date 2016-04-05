using UnityEngine;
using System.Collections;

public class InfectionBar : MonoBehaviour {

	// PARAMETERS
	float speed = 28f;
	Color infectionBarColor = new Color ((155f/256f), (0f/256f), (0f/256f));
	Color infectionBarLowColor = new Color ((100f / 256f), (20f / 256f), (20f / 256f), .4f);

	GameManager gManager;
	PlayerController necromancer;
	Material material;
	public float infectionCharge;

	// Use this for initialization
	public void init (GameManager gameManager, PlayerController owner) {
		gManager = gameManager;
		necromancer = owner;
		GameObject outline = new GameObject ();
		gManager.MakeSprite (outline, "BarOutline", necromancer.transform, 
			8/necromancer.transform.localScale.x, -3.93f/necromancer.transform.localScale.y, 
			1.3725f/necromancer.transform.localScale.x, 2.5f/necromancer.transform.localScale.y, 200);
		gameObject.name = "InfectionBarOutline";
		gManager.MakeSprite (gameObject, "Bar", outline.transform, 0, -.425f, 1f, 0f, 200, .5f, 0);
		material = gameObject.GetComponent<SpriteRenderer> ().material;
		material.color = infectionBarColor;
		infectionCharge = 0;
	}


	// Update is called once per frame
	void Update () {
		if (infectionCharge < 100) {
			infectionCharge += speed*Time.deltaTime;
			transform.localScale = new Vector3 (1, infectionCharge / 100f, 0);
		} else {
			infectionCharge = 100;
			transform.localScale = new Vector3(1, 1, 0);
		}
		if (infectionCharge < 50) {
			material.color = infectionBarLowColor;
		} else {
			material.color = infectionBarColor;
		}
	}
}
