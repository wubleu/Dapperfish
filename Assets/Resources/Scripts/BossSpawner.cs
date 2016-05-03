using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour {

	// populated w spawn points going clockwise starting from the gate
	Vector3[] spawnPoints;
	float minInterval = .1f, maxInterval = 8f;
	float peasantRate = 70f;
	float spawnInterval;
	float clock = 0;
	bool paused = false;
	bool waiting = true;

	// Use this for initialization
	void Start () {
		spawnPoints = new Vector3[6];
		spawnPoints [0] = new Vector3 (22f, .0833f, 14.1f);
		spawnPoints [1] = new Vector3 (29.4f, .0833f, 13.9f);
		spawnPoints [2] = new Vector3 (35.4f, .0833f, 2.7f);
		spawnPoints [3] = new Vector3 (35f, .0833f, -4.5f);
		spawnPoints [4] = new Vector3 (29.2f, .0833f, -11.8f);
		spawnPoints [5] = new Vector3 (20.1f, .0833f, -11.7f);
		transform.position = new Vector3 (25, .0833f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (paused) {
			return;
		}
		if (waiting) {
			if (FindObjectOfType<NecromancerBoss> ().waiting) {
				return;
			} else {
				waiting = false;
				spawnInterval = Random.Range (minInterval, maxInterval);
			}
		}

		if ((clock += Time.deltaTime) > spawnInterval) {
			if (Random.Range (0f, 100f) < peasantRate) {
				Enemies.makePeasant (FindObjectOfType<GameManager> (), FindObjectOfType<EnemyManager> (), FindObjectOfType<PlayerController> (), 
					spawnPoints [Random.Range (0, 6)], false, true);
			} else {
				Enemies.makeArcher (FindObjectOfType<GameManager> (), FindObjectOfType<EnemyManager> (), FindObjectOfType<PlayerController> (), 
					spawnPoints [Random.Range (0, 6)], false, true);
			}
			clock = 0;
			spawnInterval = Random.Range (minInterval, maxInterval);
		}
	}

	public void PauseSpawning() {
		paused = true;
	}

	public void UnPauseSpawning() {
		paused = false;
	}

}
