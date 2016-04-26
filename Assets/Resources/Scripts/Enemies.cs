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
		col.radius *= 1.5f;

//		col.isTrigger = true;

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

	public static GameObject makePeasant(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos) {
		GameObject peasant = makeEnemy(pos, .8f, "Circle");
		peasant.name = "Peasant";
		peasant.AddComponent<Peasant>().initPeasant(gMan, owner, necro);

		return peasant;
	}

	public static GameObject makeArcher(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos) {
		GameObject archer = makeEnemy(pos, .6f, "Circle");
		archer.name = "Archer";
		archer.AddComponent<Archer>().initArcher(gMan, owner, necro);

		return archer;
	}

	public static GameObject makeKnight(GameManager gMan, EnemyManager owner, PlayerController necro, Vector3 pos) {
		GameObject knight = makeEnemy(pos, 1f, "Circle");
		SphereCollider col = knight.GetComponent<SphereCollider>();
		knight.name = "Knight";
		knight.AddComponent<Knight>().initKnight(gMan, owner, necro);

		return knight;
	}
}
