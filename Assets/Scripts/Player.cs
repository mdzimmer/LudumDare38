using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	Bullet curBullet;
	GameObject bulletPrefab;
	GameObject planet;
	Vector3 startPos;
	float jumpHeight = 0f;
	float jumpVelocity = 4f;
	float jumpHoldTime = 0f;
	float minJumpHoldTime = .1f;
	float maxJumpHoldTime = .175f;
	float gravityVelocity = 4f;
	float bulletOffset = .5f;
	float rotateVelocity = 0f;
	float rotateForce = 1f;
	float maxRotateVelocity = 1f;
	float rotateDrag = 1f;
	bool jumping = false;

	// Use this for initialization
	void Start () {
		bulletPrefab = Resources.Load ("Prefabs/Bullet") as GameObject;
		planet = GameObject.Find ("Planet");
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//VALUES
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//JUMPING
		if ((Input.GetKey (KeyCode.W) && jumpHoldTime < maxJumpHoldTime) ||
			jumping && jumpHoldTime < minJumpHoldTime) {
			jumping = true;
			jumpHeight += jumpVelocity * Time.deltaTime;
			jumpHoldTime += Time.deltaTime;
		} else {
			jumping = false;
		}
		if (!jumping) {
			jumpHeight -= gravityVelocity * Time.deltaTime;
			jumpHeight = Mathf.Max (jumpHeight, 0f);
			if (jumpHeight == 0f) {
				jumpHoldTime = 0f;
			}
		}
		transform.position = startPos + new Vector3 (0, jumpHeight, 0);
		//ROTATE
		bool right = Input.GetKey(KeyCode.D);
		bool left = Input.GetKey (KeyCode.A);
		if (right && left) {
			rotateVelocity *= (1f - Mathf.Max(rotateDrag * Time.deltaTime, 0f));
		} else if (right) {
			rotateVelocity += rotateForce * Time.deltaTime;
			rotateVelocity = Mathf.Min (rotateVelocity, maxRotateVelocity);
		} else if (left) {
			rotateVelocity -= rotateForce * Time.deltaTime;
			rotateVelocity = Mathf.Max (rotateVelocity, -maxRotateVelocity);
		} else {
			rotateVelocity *= (1f - Mathf.Max(rotateDrag * Time.deltaTime, 0f));
		}
		planet.transform.Rotate(new Vector3(0f, 0f, rotateVelocity));
		//SHOOTING
		if (Input.GetMouseButton (0)) {
			if (!curBullet) {
				GameObject bulletGO = Instantiate (bulletPrefab) as GameObject;
				curBullet = bulletGO.GetComponent<Bullet>();
			} 
			curBullet.Charge ();
			curBullet.transform.position = transform.position + new Vector3 (mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0f).normalized * bulletOffset;
		} else if (curBullet) {
			curBullet.Shoot (curBullet.transform.position - transform.position);
			curBullet = null;
		}
	}
}
