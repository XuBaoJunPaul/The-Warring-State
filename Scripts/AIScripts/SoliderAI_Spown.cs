using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderAI_Spown : MonoBehaviour {
	public EnemyPerOnceInfo enemyPerOneInfo;
	public float spawnOneceInterval=5f;  //每波兵生成间隔;
	public int currentEnemy =0; //当前第几波小兵
	public int currentIndex=0;  //当前一波小兵的第几个；
	public Transform spawnPos;

	void Awake(){
	}
	void Start(){
		InvokeRepeating ("Spown", 2f, spawnOneceInterval); //2s后生成小兵
	}
	void Spown(){currentIndex = 0; StartCoroutine ("SpawnOne");currentEnemy++;}
	IEnumerator SpawnOne(){
		GameObject currentPrefab = enemyPerOneInfo.prefab; //当前小兵的预设；
		while (currentIndex<enemyPerOneInfo.count) {
	//		Debug.Log ("生成第"+currentEnemy+"波兵;"+"生成第"+currentIndex+"个小兵;"+"---共有小兵个数："+enemyPerOneInfo.count);
			Instantiate (currentPrefab, spawnPos.position, Quaternion.identity);
			currentIndex++;
			yield return new WaitForSeconds (enemyPerOneInfo.spawnInterval);
		}
		yield break;
	}



}
[System .Serializable ]
public class EnemyPerOnceInfo{ //每波兵的信息
	public GameObject prefab;  //每波兵的预设体
	public int count = 10;    //每波兵出生的个数
	public float spawnInterval; //每个小兵出生的间隔
}