using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
public class NumShow : MonoBehaviour {
	public Text NumKill;	 //击杀次数
	public Text NumDeath;	 //死亡次数
	public Text NumCount;    //补兵
	public Text NumAssists;  //助攻
	public UIManager uiManager;
	public Role_Main roleMain;
	// Use this for initialization
	void Start () {
		NumKill = GameObject.Find ("UI/DataPanel/KillorDeath/PersonalData/Kill").GetComponent <Text>();
		NumDeath=GameObject.Find ("UI/DataPanel/KillorDeath/PersonalData/Death").GetComponent <Text>();
		NumCount=GameObject.Find ("UI/DataPanel/KillorDeath/PersonalData/SoliderCount").GetComponent <Text>();
		NumAssists =GameObject.Find ("UI/DataPanel/KillorDeath/PersonalData/Assists").GetComponent <Text>();
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();
		if (uiManager.mapSelect == MapSelect.oneVSone) {
			roleMain = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main> ();
		} else {
			roleMain = Scence3v3_Intialize.Instance.role_Players[0].GetComponent <Role_Main> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		NumKill.text = roleMain.killCount.ToString ();
		NumDeath.text = roleMain.deathCount.ToString ();
		NumCount.text = roleMain.soliderKillCount.ToString ();
		NumAssists.text = roleMain.assistsCount.ToString ();
	}
}
