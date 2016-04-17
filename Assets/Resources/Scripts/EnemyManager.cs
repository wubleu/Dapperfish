using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
		
		string[] instructions = Resources.Load<TextAsset>("Scripts/level1").text.Split(new char[1]{'\n'});


		foreach (string instruction in instructions) {
			string[] parts = instruction.Split (new char[1]{ ':' });
			GameObject spawner = GameObject.Find ("Spawn Zone " + parts[0]);
			for (int i = 1; i <= 3; i++) {
				for (int j = 0; j < Int32.Parse (parts [i]); j++) {
					PeasantSpawn (spawner.transform.position, i);
				}
			}
		}
	}


	// Update is called once per frame
//	void Update () {
//		if (PeasantSpawn ()) {
//			peasantCount++;
//		}
//	}

	void PeasantSpawn(Vector3 zone, int type) {
			GameObject peasant = new GameObject ();
			peasant.AddComponent<Peasant> ().init (gManager, this);
			peasant.transform.position = zone;
			peasantCount++;
	}
}
