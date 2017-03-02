using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

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

	void Awake()
	{

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
			canvas01.SetActive (false);
			canvas02.SetActive (false);
			canvas03.SetActive (false);
			canvas04.SetActive (true);
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
		string path  = Application.dataPath + "/Resources/InitializeInfo/";
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
			string pathUser = Application.dataPath + "/Resources/InitializeInfo/DataOfUser/";
			pathUser =pathUser + userName.text + ".text";
			Debug.Log ("登录的用户"+ userName.text+pathUser );
			DataOfUser dataOfUser = JsonUti.JsonstreamToObject<DataOfUser> (pathUser);
			Debug.Log ("登录的用户"+dataOfUser.username );
			string curLoadPlayer = Application.dataPath + "/Resources/InitializeInfo/CurLoadPlayer.text";
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
		string path  = Application.dataPath + "/Resources/InitializeInfo/";
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
			string path2 = Application.dataPath + "/Resources/InitializeInfo/Login.text";
			dataofuser.username = userName.text;
			string pathUser = Application.dataPath + "/Resources/InitializeInfo/DataOfUser/";
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