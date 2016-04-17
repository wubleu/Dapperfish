using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	
		GameManager gManager;
		public PlayerController necromancerController;
	public int peasantCount = 0;
	
	
		// Use this for initialization
		public void init (GameManager gMan, PlayerController pController) {
			gManager = gMan;
			necromancerController = pController;
			transform.parent = gManager.transform;
			name = "EnemyManager";
			transform.rotation = transform.parent.rotation;
	
			for (int i = 1; i <= 16; i++) {
				GameObject spawner = GameObject.Find ("Spawn Zone " + i.ToString ());
				PeasantSpawn (spawner.transform.position);
				PeasantSpawn (spawner.transform.position);
			}
		}
	
	
		// Update is called once per frame
	//	void Update () {
	//		if (PeasantSpawn ()) {
	//			peasantCount++;
	//		}
	//	}
	
		void PeasantSpawn(Vector3 zone) {
				GameObject peasant = new GameObject ();
				peasant.AddComponent<Peasant> ().init (gManager, this);
				peasant.transform.position = zone;
		peasantCount++;
		}
}
