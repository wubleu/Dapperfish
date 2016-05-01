using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	SphereCollider pause;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<Rigidbody> ().useGravity = false;
		pause = gameObject.AddComponent<SphereCollider> ();
		pause.isTrigger = true;
		pause.radius = 1000;
		pause.name = "PauseCollider";
	}
	
	void OnTriggerEnter(Collider coll) {
		SpellEffect spell = coll.gameObject.GetComponent<SpellEffect> ();
		Spell bossSpell = coll.gameObject.GetComponent<Spell> ();
		AIBehavior AI = coll.gameObject.GetComponent<AIBehavior> ();
		if (AI != null) {
			AI.PauseAI ();
		} else if (spell != null) {
			spell.PauseSpell ();
		} else if (bossSpell != null) {
			bossSpell.PauseSpell ();
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
		foreach (Spell spell in GameObject.FindObjectsOfType<Spell>()) {
			spell.UnPauseSpell ();
		}
		GameObject.FindObjectOfType<PlayerController> ().UnPausePlayer();
		Destroy (gameObject);
	}
}
