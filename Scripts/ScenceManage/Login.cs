using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
	public HeroChooseUIManager ui;

	public AudioSource audioSource;

	public GameObject tips;
	public GameObject tips2;

	public GameObject mapChoice;

	public GameObject music;

	public GameObject canvas01;
	public GameObject canvas02;
	public GameObject canvas03;
	public GameObject canvas04;
	public GameObject canvas05;
	public GameObject cameraShow;

	public InputField userName;
	public InputField passWord;
	//	AsyncOperation prog;
	public UserDataS dataS;
	public UserData data;
	public DataOfUser dataofuser;

	public static MapSelect mapSelect;

	public static bool randomMode;

	GameObject [] x;

	public GameObject victory;
	public GameObject lose;
	public CountPanel[] count;
	public GameObject[] countDetailPanels;
	public Text totalKill_Red;
	public Text totalKill_Blue;
	public int totalK_Red;
	public int totalK_Blue;

	void Awake()
	{

		ui = transform.GetComponent<HeroChooseUIManager> ();

		randomMode = false;

		audioSource = transform.GetComponent<AudioSource> ();
		tips = GameObject.Find ("Canvas01/Tips");
		tips2 = GameObject.Find ("Canvas02/Tips");

		mapChoice = GameObject.Find("Canvas02/1V1and3V3");

		userName = GameObject.Find ("Canvas01/Username/Name/InputField").GetComponent<InputField> ();
		passWord = GameObject.Find ("Canvas01/Password/Password/InputField").GetComponent<InputField> ();
		music = GameObject.Find ("Canvas01/Music");
		cameraShow = GameObject.Find ("Camera");

		canvas01 = GameObject.Find ("Canvas01");
		canvas02 = GameObject.Find ("Canvas02");
		canvas03 = GameObject.Find ("Canvas");
		canvas04 = GameObject.Find ("Canvas04");
		canvas05 = GameObject.Find ("LoadScene");

		if (scencePass.IsGameOver) {
			count = new CountPanel[6];
			countDetailPanels = new GameObject[6];
			for (int i = 0; i < 6; i++) {
				int j = i + 1;
				count [i].heroHeads = GameObject.Find ("Canvas04/Tongji/" + j + "/Head").GetComponent<Image> ();
				count [i].heroName = GameObject.Find ("Canvas04/Tongji/" + j + "/ID").GetComponent<Text> ();
				count [i].heroKills = GameObject.Find ("Canvas04/Tongji/" + j + "/TotalDamage").GetComponent<Text> ();
				count [i].heroDeath = GameObject.Find ("Canvas04/Tongji/" + j + "/GetDamage").GetComponent<Text> ();
				count [i].heroGets = GameObject.Find ("Canvas04/Tongji/" + j + "/Kill").GetComponent<Text> ();
				countDetailPanels [i] = GameObject.Find ("Canvas04/Tongji/" + j);
			}
			victory = GameObject.Find ("Canvas04/Panel/Victory");
			lose = GameObject.Find ("Canvas04/Panel/Lose");
			totalKill_Blue = GameObject.Find ("Canvas04/Tongji/LeftKill/Text").GetComponent<Text>();
			totalKill_Red = GameObject.Find ("Canvas04/Tongji/RightKill/Text").GetComponent<Text>();

			StartCoroutine ("SetCountPanel");
			canvas04.SetActive (true);
			canvas01.SetActive (false);
			canvas02.SetActive (false);
			canvas03.SetActive (false);
			canvas05.SetActive (false);
			scencePass.IsGameOver = false;
		}
	}

	void Start () {
		tips.SetActive (false);
		tips2.SetActive (false);
		mapChoice.SetActive (false);

		canvas02.SetActive (false);
		canvas03.SetActive (false);
		cameraShow.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		//		if (prog.progress >= 0.9f) {
		//			prog.allowSceneActivation = true;
		//		}
		//		
	}

	public void LoginGame()
	{
		string path  = Application.dataPath + "/InitializeInfo/";
		path += "Login.text";
		UserDataS userdata =  JsonUti.JsonstreamToObject<UserDataS> (path);
		bool canLod = false;
		Debug.Log ("登陆时，所填写:" + userName.text + passWord.text);
		for (int i = 0; i < userdata.userdatas.Count; i++) {
			if (userName.text == userdata.userdatas [i].username) {
				if (passWord.text == userdata.userdatas [i].password) {
					canLod = true;
					break;
				} else {
					//密码错误
					tips.SetActive (true);
					Debug.Log ("密码错误");
					break;
				}
			} else {
				continue;
			}
		}	
		if(!canLod) {
			//用户不存在，请注册
			tips.transform.Find("Text").GetComponent<Text> ().text = "用户不存在\n请注册";
			tips.SetActive(true);
		}
		if (canLod) {
			string pathUser = Application.dataPath + "/InitializeInfo/DataOfUser/";
			pathUser =pathUser + userName.text + ".text";
			Debug.Log ("登录的用户"+ userName.text+pathUser );
			DataOfUser dataOfUser = JsonUti.JsonstreamToObject<DataOfUser> (pathUser);
			Debug.Log ("登录的用户"+dataOfUser.username );
			string curLoadPlayer = Application.dataPath + "/InitializeInfo/CurLoadPlayer.text";
			JsonUti.ObjectToJsonStream<DataOfUser> (curLoadPlayer,dataOfUser);
			Debug.Log ("登录的用户"+ dataOfUser.Level);


			Debug.Log("进入游戏");
			//			StartCoroutine (LoadScene ());	//异步加载场景
			canvas03.SetActive(false);
			canvas01.SetActive (false);
			canvas02.SetActive (true);
			cameraShow.SetActive (false);
			Debug.Log (dataOfUser.Money);
			ClearInput ();
			//			Debug.Log (dataOfUser.OwnHero);
		}
		ClearInput ();

	}

	public void ZhuceGame()
	{
		string path  = Application.dataPath + "/InitializeInfo/";
		path += "Login.text";
		UserDataS userdata =  JsonUti.JsonstreamToObject<UserDataS> (path);
		bool isContain = false;	
		for (int i = 0; i < userdata.userdatas.Count; i++) {
			if (userName.text == userdata.userdatas[i].username) {
				isContain = true;
				break;
			} 
		}
		if (!isContain) {
			data.username = userName.text;
			data.password = passWord.text;
			dataS.userdatas.Add (data);
			string path2 = Application.dataPath + "/InitializeInfo/Login.text";
			dataofuser.username = userName.text;
			string pathUser = Application.dataPath + "/InitializeInfo/DataOfUser/";
			pathUser += data.username + ".text";
			JsonUti.ObjectToJsonStream<UserDataS> (path2, dataS);
			JsonUti.ObjectToJsonStream<DataOfUser> (pathUser, dataofuser);
		} else {
			tips.transform.Find("Text").GetComponent<Text> ().text = "用户名已\n存在";
			tips.SetActive(true);
			Debug.Log ("用户名已存在");
		}
		ClearInput ();

	}

	//协同加载场景
	//	IEnumerator LoadScene()
	//	{
	//		prog = SceneManager.LoadSceneAsync (1);
	//		prog.allowSceneActivation = false;
	//		yield return new WaitForSeconds (3f);
	//		prog.allowSceneActivation = true;
	//		yield break;
	//	}

	public void Mute()
	{
		if (!music.transform.GetComponent<Toggle> ().isOn) {
			audioSource.Pause ();
		} else {
			audioSource.Play ();
		}
	}

	public void QuitTips()
	{
		tips.SetActive (false);
		ClearInput ();
	}

	public void ClearInput()
	{
		passWord.text = null;
		userName.text = null;
		Debug.Log("qing kong :"+passWord.text  + userName.text );
	}

	public void SelectOnlineMode()
	{
		tips2.SetActive (true);
	}
	public void QuitTips2()
	{
		tips2.SetActive (false);
	}

	public void SelectRandomMode()
	{
		mapSelect = MapSelect.threeVSthree;
		randomMode = true;
	}

	public void SelectSoloMode()
	{
		mapChoice.SetActive (true);
	}

	public void Select_1V1()
	{
		canvas03.SetActive(true);
		mapChoice.SetActive (false);
		canvas02.SetActive (false);
		cameraShow.SetActive (true);
		mapSelect = MapSelect.oneVSone;

		if (Login.mapSelect == MapSelect.oneVSone) {
			x = new GameObject[4];
			x [0] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/X1");
			x [1] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/X2");
			x [2] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/X1");
			x [3] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/X2");
			for (int i = 0; i < x.Length; i++) {
				x [i].SetActive (true);
			}
		}
	}

	public void Select_3V3()
	{
		canvas03.SetActive(true);
		mapChoice.SetActive (false);
		canvas02.SetActive (false);
		cameraShow.SetActive (true);
		mapSelect = MapSelect.threeVSthree;
		if (Login.mapSelect == MapSelect.threeVSthree) {
			x = new GameObject[4];
			x [0] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/X1");
			x [1] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/X2");
			x [2] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/X1");
			x [3] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/X2");
			for (int i = 0; i < x.Length; i++) {
				x [i].SetActive (false);
			}
		}
	}

	public void QuitMapChoice()
	{
		mapChoice.SetActive (false);
	}

	public void Back()
	{
		canvas01.SetActive (true);
		canvas02.SetActive (false);
	}
	public void Back2()
	{
		canvas02.SetActive (true);
		canvas03.SetActive(false);
		cameraShow.SetActive (false);
		ui.ClearAllSet ();
	}

	public void GoBack(){
		canvas02.SetActive (true);
		canvas04.SetActive (false);
	}

	IEnumerator SetCountPanel(){
		if (scencePass.camp == Role_Camp.Blue) {
			if (scencePass.blueWin)
				lose.SetActive (false);
			else
				victory.SetActive (false);
			if (scencePass.mapSelect == MapSelect.threeVSthree) {
				for (int i = 0; i < 6; i++) {
					if (i <= 2) {
						if (i == 0)
							count [i].heroName.text = "玩家";
						else
							count [i].heroName.text = "友方AI";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Blue [i].role);
						count [i].heroKills.text = "" + scencePass.role_Blue [i].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Blue [i].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Blue [i].soliderKillCount;
						totalK_Blue += scencePass.role_Blue [i].killCount;
					} 

					else if (i > 2) {
						count [i].heroName.text = "敌方AI";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Red [i-3].role);
						count [i].heroKills.text = "" + scencePass.role_Red [i-3].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Red [i-3].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Red [i-3].soliderKillCount;
						totalK_Red += scencePass.role_Red [i-3].killCount;
					}
				}
			}
			else if (scencePass.mapSelect == MapSelect.oneVSone) {
				for (int i = 0; i < 6; i++) {
					if (i == 0) {
						count [i].heroName.text = "玩家";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Blue [0].role);
						count [i].heroKills.text = "" + scencePass.role_Blue [0].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Blue [0].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Blue [0].soliderKillCount;
						totalK_Blue += scencePass.role_Blue [0].killCount;
					} else if (i == 3) {
						count [i].heroName.text = "敌方AI";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Red [0].role);
						count [i].heroKills.text = "" + scencePass.role_Red [0].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Red [0].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Red [0].soliderKillCount;
						totalK_Red += scencePass.role_Red [0].killCount;
					}
					else {
						countDetailPanels [i].SetActive (false);
					}
				}
			}
		} 
		else if (scencePass.camp == Role_Camp.Red) {
			if (scencePass.blueWin)
				victory.SetActive (false);
			else
				lose.SetActive (false);
			if (scencePass.mapSelect == MapSelect.threeVSthree) {
				for (int i = 0; i < 6; i++) {
					if (i <= 2) {
						count [i].heroName.text = "敌方AI";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Blue [i].role);
						count [i].heroKills.text = "" + scencePass.role_Blue [i].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Blue [i].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Blue [i].soliderKillCount;
						totalK_Blue += scencePass.role_Blue [i].killCount;
					} 

					else if (i > 2) {
						if (i == 3)
							count [i].heroName.text = "玩家";
						else
							count [i].heroName.text = "友方AI";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Red [i-3].role);
						count [i].heroKills.text = "" + scencePass.role_Red [i-3].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Red [i-3].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Red [i-3].soliderKillCount;
						totalK_Red += scencePass.role_Red [i-3].killCount;
					}
				}
			}
			else if (scencePass.mapSelect == MapSelect.oneVSone) {
				for (int i = 0; i < 6; i++) {
					if (i == 0) {
						Debug.Log ((int)scencePass.role_Blue [0].role);
						count [i].heroName.text = "敌方AI";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Blue [0].role);
						count [i].heroKills.text = "" + scencePass.role_Blue [0].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Blue [0].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Blue [0].soliderKillCount;
						totalK_Blue += scencePass.role_Blue [0].killCount;
					} else if (i == 3) {
						Debug.Log ((int)scencePass.role_Red [0].role);
						count [i].heroName.text = "玩家";
						count [i].heroHeads.sprite = Resources.Load<Sprite> ("UI/hero_head/" + (int)scencePass.role_Red [0].role);
						count [i].heroKills.text = "" + scencePass.role_Red [0].killCount;
						count [i].heroDeath.text = "" + scencePass.role_Red [0].deathCount;
						count [i].heroGets.text = "" + scencePass.role_Red [0].soliderKillCount;
						totalK_Red += scencePass.role_Red [0].killCount;
					}
					else {
						countDetailPanels [i].SetActive (false);
					}
				}
			}
		} 
		totalKill_Blue.text = "" + totalK_Blue;
		totalKill_Red.text = "" + totalK_Red;

		yield return null;
	}
}

[System .Serializable]
public struct UserDataS
{
	public List<UserData> userdatas;
}

[System .Serializable ]
public struct UserData
{
	public string username;
	public string password;	
}


[System .Serializable ]
public struct DataOfUser
{
	public string username;
	public int Level;
	public int Money;
	public List<int> OwnHero;
}

public struct CountPanel{
	public Image heroHeads;
	public Text  heroName;
	public Text  heroKills;
	public Text  heroDeath;
	public Text  heroGets;
}