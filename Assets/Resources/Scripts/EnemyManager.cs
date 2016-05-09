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
	public float wave = 20; //do not change. This is just a counter
	public float wave2 = 16; //wave interval for level 2
	public float wave3 = 9; //wave interval for level 3
	public float wave4 = 10;
	public int currentWave = 1; //for level 2. the wave currently being done. if 1, then wave1 will be called in delayedSpawn
	public int waveCount = 3; //number of wave types in level 2
	public int[] waveNumbers; //count of each wave type in level 2. set in init
	public bool beginWaves = true; //ultimately will only be set true once waves should begin
	public float countdown = 5;
	public int arenaWave = 0;
	public int offset = 0;
	public int arenaWaves = 3;

	// Use this for initialization
	public void init (GameManager gMan, PlayerController pController) {
		waveNumbers = new int[3];
		waveNumbers [0] = 4;
		waveNumbers [1] = 3;
		waveNumbers [2] = 2;
		gManager = gMan;
		necromancerController = pController;
		transform.parent = gManager.transform;
		name = "Enemy Manager";
		transform.rotation = transform.parent.rotation;
		print (Resources.Load<TextAsset>("Scripts/level" + gManager.level.ToString()));
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
							bool[] isElite = new bool[2];
							isElite [0] = false;
							isElite [1] = false;
							Spawn (spawner.transform.position, i, Int32.Parse (parts [4]), isElite);
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
		if (gManager.level == 2 && gManager.wavebegin) {
			if ((wave += Time.deltaTime) > wave2 && currentWave <= waveCount) {
				wave = 0;
				delayedSpawn ("wave" + currentWave.ToString (), true);
				waveNumbers [currentWave - 1] -= 1;
				if (waveNumbers [currentWave - 1] == 0) {
					currentWave += 1;
				}
			} 
			if (currentWave > waveCount) {
				if ((countdown -= Time.deltaTime) <= 0) {
					if (gManager.AreaClear (2, 7, 6, 10)) {
						gManager.waveclear = true;
					}
				}
			}
		} else if (gManager.level == 3 && !gManager.waveclear && gManager.wavebegin) {
			if ((wave+=Time.deltaTime)>wave3){
				wave = UnityEngine.Random.Range(0,4);
				int w = UnityEngine.Random.Range (1, 6);
				delayedSpawn ("wave" + w.ToString(),true);
				delayedSpawn ("wave" + ((w+1)%6).ToString(),true);
			}
		}
		else if (gManager.level == 4) {
			if ((wave+=Time.deltaTime)>wave4){
				print (arenaWave);
				wave = 0;
				delayedSpawn ("wave" + (arenaWave + offset).ToString(),true);
				arenaWave = (arenaWave + 1) % 2;
				if (arenaWave == 0) {
					arenaWaves -= 1;
					if (arenaWaves == 0) {
						offset += 2;
						arenaWaves = 3;
						if (offset > 4) {
							offset = 4;
						}
					}
				}
			}
		}
	}

	public void delayedSpawn(String tag, bool isWave){
		string[] instructions = Resources.Load<TextAsset>("Scripts/level" + gManager.level.ToString()).text.Split(new char[1]{'\n'});
		bool[] isElite = new bool[2];
		isElite [0] = false;
		isElite [1] = isWave;
		if (tag == "wave3" && gManager.level==3) {
			isElite [0] = true;
		}
		foreach (string instruction in instructions) {
			string[] parts = instruction.Split (new char[1]{ ':' });
			if (parts.Length > 6) {
				if (parts [5] == tag || parts[5] == (tag+"/r")) {
					GameObject spawner = GameObject.Find ("Spawn Zone " + parts [0]);
					for (int i = 1; i <= 3; i++) {
						for (int j = 0; j < Int32.Parse (parts [i]); j++) {
							if (!isWave) {
								Spawn (spawner.transform.position, i, Int32.Parse (parts [4]),isElite);
							} else {
								Spawn (spawner.transform.position, i, Int32.Parse (parts [4]),isElite);
							}
						}
					}
				}
			}
		}
	}

	void Spawn(Vector3 zone, int type, int radius, bool[] isElite) {
		zone = new Vector3 (zone.x + UnityEngine.Random.Range (-radius, radius) + UnityEngine.Random.value, 0, zone.z + UnityEngine.Random.Range (-radius, radius) + UnityEngine.Random.value);
		switch (type) {
			case 1:
			Enemies.makePeasant(gManager, this, necromancerController, zone, isElite);
				peasantCount++;
				break;
			case 2:
			Enemies.makeArcher(gManager, this, necromancerController, zone, isElite);
				peasantCount++;
				break;
			case 3:
			Enemies.makeKnight(gManager, this, necromancerController, zone, isElite);
				peasantCount++;
				break;
		}
	}
}
