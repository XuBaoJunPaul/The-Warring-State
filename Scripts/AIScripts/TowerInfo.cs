using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : RoleInfo {
	void Start(){
		BaseStart ();
		if (roleCamp == Role_Camp.Blue) {
			FriendLayer = LayerMask.GetMask ("Blue");
			EnemyLayer = LayerMask.GetMask ("Red");
		} else {
			FriendLayer = LayerMask.GetMask ("Red");
			EnemyLayer = LayerMask.GetMask ("Blue");
		}
	}
	void Update(){
	}
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role) // 技能被动说明：普攻伤害
	{
		if (!IsDeath) {
			switch (role ) {
			case Type_Allrole.role:
				Hp = Hp - hurt_Physic * DefensePhysical / (DefenseMagic + 100) - hurt_Magic * DefenseMagic / (DefenseMagic + 100);
				break;
			case Type_Allrole.soldier:
				Hp = Hp - hurt_Physic * DefensePhysical / (DefenseMagic + 100) - hurt_Magic * DefenseMagic / (DefenseMagic + 100);
				Hp -= Hp * 0.5f;
				break;
			}
			if (Hp <=0) {
				IsDeath = true;
				colRole.enabled = false;
				Destroy (transform .Find ("Tower_base/Tower_Long").gameObject, 1f);
			}
		}
	}
	public override void GetHurt(int hurt_Physic, int hurt_Magic,Role_Main roleMain)                    
	{ 
		if (IsDeath ==false) {
			Hp = Hp - hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) - hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
			if (Hp <=0) {
				roleMain.ReceiveExpAndGold (worthExp, worthMoney);
				IsDeath = true;
				colRole.enabled = false;
				Destroy (transform .Find ("Tower_base/Tower_Long").gameObject, 1f);
			}
		}
	}
	void OnDrawGizmos(){
		Gizmos.color = new Color (0f,0f,1f,0.5f);
		Gizmos.DrawSphere (transform.position, attack_Radius);
	}
}
