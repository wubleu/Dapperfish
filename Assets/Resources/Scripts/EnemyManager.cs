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
	public float wave = 0;
	public float wave2 = 5;
	public float wave3 = 5;
	public int waveCount = 3;

	// Use this for initialization
	public void init (GameManager gMan, PlayerController pController) {
		gManager = gMan;
		necromancerController = pController;
		transform.parent = gManager.transform;
		name = "Enemy Manager";
		transform.rotation = transform.parent.rotation;
		string[] instructions = Resources.Load<TextAsset>("Scripts/level" + gManager.level.ToString()).text.Split(new char[1]{'\n'});
		print ("Scripts/level" + gManager.level.ToString ());

		bool links = true;
		int linkCount = 0;
		bool keys = true;
		int keyCount = 0;
		bool readNum = true;
		foreach (string instruction in instructions) {
			if (readNum){
				int num = Int32.Parse(instruction);
				if(links){
					linkCount = num;
				} else{
					keyCount = num;
				}
				readNum = false;
			} else if (links && linkCount>0) {
					string[] parts = instruction.Split (new char[1]{ ':' });
					Vector3 start = new Vector3 (float.Parse (parts [0]), float.Parse (parts [1]), float.Parse (parts [2]));
					Vector3 end = new Vector3 (float.Parse (parts [3]), float.Parse (parts [4]), float.Parse (parts [5]));
					gMan.links.Add (new Link (start, end, parts [6], parts [7], parts [8], parts [9]));
				linkCount = linkCount - 1;
				if (linkCount == 0) {
					links = false;
					readNum = true;
				}
			} else if (keys && keyCount>0) {
					string[] parts = instruction.Split (new char[1]{ ':' });
					Vector3 location = new Vector3 (float.Parse (parts [0]), float.Parse (parts [1]), float.Parse (parts [2]));
					gMan.keys.Add (new KeyInfo (location, parts [3]));
				keyCount = keyCount - 1;
			}else {
				string[] parts = instruction.Split (new char[1]{ ':' });
				if (parts.Length == 5) {
					GameObject spawner = GameObject.Find ("Spawn Zone " + parts [0]);
					for (int i = 1; i <= 3; i++) {
						for (int j = 0; j < Int32.Parse (parts [i]); j++) {
							Spawn (spawner.transform.position, i, Int32.Parse (parts [4]));
						}
					}
				} else if (parts.Length == 2) {
					if (parts [0] == "9999") {
						Enemies.makeNecroBoss (gManager, this, necromancerController, GameObject.Find ("Spawn Zone " + parts [1]).transform.position);
					}
				}
			}
		}
	}

	void Update(){
		if (gManager.level == 2) {
			
		} else if (gManager.level == 3) {
			if ((wave+=Time.deltaTime)>wave3){
				wave = 0;
				delayedSpawn ("wave");
			}
		}
	}

	public void delayedSpawn(String tag){
		string[] instructions = Resources.Load<TextAsset>("Scripts/level" + gManager.level.ToString()).text.Split(new char[1]{'\n'});

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
