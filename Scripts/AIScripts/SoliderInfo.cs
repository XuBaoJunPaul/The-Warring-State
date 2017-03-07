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
	}
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role) // 技能被动说明：普攻伤害
	{
		if (!IsDeath ) {
			Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
			if (Hp <=0) {
				Hp = 0;
				IsDeath = true;
			}
		}

	}
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Role_Main role) // 技能被动说明：普攻伤害
	{
		if (!IsDeath ) {
			this.RoleAtk = role; 
			Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
			if (Hp <=0) {
				Debug.Log ("你补到一个小兵啦");
				Hp = 0;
				GetComponent <HpShow> ().HpSilider.value = 0f;
				role.soliderKillCount++;
				role.ReceiveExpAndGold(worthExp,worthMoney) ;
				
				Collider[] cols = Physics.OverlapSphere (transform.position, expRadius, EnemyLayer);  //给周围的英雄加金币
				if (cols .Length >0) { 
					for (int i = 0; i < cols.Length; i++) {
						if (cols [i].gameObject .layer ==EnemyLayerNum && cols[i].GetComponent<Role_Main>()!=null) {
							cols [i].GetComponent <Role_Main> ().ReceiveExpAndGold(worthExp,0);
						}
					}
				}
				transform.GetComponent <CharacterController> ().enabled = false;
				Destroy (this.gameObject, 1f);
			}
		}
	}


	public void OnDrawGizmo(){
		if (roleCamp==Role_Camp.Blue ) {
			Gizmos.color = new Color (0, 0, 1, 0.2f);
		}

		Gizmos .DrawSphere (transform .position ,5f);
	}
}
