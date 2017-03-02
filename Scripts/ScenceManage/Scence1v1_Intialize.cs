using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scence1v1 intialize.
/// 生成自己和对方的英雄，并且初始化英雄属性；
/// </summary>
public class Scence1v1_Intialize : MonoBehaviour {   
	private static Scence1v1_Intialize _instance;
	public static Scence1v1_Intialize Instance{
		get { return _instance;}
	}
	public Enum_Role kindOfPlayer;
	public Enum_Role kindOfAI;
	//使用GameObject来储存所有玩家；到时直接使用gameobject来获取所有玩家及AI的对战信息；
	public GameObject role_Player;   //自己选的英雄；
	public GameObject role_AI;       //敌方选的英雄；
	public Role_Camp playerCamp;
	public Transform pos_PlayerInti;
	public Transform pos_AiInti;
	public UIManager uimanager;

	void Awake(){
		uimanager = GameObject.Find ("UI").GetComponent<UIManager> ();
		uimanager.mapSelect = MapSelect.oneVSone;                          //初始化是1V1 or 3V3

		_instance=this;
		IntializeRoleType(Application .dataPath +"/Resources/InitializeInfo/HerosChooosed.text");  //选择玩家的英雄角色
		playerCamp =ChoiceCamp ();        //选择红蓝方，包括出生位置
		ChoiceBirthPos (playerCamp);

		int skinNumPlayer=0;              //当前的皮肤，应当从施传来
		GameObject role_PlayerPrefab = ChoicePrefab (kindOfPlayer,skinNumPlayer);       
		role_Player =Instantiate (role_PlayerPrefab, pos_PlayerInti.position , Quaternion.identity)as GameObject ; //生成玩家的英雄

		int skinNumEnum = 0;            //当前的皮肤，应当从施传来
		GameObject role_AiPrefab = ChoicePrefab (kindOfAI,skinNumEnum);
		role_AI =Instantiate (role_AiPrefab, pos_AiInti .position , Quaternion.identity)as GameObject ;           //生成电脑英雄；

		IntilalizeCamp (playerCamp);     //初始化两边的整容；
	}


	void Start () {
		IntializeDataRole (role_Player,kindOfPlayer);   //初始化玩家的信息
		string pathTemp2=Application .dataPath +"/Resources/InitializeInfo/";
		Debug.Log ("PathTemp:" + pathTemp2);
		IntializeDataUser (role_Player, pathTemp2+"CurLoadPlayer.text"); //将玩家的名字，在游戏外的等级等给player；
		IntializeOtherkill(pathTemp2+"HerosChooosed.text");              //初始化两边的召唤师技能（skill——D skill——F）


		//给UI的委托加事件;将玩家的技能绑定到ui界面上；
		OperatePanel.skillButtonDelegate =role_Player.GetComponent <Role_Main>().SetSkillForUI;  
		OperatePanel .publicSkillButtonDelegate=role_Player.GetComponent <Role_Main>().SetOtherSkillForUI;  
		OperatePanel.attackButtonDelegate = role_Player.GetComponent<Role_Main>().Akt_normal;  

	}

	// Update is called once per frame
	void Update () {

	}
	public GameObject ChoicePrefab(Enum_Role role,int skinNum){
		switch (role) {
		case Enum_Role.GodOfMoon:
			return Resources.Load <GameObject> ("Prefeb_Role/Heros/ChangE"+"_0"+skinNum);
		case Enum_Role.YangJian:
			return Resources .Load <GameObject>("Prefeb_Role/Heros/YangJian"+"_0"+skinNum);
		case Enum_Role.JinKe:
			return Resources.Load <GameObject> ("Prefeb_Role/Heros/JianMo"+"_0"+skinNum);
		case Enum_Role.Amumu:
			return Resources.Load <GameObject> ("Prefeb_Role/Heros/Amumu"+"_0"+skinNum);
		case Enum_Role.XiaoYaoZi:
			return Resources.Load <GameObject> ("Prefeb_Role/Heros/XiaoYaoZi"+"_0"+skinNum);
		case Enum_Role.Ahri:
			return Resources.Load <GameObject> ("Prefeb_Role/Heros/Ahri"+"_0"+skinNum);
		default :
			return Resources.Load <GameObject> ("Prefeb_Role/Heros/Ahri"+"_0"+skinNum);
		}
	}

	public Data_Rloe ChoiceDataRole(Enum_Role role){
		string path = Application.dataPath + "/Resources/InitializeInfo/HeroData/";
		switch (role) {
		case Enum_Role.GodOfMoon:
			path += "ChangE.text";
			Data_Rloe roleData = JsonUti.JsonstreamToObject <Data_Rloe> (path);
			return roleData;
		case Enum_Role.YangJian:
			path += "YangJian.text";
			return JsonUti.JsonstreamToObject<Data_Rloe> (path);
		case Enum_Role.JinKe:
			path += "JianMo.text";
			return JsonUti.JsonstreamToObject<Data_Rloe> (path);
		case Enum_Role.Amumu:
			path += "Amumu.text";
			return JsonUti.JsonstreamToObject<Data_Rloe> (path);
		case Enum_Role.XiaoYaoZi:
			path += "XiaoYaoZi.text";
			return JsonUti.JsonstreamToObject<Data_Rloe> (path);
		case Enum_Role.Ahri:
			path += "Ahri.text";
			return JsonUti.JsonstreamToObject<Data_Rloe> (path);
		default :
			return JsonUti.JsonstreamToObject<Data_Rloe> (path);
		}
	}
	public void IntializeDataRole(GameObject role,Enum_Role enumRole){
		Data_Rloe data = ChoiceDataRole (enumRole);
		Role_Main playerData = role.GetComponent <Role_Main > ();
		playerData.skinNum = data.skinNum;
		playerData.type_Allrole = data.RorS;
		playerData.HpMax = data.HpMax;
		playerData.MpMax = data.MpMax;
		playerData.DefensePhysical = data.DefensePhysical;
		playerData.DefenseMagic = data.DefenseMagic;
		playerData.attack_Physical = data.attack_Physical;
		playerData.attack_Magic = data.attack_Magic;
		playerData.moveSpeed = data.moveSpeed;
		playerData.attack_Radius = data.attack_Radius;
		playerData.attack_Speed = data.attack_Speed;
		playerData.Level = data.Level;
		playerData.Levl_exp = data.Levl_exp;
		if (role.GetComponent<RoleInfo>().type_Range ==Type_Range.Long) {                        //初始化子弹层
			role.transform.FindChild ("weapon/WEAPON_1").gameObject.layer = LayerMask.NameToLayer ("Bullet");
		}
	}
	public void IntializeDataUser(GameObject role,string pathTemp){
		Debug.Log ("IntializeDataUser:" + role + pathTemp);
		DataOfUser data = JsonUti.JsonstreamToObject <DataOfUser> (pathTemp);
		role.GetComponent<Role_Main> ().playerName = data.username;
	}
	public void IntializeOtherkill(string pathTemp){
		HeroChooseInfoS chioce = JsonUti.JsonstreamToObject<HeroChooseInfoS> (pathTemp);
		Role_Main player = role_Player.GetComponent <Role_Main> ();
		Role_Main AIPlayer = role_AI.GetComponent <Role_Main> ();
		player.skill_D.skillName = chioce.hero [0].Skill_D;
		player.skill_F.skillName = chioce.hero [0].Skill_F;
		AIPlayer.skill_D.skillName = chioce.hero [3].Skill_D;
		AIPlayer.skill_F.skillName = chioce.hero [3].Skill_F;
	}
	public void IntializeRoleType(string pathTemp){
//		Debug.Log ("IntializeRoleType_Path:" + pathTemp);
		HeroChooseInfoS chioce = JsonUti.JsonstreamToObject<HeroChooseInfoS> (pathTemp);
//		Debug.Log ("IntializeRoleType_Chioce:" + chioce);
		kindOfPlayer = chioce.hero [0].hero;
		kindOfAI = chioce.hero [3].hero;
	}

	public Role_Camp ChoiceCamp(){                     //使用重载，可以自选红蓝方，也可自己选择一方；
		int temp = Random.Range (0, 2);
		if (temp == 0) {
			return Role_Camp.Red;
		} else {
			return Role_Camp.Blue;
		}
	}
	public void ChoiceBirthPos(Role_Camp camp){      //红色方在point_01；蓝色方在point_05
		switch(camp ){
		case Role_Camp.Red:
			pos_PlayerInti = GameObject.Find ("RebirthPos/RebirthPos_Red").transform;
			pos_AiInti = GameObject.Find ("RebirthPos/RebirthPos_Blue").transform;
			break;
		case Role_Camp.Blue:
			pos_PlayerInti = GameObject.Find ("RebirthPos/RebirthPos_Blue").transform;
			pos_AiInti = GameObject.Find ("RebirthPos/RebirthPos_Red").transform;
			break;
		}
	}
	public void IntilalizeCamp(Role_Camp camp){                                    
		switch(camp){
		case Role_Camp.Red:
			role_Player.GetComponent <Role_Main> ().roleCamp = Role_Camp.Red;
			role_Player.layer = LayerMask.NameToLayer ("Red");                                                         //初始化role的层
			role_AI.GetComponent <Role_Main> ().roleCamp = Role_Camp.Blue;
			role_AI.layer = LayerMask.NameToLayer ("Blue"); 

			break;
		case Role_Camp.Blue:
			role_Player.GetComponent <Role_Main> ().roleCamp = Role_Camp.Blue;
			role_Player.layer = LayerMask.NameToLayer ("Blue");
			role_AI.GetComponent <Role_Main> ().roleCamp = Role_Camp.Red;
			role_AI.layer = LayerMask.NameToLayer ("Red");
			break;
		}
	}

}
