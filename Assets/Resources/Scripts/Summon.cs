using UnityEngine;
using System.Collections;

public class Summon : Spell {

	//PARAMETERS
	float size = .85f, lifeLength = .4f, percentPeasant = 50, percentArcher = 30;
	int minNumEnemies = 3, maxNumEnemies = 4;


	// Use this for initialization
	public void init (Vector3 pos) {
		base.init (pos, size, lifeLength, 0, "Spell Effects Sprite Sheet", "Blight Controller", "Summon");
		for (int i = 1; i <= Random.Range (1, 6); i++) {
			int rand = Random.Range (1, 100);
			if (rand <= percentPeasant) {
				Enemies.makePeasant (GameObject.FindObjectOfType<GameManager>(), GameObject.FindObjectOfType<EnemyManager>(), GameObject.FindObjectOfType<PlayerController>(), pos, true);
			} else if (rand <= percentArcher+percentPeasant) {
				Enemies.makeArcher (GameObject.FindObjectOfType<GameManager>(), GameObject.FindObjectOfType<EnemyManager>(), GameObject.FindObjectOfType<PlayerController>(), pos, true);
			} else {
				Enemies.makeKnight (GameObject.FindObjectOfType<GameManager>(), GameObject.FindObjectOfType<EnemyManager>(), GameObject.FindObjectOfType<PlayerController>(), pos, true);
			}
		}
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
	}
}
