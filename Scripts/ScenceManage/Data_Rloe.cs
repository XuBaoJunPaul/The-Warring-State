using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System .Serializable]
public class Data_Rloe { 
	public Type_Allrole RorS;       //英雄 or solider or buffer怪 or 塔
	public int HpMax;               //当前一级时的生命值；
	public int MpMax;				//当前一级时的魔法值；
	public int skinNum;             //选择的皮肤
	public int DefensePhysical;		//当前一级时的物理防御
	public int DefenseMagic; 		//当前一级时的魔法防御
	public int attack_Physical;     //当前一级时的物理攻击
	public int attack_Magic;		//当前一级时的魔法攻击
	public int attack_Speed;		//当前一级时的攻击速度
	public float moveSpeed;			//当前一级时的移动速度
	public float attack_Radius;		//当前一级时的攻击半径
	public int Levl_exp;			//当前一级时的经验值
	public int Level;				//当前一级时的等级
}

[System .Serializable]
public struct Record_Role{
	public string name;
	public Enum_Role role;
	public int assistsCount	;   //助攻次数；
	public int killCount	;   //击杀次数
	public int deathCount	;	//死亡次数；
	public int soliderKillCount;//补兵的个数
}