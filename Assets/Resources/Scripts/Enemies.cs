using UnityEngine;
using System.Collections;

public static class Enemies {

	static GameObject makeEnemy(Vector3 pos, float scale, string texture) {
		GameObject enemy = new GameObject();
		enemy.tag = "AI";
		enemy.transform.position = pos;
		enemy.transform.localScale = new Vector3(scale, scale, scale);

		NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
		agent.updateRotation = false;

		Rigidbody rigid = enemy.AddComponent<Rigidbody>();
//		rigid.isKinematic = true;
		rigid.useGravity = false;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;

		SphereCollider col = enemy.AddComponent<SphereCollider>();
		col.radius *= 1.3f;

//		col.isTrigger = true;
		//float rand = Random.Range(0, 360);
		GameObject model = new GameObject();
		model.name = "Model";
		model.transform.parent = enemy.transform;
		model.transform.localPosition = new Vector3(0, 1, 0);
		model.transform.localEulerAngles = new Vector3(90, 180, 0);
		model.transform.localScale = new Vector3(1, 1, 1);
		SpriteRenderer rend = model.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/" + texture);

		return enemy;
	}

	public static GameObject makePeasant(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos, params bool[] isElite) {
		GameObject peasant = makeEnemy(pos, .8f, "Circle");
		peasant.name = "Peasant";
		if (isElite.Length == 0) {
			peasant.AddComponent<Peasant> ().initPeasant (gMan, owner, necro);
		} else if (isElite.Length == 1) {
			peasant.AddComponent<Peasant> ().initPeasant (gMan, owner, necro, isElite [0]);
		} else if (isElite.Length == 2) {
			peasant.AddComponent<Peasant> ().initPeasant (gMan, owner, necro, isElite[0], isElite [1]);
		}

		return peasant;
	}

	public static GameObject makeArcher(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos, params bool[] isElite) {
		GameObject archer = makeEnemy(pos, .6f, "Circle");
		archer.name = "Archer";
		if (isElite.Length == 0) {
			archer.AddComponent<Archer> ().initArcher (gMan, owner, necro);
		} else if (isElite.Length == 1) {
			archer.AddComponent<Archer> ().initArcher (gMan, owner, necro, isElite [0]);
		} else if (isElite.Length == 2) {
			archer.AddComponent<Archer> ().initArcher (gMan, owner, necro, isElite[0], isElite [1]);
		}

		return archer;
	}

	public static GameObject makeKnight(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos, params bool[] isElite) {
		GameObject knight = makeEnemy(pos, 1f, "Circle");
		SphereCollider col = knight.GetComponent<SphereCollider>();
		knight.name = "Knight";
		if (isElite.Length == 0) {
			knight.AddComponent<Knight> ().initKnight (gMan, owner, necro);
		} else if (isElite.Length == 1) {
			knight.AddComponent<Knight> ().initKnight (gMan, owner, necro, isElite [0]);
		} else if (isElite.Length == 2) {
			knight.AddComponent<Knight> ().initKnight (gMan, owner, necro, isElite[0], isElite [1]);
		}

		return knight;
	}

	public static GameObject makeNecroBoss(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos, params bool[] isElite) {
		GameObject necroBoss = makeEnemy (pos, 1.5f, "Circle");
		necroBoss.transform.localEulerAngles = new Vector3 (0, -90, 0);
		necroBoss.name = "Necromancer Boss";
		necroBoss.AddComponent<NecromancerBoss> ().initNecroBoss(gMan, owner, necro);

		return necroBoss;
	}
}
