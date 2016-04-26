using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// PARAMETERS
	public float gridSize = 10f;

	PlayerController necromancer;
	EnemyManager eManager;
	public List<AIBehavior>[,] enemyGrid;
	public int xGridOrigin = -20;
	public int yGridOrigin = -80;
	public int xDimension;
	public int yDimension;
	bool done = false;
	public Text objectives;
	public Text alert;
	public AudioClip chargeClip;
	public AudioClip scratch;


	void Start() {
		xDimension = 16;
		yDimension = 10;
		enemyGrid = new List<AIBehavior>[xDimension, yDimension];
		for (int x = 0; x < xDimension; x++) {
			for (int y = 0; y < yDimension; y++) {
				enemyGrid [x, y] = new List<AIBehavior> ();
			}
		}

		chargeClip = Resources.Load ("Sounds/Charge") as AudioClip;
		scratch = Resources.Load ("Sounds/Scratch") as AudioClip;

		necromancer = new GameObject().AddComponent<PlayerController>();
		necromancer.transform.position = new Vector3(0, 0, 0);
		Camera.main.transform.parent = necromancer.transform;
		Camera.main.transform.localEulerAngles = new Vector3(90, 0, 0);
		Camera.main.transform.localPosition = new Vector3(0, 20, 0);
		Camera.main.orthographicSize = 10;
		eManager = new GameObject().AddComponent<EnemyManager> ();
		eManager.init(this, necromancer);
		necromancer.init(this, eManager);
		testing123();
		RefillGrid ();
	}

	void Update() {
		RefillGrid ();
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
		GameObject death = new GameObject();
		death.transform.position = necromancer.transform.position;
		death.transform.localEulerAngles = new Vector3 (90, 0, 0);
		death.AddComponent<SpriteRenderer>();
		Animator anim = death.AddComponent<Animator>();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Necromancer Death Controller");
		alert.text = "Game Over.";
		Destroy(necromancer.gameObject);
		Destroy(death, 1);
	}

	public void Finish() {
		if (!done) {
			alert.text =  "You beat the level!";
			done = true;
		}
	}

	public void EnemyDeath(Vector3 pos, string name, bool enemy){
		GameObject death = new GameObject ();
		SpriteRenderer rend = death.AddComponent<SpriteRenderer> ();
		death.transform.position = pos;
		death.transform.localEulerAngles = new Vector3 (90, 0, 0);
		Sprite[] cSprites;
		if (name == "Archer") {
			cSprites = Resources.LoadAll<Sprite> ("Textures/Skeleton Archer Sprite Sheet");
			if (enemy) {
				rend.sprite = cSprites [2];
			} else {
				rend.sprite = cSprites [5];
			}
			Destroy (death.gameObject, 2f);
		}
		if (name == "Peasant") {
			cSprites = Resources.LoadAll<Sprite> ("Textures/Zombie Sprite Sheet");
			if (enemy) {
				rend.sprite = cSprites [6];
			} else {
				rend.sprite = cSprites [2];
			}
			Destroy (death.gameObject, 2f);
		}
		if (name == "Knight") {
			cSprites = Resources.LoadAll<Sprite> ("Textures/Knight Sprite Sheet");
			rend.sprite = cSprites [9];
			Destroy (death.gameObject, 2f);
		}
	}

	void testing123() {
		for (int i = 0; i < 5; i++) {
			Enemies.makeArcher(this, eManager, necromancer, new Vector3(40 + i, 0, 4));
		}
	}

	public void makeText(string stuff) {
		GameObject textObject = new GameObject();
		textObject.name = "text";
		textObject.transform.position = necromancer.transform.position;
		textObject.transform.eulerAngles = new Vector3(90, 0, 0);
		textObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		TextMesh text = textObject.AddComponent<TextMesh>();
		text.fontSize = 85;
		text.color = new Color(0, 0, 0);
		text.text = stuff;
	}

	private void RefillGrid() {
		for (int x = 0; x < xDimension; x++) {
			for (int y = 0; y < yDimension; y++) {
				enemyGrid [x, y].Clear ();
			}
		}
		foreach (AIBehavior unit in GameObject.FindObjectsOfType<AIBehavior>()) {
			int xSquare = ((int)unit.gameObject.transform.position.x - xGridOrigin) / 10;
			int ySquare = ((int)unit.gameObject.transform.position.y - yGridOrigin) / 10;
			enemyGrid [xSquare - 1, ySquare - 1].Add (unit);
		}
	}
}
