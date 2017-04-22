using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public bool surfaced = false;
	public int level = 1;

	Player player;
	Vector3 centerPoint;
	float speed = .25f;
	float surfaceHeight = 1.34f;
	float angularSurfaceSpeed = 12f; //degrees per second

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").GetComponent<Player> ();
		centerPoint = GameObject.Find ("Planet").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//VALUES
		float angle = Mathf.Atan2(transform.localPosition.y, transform.localPosition.x);
		if (angle < 0f) {
			angle = 2f * Mathf.PI + angle;
		}
		angle *= Mathf.Rad2Deg;
		//UPRIGHT
		if (surfaced) {
			transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
		}
		//MOVE
		if (!surfaced) {
			float rotation = transform.localRotation.eulerAngles.z * Mathf.Deg2Rad;
			transform.localPosition = transform.localPosition + new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation), 0f).normalized * speed * Time.deltaTime;
			if (transform.localPosition.magnitude > surfaceHeight) {
				surfaced = true;
			}
		} else {
			float parentRotation = transform.parent.rotation.eulerAngles.z;
			float goal = (360f - (parentRotation - 90f)) % 360f;
			bool right = true;
			if (goal > angle) {
				float delta = goal - angle;
				if (delta < 180f) {
					right = false;
				}
			} else {
				float delta = angle - goal;
				if (delta >= 180f) {
					right = false;
				}
			}
			if (right) {
				angle -= angularSurfaceSpeed * Time.deltaTime;
			} else {
				angle += angularSurfaceSpeed * Time.deltaTime;
			}
			transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
			float radAngle = angle * Mathf.Deg2Rad;
			transform.localPosition = new Vector3 (Mathf.Cos (radAngle), Mathf.Sin (radAngle), 0f).normalized * surfaceHeight;
		}
	}

	public void Begin(float angle, int _level) {
		level = _level;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		transform.localScale = transform.localScale * level;
		speed /= level;
		angularSurfaceSpeed /= level;
		surfaceHeight += (level - 1) * .1f;
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		switch (level) {
		case 2:
			sr.sprite = Resources.Load<Sprite> ("Sprites/EnemyLevel2");
			break;
		case 3:
			sr.sprite = Resources.Load<Sprite>("Sprites/EnemyLevel3");
			break;
		default:
			break;
		}
	}
}
