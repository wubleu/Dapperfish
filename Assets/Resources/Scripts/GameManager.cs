using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {

	// PARAMETERS
	public float gridSize = 10f;
	public Button restart;
	PlayerController necromancer;
	EnemyManager eManager;
	Pause pause;
	BossSpawner bSpawner;
	public List<AIBehavior>[,] enemyGrid;
	public int xGridOrigin = -30;
	public int yGridOrigin = -90;
	public int xDimension;
	public int yDimension;
	bool done = false;
	public bool checkpoint = false;
	public bool dead = false;
	public Text objectives;
	public Text alert;
	public AudioClip chargeClip;
	public AudioClip scratch;
	public List<Link> links;
	public List<KeyInfo> keys;
	public int Encounter = 0;
	public bool waveclear = false;
	public bool wavebegin = false;
	public int dungeonKeys = 0;
	public GameObject Gate3;

	// THIS IS JUST UNTIL EVAN GETS THE RESTART BUTTON UP
	public int level;
	// TESTING PURPOSES- FEEL FREE TO DELETE, THESE ARE JUST TO DEMONSTRATE PAUSE FUNCTIONALITY
//	float playInterval = 4f;
//	float pauseInterval = 1.5f;
//	float playTimer = 0;
//	float pauseTimer = 0;

	void Start() {
		init ();
		GameObject.Find ("DBox").SetActive (false);
	}

	void init() {
		// ALSO UNTIL EVAN GETS RESTART BUTTON
//		restart.gameObject.SetActive (false);
		dead = false;

		level = Int32.Parse(Application.loadedLevelName.Substring (5));
		print (level);


		xDimension = 18;
		yDimension = 12;
		links = new List<Link>();
		keys = new List<KeyInfo> ();
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
		necromancer.init(this, eManager);
		eManager.init(this, necromancer);
		RefillGrid ();

		foreach (KeyInfo k in keys) {
			print (k.location);
		}
		ChangeObjective ("");

		if (level == 2) {
			necromancer.EnableBlight ();
			necromancer.EnableBlink ();
			necromancer.EnableDamage ();
		}

		if (level == 3 || level == 4) {
			necromancer.EnableRoot ();
			necromancer.EnableBlight ();
			necromancer.EnableBlink ();
			necromancer.EnableDamage ();
//			bSpawner = new GameObject ().AddComponent<BossSpawner> ();
		}
		GameObject gM = GameObject.Find ("tempGManager");
		if (gM != null && level == 1) {
			checkpoint = gM.GetComponent<GameManager>().checkpoint;
			Destroy (gM);
		}
		if (checkpoint && level == 1) {
			Encounter = 4;
			objectives.text = "Open the Fort gate.";
			Destroy(GameObject.Find ("Encounter0"));
			Destroy(GameObject.Find ("Encounter1"));
			Destroy(GameObject.Find ("Encounter1"));
			Destroy(GameObject.Find ("Encounter1"));
			Destroy(GameObject.Find ("Encounter2"));
			Destroy(GameObject.Find ("Encounter3"));
			Destroy(GameObject.Find ("Encounter4"));
			necromancer.EnableBlight ();
			foreach (Key key in GameObject.FindObjectsOfType<Key>()) {
				if (key.transform.position.x > 50) {
					Destroy (key.gameObject);
				}
			}
			necromancer.hasFortKey = true;

			foreach (AIBehavior unit in GameObject.FindObjectsOfType<AIBehavior>()) {
				if ((unit.transform.position.x > 25 && unit.transform.position.x < 55) &&
					(unit.transform.position.z > -30 && unit.transform.position.z < -5)) {
					Destroy (unit.gameObject);
				}
			}
			necromancer.transform.position = new Vector3 (40f, necromancer.transform.position.y, -22f);
		}
	}

	void Update() {
		//GOES WITH THE COMMENTED OUT PLAY/PAUSE TIMER/INTERVAL FIELDS, DEMONSTRATES PAUSE FUNCTIONALITY
//		if (playTimer < playInterval && (playTimer += Time.deltaTime) > playInterval) {
//			PauseGame ();
//			pauseTimer = 0;
//		}
//		else if (pauseTimer < pauseInterval && (pauseTimer += Time.deltaTime) > pauseInterval) {
//			UnPauseGame ();
//			playTimer = 0;
//		}
		//THESE IFS ARE ALSO TEMPORARY TILL BUTTON'S UP

		if (Encounter == 2 && level == 1) {
			necromancer.EnableBlight ();
		}
		if (Encounter == 4 && level == 1) {
			checkpoint = true;
			necromancer.EnableBlink ();
		}
		if (Encounter == 6 && level == 1) {
			necromancer.EnableDamage ();
		}
		if (Encounter == 8 && level == 1) {
			checkpoint = false;
			NextLevel ();
		}

		if (Encounter == 2 && level == 2) {
			wavebegin = true;
			necromancer.EnableRoot ();
		}
		if (Encounter == 4 && level == 2) {
			NextLevel ();
		}

		if (level == 2 && Encounter == 3) {
			Destroy (GameObject.Find ("Boss"));
		}

		if (Encounter == 2 && level == 3) {
			if (!wavebegin) {
				GameObject.Find ("Necromancer Boss").GetComponent<NecromancerBoss> ().waiting = false;
				wavebegin = true;
				Gate3.GetComponent<Gate3> ().finalfight = true;
				Gate3.SetActive (true);
			}

		}
		if (Encounter == 3 && level == 3) {
			alert.text = "You Win?";

			GameObject.Find ("Necromancer Boss").GetComponent<NecromancerBoss> ().Die ();
			foreach (AIBehavior unit in FindObjectsOfType<AIBehavior>()) {
				Destroy (unit.gameObject);
			}
			foreach (SpellShot unit in FindObjectsOfType<SpellShot>()) {
				Destroy (unit.gameObject);
			}
			Encounter++;
		}

		RefillGrid ();
	}

	public void Death() {
		if (bSpawner != null) {
			Destroy (bSpawner);
		}
		restart.gameObject.SetActive (true);
		Camera.main.transform.SetParent (null, true);
		foreach (AIBehavior unit in FindObjectsOfType<AIBehavior>()) {
			Destroy (unit.gameObject);
		}
		foreach (SpellShot unit in FindObjectsOfType<SpellShot>()) {
			Destroy (unit.gameObject);
		}
		NecromancerBoss necroBoss = FindObjectOfType<NecromancerBoss> ();
		if (necroBoss != null) {
			Destroy (necroBoss);
		}
		foreach (Dialogue encounter in FindObjectsOfType<Dialogue>()){
			Destroy(encounter);
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
		dead = true;
	}

	public void Finish() {
		if (!done) {
			//alert.text =  "Objective Complete! Your Conquest Continues!";
			done = true;
		}
		//NextLevel ();
	}

	public void Reset() {
		alert.text = "";
		init ();
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
		for (int x = 0; x < xDimension; ++x) {
			for (int y = 0; y < yDimension; ++y) {
				enemyGrid [x, y].Clear ();
			}
		}
		foreach (AIBehavior unit in GameObject.FindObjectsOfType<AIBehavior>()) {
			int xSquare = ((int)unit.gameObject.transform.position.x - xGridOrigin) / 10;
			int ySquare = ((int)unit.gameObject.transform.position.z - yGridOrigin) / 10;
			//print (xSquare + "   " + ySquare + "   " + unit.transform.position);
			enemyGrid [xSquare, ySquare].Add (unit);
		}
	}

	public void ChangeObjective(string obj){
		objectives.text = obj;
	}

	public void PauseGame() {
		if (pause == null) {
			pause = new GameObject().AddComponent<Pause> ();
		}
	}

	public void UnPauseGame () {
		if (pause != null) {
			pause.UnPauseAll ();
		}
	}

	public bool AreaClear(int minx, int maxx, int miny, int maxy){
		RefillGrid ();
		for (int x = minx; x <= maxx; x++) {
			for (int y = miny; y <= maxy; y++) {
				foreach (AIBehavior unit in enemyGrid[x,y].ToArray()) {
					if (unit.isEnemy) {
						return false;
					}
				}
			}
		}
		return true;
	}

	public void NextLevel(){
		String nextlevel = "Level " + (level + 1);
		SceneManager.LoadScene (nextlevel);
	}

	public void Restart(){
		if (checkpoint) {
			DontDestroyOnLoad (gameObject);
			name = "tempGManager";
		}
		SceneManager.LoadScene("Level " + level);
		Destroy(GameObject.Find ("Encounter3"));
	}

	public void Menu(){

		SceneManager.LoadScene (0);
	}
	public void Quit(){
		Application.Quit ();
	}
}
