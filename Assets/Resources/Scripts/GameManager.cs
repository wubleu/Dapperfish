using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// PARAMETERS
//	Color backgroundColor = new Color((100f/256f), (150f/256f), (100f/256f));

	PlayerController necromancer;
//	GameObject background;
//	GameObject infectionBar;
	EnemyManager eManager;


	void Start() {
		necromancer = new GameObject().AddComponent<PlayerController>();
		necromancer.transform.position = new Vector3(0, 0, 0);
		Camera.main.transform.parent = necromancer.transform;
		Camera.main.transform.localEulerAngles = new Vector3(90, 0, 0);
		Camera.main.transform.localPosition = new Vector3(0, 20, 0);
		Camera.main.orthographicSize = 7;
		eManager = new GameObject().AddComponent<EnemyManager> ();
		eManager.init(this, necromancer);
		necromancer.init(this, eManager);
	}

	public void Death() {
		Camera.main.transform.SetParent (null, true);
		foreach (AIBehavior unit in FindObjectsOfType<AIBehavior>()) {
			Destroy (unit.gameObject);
		}
		foreach (SpellShot unit in FindObjectsOfType<SpellShot>()) {
			Destroy (unit.gameObject);
		}
		Destroy(eManager);
		Destroy (necromancer);
	}
		

//	public void MakeSprite(GameObject obj, string textureName, Transform parentTransform, 
//						   float x, float y, float xScale, float yScale, float pixelsPer, params float[] pivot) {
//		obj.transform.parent = parentTransform;
//		if (parentTransform != null) {
//			obj.transform.rotation = obj.transform.parent.rotation;
//		}
//		obj.transform.localPosition = new Vector3 (x, y, 0);
//		obj.transform.localScale = new Vector3 (xScale, yScale, 1);
//		obj.name = textureName;
//		SpriteRenderer rend = obj.AddComponent<SpriteRenderer> ();
//		Texture2D texture = Resources.Load<Texture2D> ("Textures/" + textureName);
//		float xPiv = .5f;
//		float yPiv = .5f;
//		if (pivot.Length > 1) {
//			xPiv = pivot[0];
//			yPiv = pivot [1];
//		}
//		rend.sprite  = Sprite.Create (texture,
//			new Rect(0, 0, texture.width, texture.height), new Vector2 (xPiv, yPiv), pixelsPer);
//	}
}
