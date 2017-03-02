using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .AI ;
public class Role_mumu :Role_Main {
	void Start(){
		StartCoroutine(base.Start());
		StartCoroutine (base.Start ());
		type_Range = Type_Range.Near;
	}

	public override void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role) // 技能被动说明：普攻伤害
	{
		if (IsDeath ==false) {
			Debug.Log ("受伤类");
			switch (role ) {
			case Type_Allrole.role:
				Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
				break;
			case Type_Allrole.soldier:
				int hurtTemp = hurt_Physic - hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
				Hp -= hurtTemp * 0.5f;
				break;
			}
		}
	}


	public override void Akt_normal ()
	{
		if (target !=null ) {
			if (Vector3 .Distance (transform .position ,target .position )>=0.5f) {
				SetMoveTarget(target.position );
			}
			ani.SetTrigger ("Akt");
			target.GetComponent <Role_Main> ().GetHurt (attack_Physical, 0,Type_Allrole.role);
		}
	}
	public override void SetSkill_Q ()
	{
		ani.SetTrigger ("CanSkill_Q");
	}
	public override void  SetSkill_W ()
	{
		ani.SetTrigger ("CanSkill_W");

	}
	public override void  SetSkill_E ()
	{
		ani.SetTrigger ("CanSkill_E");
	}
	public override void SetSkill_D ()
	{
	}
	public override void SetSkill_F ()
	{
	}
	public override void DoPasssive ()
	{
	}

}
