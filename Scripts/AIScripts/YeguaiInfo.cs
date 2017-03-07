using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YeguaiInfo : RoleInfo {
	public DeathInfo[] enemyInfo;  //助攻者：被敌人打到的集合
	public Animator ani;
	public NavMeshAgent agent;
	public AnimatorStateInfo aniInfo;
	public float reBornCD;
	public float timeCount;
	public bool isDead;
	void Start(){
		ani = GetComponent <Animator> ();
		agent = GetComponent <NavMeshAgent> ();
		agent.speed = moveSpeed;
		reBornCD = 30f;
		isDead = false;

	}
	void Update(){
		if (Hp <=0 && !isDead) {
			Hp = 0;
			isDead = true;
			ani.SetTrigger("CanDeath");
			timeCount +=Time.deltaTime;
			transform.gameObject.SetActive (false);
			Hp = HpMax;
			ani.SetTrigger ("CanReAlive");
		}
		if (timeCount >= reBornCD) {
			timeCount = 0f;
			isDead = false;
			transform.gameObject.SetActive (true);
		}
		for (int i = 0; i < enemyInfo .Length; i++) {
			enemyInfo [i].time_LastAtk += Time.deltaTime;
		}


	}
		
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role) // 技能被动说明：普攻伤害
	{
		Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);

	}
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Role_Main role) // 技能被动说明：普攻伤害
	{
		Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
		if (Hp <=0) {
			Debug.Log ("你补到一个小兵啦");
			Hp = 0;
			GetComponent <HpShow> ().HpSilider.value = 0f;
			Destroy (this.gameObject, 1f);
			role.soliderKillCount++;
			role.ReceiveExpAndGold (worthExp,worthMoney);
		}
	}

	public void OnDrawGizmo(){
		if (roleCamp==Role_Camp.Blue ) {
			Gizmos.color = new Color (0, 0, 1, 0.2f);
		}

		Gizmos .DrawSphere (transform .position ,5f);
	}
}