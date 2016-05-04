using UnityEngine;
using System.Collections;

public class Gate3 : MonoBehaviour {

	public GameManager Gman;
	public bool finalfight = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Gman.dungeonKeys == 2 && !finalfight) {
			gameObject.SetActive (false);
		}
		if (finalfight) {
			gameObject.SetActive (true);
			Gman.dungeonKeys = 0;
		}
	}
}