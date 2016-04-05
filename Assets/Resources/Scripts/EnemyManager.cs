using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	GameManager gManager;
	public PlayerController necromancerController;
	float peasantSpawnRate;
	public int peasantCount;


	// Use this for initialization
	public void init (GameManager gMan, PlayerController pController) {
		gManager = gMan;
		necromancerController = pController;
		transform.parent = gManager.transform;
		name = "EnemyManager";
		peasantSpawnRate = 5f;
		peasantCount = 0;
	}

	
	// Update is called once per frame
	void Update () {
		if (PeasantSpawn ()) {
			peasantCount++;
		}
	}


	bool PeasantSpawn() {
		float rand = Random.Range (-100, 100);
		if (Mathf.Abs (rand) <= peasantSpawnRate) {
			GameObject peasant = new GameObject ();
			peasant.AddComponent<Peasant> ().init (gManager, this);
			if (rand > 0) {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x-11, 
					necromancerController.gameObject.transform.position.y+Random.Range (-6, 6));
			} else {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 
					necromancerController.gameObject.transform.position.y-6);
			}
			print (peasant.transform.position.ToString());
			return true;
		} else if (Mathf.Abs (rand) <= peasantSpawnRate*2) {
			GameObject peasant = new GameObject ();
			peasant.AddComponent<Peasant> ().init (gManager, this);
			if (rand > 0) {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+11, 
					necromancerController.gameObject.transform.position.y+Random.Range (-6, 6));
			} else {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 
					necromancerController.gameObject.transform.position.y+6);
			}
			print (peasant.transform.position.ToString());
			return true;
		}
		return false;
	}
}
