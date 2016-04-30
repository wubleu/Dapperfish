using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	SphereCollider pause;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<Rigidbody> ();
		pause = gameObject.AddComponent<SphereCollider> ();
		pause.isTrigger = true;
		pause.radius = 1000;
		pause.name = "PauseCollider";
		print ("PauseCollider made");
	}
	
	void OnTriggerEnter(Collider coll) {
		SpellEffect spell = coll.gameObject.GetComponent<SpellEffect> ();
		AIBehavior AI = coll.gameObject.GetComponent<AIBehavior> ();
		if (AI != null) {
			AI.PauseAI ();
		} else if (spell != null) {
			spell.PauseSpell ();
		} else if (coll.name == "Necromancer") {
			coll.GetComponent<PlayerController> ().PausePlayer();
		}
	}

	public void UnPauseAll() {
		foreach (AIBehavior AI in GameObject.FindObjectsOfType<AIBehavior>()) {
			AI.UnPauseAI ();
		} 
		foreach (SpellEffect spell in GameObject.FindObjectsOfType<SpellEffect>()) {
			spell.UnPauseSpell ();
		}
		GameObject.FindObjectOfType<PlayerController> ().UnPausePlayer();
		Destroy (gameObject);
	}
}
