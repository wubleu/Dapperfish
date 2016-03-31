using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	GameObject player;


	// Use this for initialization
	void Start () {
		player = new GameObject ();
		player.AddComponent<PlayerController> ().init (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void MakeSprite(GameObject obj, Sprite s, Transform parentTransform, 
						   float x, float y, float xScale, float yScale, float pixelsPer, params float[] pivot) {
		obj.transform.parent = parentTransform;
		obj.transform.localPosition = new Vector3 (x, y, 0);
		obj.transform.localScale = new Vector3 (xScale, yScale, 1);
		obj.name = s.name + "Sprite";
		SpriteRenderer rend = obj.AddComponent<SpriteRenderer> ();
		Sprite texture = s;
		float xBound = .5f;
		float yBound = .5f;
		if (pivot.Length > 0) {
			xBound = pivot [0];
		} if (pivot.Length > 1) {
			yBound = pivot [1];
		}
		rend.sprite = texture;
	}
}
