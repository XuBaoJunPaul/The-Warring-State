using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System .Serializable]
public abstract class RoleInfo : MonoBehaviour {
	public Role_Camp roleCamp;
	public Role_Camp playerCamp;
	public Type_Allrole type_Allrole;
	public Type_Range type_Range;
	public int HpMax;
	public float Hp;
	public int MpMax;
	public int Mp;
	public int DefensePhysical;
	public int DefenseMagic;
	public int attack_Physical;
	public int attack_Magic;
	public float attack_Speed;
	public float moveSpeed;       //将moveSpeed赋值给NavMeshAgent agent的speed即控制移动速度；
	public float attack_Radius;
	public int Levl_exp;
	public int Level;
	public int worthExp;
	public int worthMoney;
	public  LayerMask FriendLayer;
	public LayerMask EnemyLayer;
	public int EnemyLayerNum;  //敌人的层数；
	public bool IsDeath=false ;//是否死亡； true:已经死亡；   false：没有死
	protected Collider colRole;
	public Type_Allrole role_LastAyk;   //最后是被谁打的（	role,soldier,tower,buffer）；
	public UIManager uiManager;
	public void BaseStart(){
		uiManager = GameObject.Find ("UI").GetComponent <UIManager> ();
		if (type_Allrole!=Type_Allrole.buffer) {           //是buffer的话就不设置
			if (uiManager.mapSelect ==MapSelect.oneVSone) {
				playerCamp = Scence1v1_Intialize.Instance.playerCamp;
			}
			if (uiManager .mapSelect ==MapSelect.threeVSthree) {
				playerCamp = Scence3v3_Intialize.Instance.playerCamp;
			}
			if (roleCamp == playerCamp) {
				GetComponent <FOWRevealer> ().enabled = true;
				GetComponent <FOWRenderers> ().enabled = false;
			} else {
				GetComponent <FOWRevealer> ().enabled = false;
				GetComponent <FOWRenderers> ().enabled = true;
			}
		}
	}

	public abstract void GetHurt (int hurt_Physic, int hurt_Magic,Type_Allrole role);
	public abstract void GetHurt(int hurt_Physic, int hurt_Magic,Role_Main role);
	public void GetHurt (int hurt_Physic, int hurt_Magic)
	{
		if (IsDeath ==false) {
			Hp = Hp -hurt_Physic + hurt_Physic * DefensePhysical / (DefensePhysical + 100) -hurt_Magic + hurt_Magic * DefenseMagic / (DefenseMagic + 100);
		}
	}

}
public enum Type_Allrole{
	role,
	soldier,
	tower,
	buffer
}
public enum Role_Camp{
	Red,
	Blue
}
public enum Type_Range{
	Near,
	Long
}