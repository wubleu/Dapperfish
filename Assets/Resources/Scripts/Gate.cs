﻿using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
		// Use this for initialization
		void Start () {
		}

		// Update is called once per frame
		void Update () {

		}

		void OnCollisionEnter(Collision coll){
			if (coll.gameObject.name == "Necromancer") {
				if (coll.gameObject.GetComponent<PlayerController> ().hasKey) {
					Destroy (this.gameObject, 1);
				}
			}
		}
	}
