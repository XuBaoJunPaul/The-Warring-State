using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .UI ;
using UnityEngine.SceneManagement;

/// <summary>
/// Game count.
/// 用于死亡之后的胜利或者失败的ui画面，确认按钮的弹出
/// 用于实时统计每个英雄的击杀死亡助攻补兵的战绩统计
/// </summary>
public class GameCount : MonoBehaviour {
	private static GameCount _instance;
	public static GameCount Instance{
		get { return _instance;}
	}
	public Role_Camp camp;
	public TowerBaseInfo towerRed;
	public TowerBaseInfo towerBlue;
	public bool CanOver;
	public bool redWin;
	public bool blueWin;

	private Transform cam;
	public Transform redPos;
	public Transform bluePos;

	private GameObject obj_win;
	private GameObject obj_lose;
	private GameObject obj_button;

	public Role_Main[] roleMains_Red;
	public Role_Main[] roleMains_Blue; 
	public Record_Role[] roles_Red;
	public Record_Role[] roles_Blue;
	private UIManager uiManager;
	// Use this for initialization
	void Start () {
		_instance = this;

		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();                       //1V1 or 3V3
		if (uiManager.mapSelect ==MapSelect.oneVSone) {
			camp = Scence1v1_Intialize.Instance.playerCamp;
			roleMains_Red = new Role_Main[1];
			roleMains_Blue = new Role_Main[1];
			roles_Red = new Record_Role[1];
			roles_Blue = new Record_Role[1];
			if (camp == Role_Camp.Red) {
				roleMains_Red [0] = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main> ();
				roleMains_Blue [0] = Scence1v1_Intialize.Instance.role_AI.GetComponent <Role_Main > ();
			} 
			if (camp == Role_Camp.Blue) {
				roleMains_Red [0] = Scence1v1_Intialize.Instance.role_AI.GetComponent <Role_Main> ();
				roleMains_Blue [0] = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main > ();
			} 
			
		}
		if (uiManager.mapSelect==MapSelect.threeVSthree) {
			camp = Scence3v3_Intialize.Instance.playerCamp;
			roleMains_Red = new Role_Main[3];
			roleMains_Blue = new Role_Main[3];
			roles_Red = new Record_Role[3];
			roles_Blue = new Record_Role[3];
			if (camp==Role_Camp.Red) {
				for (int i = 0; i < Scence3v3_Intialize .Instance .role_Players.Length; i++) {
					roleMains_Red [i] = Scence3v3_Intialize.Instance.role_Players [i].GetComponent <Role_Main> ();
					roleMains_Blue [i] = Scence3v3_Intialize.Instance.role_AIs [i].GetComponent <Role_Main> ();
				}
			}
			if (camp== Role_Camp.Blue) {
				for (int i = 0; i < Scence3v3_Intialize .Instance .role_AIs.Length; i++) {
					roleMains_Red [i] = Scence3v3_Intialize.Instance.role_AIs [i].GetComponent<Role_Main> ();
					roleMains_Blue [i] = Scence3v3_Intialize.Instance.role_Players [i].GetComponent <Role_Main> ();
				}
			}
		}



		towerBlue = GameObject.Find ("shuiJingBlue").GetComponent <TowerBaseInfo>();
		towerRed = GameObject.Find ("shuiJingRed").GetComponent <TowerBaseInfo> ();
		CanOver = false;
		cam = Camera.main.transform;
		redPos = GameObject.Find ("shuiJingRed").transform;
		bluePos = GameObject.Find ("shuiJingBlue").transform;

		obj_win = GameObject.Find ("UI/gameOverShow/win");
		obj_lose =GameObject .Find ("UI/gameOverShow/lose");
		obj_button = GameObject.Find ("UI/gameOverShow/gameover");
		obj_win.SetActive (false);
		obj_lose.SetActive (false);
		obj_button.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < roles_Red.Length ; i++) {
			roles_Red [i].name = roleMains_Red [i].name;
			roles_Red [i].role = roleMains_Red [i].roleName;
			roles_Red [i].killCount = roleMains_Red [i].killCount;
			roles_Red [i].assistsCount = roleMains_Red [i].assistsCount;
			roles_Red [i].deathCount = roleMains_Red [i].deathCount;
			roles_Red [i].soliderKillCount = roleMains_Red [i].soliderKillCount;

			roles_Blue[i].name = roleMains_Blue [i].name;
			roles_Blue[i].role = roleMains_Blue [i].roleName;
			roles_Blue[i].killCount = roleMains_Blue [i].killCount;
			roles_Blue[i].assistsCount = roleMains_Blue [i].assistsCount;
			roles_Blue[i].deathCount = roleMains_Blue [i].deathCount;
			roles_Blue[i].soliderKillCount = roleMains_Blue [i].soliderKillCount;
		}


		if (towerRed .Hp <=0) {
			CanOver = true;
			redWin = false ;
			blueWin = true;
			scencePass.IsGameOver = true;
		}
		if (towerBlue.Hp <=0) {
			CanOver = true;
			redWin = true;
			blueWin = false;
			scencePass.IsGameOver = true;
		}

		if (CanOver ==true ) {
			if (redWin) {
				CameraMove (bluePos.position);
				JudgeGame (Role_Camp.Red, camp);
			} else {
				CameraMove (redPos.position);
				JudgeGame (Role_Camp.Blue, camp);
			}
		}
	}

	void CameraMove(Vector3 target){
		target.y += 10f;
		target.z -= 7f;
		cam.position = Vector3.Slerp (cam.position, target, Time.deltaTime * 5f);
	}
	public void JudgeGame(Role_Camp winner,Role_Camp player){
		if (winner == player) {
			obj_win.SetActive (true);
			obj_button.SetActive (true);
		} else {
			obj_lose.SetActive (true);
			obj_button.SetActive (true);
		}
	}


	public void OnGameOverButton(){
		scencePass.IsGameOver = true;
		SceneManager.LoadScene (0);
	}
}
