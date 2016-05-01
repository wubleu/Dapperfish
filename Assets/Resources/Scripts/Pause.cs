using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	SphereCollider pause;

	// Use this for initialization
	void Start () {
		foreach (AIBehavior AI in GameObject.FindObjectsOfType<AIBehavior>()) {
			AI.PauseAI ();
		} 
		foreach (SpellEffect spell in GameObject.FindObjectsOfType<SpellEffect>()) {
			spell.PauseSpell ();
		}
		foreach (Spell spell in GameObject.FindObjectsOfType<Spell>()) {
			spell.PauseSpell ();
		}
		GameObject.FindObjectOfType<PlayerController> ().PausePlayer();
		NecromancerBoss necroBoss = GameObject.FindObjectOfType<NecromancerBoss> ();
		if (necroBoss != null) {
			necroBoss.PauseBoss ();
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
<<<<<<< HEAD
		//GameObject.FindObjectOfType<NecromancerBoss> ().UnPauseBoss ();
=======
		NecromancerBoss necroBoss = GameObject.FindObjectOfType<NecromancerBoss> ();
		if (necroBoss != null) {
			necroBoss.UnPauseBoss ();
		}
>>>>>>> origin/RyanMechanicStudy
		Destroy (gameObject);
	}
}
