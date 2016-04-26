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
			if (parts.Length == 5) {
				GameObject spawner = GameObject.Find ("Spawn Zone " + parts [0]);
				for (int i = 1; i <= 3; i++) {
					for (int j = 0; j < Int32.Parse (parts [i]); j++) {
						Spawn (spawner.transform.position, i, Int32.Parse (parts [4]));
					}
				}
			}
		}
	}

	public void delayedSpawn(String tag){
		string[] instructions = Resources.Load<TextAsset>("Scripts/level1").text.Split(new char[1]{'\n'});

		foreach (string instruction in instructions) {
			string[] parts = instruction.Split (new char[1]{ ':' });
			if (parts.Length == 6) {
				if (parts [5] == tag) {
					GameObject spawner = GameObject.Find ("Spawn Zone " + parts [0]);
					for (int i = 1; i <= 3; i++) {
						for (int j = 0; j < Int32.Parse (parts [i]); j++) {
							Spawn (spawner.transform.position, i, Int32.Parse (parts [4]));
						}
					}
				}
			}
		}
	}

	void Spawn(Vector3 zone, int type, int radius) {
		zone = new Vector3 (zone.x + UnityEngine.Random.Range (-radius, radius) + UnityEngine.Random.value, 0, zone.z + UnityEngine.Random.Range (-radius, radius) + UnityEngine.Random.value);
		switch (type) {
			case 1:
				Enemies.makePeasant(gManager, this, necromancerController, zone);
				peasantCount++;
				break;
			case 2:
				Enemies.makeArcher(gManager, this, necromancerController, zone);
				peasantCount++;
				break;
			case 3:
				Enemies.makeKnight(gManager, this, necromancerController, zone);
				peasantCount++;
				break;
		}
	}
}
