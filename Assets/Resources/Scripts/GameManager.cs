using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// PARAMETERS
	Color backgroundColor = new Color((100f/256f), (180f/256f), (100f/256f));

	GameObject player;
	GameObject background;
	GameObject infectionBar;


	// Use this for initialization
	void Start () {
		player = new GameObject ();
		player.AddComponent<PlayerController> ().init (this);

		background = GameObject.CreatePrimitive (PrimitiveType.Quad);
		background.transform.localScale = new Vector3 (180, 100, 1);
		background.GetComponent<Renderer> ().material.color = backgroundColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void MakeSprite(GameObject obj, string textureName, Transform parentTransform, 
						   float x, float y, float xScale, float yScale, float pixelsPer, params float[] pivot) {
		obj.transform.parent = parentTransform;
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
		print (obj.GetComponent<SpriteRenderer> ().sprite.texture.ToString ());
	}
}
