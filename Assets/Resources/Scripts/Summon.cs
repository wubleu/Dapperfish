using UnityEngine;
using System.Collections;

public class Summon : Spell {

	//PARAMETERS
	float size = 2, lifeLength = .8f, percentPeasantvsArcher = 60, percentKnight = 15;
	int minNumEnemies = 2, maxNumEnemies = 4;


	// Use this for initialization
	public int init (Vector3 pos) {
		base.init (pos, size, lifeLength, 0, "Boss Summon Sprite Sheet", "Boss Summon Controller", "Summon");
		int retVal;
		if (Random.Range (0, 100) < percentKnight) {
			Enemies.makeKnight (GameObject.FindObjectOfType<GameManager>(), GameObject.FindObjectOfType<EnemyManager>(), GameObject.FindObjectOfType<PlayerController>(), pos, true);
			return 1;
		}
		for (int i = 1; i <= (retVal = Random.Range (minNumEnemies, maxNumEnemies)); i++) {
			int rand = Random.Range (0, 100);
			if (rand < percentPeasantvsArcher) {
				Enemies.makePeasant (GameObject.FindObjectOfType<GameManager>(), GameObject.FindObjectOfType<EnemyManager>(), GameObject.FindObjectOfType<PlayerController>(), pos, true);
			} else {
				Enemies.makeArcher (GameObject.FindObjectOfType<GameManager>(), GameObject.FindObjectOfType<EnemyManager>(), GameObject.FindObjectOfType<PlayerController>(), pos, true);
			}
		}
		return retVal;
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
	}
}
