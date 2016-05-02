using UnityEngine;
using System.Collections;

public class Gate2 : MonoBehaviour {

	public GameManager Gman;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Gman.waveclear) {
			Destroy (this.gameObject);
		}
	}
}
