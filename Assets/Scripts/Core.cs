using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour {
	public bool debug = false; //DEBUG VALUE
	public bool spawn = true;

	GameObject enemyPrefab;
	Vector3 startScale;
	float curScale = 1f;
	float scaleReductionPerSpawn = .0025f;
	float spawnMinInterval = 1f;
	float spawnMaxInterval = 2f;
	float nextSpawnInterval = 0f;
	float timeSinceLastSpawn = 0f;

	// Use this for initialization
	void Start () {
		enemyPrefab = Resources.Load ("Prefabs/Enemy") as GameObject;
		startScale = transform.localScale;
		ResetSpawnInterval ();
	}
	
	// Update is called once per frame
	void Update () {
		if (debug && spawn) {
			SpawnEnemy (1);
			spawn = false;
		} else if (spawn) {
			timeSinceLastSpawn += Time.deltaTime;
			if (timeSinceLastSpawn >= nextSpawnInterval) {
				timeSinceLastSpawn = 0f;
				ResetSpawnInterval ();
				DoSpawn (3);
			}
		}
	}

	public void DoSpawn(int level) {
		if (!spawn) {
			return;
		}
		switch (level) {
		case 1:
			SpawnEnemy (1);
			break;
		case 2:
			if (Random.Range (0f, 1f) > .5f) {
				SpawnEnemy (1);
				SpawnEnemy (1);
			} else {
				SpawnEnemy (2);
			}
			break;
		case 3:
			float rand = Random.Range (0f, 1f);
			if (rand > .66f) {
				SpawnEnemy (1);
				SpawnEnemy (1);
				SpawnEnemy (1);
			} else if (rand > .33f) {
				SpawnEnemy (1);
				SpawnEnemy (2);
			} else {
				SpawnEnemy (3);
			}
			break;
		default:
			break;
		}
	}

	void SpawnEnemy(int level) {
		if (!spawn) {
			return;
		}
		curScale -= scaleReductionPerSpawn * level;
		if (curScale <= 0f) {
			//TODO: victory
		}
		transform.localScale = startScale * curScale;
		GameObject enemyGO = Instantiate (enemyPrefab) as GameObject;
		enemyGO.transform.parent = transform.parent;
		Enemy enemy = enemyGO.GetComponent<Enemy> ();
		enemy.Begin (Random.Range (0f, 360f), level);
	}

	void ResetSpawnInterval() {
		nextSpawnInterval = Random.Range (spawnMinInterval, spawnMaxInterval);
	}
}
