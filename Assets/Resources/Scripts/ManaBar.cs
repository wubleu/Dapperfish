using UnityEngine;
using System.Collections;

public class ManaBar : MonoBehaviour {

	// PARAMETERS
	public float maxMana, speed;

	PlayerController necromancer;
	SpriteRenderer bar;

	public void init (float mana, float recharge) {
		necromancer = GameObject.Find("Necromancer").GetComponent<PlayerController>();

		gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/BarOutline");
		name = "Mana";
		transform.parent = necromancer.transform;
		transform.localPosition = new Vector3(8.5f, 10, -6);
		transform.localEulerAngles = new Vector3(90, 0, 0);

		bar = new GameObject().AddComponent<SpriteRenderer>();
		bar.name = "ManaBar";
		bar.transform.parent = transform;
		bar.transform.localPosition = Vector3.zero;
		bar.transform.localEulerAngles = new Vector3(0, 0, 0);
		bar.sprite = Resources.Load<Sprite>("Textures/Bar");
		bar.color = Color.blue; //new Color(0, 0, 155/256, 0.5f);

		maxMana = mana;
		speed = recharge;
	}
//

	// Update is called once per frame
	void Update () {
		if (necromancer.mana < maxMana) {
			necromancer.mana += speed * Time.deltaTime;
			if (necromancer.mana > maxMana) {
				necromancer.mana = maxMana;
			}
		}
		bar.transform.localScale = new Vector3(1, necromancer.mana / maxMana, 1);
		bar.transform.localPosition = new Vector3(0, (necromancer.mana / maxMana - 1) / 2);
	}
}
