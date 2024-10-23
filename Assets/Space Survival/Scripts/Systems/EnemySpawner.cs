using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;      // 적 프리팹
	public float spawnInterval;         // 생성 간격

	private void Start()
	{
		StartCoroutine(SpawnCoroutine());
	}

	private IEnumerator SpawnCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnInterval);
			Spawn();		// 몬스터 스폰
		}
	}

	private void Spawn()
	{
		Instantiate(enemyPrefab);
	}
}
