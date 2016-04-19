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
		name = "Enemy Manager";
		transform.rotation = transform.parent.rotation;
		
		string[] instructions = Resources.Load<TextAsset>("Scripts/level1").text.Split(new char[1]{'\n'});

		foreach (string instruction in instructions) {
			string[] parts = instruction.Split (new char[1]{ ':' });
			GameObject spawner = GameObject.Find ("Spawn Zone " + parts[0]);
			for (int i = 1; i <= 3; i++) {
				for (int j = 0; j < Int32.Parse (parts [i]); j++) {
					Spawn (spawner.transform.position, i, Int32.Parse(parts[4]));
				}
			}
		}
	}

	void Spawn(Vector3 zone, int type, int radius) {
		zone = new Vector3 (zone.x + UnityEngine.Random.Range (-radius, radius) + UnityEngine.Random.value, 0, zone.z + UnityEngine.Random.Range (-radius, radius) + UnityEngine.Random.value);
		if (type == 1) {
			Enemies.makePeasant (gManager, this, necromancerController, zone);
			peasantCount++;
		}
		if (type == 2) {
			Enemies.makePeasant (gManager, this, necromancerController, zone);
			peasantCount++;
		}
		if (type == 2) {
			Enemies.makeKnight (gManager, this, necromancerController, zone);
			peasantCount++;
		}
	}
}
