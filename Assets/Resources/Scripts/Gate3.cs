using UnityEngine;
using System.Collections;

public class Gate3 : MonoBehaviour {

	public GameManager Gman;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Gman.dungeonKeys == 2) {
			Destroy (this.gameObject);
			//Gman.wavebegin = true;
		}
	}
}