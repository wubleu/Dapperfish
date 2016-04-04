using UnityEngine;
using System.Collections;

public class InfectionBar : MonoBehaviour {

	// PARAMETERS
	float speed = .2f;
	Color infectionBarColor = new Color ((155f/256f), (0f/256f), (0f/256f));

	GameManager gManager;
	PlayerController necromancer;

	// Use this for initialization
	public void init (GameManager gameManager, PlayerController owner) {
		gManager = gameManager;
		necromancer = owner;
		GameObject outline = new GameObject ();
		gManager.MakeSprite (outline, "BarOutline", necromancer.transform, 32, -15.7f, 4f, 5f, 200);
		gameObject.name = "InfectionBarOutline";
		gManager.MakeSprite (gameObject, "Bar", outline.transform, 0, -.425f, 1f, 0f, 200, .5f, 0);
		gameObject.GetComponent<SpriteRenderer> ().material.color = infectionBarColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.y <= 1) {
			transform.localScale += new Vector3(0, speed*Time.deltaTime, 0);
		} else {
			transform.localScale = new Vector3(1, 1, 0);
		}
	}
}
