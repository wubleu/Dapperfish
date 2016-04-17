//using UnityEngine;
//using System.Collections;
//
//public class EnemySpawner : MonoBehaviour {
//
//	GameManager gManager;
//	public PlayerController necromancerController;
//
//
//	// Use this for initialization
//	public void init (GameManager gMan, PlayerController pController) {
//		gManager = gMan;
//		necromancerController = pController;
//		transform.parent = gManager.transform;
//		name = "EnemyManager";
//		transform.rotation = transform.parent.rotation;
//
//		for (int i = 1; i <= 16; i++) {
//			GameObject spawner = GameObject.Find ("Spawn Zone " + i.ToString ());
//			PeasantSpawn (spawner.transform.position);
//			PeasantSpawn (spawner.transform.position);
//		}
//	}
//
//
//	// Update is called once per frame
////	void Update () {
////		if (PeasantSpawn ()) {
////			peasantCount++;
////		}
////	}
//
//	void PeasantSpawn(Vector3 zone) {
//			GameObject peasant = new GameObject ();
//			peasant.AddComponent<Peasant> ().init (gManager, this);
//			peasant.transform.position = zone;
//	}
//
////	bool PeasantSpawn() {
////		float rand = Random.Range (-100, 100);
////		if (Mathf.Abs (rand) <= peasantSpawnRate) {
////			GameObject peasant = new GameObject ();
////			peasant.AddComponent<Peasant> ().init (gManager, this);
////			if (rand > 0) {
////				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x-11, 0,
////					necromancerController.gameObject.transform.position.z+Random.Range (-6, 6));
////			} else {
////				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 0,
////					necromancerController.gameObject.transform.position.z-6);
////			}
////			return true;
////		} else if (Mathf.Abs (rand) <= peasantSpawnRate*2) {
////			GameObject peasant = new GameObject ();
////			peasant.AddComponent<Peasant> ().init (gManager, this);
////			if (rand > 0) {
////				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+11, 0,
////					necromancerController.gameObject.transform.position.z+Random.Range (-6, 6));
////			} else {
////				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 0,
////					necromancerController.gameObject.transform.position.z+6);
////			}
////			return true;
////		}
////		return false;
////	}
//}
//
//
//using UnityEngine;
//using System.Collections;
//
//public class EnemyManager : MonoBehaviour {
//
//	GameManager gManager;
//	public PlayerController necromancerController;
//	float peasantSpawnRate;
//	public int peasantCount;
//
//
//	// Use this for initialization
//	public void init (GameManager gMan, PlayerController pController) {
//		gManager = gMan;
//		necromancerController = pController;
//		transform.parent = gManager.transform;
//		name = "EnemyManager";
//		peasantSpawnRate = 5f;
//		peasantCount = 0;
//		transform.rotation = transform.parent.rotation;
//	}
//
//
//	// Update is called once per frame
//	void Update () {
//		if (PeasantSpawn ()) {
//			peasantCount++;
//		}
//	}
//
//
//	bool PeasantSpawn() {
//		float rand = Random.Range (-100, 100);
//		if (Mathf.Abs (rand) <= peasantSpawnRate) {
//			GameObject peasant = new GameObject ();
//			peasant.AddComponent<Peasant> ().init (gManager, this);
//			if (rand > 0) {
//				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x-11, 0,
//					necromancerController.gameObject.transform.position.z+Random.Range (-6, 6));
//			} else {
//				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 0,
//					necromancerController.gameObject.transform.position.z-6);
//			}
//			return true;
//		} else if (Mathf.Abs (rand) <= peasantSpawnRate*2) {
//			GameObject peasant = new GameObject ();
//			peasant.AddComponent<Peasant> ().init (gManager, this);
//			if (rand > 0) {
//				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+11, 0,
//					necromancerController.gameObject.transform.position.z+Random.Range (-6, 6));
//			} else {
//				peasant.transform.position = new Vector3 (necromancerController.gameObject.transform.position.x+Random.Range (-11, 11), 0,
//					necromancerController.gameObject.transform.position.z+6);
//			}
//			return true;
//		}
//		return false;
//	}
//}
