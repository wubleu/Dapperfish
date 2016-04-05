using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blight : MonoBehaviour {

	// PARAMETERS
	float duration = .7f;
	float size = 1.5f;
	Color blightColor = new Color(.2f, .1f, .4f, .7f);

	PlayerController necromancer;
	GameManager gManager;
	Material material;
	float blightPower;
	List<AIBehavior> infected;


	// Use this for initialization
	public void init (PlayerController owner, GameManager gMan, float blightStrength) {
		necromancer = owner;
		gManager = gMan;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		gManager.MakeSprite (gameObject, "SolidCircle", gMan.transform, mousePos.x, mousePos.y, size, size, 200);
		material = gameObject.GetComponent<SpriteRenderer> ().material;
		material.color = blightColor;
		name = "Blight";
		gameObject.AddComponent<CircleCollider2D> ().isTrigger = true;
		blightPower = blightStrength;
		infected = new List<AIBehavior> ();
	}
	
	// Update is called once per frame
	void Update () {
		material.color = new Color (.2f, .1f, .4f, duration);
		duration -= Time.deltaTime;
		if (duration <= 0) {
//			foreach (AIBehavior infectedAI in infected) {
//				if (infectedAI != null) {
//					infectedAI.Infect ();
//				}
//			}
			Destroy (gameObject);
		}
	}

	public void Infect(float infectionCost, AIBehavior infectedAI) {
		if (blightPower > infectionCost) {
			blightPower -= infectionCost;
			infectedAI.Infect ();
		}
	}
}
