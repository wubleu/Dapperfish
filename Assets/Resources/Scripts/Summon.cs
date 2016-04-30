using UnityEngine;
using System.Collections;

public class Summon : Spell {

	// Use this for initialization
	public void init (Vector3 pos, float lifeLength) {
		base.init (pos, .85f, lifeLength, 0, "Spell Effects Sprite Sheet", "Blight Controller", "Summon");
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

	}
}
