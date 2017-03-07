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

	public Animator ani;

	public UIManager uimanager;


	public AIOfHeroState aiOfHeroState;

	private int roadLine;

	NavMeshAgent agent;

	void Awake(){
	}

	void Start () {
		roleInfo = transform.GetComponent<RoleInfo> ();
		roleMain = transform.GetComponent<Role_Main> ();
		ani = transform.GetComponent<Animator> ();
		agent = GetComponent <NavMeshAgent> ();
		uimanager = GameObject.Find ("UI").GetComponent<UIManager> ();
		if (roleMain.aiOrPlayer == AiOrPlayerType.Player) {
			this.enabled = false;
		}
		roadLine = Random.Range (0, 3);
			
		aiOfHeroState = AIOfHeroState.patrol;

		if (uimanager.mapSelect == MapSelect.oneVSone) {
			if (roleInfo.roleCamp == Role_Camp.Blue) {
				targetOfAI.target01 = GameObject.Find ("Tower_Red_01").transform;
				targetOfAI.target02 = GameObject.Find ("shuiJingBlueRoot").transform;
				targetOfAI.target03 = GameObject.Find ("shuiJingRedRoot").transform;		
			} else {
				targetOfAI.target01 = GameObject.Find ("Tower_Blue_01").transform;
				targetOfAI.target02 = GameObject.Find ("shuiJingRedRoot").transform;
				targetOfAI.target03 = GameObject.Find ("shuiJingBlueRoot").transform;
			}
		} else if (uimanager.mapSelect == MapSelect.threeVSthree) {
			if (roleInfo.roleCamp == Role_Camp.Blue) {
				if (roadLine == 0) {
					targetOfAI.target01 = GameObject.Find ("Environment/Building/A_Towers/A_Tower_X02").transform;
				} else if (roadLine == 1) {
					targetOfAI.target01 = GameObject.Find ("Environment/Building/A_Towers/A_Tower_Z02").transform;
				}else if (roadLine == 2) {
					targetOfAI.target01 = GameObject.Find ("Environment/Building/A_Towers/A_Tower_S02").transform;
				}
					targetOfAI.target02 = GameObject.Find ("shuiJingBlueRoot").transform;
					targetOfAI.target03 = GameObject.Find ("shuiJingRedRoot").transform;						
				
			} else {
				if (roadLine == 0) {
					targetOfAI.target01 = GameObject.Find ("Environment/Building/B_Towers/B_Tower_X02").transform;
				} else if (roadLine == 1) {
					targetOfAI.target01 = GameObject.Find ("Environment/Building/B_Towers/B_Tower_Z02").transform;
				}else if (roadLine == 2) {
					targetOfAI.target01 = GameObject.Find ("Environment/Building/B_Towers/B_Tower_S02").transform;
				}
				targetOfAI.target02 = GameObject.Find ("shuiJingRedRoot").transform;
				targetOfAI.target03 = GameObject.Find ("shuiJingBlueRoot").transform;
			}
		}
	}
	

	void Update () {

		Collider[] col = Physics.OverlapSphere (transform.position, 3.0f,roleInfo.EnemyLayer);

		//进入残血状态
		if (roleInfo.Hp <= (roleInfo.HpMax / 2)) {
			aiOfHeroState = AIOfHeroState.halfBlood;
		}
		//进入遇敌状态
		if (col.Length > 0 && aiOfHeroState != AIOfHeroState.halfBlood) {
					aiOfHeroState = AIOfHeroState.meetEnemy;													
		}
		//进入巡逻状态
		if (aiOfHeroState != AIOfHeroState.halfBlood && col.Length == 0) {
			aiOfHeroState = AIOfHeroState.patrol;
		}


		//残血状态
		if (aiOfHeroState == AIOfHeroState.halfBlood) {
			agent.SetDestination (targetOfAI.target02.position);
			ani.SetTrigger ("CanRun");
			if (Vector3.Distance (transform.position, targetOfAI.target02.position) <= 0.5f) {
				ani.SetTrigger ("CanStop");
			}
			if (roleInfo.Hp == roleInfo.HpMax) {
				aiOfHeroState = AIOfHeroState.patrol;
			}
		}

		//巡逻状态
		if (aiOfHeroState == AIOfHeroState.patrol) {
			Transform target;
			if (targetOfAI.target01 == null) {
				target = targetOfAI.target03;
				agent.SetDestination (target.position);
				ani.SetTrigger ("CanRun");
			}
			else{
				target = targetOfAI.target01;
				agent.SetDestination (target.position);
				ani.SetTrigger ("CanRun");
			}
			if (Vector3.Distance (transform.position, target.position) <= 0.5f) {
				ani.SetTrigger ("CanStop");
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
			if (col [chioce].transform != null) {
				agent.SetDestination (col [chioce].transform.position);
			}
			//在一定范围内攻击
			if (Vector3.Distance (transform.position, col [chioce].transform.position) <= 0.5f) {			
				if (atkTimeCount >= (1 / roleInfo.attack_Speed)) {
					//英雄普通攻击
					ani.SetTrigger("CanSkill_Akt");
					atkTimeCount = 0;
				}
				if(Q_skillTimeCount >= 3f){
					//英雄技能Q
					ani.SetTrigger("CanSkill_Q");
					Q_skillTimeCount = 0;
				}
				if(W_skillTimeCount >= 5f){
					//英雄技能W
					ani.SetTrigger("CanSkill_W");
					W_skillTimeCount = 0;
				}
				if(E_skillTimeCount >= 7f){
					//英雄技能E
					ani.SetTrigger("CanSkill_E");
					E_skillTimeCount = 0;
				}
			}
			if (col.Length == 0) {
				aiOfHeroState = AIOfHeroState.patrol;
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
[System .Serializable]
public struct TargetOfAI
{
	public Transform target01;
	public Transform target02;
	public Transform target03;
}
