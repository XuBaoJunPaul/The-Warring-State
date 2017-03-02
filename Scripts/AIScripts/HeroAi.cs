using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroAi : MonoBehaviour {

	public RoleInfo roleInfo;
	public Role_Main roleMain;
	public TargetOfAI targetOfAI;

	public float atkTimeCount;
	public float Q_skillTimeCount;
	public float W_skillTimeCount;
	public float E_skillTimeCount;


	public AIOfHeroState aiOfHeroState;

	NavMeshAgent agent;

	void Awake()
	{
		roleInfo = transform.GetComponent<RoleInfo> ();
		roleMain = transform.GetComponent<Role_Main> ();
	}

	void Start () {
		if (roleMain.aiOrPlayer == AiOrPlayerType.Player) {
			this.enabled = false;
		}
			
		aiOfHeroState = AIOfHeroState.patrol;

		if (roleInfo.roleCamp == Role_Camp.Blue) {
			targetOfAI.target01 = GameObject.Find ("Tower_Blue_01").transform;
			targetOfAI.target02 = GameObject.Find ("Shuijing_Blue").transform;
			targetOfAI.target03 = GameObject.Find ("Shuijing_Red").transform;		
		} else {
			targetOfAI.target01 = GameObject.Find ("Tower_Red_01").transform;
			targetOfAI.target02 = GameObject.Find ("Shuijing_Red").transform;
			targetOfAI.target03 = GameObject.Find ("Shuijing_Blue").transform;
		}

	}
	

	void Update () {

		Collider[] col = Physics.OverlapSphere (transform.position, 10.0f);

		//进入残血状态
		if (roleInfo.Hp <= (roleInfo.HpMax / 2)) {
			aiOfHeroState = AIOfHeroState.halfBlood;
		}
		//进入遇敌状态
		if (col.Length > 0) {
			for (int i = 0; i < col.Length; i++) {
				if (col [i].GetComponent<RoleInfo> ().roleCamp != this.roleInfo.roleCamp && aiOfHeroState != AIOfHeroState.halfBlood) {
					aiOfHeroState = AIOfHeroState.meetEnemy;					
				}
			}		
		}
		//进入巡逻状态
		if (aiOfHeroState != AIOfHeroState.halfBlood && aiOfHeroState != AIOfHeroState.meetEnemy) {
			aiOfHeroState = AIOfHeroState.patrol;
		}


		//残血状态
		if (aiOfHeroState == AIOfHeroState.halfBlood) {
			agent.SetDestination (targetOfAI.target02.position);
		}

		//巡逻状态
		if (aiOfHeroState == AIOfHeroState.patrol) {
			if (targetOfAI.target01 == null)
				agent.SetDestination (targetOfAI.target03.position);
			else{
				agent.SetDestination (targetOfAI.target01.position);
			}
		}
		//遇敌状态
		if (aiOfHeroState == AIOfHeroState.meetEnemy) {
			atkTimeCount += Time.deltaTime;
			Q_skillTimeCount += Time.deltaTime;
			W_skillTimeCount += Time.deltaTime;
			E_skillTimeCount += Time.deltaTime;
			//获取范围内敌人信息，跟随最近敌人
			float minDistance = Vector3.Distance (transform.position, col[0].transform.position);
			int chioce=0;
			for (int i = 0; i < col.Length ; i++) {
				float currentDis = Vector3.Distance (transform.position, col [i].transform.position);
				if (currentDis <minDistance ) {
					chioce = i;
				}
			}
			agent.SetDestination (col [chioce].transform.position);
			//在一定范围内攻击
			if (atkTimeCount >= (1 / roleInfo.attack_Speed)) {
				//英雄普通攻击
				atkTimeCount = 0;
			}
			if(Q_skillTimeCount >= 3f){
				//英雄技能Q
				Q_skillTimeCount = 0;
			}
			if(W_skillTimeCount >= 5f){
				//英雄技能W
				W_skillTimeCount = 0;
			}
			if(E_skillTimeCount >= 7f){
				//英雄技能E
				E_skillTimeCount = 0;
			}
		}


		
	}
}

public enum AIOfHeroState
{
	patrol,
	meetEnemy,
	halfBlood
}

public struct TargetOfAI
{
	public Transform target01;
	public Transform target02;
	public Transform target03;
}
