using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_Item {
	None,
	草鞋,
	力量手套,
	短剑,
	残镰,
	学徒宝典,
	圣杯,
	原力法杖,
	魔法宝典,
	水晶杖,
	暗黑杖,
	幻影之刃,
	鹰角弓,
	疾风刃,
	血刃,
	契约与胜利之剑们,
	军团圣盾,
	血龙甲,
	能量之甲,
	血手,
	gay甲,
}
[System .Serializable]
public struct Slot_Item{
	public Data_Item item1;
	public Data_Item item2;
	public Data_Item item3;
	public Data_Item item4;
	public Data_Item item5;
	public Data_Item item6;
}

[System .Serializable]
public struct Data_Item{
	public Enum_Item Name;
	public int Gold;
	public int HP;
	public int MP;
	public int PDam;
	public int MDam;
	public int PDef;
	public int MDef;
	public float MS;
	public float AS;
	public Sprite Tex;
}


