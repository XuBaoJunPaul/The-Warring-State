using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAutoDefense : MonoBehaviour {
	float timeCount=0;
	public TowerInfo towerInfo;
	public float atkInterval;
	public GameObject bullet;
	public Transform firePos;
	public Transform gizmoPos;
	public Collider[] target;
	// Use this for initialization
	void Start () {
		towerInfo = GetComponent <TowerInfo> ();
		firePos = transform.Find("Fire_pos");
		bullet = Resources.Load <GameObject> ("Prefeb_Role/bullet_T");
	}
	
	// Update is called once per frame
	void Update () {
		TowerAI ();
	}
	public void TowerAI(){
		timeCount += Time.deltaTime;
		LayerMask layerEnemy=GetComponent<RoleInfo>().EnemyLayer;
		Collider[] cols = Physics.OverlapSphere (gizmoPos.position, towerInfo .attack_Radius, layerEnemy);
		target = cols;
		if (cols.Length > 0) {                         //有目标
			float minDistance = Vector3.Distance (gizmoPos.position, cols [0].transform.position);
			int chioce = 0;
			for (int i = 0; i < cols.Length; i++) {
				float currentDis = Vector3.Distance (gizmoPos.position, cols [i].transform.position);
				if (currentDis < minDistance) {
					chioce = i;
				}
			}
			if (timeCount > atkInterval) {
				GameObject bulletTower = Instantiate (bullet,firePos.position, Quaternion.identity)as GameObject;
				bulletTower.GetComponent <BulletAction> ().GetNumber ( GetComponent<RoleInfo> () );
				bulletTower.GetComponent <BulletAction> ().SetTarget (cols [chioce].transform);
				timeCount = 0;
			}
		}

	}//AI_move

}
