using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {
	public static Data_Item[] items;

	void Awake(){
		items = new Data_Item[20];

		items[0].Name = Enum_Item.草鞋;
		items[0].Gold = 200;
		items[0].HP = 10;
		items[0].MP = 0;
		items[0].PDam = 0;
		items[0].MDam = 0;
		items[0].PDef = 0;
		items[0].MDef = 0;
		items[0].MS = 1.3f;
		items[0].AS = 1;
		items[0].Tex = Resources.Load<Sprite>("UI/Item_Texture/1");

		items[1].Name = Enum_Item.力量手套;
		items[1].Gold = 200;
		items[1].HP = 10;
		items[1].MP = 0;
		items[1].PDam = 10;
		items[1].MDam = 0;
		items[1].PDef = 0;
		items[1].MDef = 0;
		items[1].MS = 1;
		items[1].AS = 1;
		items[1].Tex = Resources.Load<Sprite>("UI/Item_Texture/2");

		items[2].Name = Enum_Item.残镰;
		items[2].Gold = 200;
		items[2].HP = 0;
		items[2].MP = 0;
		items[2].PDam = 10;
		items[2].MDam = 0;
		items[2].PDef = 0;
		items[2].MDef = 0;
		items[2].MS = 1;
		items[2].AS = 1.1f;
		items[2].Tex = Resources.Load<Sprite>("UI/Item_Texture/3");

		items[3].Name = Enum_Item.短剑;
		items[3].Gold = 200;
		items[3].HP = 0;
		items[3].MP = 0;
		items[3].PDam = 20;
		items[3].MDam = 0;
		items[3].PDef = 0;
		items[3].MDef = 0;
		items[3].MS = 1;
		items[3].AS = 1;
		items[3].Tex = Resources.Load<Sprite>("UI/Item_Texture/4");

		items[4].Name = Enum_Item.学徒宝典;
		items[4].Gold = 200;
		items[4].HP = 0;
		items[4].MP = 0;
		items[4].PDam = 0;
		items[4].MDam = 40;
		items[4].PDef = 0;
		items[4].MDef = 0;
		items[4].MS = 1;
		items[4].AS = 1;
		items[4].Tex = Resources.Load<Sprite>("UI/Item_Texture/5");

		items[5].Name = Enum_Item.圣杯;
		items[5].Gold = 1000;
		items[5].HP = 0;
		items[5].MP = 200;
		items[5].PDam = 0;
		items[5].MDam = 60;
		items[5].PDef = 0;
		items[5].MDef = 20;
		items[5].MS = 1;
		items[5].AS = 1;
		items[5].Tex = Resources.Load<Sprite>("UI/Item_Texture/6");

		items[6].Name = Enum_Item.原力法杖;
		items[6].Gold = 1000;
		items[6].HP = 0;
		items[6].MP = 100;
		items[6].PDam = 0;
		items[6].MDam = 100;
		items[6].PDef = 0;
		items[6].MDef = 0;
		items[6].MS = 1;
		items[6].AS = 1;
		items[6].Tex = Resources.Load<Sprite>("UI/Item_Texture/7");

		items[7].Name = Enum_Item.暗黑杖;
		items[7].Gold = 1000;
		items[7].HP = 100;
		items[7].MP = 100;
		items[7].PDam = 0;
		items[7].MDam = 80;
		items[7].PDef = 0;
		items[7].MDef = 0;
		items[7].MS = 1;
		items[7].AS = 1;
		items[7].Tex = Resources.Load<Sprite>("UI/Item_Texture/8");

		items[8].Name = Enum_Item.水晶杖;
		items[8].Gold = 1000;
		items[8].HP = 0;
		items[8].MP = 0;
		items[8].PDam = 0;
		items[8].MDam = 80;
		items[8].PDef = 0;
		items[8].MDef = 0;
		items[8].MS = 1.3f;
		items[8].AS = 1;
		items[8].Tex = Resources.Load<Sprite>("UI/Item_Texture/9");

		items[9].Name = Enum_Item.魔法宝典;
		items[9].Gold = 1000;
		items[9].HP = 0;
		items[9].MP = 100;
		items[9].PDam = 0;
		items[9].MDam = 70;
		items[9].PDef = 0;
		items[9].MDef = 0;
		items[9].MS = 1.1f;
		items[9].AS = 1;
		items[9].Tex = Resources.Load<Sprite>("UI/Item_Texture/10");

		items[10].Name = Enum_Item.gay甲;
		items[10].Gold = 200;
		items[10].HP = 0;
		items[10].MP = 0;
		items[10].PDam = 0;
		items[10].MDam = 0;
		items[10].PDef = 10;
		items[10].MDef = 0;
		items[10].MS = 1;
		items[10].AS = 1;
		items[10].Tex = Resources.Load<Sprite>("UI/Item_Texture/11");

		items[11].Name = Enum_Item.军团圣盾;
		items[11].Gold = 1000;
		items[11].HP = 0;
		items[11].MP = 0;
		items[11].PDam = 0;
		items[11].MDam = 0;
		items[11].PDef = 20;
		items[11].MDef = 20;
		items[11].MS = 1.1f;
		items[11].AS = 1;
		items[11].Tex = Resources.Load<Sprite>("UI/Item_Texture/12");

		items[12].Name = Enum_Item.能量之甲;
		items[12].Gold = 1000;
		items[12].HP = 0;
		items[12].MP = 0;
		items[12].PDam = 0;
		items[12].MDam = 0;
		items[12].PDef = 0;
		items[12].MDef = 50;
		items[12].MS = 1;
		items[12].AS = 1;
		items[12].Tex = Resources.Load<Sprite>("UI/Item_Texture/13");

		items[13].Name = Enum_Item.血手;
		items[13].Gold = 1000;
		items[13].HP = 0;
		items[13].MP = 0;
		items[13].PDam = 10;
		items[13].MDam = 0;
		items[13].PDef = 10;
		items[13].MDef = 10;
		items[13].MS = 1.5f;
		items[13].AS = 1;
		items[13].Tex = Resources.Load<Sprite>("UI/Item_Texture/14");

		items[14].Name = Enum_Item.血龙甲;
		items[14].Gold = 1000;
		items[14].HP = 0;
		items[14].MP = 0;
		items[14].PDam = 0;
		items[14].MDam = 0;
		items[14].PDef = 60;
		items[14].MDef = 0;
		items[14].MS = 1;
		items[14].AS = 1;
		items[14].Tex = Resources.Load<Sprite>("UI/Item_Texture/15");

		items[15].Name = Enum_Item.契约与胜利之剑们;
		items[15].Gold = 1000;
		items[15].HP = 0;
		items[15].MP = 0;
		items[15].PDam = 50;
		items[15].MDam = 0;
		items[15].PDef = 0;
		items[15].MDef = 0;
		items[15].MS = 1;
		items[15].AS = 1.3f;
		items[15].Tex = Resources.Load<Sprite>("UI/Item_Texture/16");

		items[16].Name = Enum_Item.幻影之刃;
		items[16].Gold = 1000;
		items[16].HP = 0;
		items[16].MP = 0;
		items[16].PDam = 60;
		items[16].MDam = 0;
		items[16].PDef = 0;
		items[16].MDef = 0;
		items[16].MS = 1;
		items[16].AS = 1.1f;
		items[16].Tex = Resources.Load<Sprite>("UI/Item_Texture/17");

		items[17].Name = Enum_Item.疾风刃;
		items[17].Gold = 1000;
		items[17].HP = 0;
		items[17].MP = 0;
		items[17].PDam = 30;
		items[17].MDam = 0;
		items[17].PDef = 0;
		items[17].MDef = 0;
		items[17].MS = 1;
		items[17].AS = 2;
		items[17].Tex = Resources.Load<Sprite>("UI/Item_Texture/18");

		items[18].Name = Enum_Item.血刃;
		items[18].Gold = 1000;
		items[18].HP = 0;
		items[18].MP = 0;
		items[18].PDam = 80;
		items[18].MDam = 0;
		items[18].PDef = 0;
		items[18].MDef = 0;
		items[18].MS = 1;
		items[18].AS = 1;
		items[18].Tex = Resources.Load<Sprite>("UI/Item_Texture/19");

		items[19].Name = Enum_Item.鹰角弓;
		items[19].Gold = 1000;
		items[19].HP = 0;
		items[19].MP = 0;
		items[19].PDam = 40;
		items[19].MDam = 0;
		items[19].PDef = 0;
		items[19].MDef = 0;
		items[19].MS = 1.2f;
		items[19].AS = 1.2f;
		items[19].Tex = Resources.Load<Sprite>("UI/Item_Texture/20");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
