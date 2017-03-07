using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroChooseUIManager : MonoBehaviour {
	DFSkillPickedType type;
	public GameObject dfSkillPanel;
	public GameObject cameraShow;

	public bool heroCheck;
	public bool skill_D_Check;
	public bool skill_F_Check;

	GameObject checkButton;
	GameObject startGameButton;

	int heroChooseRound;

	public HeroChooseInfo [] heros;
	public HeroChooseInfoS heroS;

	GameObject [] heroChoose;
	Image [] heroHead;
	Image [] heroC;

	Sprite skill_DF_EmptyTex;
	Image skill_D_TexPos;
	Image skill_F_TexPos;

	Ray ray;
	RaycastHit hit;
	LayerMask mask;

	public AsyncOperation asy;
	public GameObject loadProgress;

	bool canLoadScene;
	bool canSaveData;

	GameObject canvas;
	GameObject load;
	GameObject jiazai;

	Role_Main player;

	Button skill_D_Button;
	Button Skill_F_Button;

	GameObject cam;

	GameObject canvas02;

	GameObject [] hero3d;
	GameObject herosFor3d;

	Image Skill_Q;
	Image Skill_W;
	Image Skill_E;

	void Awake(){
		Skill_Q = GameObject.Find ("Canvas/SkillInfoPanel/Skill_Q").GetComponent<Image> ();
		Skill_W = GameObject.Find ("Canvas/SkillInfoPanel/Skill_W").GetComponent<Image> ();
		Skill_E = GameObject.Find ("Canvas/SkillInfoPanel/Skill_E").GetComponent<Image> ();

		hero3d = new GameObject[6];
		herosFor3d = GameObject.Find("Heros/GameObject");

		for (int i = 0; i < hero3d.Length; i++) {
			hero3d [i] = herosFor3d.transform.GetChild (i).gameObject;
			hero3d [i].SetActive (false);
		}

		canvas02 = GameObject.Find ("Canvas02");

		cam = GameObject.Find ("Camera");

		canLoadScene = false;
		canSaveData = false;

		heroHead = new Image[6];
		heroChoose = new GameObject[6];
		heroC = new Image[6];
		heros = new HeroChooseInfo [6];

		heroCheck = false;
		skill_D_Check = false;
		skill_F_Check = false;
		heroChooseRound = 0;
		type = DFSkillPickedType.None;
		dfSkillPanel = GameObject.Find ("Canvas/SkillInfoPanel/DFSkillPanel");

		heroHead[0] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/HeroHead_Player/Image").GetComponent<Image> ();
		heroHead[1] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/HeroHead_AI_1/Image").GetComponent<Image> ();
		heroHead[2] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/HeroHead_AI_2/Image").GetComponent<Image> ();
		heroHead[3] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/HeroHead_EnemyAI_1/Image").GetComponent<Image> ();
		heroHead[4] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/HeroHead_EnemyAI_2/Image").GetComponent<Image> ();
		heroHead[5] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/HeroHead_EnemyAI_3/Image").GetComponent<Image> ();

		heroChoose [0] = GameObject.Find ("Canvas/HeroPanel/HeroChoose/Heros/1");
		heroChoose [1] = GameObject.Find ("Canvas/HeroPanel/HeroChoose/Heros/2");
		heroChoose [2] = GameObject.Find ("Canvas/HeroPanel/HeroChoose/Heros/3");
		heroChoose [3] = GameObject.Find ("Canvas/HeroPanel/HeroChoose/Heros/4");
		heroChoose [4] = GameObject.Find ("Canvas/HeroPanel/HeroChoose/Heros/5");
		heroChoose [5] = GameObject.Find ("Canvas/HeroPanel/HeroChoose/Heros/6");

		heroC [0] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/HeroHead_Player").GetComponent<Image> ();
		heroC [1] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/HeroHead_AI_1").GetComponent<Image> ();
		heroC [2] = GameObject.Find ("Canvas/HeroPanel/HeroHead_A/HeroHead_AI_2").GetComponent<Image> ();
		heroC [3] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/HeroHead_EnemyAI_1").GetComponent<Image> ();
		heroC [4] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/HeroHead_EnemyAI_2").GetComponent<Image> ();
		heroC [5] = GameObject.Find ("Canvas/HeroPanel/HeroHead_B/HeroHead_EnemyAI_3").GetComponent<Image> ();

		checkButton = GameObject.Find ("Canvas/CheckButton");
		startGameButton = GameObject.Find ("Canvas/StartGameButton");
		loadProgress = GameObject.Find ("LoadScene/Image");
		canvas = GameObject.Find ("Canvas");
		load = GameObject.Find ("LoadScene");
		jiazai = GameObject.Find ("LoadScene/BG");

		skill_DF_EmptyTex = Resources.Load<Sprite> ("UI/UI_button/move");

		skill_D_TexPos = GameObject.Find ("Canvas/SkillInfoPanel/Skill_D").GetComponent<Image>();
		skill_F_TexPos = GameObject.Find ("Canvas/SkillInfoPanel/Skill_F").GetComponent<Image>();

		skill_D_Button = GameObject.Find ("Canvas/SkillInfoPanel/Skill_D").GetComponent<Button> ();
		Skill_F_Button = GameObject.Find ("Canvas/SkillInfoPanel/Skill_F").GetComponent<Button>();
	}
	// Use this for initialization
	void Start () {
		cameraShow = GameObject.Find ("Camera");
		load.SetActive (false);
		startGameButton.SetActive (false);
		dfSkillPanel.SetActive (false);
		heroC [heroChooseRound].color = new Color (1, 0.5f, 0.5f, 1);
	}

	// Update is called once per frame
	void Update () {
		if (canLoadScene) {
			asy.allowSceneActivation = false;
			loadProgress.GetComponent<Slider>().value = asy.progress;
			if (asy.progress >= 0.9f) {
				StartCoroutine ("StartNextScene");
			}
		}

		if (Login.randomMode) {
			for (int i = 0; i < 6; i++) {
				heros [i].num = i;
				heros [i].hero = (Enum_Role)Random.Range(0,6);
				heros [i].Skill_D = (OtherSkill_Name)Random.Range (0, 6);
				heros [i].Skill_F = (OtherSkill_Name)Random.Range (0, 6);
			}
			Jason ();
			JiaZai ();
			canvas02.SetActive (false);
		}
	}

	public void OnDFPanelClick(int num){
		if (type == DFSkillPickedType.None) {
			dfSkillPanel.SetActive (true);
			type = (DFSkillPickedType)num;
		}
	}

	public void OnHeroChooseClick(int num){
		heros [heroChooseRound].num = num;
		heros [heroChooseRound].hero = (Enum_Role)num;
		heroCheck = true;
		Sprite heroSprite = Resources.Load<Sprite> ("UI/hero_head/" + num);
		heroHead[heroChooseRound].sprite = heroSprite;
		heroHead[heroChooseRound].enabled = true;
		ShowHero3D (num);
		Skill_Q.sprite = Resources.Load<Sprite>("UI/hero_Skill/" + num + "/Q");
		Skill_W.sprite = Resources.Load<Sprite>("UI/hero_Skill/" + num + "/W");
		Skill_E.sprite = Resources.Load<Sprite>("UI/hero_Skill/" + num + "/E");
	}

	public void OnDFSkillClick(int num){
		if (type == DFSkillPickedType.D){
			if (!skill_F_Check) {
				Sprite otherSkill = Resources.Load<Sprite> ("UI/Skills/OtherSkills/" + num);
				heros [heroChooseRound].Skill_D = (OtherSkill_Name)(num +1);
				skill_D_Check = true;
				skill_D_TexPos.sprite = otherSkill;
				dfSkillPanel.SetActive (false);
				type = DFSkillPickedType.None;
			} 
			else if (skill_F_Check && num != ((int)heros [heroChooseRound].Skill_F) -1)
			{
				Sprite otherSkill = Resources.Load<Sprite> ("UI/Skills/OtherSkills/" + num);
				heros [heroChooseRound].Skill_D = (OtherSkill_Name)(num +1);
				skill_D_Check = true;
				skill_D_TexPos.sprite = otherSkill;
				dfSkillPanel.SetActive (false);
				type = DFSkillPickedType.None;
			}
		} 
		else if (type == DFSkillPickedType.F) {
			if (!skill_D_Check) {
				Sprite otherSkill = Resources.Load<Sprite> ("UI/Skills/OtherSkills/" + num);
				heros [heroChooseRound].Skill_F = (OtherSkill_Name)(num +1);
				skill_F_Check = true;
				skill_F_TexPos.sprite = otherSkill;
				dfSkillPanel.SetActive (false);
				type = DFSkillPickedType.None;
			} 
			else if (skill_D_Check && num != ((int)heros [heroChooseRound].Skill_D)-1) 
			{
				Sprite otherSkill = Resources.Load<Sprite> ("UI/Skills/OtherSkills/" + num);
				heros [heroChooseRound].Skill_F = (OtherSkill_Name)(num +1);
				skill_F_Check = true;
				skill_F_TexPos.sprite = otherSkill;
				dfSkillPanel.SetActive (false);
				type = DFSkillPickedType.None;
			}
		}
	}

	public void OnEmptyClick(){
		if (type != DFSkillPickedType.None) {
			dfSkillPanel.SetActive (false);
			type = DFSkillPickedType.None;
		}
	}

	public void OnFinishClick(){
		if (Login.mapSelect == MapSelect.threeVSthree) {
			if (heroCheck && skill_D_Check && skill_F_Check) {
				heroChooseRound++;
				heroCheck = false;
				skill_D_Check = false;
				skill_F_Check = false;
				skill_D_TexPos.sprite = skill_DF_EmptyTex;
				skill_F_TexPos.sprite = skill_DF_EmptyTex;
				type = DFSkillPickedType.None;
				heroChoose [heros [heroChooseRound - 1].num].GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);
				heroChoose [heros [heroChooseRound - 1].num].GetComponent<Button> ().enabled = false;
				heroC [heroChooseRound - 1].color = new Color (1, 1, 1, 1);
			}
			if (heroChooseRound <= 5) {
				heroC [heroChooseRound].color = new Color (1, 0.5f, 0.5f, 1);
			}
			if (heroChooseRound == 3) {
				for (int i = 0; i < 6; i++) {
					heroChoose [i].GetComponent<Image> ().color = new Color (1, 1, 1, 1);
					heroChoose [i].GetComponent<Button> ().enabled = true;
				}
			}
			if (heroChooseRound > 5) {
				checkButton.SetActive (false);
				startGameButton.SetActive (true);
				skill_D_Button.enabled = false;
				Skill_F_Button.enabled = false;
			}
		}

		else if (Login.mapSelect == MapSelect.oneVSone) {
			if (heroCheck && skill_D_Check && skill_F_Check) {
				heroCheck = false;
				skill_D_Check = false;
				skill_F_Check = false;
				skill_D_TexPos.sprite = skill_DF_EmptyTex;
				skill_F_TexPos.sprite = skill_DF_EmptyTex;
				type = DFSkillPickedType.None;
				if (heroChooseRound == 0) {
					heroChooseRound += 3;
					heroC [0].color = new Color (1, 1, 1, 1);
					heroC [heroChooseRound].color = new Color (1, 0.5f, 0.5f, 1);
				}
				else if (heroChooseRound == 3) {
					checkButton.SetActive (false);
					startGameButton.SetActive (true);
					skill_D_Button.enabled = false;
					Skill_F_Button.enabled = false;
				}
			}
		}
	}

	public void StartGame(){
		Jason ();
		JiaZai ();
	}

	void Jason(){
		if (!canSaveData) {
			heroS.hero = this.heros;
			string path = Application.dataPath + "/InitializeInfo/HerosChooosed.text";
			JsonUti.ObjectToJsonStream <HeroChooseInfoS> (path, heroS);
			canSaveData = true;
		}
	}

	void JiaZai(){
		if (!canLoadScene) {
			jiazai.GetComponent<Image> ().sprite = Resources.Load <Sprite>("UI/jiazai/" + Random.Range (0, 8));
			canvas.SetActive (false);
			load.SetActive (true);
			StartCoroutine ("LoadScene");
			canLoadScene = true;
		}
	}

	IEnumerator LoadScene(){
		cam.SetActive (false);
		if (Login.mapSelect == MapSelect.oneVSone) 
			asy = SceneManager.LoadSceneAsync (1);
		else if (Login.mapSelect == MapSelect.threeVSthree) 
			asy = SceneManager.LoadSceneAsync (2);
		yield return null;
	}

	IEnumerator StartNextScene(){
		yield return new WaitForSeconds(2f);
		asy.allowSceneActivation = true;
		cameraShow.SetActive (false);
		yield return null;
	}

	void ShowHero3D(int code){
		for (int i = 0; i < hero3d.Length; i++) {
			if (i == code)
				hero3d [i].SetActive (true);
			else
				hero3d [i].SetActive (false);
		}
	}

	public void ClearAllSet(){
		heroChooseRound = 0;
		for (int i = 0; i < 6; i++) {
			heroHead [i].enabled = false;
			if (i == 0)
				heroC [i].color = new Color (1, 0.5f, 0.5f, 1);
			else
				heroC [i].color = new Color (1, 1, 1, 1);
		}
	}
}

public enum DFSkillPickedType{
	None,
	D,
	F
}

