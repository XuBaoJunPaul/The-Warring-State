using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .AI ;

[ExecuteInEditMode]
public class SoliderInfo : RoleInfo {
	public float expRadius;
	public Animator ani;
	public NavMeshAgent agent;
	public AnimatorStateInfo aniInfo;
	public Role_Main RoleAtk;   //被攻击的英雄；
	void Start(){
		BaseStart ();
		ani = GetComponent <Animator> ();
		agent = GetComponent <NavMeshAgent> ();
		agent.speed = moveSpeed;
		expRadius = attack_Radius * 2.5f;
	}
	void Update(){
		if (Hp <=0) {
			Hp = 0;
			GetComponent <HpShow> ().HpSilider.value = 0f;
			Collider[] cols = Physics.OverlapSphere (transform.position, expRadius, EnemyLayer);
			if (cols .Length >0) {
				for (int i = 0; i < cols.Length; i++) {
					if (cols [i].gameObject .layer ==EnemyLayerNum && cols[i].GetComponent<Role_Main>()!=null) {
						cols [i].GetComponent <Role_Main> ().Levl_exp += worthExp;
					}
				}
			}

			Destroy (this.gameObject, 1f);

		}
	}
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role) // 技能被动说明：普攻伤害
	{
		Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);

	}
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Role_Main role) // 技能被动说明：普攻伤害
	{
		this.RoleAtk = role; 
		Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
		if (Hp <=0) {
			Debug.Log ("你补到一个小兵啦");
			Hp = 0;
			GetComponent <HpShow> ().HpSilider.value = 0f;
			Destroy (this.gameObject, 1f);
			role.soliderKillCount++;
			role.money += worthMoney;
			role.Levl_exp += worthExp;
		}
	}


	public void OnDrawGizmo(){
		if (roleCamp==Role_Camp.Blue ) {
			Gizmos.color = new Color (0, 0, 1, 0.2f);
		}

		Gizmos .DrawSphere (transform .position ,5f);
	}
}
