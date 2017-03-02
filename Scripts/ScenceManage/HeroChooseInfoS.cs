using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HeroChooseInfoS{
	public HeroChooseInfo[] hero;
}
[System.Serializable]
public struct HeroChooseInfo{
	public int num;
	public Enum_Role hero;
	public OtherSkill_Name Skill_D;
	public OtherSkill_Name Skill_F;
}
