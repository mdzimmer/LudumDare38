using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	Vector3 startScale = Vector3.zero;
	Vector3 velocity;
	Vector3 centerPoint;
	int charge = 1;
	int maxCharge = 3;
	float timeCharged = 0f;
	float timeChargeIncrement = 2f;
	float startImpulseChargeScale = .25f;
	float startImpulseBaseScale = .6f;
	float gravityScale = 2f;
	float gravityChargeScale = .02f;
//	float destroyDistance = 1.5f;
	float drag = 1f; //lose (x/100)% every second
//	float scaleBaseIncrease = 8f;
//	float scaleExponentialIncrease = .8f;
//	float scaleResultScalar = .05f;
	float scaleIncreaseRate = 1f;
	float chargeRate = 6f;
	bool move = false;

	// Use this for initialization
	void Start () {
		centerPoint = GameObject.Find ("Planet").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (move) {
			velocity *= (1f - Mathf.Max(drag * Time.deltaTime, 0f));
			velocity -= (transform.position - centerPoint).normalized * (1f + charge * gravityChargeScale) * gravityScale * Time.deltaTime;
			transform.Translate (velocity);
//			if (Vector3.Distance(transform.position, centerPoint) <= destroyDistance) {
//				Destroy(this.gameObject);
//			}
		}
	}

	public void Charge() {
		if (startScale == Vector3.zero) {
			startScale = transform.localScale;
		}
		timeCharged += Time.deltaTime * chargeRate;
		charge = 1 + (int)Mathf.Floor(timeCharged / timeChargeIncrement);
		charge = Mathf.Min (charge, maxCharge);
//		float newScale = startScale.x + Mathf.Pow (scaleBaseIncrease, (charge - 1f) * scaleExponentialIncrease) * scaleResultScalar;
		UpdateScale();
	}

	public void Shoot(Vector3 dir) {
		velocity = dir * (1f + charge * startImpulseChargeScale) * startImpulseBaseScale;
		move = true;
	}

	void UpdateScale() {
		float newScale = startScale.x + startScale.x * (charge - 1f) * scaleIncreaseRate;
		transform.localScale = new Vector3(newScale, newScale, newScale);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Enemy") {
			int enemyLevel = col.gameObject.GetComponent<Enemy>().level;
			if (charge > enemyLevel) {
				Destroy (col.gameObject);
				charge -= enemyLevel;
				UpdateScale();
			} else if (charge == enemyLevel) {
				Destroy (col.gameObject);
				Destroy (gameObject);
			} else {
				Destroy (gameObject);
			}
		} else if (col.tag == "Core") {
			col.gameObject.GetComponent<Core> ().DoSpawn (charge);
			Destroy (gameObject);
		}
	}
}
