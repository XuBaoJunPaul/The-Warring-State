using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OperatePanel : MonoBehaviour {
	public bool detailPanelOpen;
	public bool buyPanelOpen;
	public bool setPanelOpen;
	public PanelOpen panelState;

	public delegate void OnSkillButtonDelegate (SkillButtonName name);
	public delegate void OnPublicSkillButtonDelegate (SkillButtonName name);
	public delegate void OnAttackButtonDelegate ();
	public delegate void OnAddSkillButtonDelegate (SkillButtonName name);  

	public static OnSkillButtonDelegate skillButtonDelegate;
	public static  OnPublicSkillButtonDelegate publicSkillButtonDelegate;
	public static  OnAttackButtonDelegate attackButtonDelegate;
	public static  OnAddSkillButtonDelegate addSkillButtonDelegate;     //在SkillManger中添加； 需要商量

	Button button_SkillQ;
	Button button_SkillW;
	Button button_SkillE;
	Button button_SkillD;
	Button button_SkillF;
	Button button_Attack;
	Button button_AddSkillQ;
	Button button_AddSkillW;
	Button button_AddSkillE;

	public Image [] items;
	public bool[] haveItemOrNot;

	public int moneyChange;

	bool firstClick;

	public float doubleClickTimeCount;
	float doubleClickLimitTime;

	public int itemCount;

	public Text gold;

	UIManager ui;

	AudioClip buySound;

	public Camera cam;

	void Awake(){
		ui = transform.parent.GetComponent<UIManager> ();
		cam = Camera.main;
	}
	void Start () 
	{
		buySound = Resources.Load<AudioClip> ("Audios/UI/Buy");

		itemCount = 0;
		doubleClickTimeCount = 0;
		doubleClickLimitTime = 0.5f;
		firstClick = false;
		moneyChange = 0;
		detailPanelOpen = false;
		buyPanelOpen = false;
		setPanelOpen = false;
		panelState = PanelOpen.None;

		gold = GameObject.Find ("UI/DataPanel/Gold/Text").GetComponent<Text> ();

		//把EventTriggerListener添加给所有的按钮，并且绑定这个函数的委托；
		button_SkillQ = transform.Find("Skill_Q").GetComponent<Button>();
		EventTriggerListener.Get(button_SkillQ.gameObject).onClick =OnSkillButtonClick;
		button_SkillW = transform.Find("Skill_W").GetComponent<Button>();
		EventTriggerListener.Get(button_SkillW.gameObject).onClick =OnSkillButtonClick;
		button_SkillE = transform.Find("Skill_E").GetComponent<Button>();
		EventTriggerListener.Get(button_SkillE.gameObject).onClick =OnSkillButtonClick;
		button_SkillD = transform.Find("Skill_D").GetComponent<Button>();
		EventTriggerListener.Get(button_SkillD.gameObject).onClick =OnSkillButtonClick;
		button_SkillF = transform.Find("Skill_F").GetComponent<Button>();
		EventTriggerListener.Get(button_SkillF.gameObject).onClick =OnSkillButtonClick;
		//skill
		button_Attack = transform.Find("Attack").GetComponent<Button>();
		EventTriggerListener.Get(button_Attack.gameObject).onClick =OnAttackButtonClick;
		//attack
		button_AddSkillQ = transform.Find("Skill_Q/Add_Skill_Q").GetComponent<Button>();
		EventTriggerListener.Get(button_AddSkillQ.gameObject).onClick =OnAddSkillButtonClick;
		button_AddSkillW = transform.Find("Skill_W/Add_Skill_W").GetComponent<Button>();
		EventTriggerListener.Get(button_AddSkillW.gameObject).onClick =OnAddSkillButtonClick;
		button_AddSkillE = transform.Find("Skill_E/Add_Skill_E").GetComponent<Button>();
		EventTriggerListener.Get(button_AddSkillE.gameObject).onClick =OnAddSkillButtonClick;
		//add skill

		items = new Image[6];
		for (int i = 0; i < 6; i++) {
			int l = i + 1;
			items[i] = GameObject.Find("UI/DataPanel/DetailPanel/Item/Slot" + l +"/Image").GetComponent<Image>();
		}
		HideAllItemTex ();
		haveItemOrNot = new bool[6];
		for (int i = 0; i < 6; i++) {
			haveItemOrNot [i] = false;
		}

		hideAddSkillButton ();
	}

	void Update(){
		if (firstClick) {
			doubleClickTimeCount += Time.deltaTime;
		}
		if (doubleClickTimeCount > doubleClickLimitTime) {
			doubleClickTimeCount = 0;
			firstClick = false;
		}

	}

	private void OnSkillButtonClick(GameObject go){
		if(go == button_SkillQ.gameObject){
			if (skillButtonDelegate != null) {
				skillButtonDelegate (SkillButtonName.Button_Q);
			} else {
				Debug.Log ("Delegate unset!");
			}
			StartCoroutine ("CDCount","Q");
		}
		if(go == button_SkillW.gameObject){
			if (skillButtonDelegate != null) {
				skillButtonDelegate (SkillButtonName.Button_W);
			} else {
				Debug.Log ("Delegate unset!");
			}
			StartCoroutine ("CDCount","W");
		}
		if(go == button_SkillE.gameObject){
			if (skillButtonDelegate != null) {
				skillButtonDelegate (SkillButtonName.Button_E);
			} else {
				Debug.Log ("Delegate unset!");
			}
			StartCoroutine ("CDCount","E");
		}
	}

	private void OnPublicSkillButtonClick(GameObject go){
		if(go == button_SkillD.gameObject){
			if (publicSkillButtonDelegate != null) {
				publicSkillButtonDelegate (SkillButtonName.Button_D);
			} else {
				Debug.Log ("Delegate unset!");
			}
		}
		if(go == button_SkillF.gameObject){
			if (publicSkillButtonDelegate != null) {
				publicSkillButtonDelegate (SkillButtonName.Button_F);
			} else {
				Debug.Log ("Delegate unset!");
			}
		}
	}

	private void OnAttackButtonClick(GameObject go){
		if (go == button_Attack.gameObject) {
			if (attackButtonDelegate != null) {
				attackButtonDelegate ();
			} else {
				Debug.Log ("Delegate unset!");
			}
		}
	}

	private void OnAddSkillButtonClick(GameObject go){
		if (go == button_AddSkillQ.gameObject) {
			if (addSkillButtonDelegate != null) {
				addSkillButtonDelegate (SkillButtonName.Button_Q);
			} else {
				Debug.Log ("Delegate unset!");
			}
		}
		if (go == button_AddSkillW.gameObject) {
			if (addSkillButtonDelegate != null) {
				addSkillButtonDelegate (SkillButtonName.Button_W);
			} else {
				Debug.Log ("Delegate unset!");
			}
		}
		if (go == button_AddSkillE.gameObject) {
			if (addSkillButtonDelegate != null) {
				addSkillButtonDelegate (SkillButtonName.Button_E);
			} else {
				Debug.Log ("Delegate unset!");
			}
		}
	}

	void hideAddSkillButton(){
		button_AddSkillQ.gameObject.SetActive (false);
		button_AddSkillW.gameObject.SetActive (false);
		button_AddSkillE.gameObject.SetActive (false);
	}

	public void OnBuyItems (int code){
		if (doubleClickTimeCount <= doubleClickLimitTime && firstClick && itemCount<6 && Convert.ToInt32(gold.text) >= Items.items[code-1].Gold) {
			AudioSource.PlayClipAtPoint (buySound,cam.transform.position);
			Sprite target = Resources.Load<Sprite> ("UI/Item_Texture/" + code);
			GetItemTex (target);
			moneyChange -= Items.items [code - 1].Gold;
			firstClick = false;
			doubleClickTimeCount = 0;
			itemCount++;
			ui.AddItemState (code - 1);
		}
		if (!firstClick) {
			firstClick = true;
		}
	}

	void GetItemTex(Sprite target){
		if (!haveItemOrNot[0]) {
			items [0].sprite = target;
			items [0].gameObject.SetActive (true);
			haveItemOrNot [0] = true;
		}
		else if (!haveItemOrNot[1]) {
			items [1].sprite = target;
			items [1].gameObject.SetActive (true);
			haveItemOrNot [1] = true;
		}
		else if (!haveItemOrNot[2]) {
			items [2].sprite = target;
			items [2].gameObject.SetActive (true);
			haveItemOrNot [2] = true;
		}
		else if (!haveItemOrNot[3]) {
			items [3].sprite = target;
			items [3].gameObject.SetActive (true);
			haveItemOrNot [3] = true;
		}
		else if (!haveItemOrNot[4]) {
			items [4].sprite = target;
			items [4].gameObject.SetActive (true);
			haveItemOrNot [4] = true;
		}
		else if (!haveItemOrNot[5]) {
			items [5].sprite = target;
			items [5].gameObject.SetActive (true);
			haveItemOrNot [5] = true;
		}
	}

	void HideAllItemTex(){
		for (int i = 0; i < 6; i++) {
			items [i].gameObject.SetActive (false);
		}
	}

	IEnumerator CDCount(string QWE){
		Image CD = transform.Find ("Skill_" + QWE + "/CD").GetComponent<Image> ();
		switch (QWE){
		case "Q":
			if (ui.player.skill_Q.level > 0) {
				float tmp = 0;
				while (tmp >= 0) {
					tmp = 1 - ui.player.skill_Q.timeCount / ui.player.skill_Q.CD;
					CD.fillAmount = tmp;
				}
			}
			break;

		case "W":
			if (ui.player.skill_W.level > 0) {
				float tmp2 = 0;
				while (tmp2 >= 0) {
					tmp2 = 1 - ui.player.skill_W.timeCount / ui.player.skill_W.CD;
					CD.fillAmount = tmp2;
				}
			}
			break;

		case "E":
			if (ui.player.skill_E.level > 0) {
				float tmp3 = 0;
				while (tmp3 >= 0) {
					tmp3 = 1 - ui.player.skill_E.timeCount / ui.player.skill_E.CD;
					CD.fillAmount = tmp3;
				}
			}
			break;
		}
		yield return null;
	}
}

public enum SkillButtonName{
	Button_Q,
	Button_W,
	Button_E,
	Button_D,
	Button_F,
}

public enum PanelOpen{
	None,
	DetailPanel,
	SetPanel,
	BuyPanel
}
