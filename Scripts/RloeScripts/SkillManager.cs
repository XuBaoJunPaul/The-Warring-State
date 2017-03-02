using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
	public Role_Main roleMain;
	public int maxSkillCount=3;
	public bool canAddSkill_Q;
	public bool canAddSkill_W;
	public bool canAddSkill_E;
	public bool CanAtkNormal=false;
	float timeCouAtkNormal=0f;
	float atkInterval;
	private UIManager uiManager;
	void Awake (){
	}
	// Use this for initialization
	void Start () {
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();                       //1V1 or 3V3
		if (uiManager.mapSelect == MapSelect.oneVSone) {
			roleMain = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main> ();
		} else {
			roleMain = Scence3v3_Intialize.Instance.role_Players [0].GetComponent <Role_Main > ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		atkInterval = 1 / roleMain.attack_Speed;    //万一改变速度类呐；
		timeCouAtkNormal += Time.deltaTime;
		if (timeCouAtkNormal>=atkInterval) {
			CanAtkNormal = true;
			timeCouAtkNormal = 0f;
		}
	}
	public void AddSkillButton(SkillButtonName buttonName){
		switch (buttonName) {
		case SkillButtonName.Button_Q:
			roleMain.skill_Q.level += 1;
			break;
		case SkillButtonName.Button_W:
			roleMain.skill_W.level += 1;
			break;
		case SkillButtonName.Button_E:
			roleMain.skill_E.level += 1;
			break;
		}
	}

	public void AddSkill_Q(){
		roleMain.skill_Q.level += 1;

	}
	public void AddSkill_W(){
		roleMain.skill_W.level += 1;
	}
	public void AddSkill_E(){
		roleMain.skill_E.level += 1;
	}
	public void IsCanShowAdd(){
		if (roleMain .skill_Q .level +roleMain .skill_W .level +roleMain .skill_E .level < roleMain .Level ) {
			if (roleMain .skill_Q .level< maxSkillCount ) {
				canAddSkill_Q = true ;
			}
			if (roleMain .skill_W .level<maxSkillCount ) {
				canAddSkill_W = true ;
			}
			if ((roleMain .Level>3&&roleMain .skill_E .level<1) || (roleMain .Level>5 &&roleMain .skill_E .level<2) || (roleMain .Level>11 && roleMain .skill_E .level<3 ) ) {
				canAddSkill_E = true ;
			}
		}
	}
}
