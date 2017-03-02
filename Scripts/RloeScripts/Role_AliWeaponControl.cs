using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role_AliWeaponControl : MonoBehaviour {
	public Role_ali role;
	// Use this for initialization
	void Start () {
		role = transform.GetComponentInParent <Role_ali > ();
	}
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerEnter(Collider col){
		
		Debug.Log ("阿狸Q碰撞：" +(int)(col .gameObject.layer) + "|"+role.EnemyLayerNum);
		if(col .gameObject .layer==role.EnemyLayerNum) {  //role.skill_Q .IsContinuous && 
			if (role.Skill_Q_Damage_Real) {
				Debug.Log ("打到敌人了");
				col.transform.GetComponent<RoleInfo> ().Hp -= role.skill_Q.attack_Magic;
			} else {
				Debug.Log ("打到敌人了2");
				col.transform.GetComponent <RoleInfo> ().GetHurt (0, role.skill_Q.attack_Magic);
			}
		}
	}
}
