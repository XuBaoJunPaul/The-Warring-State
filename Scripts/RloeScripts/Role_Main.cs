using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System .Serializable]
public abstract  class Role_Main : RoleInfo {   //与徐宝骏合并	
	//一些不同的必须提前定义的属性就需要初始化给其值：string playerName； OtherSkillInfo ；Sprite roleTex;  
	//如果可以根据角色的名字就能知道的属性，就在子类的start函数中给其赋值：Enum_Role roleName； SkillInfo
	//一些是在游戏后才有的属性，不需要初始化；（但是服务器应当需要知道，暂不考虑）；
	public int ScenceType=0;           //场景的类别：0:1V1 ； 1:＃V3
	public Enum_InsertID insertID;     //游戏内部ID  ________________________________________怎么让人知道ID 就知道是这个role的gameobject呐？
	public string playerName;          //需要再注册的时候就应当填写
	public Enum_Role roleName;          //英雄种类
	public int skinNum=0;              //需要在注册的时候选择,皮肤种类
	public AiOrPlayerType aiOrPlayer;  //需要在初始化的时候给：是否是操控者
	public Sprite roleTex;             //英雄头像 需要在Start中给其赋值；
	public int money;                   //持有的金币
	public float rebirthTime=10f;   //使用isDeath来判断
	float rebTimeCount=0f;
	public bool CanAtkNormal=false;
	public SkillInfo skill_Q;        //技能图标也需要在Start中给赋值
	public SkillInfo skill_W;
	public SkillInfo skill_E;
	public OtherSkillInfo skill_D;
	public OtherSkillInfo skill_F;
	public Slot_Item items;

	public DeathInfo[] enemyInfo;  //助攻者：被敌人打到的集合；_______________________________________lastAyk>workLast_______________RoleMain怎么给它赋初值呐？
	public int assistsCount=0;  //助攻次数；
	public int killCount = 0;   //击杀次数
	public int deathCount = 0;	//死亡次数；
	public int soliderKillCount=0;
	private float TimeLastDeath=15f;

	public float ExpRadius;       //经验范围
	public State_Ani stateAni;    //当前的状态
	public Vector3 MoveTarget;	 //移动的目标
	public Transform target;     //攻击目标
	public Animator ani;
	public AnimatorStateInfo aniInfo; //获取状态机的状态用 
	public Vector3 skillLookDir;
	public UIManager uimanager;
	public NavMeshAgent agent;

	void Awake(){
	}
	protected IEnumerator Start(){           //基类初始化

		uimanager = GameObject.Find("UI").GetComponent<UIManager>();

		if (roleCamp == Role_Camp.Blue) {
			FriendLayer = LayerMask.GetMask ("Blue","Buff");
			EnemyLayer = LayerMask.GetMask ("Red","Buff");
			EnemyLayerNum = 9;
		} else {
			FriendLayer = LayerMask.GetMask ("Red","Buff");
			EnemyLayer = LayerMask.GetMask ("Blue","Buff");
			EnemyLayerNum = 10;
		}
		BaseStart ();
		Hp =HpMax ;
		Mp = MpMax;
		ani =GetComponent<Animator> ();
		agent =GetComponent <NavMeshAgent> ();
		agent.speed = moveSpeed;
		stateAni =State_Ani .State_Idle ;
		string pathTemp="UI/Skills/RoleSkillUI/"+roleName .ToString()+"/";
		roleTex = Resources.Load <Sprite> (pathTemp+"HeadTex_"+skinNum);
		skill_Q.tex = Resources.Load <Sprite> (pathTemp + "Skill_Q");
		skill_W.tex=Resources .Load <Sprite>(pathTemp + "Skill_W");
		skill_E.tex=Resources .Load <Sprite>(pathTemp + "Skill_E");
		skill_D.tex = FindTexDF (skill_D.skillName);
		skill_F.tex = FindTexDF (skill_F.skillName);
		skill_Q.timeCount = skill_Q.CD;
		skill_W.timeCount = skill_W.CD;
		skill_E.timeCount = skill_E.CD;
		skill_D.timeCount = skill_D.CD;
		skill_F.timeCount = skill_F.CD;
		colRole = GetComponent <Collider> ();
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();                       //1V1 or 3V3
		yield return null;
		//初始化enemyInfo
		if (uiManager.mapSelect ==MapSelect.oneVSone) {
			IntializeEnemyInfo1V1 (roleCamp);
		}
		if (uiManager.mapSelect==MapSelect.threeVSthree) {
			IntializeEnemyInfo3V3 (roleCamp);
		}
		yield return null ;

	}
	public void BaseUpdate(){
		rebTimeCount += Time.deltaTime;
 		aniInfo= ani.GetCurrentAnimatorStateInfo(0);
		if (Mathf .Abs (agent .remainingDistance)<0.3f && stateAni!=State_Ani .State_Idle) { //小于0.3就停下来
			Debug.Log ("停下来了");
			ani .SetTrigger ("CanStop");
			stateAni = State_Ani.State_Idle;
		} 
		if (aiOrPlayer==AiOrPlayerType.Player) {
			SetKillForKey ();
		}
		for (int i = 0; i < enemyInfo .Length; i++) {
			enemyInfo [i].time_LastAtk += Time.deltaTime;
		}
		CanDeath ();//死亡判定,判定助攻，击杀，
		skill_Q.timeCount += Time.deltaTime;
		skill_W.timeCount += Time.deltaTime;
		skill_E.timeCount += Time.deltaTime;
		skill_D .timeCount +=Time .deltaTime;
		skill_F.timeCount += Time.deltaTime;
	}
	void OnDrawGizmos(){
		Gizmos.color = new Color (1, 1, 0, 0.5f);
		Gizmos.DrawSphere (transform .FindChild ("Atk_Radius").position, attack_Radius);
	}
		
	public abstract void Akt_normal ();
	public abstract void SetSkill_Q ();
	public abstract void SetSkill_W ();
	public abstract void SetSkill_E ();
	public abstract void SetSkill_D ();
	public abstract void SetSkill_F ();
	public abstract void DoPasssive ();

	public override void GetHurt(int hurt_Physic, int hurt_Magic,Role_Main roleMain)                    
	{ 
		if (IsDeath ==false) {
			Hp = Hp - hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) - hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
			for (int i = 0; i < enemyInfo.Length; i++) {               //统计助攻；
				if (enemyInfo[i].roleMain.insertID ==roleMain .insertID ) {
					enemyInfo [i].time_LastAtk = 0f;
				}
			}
			if (Hp <=0) {
				IsDeath = true;
			}
		}
	}
	public void IntializeEnemyInfo1V1(Role_Camp camp){
		Debug.Log ("初始化IntializeEnemyInfo1V1");
		enemyInfo = new DeathInfo[1];
		Role_Main temp1 = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main> ();
		Role_Main temp2 = Scence1v1_Intialize.Instance.role_AI.GetComponent<Role_Main> ();
		if (camp==Role_Camp.Blue) {
			if (temp1 .roleCamp ==Role_Camp.Red) {
				enemyInfo [0].roleMain = temp1;
			}
			if (temp2 .roleCamp ==Role_Camp.Red) {
				enemyInfo [0].roleMain = temp2;
			}
		}
		if (camp==Role_Camp.Red) {
			if (temp1 .roleCamp ==Role_Camp.Blue) {
				enemyInfo [0].roleMain = temp1;
			}
			if (temp2 .roleCamp ==Role_Camp.Blue) {
				enemyInfo [0].roleMain = temp2;
			}
		}
	}
	public void IntializeEnemyInfo3V3(Role_Camp camp){  
		enemyInfo = new DeathInfo[1];
		for (int i = 0; i < 3; i++) {
			Role_Main temp1 = Scence3v3_Intialize.Instance.role_Players [i].GetComponent<Role_Main> ();
			Role_Main temp2 = Scence3v3_Intialize.Instance.role_AIs [i].GetComponent<Role_Main>();

			if (camp==Role_Camp.Blue) {
				if (temp1 .roleCamp ==Role_Camp.Red) {
					enemyInfo [0].roleMain = temp1;
				}
				if (temp2 .roleCamp ==Role_Camp.Red) {
					enemyInfo [0].roleMain = temp2;
				}
			}
			if (camp==Role_Camp.Red) {
				if (temp1 .roleCamp ==Role_Camp.Blue) {
					enemyInfo [0].roleMain = temp1;
				}
				if (temp2 .roleCamp ==Role_Camp.Blue) {
					enemyInfo [0].roleMain = temp2;
				}
			}
		}
	}

	public void OnMoveButton(Vector3 lookDir){                        //图标控制移动
		if (Vector3 .Distance (new Vector3(0,0,0),lookDir)>0 ) {
			transform.Translate (lookDir.normalized * moveSpeed, Space.World);
			transform.rotation = Quaternion.LookRotation (lookDir);
			if (!aniInfo .IsName ("Run")) {
				ani.SetTrigger ("Run");
			}
		}
		
	}
	public void SetMoveTarget(Vector3 target){      //在cameraControl中调用，实现右键移动至目标点
		this.MoveTarget = target;
		Vector3 lookDir = -transform.position + target;
		lookDir.y = 0f;
		Debug.Log ("出来吧，我的角度:"+Vector3 .Angle (lookDir ,transform .forward));
		if (Vector3 .Angle (lookDir ,transform .forward)>=60f) {
			transform.rotation = Quaternion.LookRotation (lookDir);
			agent.enabled = false;
			Debug.Log ("出来吧，我的角度");
		}

		if (Vector3.Distance (transform.position, MoveTarget) >= 0.3f) {  //大于0，3就自动寻路
			if (stateAni !=State_Ani.State_Run ) {
				ani.SetTrigger ("CanRun");
			}
			agent.enabled = true;
			stateAni = State_Ani.State_Run;
			Debug.Log ("自动寻路:"+target);
			agent.Resume ();
			agent.speed = moveSpeed;
			agent.SetDestination (target);  //自动寻路	
		} 
	}
	
	public void SetTarget(Transform target){ // to get the target; by 牧
		this.target =target ;
	}
	public void CanDeath(){  //死亡判断
		if (IsDeath) {                  //死亡动作；
			Hp = 0;
			ani.SetTrigger ("Can_Death");
			colRole.isTrigger = true; //不被碰撞；
			colRole.enabled = false;  //不被检测
			transform .Find ("weapon/WEAPON_1").gameObject .SetActive (false);
			GetComponent<HpShow>().canShow =false ;
			stateAni = State_Ani.State_Death;
			rebTimeCount = 0f;
			int tempChioce = 0; //最后的击杀者
			for (int i = 0; i < enemyInfo.Length; i++) {
				if (enemyInfo[i].time_LastAtk<=TimeLastDeath) {
					enemyInfo [i].roleMain.assistsCount++;
					enemyInfo [i].roleMain.ReceiveExpAndGold (worthExp / 2, worthMoney / 2);
				}
				if (enemyInfo[tempChioce].time_LastAtk>enemyInfo[i].time_LastAtk) {
					tempChioce = i;
				}
			}
			if (enemyInfo[tempChioce].time_LastAtk<TimeLastDeath ) {
				Debug.Log (playerName + "被" + enemyInfo [tempChioce].roleMain.playerName + "击杀了！！！"); //需要一个ui界面————————————————————
				enemyInfo[tempChioce].roleMain .assistsCount --;
				enemyInfo [tempChioce].roleMain.killCount++;
				enemyInfo [tempChioce].roleMain.ReceiveExpAndGold (worthExp / 2, worthMoney / 2);
			}
		}
//		if (IsDeath && rebTimeCount >=6f) {
//			transform.Find ("MeshSelf").gameObject.SetActive (false);
//		}
		
		if (IsDeath && rebTimeCount>=rebirthTime ) {
			IntiRoleRebirthData ();
		}
	}
	public void ReceiveExpAndGold(int exp,int gold){  //升级判断 :每级的经验公式：(90+(level-1)*20)
		Level_exp +=exp ;
		money += gold;
		if (Level_exp>(90+(Level-1)*20) ) {
			Level++;
			Level_exp = Level_exp - (90 + (Level - 1) * 20);
		}
	}

	public void IntiRoleRebirthData(){ 
		Debug.Log ("复活啦");
//		transform.Find ("MeshSelf").gameObject.SetActive (true);
		transform .Find ("weapon/WEAPON_1").gameObject .SetActive (true);
		transform.GetComponent <HpShow> ().canShow = true;
		colRole.enabled = true;
		colRole.isTrigger = false;
		if (uimanager.mapSelect == MapSelect.oneVSone) {
			if (roleCamp == Role_Camp.Red) {
				transform.position = GameObject.Find ("PathSolider/point_00").transform.position;
			} else {
				transform.position = GameObject.Find ("PathSolider/point_XX").transform.position;
			}
		}
		if (uimanager.mapSelect == MapSelect.threeVSthree) {
			if (roleCamp == Role_Camp.Red) {
				transform.position = GameObject.Find ("RebirthPos/RebirthPos_Red").transform.position;
			} else {
				transform.position = GameObject.Find ("RebirthPos/RebirthPos_Blue").transform.position;
			}
		}
		IsDeath = false;
		ani.SetTrigger ("CanReAlive");
		stateAni = State_Ani.State_Idle;
		Hp = HpMax;
		Mp = MpMax;
	}


	public void SetKillForKey(){  //使用键盘调用技能函数；ui也需要调用相应的函数
		if (Input .GetKeyDown (KeyCode.A )) {
			
			Akt_normal ();
		}
		if (Input .GetKeyDown (KeyCode .Q)) {
			if (skill_Q .timeCount >=skill_Q .CD && skill_Q .level>0 ) {
				transform.rotation = Quaternion.LookRotation (skillLookDir);
				SetSkill_Q ();
				skill_Q.timeCount = 0f;
			}

		}
		if (Input .GetKeyDown (KeyCode .W )) {
			if (skill_W .timeCount >=skill_W .CD && skill_W .level>0) {
				transform.rotation = Quaternion.LookRotation (skillLookDir);
				SetSkill_W ();
				skill_W.timeCount = 0f;
			}
		}
		if (Input .GetKeyDown (KeyCode .E )) {
			if (skill_E .timeCount >=skill_E .CD && skill_E .level>0) {
				transform.rotation = Quaternion.LookRotation (skillLookDir);
				SetSkill_E ();
				skill_E.timeCount = 0f;
			}
		}
	}
	public void SetSkillForUI(SkillButtonName  skillName){   //为委托准备的借口
		switch (skillName) {
		case SkillButtonName.Button_Q:
			if (skill_Q .timeCount >=skill_Q .CD && skill_Q .level>0 ) {
				transform.rotation = Quaternion.LookRotation (skillLookDir);
				SetSkill_Q ();
				skill_Q.timeCount = 0f;
			}
			break;
		case SkillButtonName.Button_W:
			if (skill_W .timeCount >=skill_W .CD && skill_W .level>0) {
				transform.rotation = Quaternion.LookRotation (skillLookDir);
				SetSkill_W ();
				skill_W.timeCount = 0f;
			}
			break;
		case SkillButtonName.Button_E :
			transform.rotation = Quaternion.LookRotation (skillLookDir);
			if (skill_E .timeCount >=skill_E .CD && skill_E .level>0) {
				SetSkill_E ();
				skill_E.timeCount = 0f;
			}
			break;
		}
	}

	public void SetOtherSkillForUI(SkillButtonName skillotherName){
		switch (skillotherName) {
		case SkillButtonName.Button_D:
			if (skill_D .timeCount >=skill_D .CD ) {
				SetOtherSkill (skill_D.skillName);
				skill_D.timeCount = 0f;
			}
			break;
		case SkillButtonName.Button_F:
			if (skill_F.timeCount >=skill_F.CD) {
				SetOtherSkill (skill_F.skillName);
				skill_F.timeCount = 0f;
			}
			break;
		}
	}

	public void SetOtherSkill(OtherSkill_Name skillName){
			switch (skillName) {
			case OtherSkill_Name.Weak:
				SetOtherSkill_Weak ();
				break;
			case OtherSkill_Name.Quicken:
				SetOtherSkill_Quicken();
				break ;
			case OtherSkill_Name.Flash:
				SetOtherSkill_Flash();
				break ;
			case OtherSkill_Name.FireHit:
				SetOtherSkill_FireHit ();
				break;
			case OtherSkill_Name.Treatment :
				SetOtherSkill_Treatment();
				break ;
			case OtherSkill_Name.PowerUp:
				SetOtherSkill_PowerUp (10,10);
				break;
			}
		}

	public Sprite FindTexDF(OtherSkill_Name skillName){
		string pathTemp="UI/Skills/OtherSkills/";
		switch (skillName) {
		case OtherSkill_Name.Weak:
			return Resources.Load <Sprite> (pathTemp + 0);
		case OtherSkill_Name.Quicken:
			return Resources.Load <Sprite> (pathTemp + 1);
		case OtherSkill_Name.Flash:
			return Resources.Load <Sprite> (pathTemp + 2);
		case OtherSkill_Name.FireHit:
			return Resources.Load <Sprite> (pathTemp + 3);
		case OtherSkill_Name.Treatment :
			return Resources.Load <Sprite> (pathTemp + 4);
		case OtherSkill_Name.PowerUp:
			return Resources.Load <Sprite> (pathTemp + 5);
		default :
			return Resources.Load <Sprite> (pathTemp + 0);
		}
	}
	public void SetOtherSkill_Weak(){
		target.GetComponent <Role_Main> ().moveSpeed -= 3;//需要后期修改参数,并且限制时间
	}
	public void SetOtherSkill_Quicken(){
		this.moveSpeed += 3;                                //需要后期修改参数
	}
	public void SetOtherSkill_Flash(){
		transform.position += transform.forward * 3f; 
	}
	public void SetOtherSkill_FireHit(){
		target.GetComponent <Role_Main > ().Hp -= 50;
	}
	public void SetOtherSkill_Treatment(){
		float radius = 3f;           //治聊半径；
		Collider[] cols= Physics.OverlapSphere (transform.position, radius, FriendLayer);
		float timeCount = 0;
		timeCount += Time.deltaTime;
		if (timeCount<3.5f ) {
			for (int i = 0; i < cols.Length ; i++) {
				cols [i].GetComponent <Role_Main> ().Hp += 55;
			}
		}
	}
	public void SetOtherSkill_PowerUp(int Add_Physical,int add_Magic){
		attack_Physical += Add_Physical;
		attack_Magic += add_Magic;
	}

}



[System .Serializable]           
public struct SkillInfo{
	public SkillName skill;
	public int level;
	public int levelMax;
	public int CD;
	public int Mp;
	public float timeCount;
	public int attack_Physical;
	public int attack_Magic;
	public bool IsUseThis;  //是否使用；
	public bool IsNeedTarget; //是否需要目标
	public bool IsContinuous; //
	public Sprite tex;
}

[System .Serializable ]
public struct OtherSkillInfo{
	public OtherSkill_Name skillName;
	public Sprite tex;
	public int CD;
	public bool IsCanUseful;
	public float timeCount;
}

public enum SkillName{
	SkillName_Q,
	SkillName_W,
	SkillName_E
}

public enum State_Skill{
	None,
	state_Q,
	state_W,
	state_E,
}

public enum OtherSkill_Name{
	Weak,
	Quicken,
	Flash,
	FireHit,
	Treatment,
	PowerUp,

}
public enum State_Ani{
	State_Idle,
	State_Skill_Atk,
	State_Skill_Q,
	State_Skill_W,
	State_Skill_E,
	State_Run,
	State_Dance,
	State_talk_01,
	State_Death
}
public enum RouteType{
	RouteUp,
	RouteMid,
	RouteDown
}
public enum AiOrPlayerType{
	AI,
	Player
}
[System .Serializable]
public struct DeathInfo{
	public Role_Main roleMain;     //最后被打的：该英雄的名字；
	public float time_LastAtk;          //最后被英雄攻击的时间；
}