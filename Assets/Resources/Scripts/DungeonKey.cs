using UnityEngine;
using System.Collections;

public class DungeonKey : MonoBehaviour {

	public GameManager Gman;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.name == "Necromancer") {
			Gman.dungeonKeys = Gman.dungeonKeys + 1;
			Destroy (gameObject);
		}
	}
}
