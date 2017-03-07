using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
	public Role_Main roleMain;
	public bool canAddSkill_Q;
	public bool canAddSkill_W;
	public bool canAddSkill_E;

	float atkInterval;
	public float[] SkillAdd_Q_Base;
	public float[] SkillAdd_Q_Phy;
	public float[] SkillAdd_Q_Mag;
	public float[] SkillAdd_W_base;
	public float[] SkillAdd_W_Phy;
	public float[] SkillAdd_W_Mag;
	public float[] SkillAdd_E_base;
	public float[] SkillAdd_E_Phy;
	public float[] SkillAdd_E_Mag;
	private UIManager uiManager;

	GameObject Add_Skill_Q;
	GameObject Add_Skill_W;
	GameObject Add_Skill_E;
	void Awake (){
	}
	// Use this for initialization
	void Start () {
		Add_Skill_Q = GameObject.Find ("UI/OperatePanel/Skill_Q/Add_Skill_Q");
		Add_Skill_W = GameObject.Find ("UI/OperatePanel/Skill_W/Add_Skill_W");
		Add_Skill_E = GameObject.Find ("UI/OperatePanel/Skill_E/Add_Skill_E");
		HideAddSkillButton ();
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();                       //1V1 or 3V3
		if (uiManager.mapSelect == MapSelect.oneVSone) {
			roleMain = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main> ();
		} else {
			roleMain = Scence3v3_Intialize.Instance.role_Players [0].GetComponent <Role_Main > ();
		}
		for (int i =1; i < 5; i++) {
			HideSkillLevelCircle (i, "Q");
			HideSkillLevelCircle (i, "W");
			HideSkillLevelCircle (i, "E");
		}

	}
	
	// Update is called once per frame
	void Update () {
		CanShowAdd ();
	}
	public void AddSkillButton(SkillButtonName buttonName){
		switch (buttonName) {
		case SkillButtonName.Button_Q:
			roleMain.skill_Q.level += 1;
			ShowSkillLevelCircle (roleMain.skill_Q.level,"Q");
			break;
		case SkillButtonName.Button_W:
			roleMain.skill_W.level += 1;
			ShowSkillLevelCircle (roleMain.skill_W.level,"W");
			break;
		case SkillButtonName.Button_E:
			roleMain.skill_E.level += 1;
			ShowSkillLevelCircle (roleMain.skill_E.level,"E");
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
	public void CanShowAdd(){
		Debug.Log ("是否升级:技能总和"+(roleMain.skill_Q.level + roleMain.skill_W.level + roleMain.skill_E.level) );
		Debug.Log ("是否升级:等级" + roleMain.Level);
		if (roleMain.skill_Q.level + roleMain.skill_W.level + roleMain.skill_E.level < roleMain.Level) {
				
			if (roleMain.skill_Q.level < roleMain.skill_Q.levelMax) {
				canAddSkill_Q = true;
			} else {
				canAddSkill_Q = false;
			}
			if (roleMain.skill_W.level < roleMain.skill_W.levelMax) {
				canAddSkill_W = true;
			} else {
				canAddSkill_W = false;
			}
			if ((roleMain.Level > 3 && roleMain.skill_E.level < 1) || (roleMain.Level > 5 && roleMain.skill_E.level < 2) || (roleMain.Level > 11 || roleMain.skill_E.level < 3)) {
				canAddSkill_E = true;
			} else {
				canAddSkill_E = false;
			}
			ShowAddSkillButton ();
			HideAddSkillButton ();
		} else {
			canAddSkill_Q = false;
			canAddSkill_W = false;
			canAddSkill_E = false;
		}
		HideAddSkillButton ();
	}
	public void ChangeValueOfSkill(SkillInfo skillInfo1){    //技能加成实现
		int i = skillInfo1.level;
		switch (skillInfo1 .skill) {
		case SkillName.SkillName_Q:
			ChangeValueOfSkill2 (skillInfo1,SkillAdd_Q_Phy[i],SkillAdd_Q_Mag[i]);
			break;
		case SkillName.SkillName_W:
			ChangeValueOfSkill2 (skillInfo1 ,SkillAdd_W_Phy[i],SkillAdd_W_Mag[i]);
			break ;
		case SkillName .SkillName_E:
			ChangeValueOfSkill2(skillInfo1,SkillAdd_E_Phy[i],SkillAdd_E_Mag [i]);
			break ;
		}
	}
	public void ChangeValueOfSkill2(SkillInfo skillInfo2, float physicalAdd,float magicAdd){
		skillInfo2.attack_Physical = (int)(roleMain.attack_Physical * physicalAdd);
		skillInfo2.attack_Magic = (int)(roleMain.attack_Magic * magicAdd);
	}

	public void ShowAddSkillButton(){
		if (canAddSkill_Q) {
			Add_Skill_Q.SetActive (true);
		} else {
			Add_Skill_Q.SetActive (false);
		}
		if (canAddSkill_W) {
			Add_Skill_W.SetActive (true);
		} else {
			Add_Skill_W.SetActive (false);
		}
		if (canAddSkill_E) {
			Add_Skill_E.SetActive (true);
		} else {
			Add_Skill_E.SetActive (false);
		}
	}

	public void HideAddSkillButton(){
		if (!canAddSkill_Q) {
			Add_Skill_Q.SetActive (false);
		}
		if (!canAddSkill_W) {
			Add_Skill_W.SetActive (false);
		}
		if (!canAddSkill_E) {
			Add_Skill_E.SetActive (false);
		}
	}

	public void ShowSkillLevelCircle(int level,string QWE){
		GameObject tmp = GameObject.Find("UI/OperatePanel/Skill_"+ QWE +"/CircleBack_"+ QWE + "/Lvl " + level);
		tmp.SetActive (true);
	}
	public void HideSkillLevelCircle(int level,string QWE){
		GameObject tmp = GameObject.Find("UI/OperatePanel/Skill_"+ QWE +"/CircleBack_"+ QWE + "/Lvl " + level);
		tmp.SetActive (false);
	}



}
