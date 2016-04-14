using UnityEngine;
using System.Collections;

public class Knight : AIBehavior {


	// Use this for initialization
	public void init (GameManager gMan, EnemyManager owner) {

		// PARAMETERS
		allyColor = new Color (0, 0, 0);
		enemyColor = new Color (1, 1, 1);
		speed = 1.5f;
		maxHP = 3f;
		infectionCost = 15;
		switchDirThreshold = .5f;
		meleeThreshold = 1f;
		meleeDamage = 1f;

		isEnemy = true;
		base.init (gMan, owner, "Circle", 0, 0, .3f, .3f);
		name = "Peasant";
		gameObject.AddComponent<SphereCollider> ().radius = .17f;
		gameObject.GetComponent<NavMeshAgent> ().radius = .17f;
	}


	// Update is called once per frame
	new void Update() {
		base.Update();
		base.MoveToward();
	}


	void OnCollisionStay(Collision coll) {
		base.OnCollision (coll);
	}


	void OnTriggerEnter(Collider coll) {
		//		base.TakeHit (coll.gameObject);
	}
}