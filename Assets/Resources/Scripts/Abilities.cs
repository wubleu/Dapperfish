using UnityEngine;
using System.Collections;

public static class Abilities {

	private static GameObject AOE(float radius, Color color, Vector3 pos) {
		GameObject spell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(radius, radius, radius);

		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/Sphere");
		spell.GetComponent<MeshRenderer> ().material.color = color;
		spell.AddComponent<SpellEffect>();

		return spell;
	}

	public static void Blight(Vector3 pos) {
		GameObject spell = AOE(3, new Color(1, 0, 0, .4f), pos);
		spell.name = "Blight";
	}

	public static void Root(Vector3 pos) {
		GameObject spell = AOE(3, new Color(0, 0, 1, .4f), pos);
		spell.name = "Root";
	}

	public static void Damage(Vector3 pos, float angle) {
		GameObject spell = AOE(.8f, new Color(0, 1, 0, .4f), pos);
		spell.GetComponent<SpellEffect>().angle = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
		spell.name = "Damage";
	}

	public static void Blink(float angle, Transform me) {
		me.Translate(4 * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)));
	}

	public static Vector3 NormalizeVector(Vector3 vector) {
		Vector3 direction = new Vector3 (vector.x, vector.y);
		float directionMagnitude = Mathf.Sqrt (Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
		direction = new Vector3 (direction.x / directionMagnitude, direction.y / directionMagnitude);
		return direction;
	}
}
