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
		testing123();
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
		GameObject death = new GameObject ();
		death.transform.position = necromancer.transform.position;
		death.transform.localEulerAngles = new Vector3 (90, 0, 0);
		death.AddComponent<SpriteRenderer> ();
		Animator anim = death.AddComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Necromancer Death Controller");
		Destroy (necromancer.gameObject);
		Destroy (death, 1);

	}

	void testing123() {
		for (int i = 0; i < 5; i++) {
			Enemies.makeArcher(this, eManager, necromancer, new Vector3(40 + i, 0, 4));
		}
	}

}
