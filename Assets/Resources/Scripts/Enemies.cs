using UnityEngine;
using System.Collections;

public static class Enemies {

	static GameObject makeEnemy(float x, float z, float scale, string texture) {
		GameObject enemy = new GameObject();
		enemy.tag = "AI";
		enemy.transform.position = new Vector3(x, 0, z);
		enemy.transform.localScale = new Vector3(scale, scale, scale);

		NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
		agent.updateRotation = false;

		Rigidbody rigid = enemy.AddComponent<Rigidbody>();
//		rigid.isKinematic = true;
		rigid.useGravity = false;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;

//		SphereCollider col = 
			enemy.AddComponent<SphereCollider>();
//		col.isTrigger = true;

		GameObject model = new GameObject();
		model.name = "Model";
		model.transform.parent = enemy.transform;
		model.transform.localPosition = new Vector3(0, 1, 0);
		model.transform.localEulerAngles = new Vector3(90, 0, 0);
		model.transform.localScale = new Vector3(1, 1, 1);
		SpriteRenderer rend = model.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/" + texture);

		return enemy;
	}

	public static GameObject makePeasant(float x = 0, float z = 0) {
		GameObject peasant = makeEnemy(x, z, 0.17f, "Circle");
		peasant.name = "Peasant";
		peasant.AddComponent<Peasant>().initPeasant();

		return peasant;
	}
}
