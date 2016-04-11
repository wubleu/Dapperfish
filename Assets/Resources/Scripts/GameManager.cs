using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// PARAMETERS
	Color backgroundColor = new Color((100f/256f), (150f/256f), (100f/256f));

	GameObject necromancer;
	GameObject background;
	GameObject infectionBar;
	GameObject eManager;


	void Start(){
		gameObject.transform.Rotate (90, 180, 180);
		GameObject camera = GameObject.Find ("Main Camera");
		camera.transform.position = new Vector3 (0, 10, 0);
		camera.transform.Rotate (90, 0, 0);
		init ();
	}


	void init () {
		necromancer = new GameObject ();
		GameObject camera = GameObject.Find ("Main Camera");
		camera.transform.position = new Vector3 (necromancer.transform.position.x, 10, necromancer.transform.position.z);
		eManager = new GameObject ();
		PlayerController necroContr = necromancer.AddComponent<PlayerController> ();
		EnemyManager eMan = eManager.AddComponent<EnemyManager> ();
		necroContr.init (this, eMan);
		eMan.init (this, necroContr);
//		if (background == null) {
//			background = GameObject.CreatePrimitive (PrimitiveType.Quad);
//			background.transform.localScale = new Vector3 (180, 1, 100);
//			background.GetComponent<Renderer> ().material.color = backgroundColor;
//		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}


	public void Death() {
		GameObject.Find ("Main Camera").transform.SetParent (null, true);
		foreach (AIBehavior unit in FindObjectsOfType<AIBehavior>()) {
			Destroy (unit.gameObject);
		}
		foreach (SpellShot unit in FindObjectsOfType<SpellShot>()) {
			Destroy (unit.gameObject);
		}
		Destroy(eManager);
		Destroy (necromancer);
		init ();
	}
		

	public void MakeSprite(GameObject obj, string textureName, Transform parentTransform, 
						   float x, float y, float xScale, float yScale, float pixelsPer, params float[] pivot) {
		obj.transform.parent = parentTransform;
		obj.transform.rotation = obj.transform.parent.rotation;
		obj.transform.localPosition = new Vector3 (x, y, 0);
		obj.transform.localScale = new Vector3 (xScale, yScale, 1);
		obj.name = textureName;
		SpriteRenderer rend = obj.AddComponent<SpriteRenderer> ();
		Texture2D texture = Resources.Load<Texture2D> ("Textures/" + textureName);
		float xPiv = .5f;
		float yPiv = .5f;
		if (pivot.Length > 1) {
			xPiv = pivot[0];
			yPiv = pivot [1];
		}
		rend.sprite  = Sprite.Create (texture,
			new Rect(0, 0, texture.width, texture.height), new Vector2 (xPiv, yPiv), pixelsPer);
	}
}
