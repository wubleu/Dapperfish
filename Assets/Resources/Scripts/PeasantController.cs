using UnityEngine;
using System.Collections;

public class PeasantController : MonoBehaviour {

	GameManager gManager;
	Material peasantMaterial;


	// Use this for initialization
	public void init (GameManager owner) {
		gManager = owner;
		gManager.MakeSprite (gameObject, "Circle", gManager.transform, 0, 0, .1f, .2f, 200);
		gameObject.name = "Peasant";
		peasantMaterial = GetComponent<SpriteRenderer> ().material;
		peasantMaterial.color = new Color (1f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
