using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .AI ;

public class Role_XiaoYaoZi :Role_Main  {
	public Transform weapon;
	public bool Skill_Q_Damage_Real;
	public float timeCount = 0;
	public GameObject bullet;
	public Transform firePos;
	float timeCouAtkNormal=0f;
	float atkInterval;
	GameObject skillPrefab;
	Transform skillFirePos;
	void Start(){
		skillPrefab = Resources.Load<GameObject> ("TeXiao/XiaoYaoZiSkill");
		skillFirePos = transform.Find ("weapon");
		StartCoroutine(base.Start());
		bullet = Resources.Load <GameObject> ("Prefeb_Role/bullet_Ice");
		type_Range = Type_Range.Long;
		weapon = transform.Find ("weapon/WEAPON_1");
	}
	void Update(){
		BaseUpdate ();                     //父类中需要在Update中需要调用的；
		skill_Q .timeCount +=Time .deltaTime ;
		if (skill_Q .timeCount >=skill_Q.CD && skill_Q.IsUseThis ) {
			skill_Q.IsContinuous = true;
		}

		atkInterval = 1f / attack_Speed;    //万一改变普功的速度了呐；
		timeCouAtkNormal += Time.deltaTime; //s:攻击的语句；其他类也需要这样写
		if (CanAtkNormal==true) {           //s:攻击的语句；其他类也需要这样写
			Akt_Target ();					//s:攻击的语句；其他类也需要这样写（远程可以相同，近站则要修改）
		}									//s:攻击的语句；其他类也需要这样写
	}
		
	public override void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role) // 技能被动说明：普攻伤害
	{
		if (IsDeath ==false ) {
			Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
		}
	}


	//普功的实现：
	public override void Akt_normal ()
	{
		CanAtkNormal = true;
	}
	public void Akt_Target(){
		Debug.Log ("普通攻击:"+target);
		if (target ==null) {                                //如果目标为空，怎自动寻找目标；
			Collider[] cols = Physics.OverlapSphere (transform .FindChild ("Atk_Radius").position, attack_Radius, EnemyLayer);
			if (cols.Length > 0) {                         //有目标
				float minDistance = Vector3.Distance (transform.position, cols [0].transform.position);
				int chioce = 0;
				for (int i = 0; i < cols.Length; i++) {
					float currentDis = Vector3.Distance (transform.position, cols [i].transform.position);
					if (currentDis < minDistance) {
						chioce = i;
					}
				}
				target = cols [chioce].transform;
			} else {
				CanAtkNormal = false;
			}
		}
		if (target != null) {
			Vector3 selfPos = transform.FindChild ("Atk_Radius").position;
			if (Vector3.Distance (selfPos, target.position) > attack_Radius) {
				Debug.Log ("跑到目标点"+Vector3.Distance (selfPos, target.position) +"半径："+attack_Radius);
				SetMoveTarget (target.position);//跑到目标点
			}
			if (Vector3.Distance (selfPos, target.position) <=attack_Radius ) {
				if (timeCouAtkNormal >= atkInterval && target != null) {
					timeCouAtkNormal = 0f;
					Vector3 lookDir = target.position - transform.position;
					lookDir.y = 0f;
					transform.rotation = Quaternion.LookRotation (lookDir);
					ani.SetTrigger ("CanSkill_Akt");
				}
			}
			if (Vector3.Distance (selfPos, target.position) < (attack_Radius - 0.7f)) {
				agent.Stop ();
				Debug.Log ("为何不停下来");
			}
		}
	}
	public void Atk_Event_Start(){      //普功的时候 动作的一个事件；
		transform.FindChild ("weapon/WEAPON_1").gameObject.SetActive (false);
		Vector3 lookDir = target.position - transform.position;
		lookDir.y = 0f;
		transform.rotation = Quaternion.LookRotation (lookDir);
		Debug.Log ("让阿里的球消失；"+transform.FindChild ("weapon/WEAPON_1").gameObject);
	}
	public void Atk_Event_Fire(){     //普功的时候 动作的一个事件；  生成子弹；
		GameObject bulletAli = Instantiate (bullet, firePos.position, Quaternion.identity)as GameObject;
		bulletAli.GetComponent <BulletAction> ().GetNumber (GetComponent<Role_Main> ());
		bulletAli.GetComponent <BulletAction> ().SetTarget (target);   //函数内有协同
		timeCouAtkNormal = 0f;
		CanAtkNormal = false;
	}
	public void Atk_Event_End(){          //普功的时候 动作的一个事件； 
		transform.FindChild ("weapon/WEAPON_1").gameObject.SetActive (true);
	}
	//普功的实现：结束了；

	public override void SetSkill_Q ()
	{
		GameObject tmp= Instantiate (skillPrefab, skillFirePos.position, skillFirePos.rotation) as GameObject;
		if (tmp != null) {
			tmp.GetComponent<XiaoYaoZiSkillControl> ().camp = this.roleCamp;
			tmp.GetComponent<XiaoYaoZiSkillControl> ().owner = this;
		}
		stateAni = State_Ani.State_Idle;
		skill_Q.IsUseThis = true;
		Debug.Log ("放技能Q了");
		ani.SetTrigger ("CanSkill_Q");
		agent.Stop ();

	}
	public void Skill_Q_Event(){   //技能Q的事件
		Skill_Q_Damage_Real = true;
		RaycastHit hit;
		Physics.Linecast (transform.position + new Vector3 (0, 1, 0), weapon.position + new Vector3 (0, -1, 1),out hit , EnemyLayer);
	}
	public void Skill_Q_Event_End(){
		stateAni = State_Ani.State_Idle;
	}

	public override void SetSkill_W ()
	{
		ani.SetTrigger ("CanSkill_W");
		agent.Stop ();
		stateAni = State_Ani.State_Idle;

	}
	public override void SetSkill_E ()
	{
		ani.SetTrigger ("CanSkill_E");
		agent.Stop ();
		stateAni = State_Ani.State_Idle;
	}
	public override void SetSkill_D ()
	{
		throw new System.NotImplementedException ();
	}
	public override void SetSkill_F ()
	{
		throw new System.NotImplementedException ();
	}

	public bool  CanSetSkill(SkillInfo skill){
		if (skill.timeCount >= skill.CD ) {
			return true;
		} else {
			return false;
		}
	}
	public override void DoPasssive ()
	{
	}


}