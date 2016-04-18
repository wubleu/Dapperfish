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
		name = "Enemy Manager";
		peasantSpawnRate = 5f;
		peasantCount = 0;
		transform.rotation = transform.parent.rotation;
	}

	
	// Update is called once per frame
	void Update () {
		if (PeasantSpawn()) {
			peasantCount++;
		}
	}

	bool PeasantSpawn() {
		float rand = Random.Range (-100, 100);
		if (Mathf.Abs (rand) <= peasantSpawnRate) {
			GameObject peasant = Enemies.makePeasant();
			if (rand > 0) {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x-11, 0,
					necromancerController.gameObject.transform.position.z+Random.Range (-6, 6));
			} else {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 0,
					necromancerController.gameObject.transform.position.z-6);
			}
			return true;
		} else if (Mathf.Abs (rand) <= peasantSpawnRate*2) {
			GameObject peasant = Enemies.makePeasant();
			if (rand > 0) {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+11, 0,
					necromancerController.gameObject.transform.position.z+Random.Range (-6, 6));
			} else {
				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 0,
					necromancerController.gameObject.transform.position.z+6);
			}
			return true;
		}
		return false;
	}
}
