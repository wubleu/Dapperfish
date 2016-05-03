﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Dialogue : MonoBehaviour {

	public GameManager Gman;
	public GameObject Dbox;
	public GameObject Necro;
	public GameObject Boss;
	public int encounter;
	bool complete = false;
	int encparts = 0;
	int level;
	string[] instructions;
	string[] line;
	bool start = false;
	int indicator;
	// Use this for initialization
	void Start () {
		level = Gman.level;
		instructions = Resources.Load<TextAsset>("Scripts/Level" + level + "Encounter" + encounter).text.Split(new char[1]{'\n'});
	}
	// Update is called once per frame
	void Update () {
		if (level == 1 && encounter == 3 ) {
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			//print (Gman.AreaClear ());
			if (Gman.AreaClear (6, 7, 6, 7)) {
				Dbox.SetActive (true);
				Gman.PauseGame ();
				start = true;
				complete = true;
			}
		} else if (level == 1 && encounter == 5 && Gman.Encounter == 4) {
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			if (GameObject.Find ("Necromancer").GetComponent<PlayerController> ().hasKey) {
				if (Gman.AreaClear (6, 7, 6, 7)) {
					Dbox.SetActive (true);
					Gman.PauseGame ();
					start = true;
					complete = true;
				}
			}
		}
		if (level == 1 && encounter == 4 && Gman.Encounter == 3) {
			gameObject.GetComponent<BoxCollider> ().enabled = true;
		}

		if (Gman.waveclear && encounter == 3) {
			Dbox.SetActive (true);
			Gman.PauseGame ();
			start = true;
			complete = true;
		}
		NecromancerBoss nBoss = FindObjectOfType<NecromancerBoss> ();
		if (nBoss != null && nBoss.dead && level == 3 && encounter==3) {
			Dbox.SetActive (true);
			Gman.PauseGame ();
			start = true;
			complete = true;
			Gman.waveclear = true;
		}

		if (start) {
			line = instructions [encparts].Split (new char[1]{ ':' });
			if (line.Length == 2) {
				if (Int32.Parse(line[0]) == 0) {
					Necro.SetActive (true);
					Boss.SetActive (false);
					Dbox.GetComponentInChildren<Text> ().text = line[1];
				}
				if (Int32.Parse(line[0]) == 1) {
					Dbox.GetComponentInChildren<Text> ().text = line[1];
					Boss.SetActive (true);
					Necro.SetActive (false);
				}
			}
			//Dbox.GetComponentInChildren<Text> ().text = instructions [encparts];

			if (Input.GetKeyUp (KeyCode.E)) {
				encparts++;
			}
			if (Int32.Parse(line[0]) == 2) {
				start = false;
				Gman.UnPauseGame ();
				encparts++;
				Dbox.GetComponentInChildren<Text> ().text = "";
				Dbox.SetActive (false);
				Necro.SetActive (false);
				if (complete) {
					if (encounter != 99 || encounter != 1) {
						Gman.objectives.text = line [1];
					}
					Gman.Encounter++;
					Destroy (this.gameObject);
				}
				if (encounter == 1 && level == 1) {
					Gman.objectives.text = line[1];
				}
			}
		}
		if (encounter < Gman.Encounter) {
			Destroy (this.gameObject);
		}


	}

	void OnCollisionStay(Collision collision){
		if (collision.gameObject.name == "Necromancer") {
			if (encounter == 4 && level == 1) {
				if (collision.gameObject.GetComponent<PlayerController> ().hasFortKey) {
					Dbox.SetActive (true);
					Gman.PauseGame ();
					start = true;
					complete = true;
				}
			} else {
				Dbox.SetActive (true);
				Gman.PauseGame ();
				start = true;

				if (encounter == 3 && level == 1 || encounter == 5 && level == 1) {
					gameObject.transform.localScale = new Vector3 (1, 1, 1);
				} 
				if (encounter == 99 && level == 1) {
					if (collision.gameObject.GetComponent<PlayerController> ().hasKey) {
						encounter = 6;
						gameObject.transform.localScale = new Vector3 (1, 1, 1);
						Gman.Encounter++;
						instructions = Resources.Load<TextAsset> ("Scripts/Level" + level + "Encounter" + encounter).text.Split (new char[1]{ '\n' });
					} else {
						if (collision.gameObject.transform.position.z > transform.position.z + transform.localScale.z / 2) {
							collision.gameObject.transform.position = new Vector3 (collision.gameObject.transform.position.x, collision.gameObject.transform.position.z, collision.gameObject.transform.position.z + .3f);
						} else if (collision.gameObject.transform.position.z < transform.position.z - transform.localScale.z / 2) {
							collision.gameObject.transform.position = new Vector3 (collision.gameObject.transform.position.x, collision.gameObject.transform.position.z, collision.gameObject.transform.position.z - .3f);
						} else {
							collision.gameObject.transform.position = new Vector3 (collision.gameObject.transform.position.x - .3f, collision.gameObject.transform.position.z, collision.gameObject.transform.position.z);
						}
						encparts = 0;
					}
				}
				 else if (encounter == 1 && level == 1) {
					if (collision.gameObject.transform.position.z > transform.position.z + transform.localScale.z / 2) {
						collision.gameObject.transform.position = new Vector3 (collision.gameObject.transform.position.x, collision.gameObject.transform.position.z, collision.gameObject.transform.position.z + .3f);
					} else if (collision.gameObject.transform.position.z < transform.position.z - transform.localScale.z / 2) {
						collision.gameObject.transform.position = new Vector3 (collision.gameObject.transform.position.x, collision.gameObject.transform.position.z, collision.gameObject.transform.position.z - .3f);
					} else {
						collision.gameObject.transform.position = new Vector3 (collision.gameObject.transform.position.x - .3f, collision.gameObject.transform.position.z, collision.gameObject.transform.position.z);
					}
					encparts = 0;
				} else {
					complete = true;
				}
			}
		}
	}
}
