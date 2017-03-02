using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
	public OperatePanel op;
	public GameCount gc;
	public GameObject instance;
	public bool isMoving;

	public Transform handControl;
	public Vector3 handStartPos;
	public float handMoveRange;
//	public GameObject player;
	public float moveSpeed;
	public static Vector3 moveVector;

	public Role_Main player;
	public float hp;

	public Text expData;
	public Text hpData;
	public Text mpData;
	public Text pDamData;
	public Text mDamData;
	public Text pDefData;
	public Text mDefData;
	public Text msData;
	public Text asData;
	public Image head;
	public float detailPanel_Level;
	public Image tex_Skill_Q;
	public Image tex_Skill_W;
	public Image tex_Skill_E;
	public Image tex_Skill_D;
	public Image tex_Skill_F;

	public Image tex_Item1;
	public Image tex_Item2;
	public Image tex_Item3;
	public Image tex_Item4;
	public Image tex_Item5;
	public Image tex_Item6;

	public GameObject detailNormalButton;
	public GameObject detailClickedButton;
	public GameObject detailPanel;
	public GameObject buyPanel;
	public GameObject setPanel;

	public Button button_DetailButton;
	public Button button_BuyButton;
	public Button button_SettingButton;

	Button [] sellButton;

	Text gold;

	public MapSelect mapSelect;

	void Awake(){
		handControl = transform.Find ("OperatePanel/HandControlBack/HandControl");
		op = transform.Find ("OperatePanel").GetComponent<OperatePanel> ();
		gc = GameObject.Find ("ScriptsManager").GetComponent<GameCount> ();

		sellButton = new Button[6];
		for (int i = 0; i < 6; i++) {
			int l = i + 1;
			sellButton[i] = GameObject.Find("UI/DataPanel/DetailPanel/Item/Slot" + l +"/Button").GetComponent<Button>();
		}
		HideAllSellButton();

		gold = transform.Find ("DataPanel/Gold/Text").GetComponent<Text> ();
	}
	// Use this for initialization
	void Start () {
		handMoveRange = 60;
		moveSpeed = 0.01f;
		isMoving = false;

		if (mapSelect == MapSelect.oneVSone) {
			player = Scence1v1_Intialize.Instance.role_Player.GetComponent <Role_Main> ();
		} else {
			player = Scence3v3_Intialize.Instance.role_Players[0].GetComponent <Role_Main> ();
		}
		instance = this.gameObject;
		handStartPos = handControl.position;

		expData = transform.Find ("DataPanel/DetailPanel/EXP/Text").GetComponent<Text>();
		hpData = transform.Find ("DataPanel/DetailPanel/HP/Text").GetComponent<Text>();
		mpData = transform.Find ("DataPanel/DetailPanel/MP/Text").GetComponent<Text>();
		pDamData = transform.Find ("DataPanel/DetailPanel/PDam/Text").GetComponent<Text>();
		mDamData = transform.Find ("DataPanel/DetailPanel/MDam/Text").GetComponent<Text>();
		pDefData = transform.Find ("DataPanel/DetailPanel/PDef/Text").GetComponent<Text>();
		mDefData = transform.Find ("DataPanel/DetailPanel/MDef/Text").GetComponent<Text>();
		msData = transform.Find ("DataPanel/DetailPanel/MS/Text").GetComponent<Text>();
		asData = transform.Find ("DataPanel/DetailPanel/AS/Text").GetComponent<Text>();
		head = transform.Find ("DataPanel/DetailPanel/Head/Head").GetComponent<Image>();
		tex_Skill_Q = transform.Find ("OperatePanel/Skill_Q").GetComponent<Image>();
		tex_Skill_W = transform.Find ("OperatePanel/Skill_W").GetComponent<Image>();
		tex_Skill_E = transform.Find ("OperatePanel/Skill_E").GetComponent<Image>();
		tex_Skill_D = transform.Find ("OperatePanel/Skill_D").GetComponent<Image>();
		tex_Skill_F = transform.Find ("OperatePanel/Skill_F").GetComponent<Image>();

		detailNormalButton = transform.Find ("DataPanel/DetailButton/Normal").gameObject;
		detailClickedButton = transform.Find ("DataPanel/DetailButton/Clicked").gameObject;
		detailPanel = transform.Find("DataPanel/DetailPanel").gameObject;
		buyPanel = transform.Find ("DataPanel/BuyPanel").gameObject;
		setPanel = transform.Find ("DataPanel/SetPanel").gameObject;

		button_DetailButton = GameObject.Find ("UI/DataPanel/DetailButton").GetComponent<Button>();
		EventTriggerListener.Get(button_DetailButton.gameObject).onClick= OnDetailButtonClick;
		button_BuyButton = GameObject.Find ("UI/DataPanel/BuyButton").GetComponent<Button>();
		EventTriggerListener.Get(button_BuyButton.gameObject).onClick= OnBuyBUttonClick;
		button_SettingButton = GameObject.Find ("UI/DataPanel/SettingButton").GetComponent<Button>();
		EventTriggerListener.Get(button_SettingButton.gameObject).onClick= OnSetButtonClick;

		tex_Item1 = transform.Find ("DataPanel/DetailPanel/Item/Slot1").GetComponent<Image> ();
		tex_Item2 = transform.Find ("DataPanel/DetailPanel/Item/Slot2").GetComponent<Image> ();
		tex_Item3 = transform.Find ("DataPanel/DetailPanel/Item/Slot3").GetComponent<Image> ();
		tex_Item4 = transform.Find ("DataPanel/DetailPanel/Item/Slot4").GetComponent<Image> ();
		tex_Item5 = transform.Find ("DataPanel/DetailPanel/Item/Slot5").GetComponent<Image> ();
		tex_Item6 = transform.Find ("DataPanel/DetailPanel/Item/Slot6").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		gold.text = "" + player.money;
		MoveControl ();
		if (player != null) {
			expData.text = player.Levl_exp + "/" + player.Level * 10;
			hpData.text = player.Hp + "/" + player.HpMax;
			mpData.text = player.Mp + "/" + player.MpMax;
			pDamData.text = player.attack_Physical + "";
			mDamData.text = player.attack_Magic + "";
			pDefData.text = player.DefensePhysical + "";
			mDefData.text = player.DefenseMagic + "";
			msData.text = player.moveSpeed + "";
			asData.text = player.attack_Speed + "";
			head.sprite = player.roleTex;
			detailPanel_Level = player.Level;

			tex_Skill_Q.sprite = player.skill_Q.tex;
			tex_Skill_W.sprite = player.skill_W.tex;
			tex_Skill_E.sprite = player.skill_E.tex;
			tex_Skill_D.sprite = player.skill_D.tex;
			tex_Skill_F.sprite = player.skill_F.tex;

			if (player.items.item1.Name != Enum_Item.None) {
				tex_Item1.sprite = player.items.item1.Tex;
			}
			if (player.items.item2.Name != Enum_Item.None) {
				tex_Item2.sprite = player.items.item2.Tex;
			}
			if (player.items.item3.Name != Enum_Item.None) {
				tex_Item3.sprite = player.items.item3.Tex;
			}
			if (player.items.item4.Name != Enum_Item.None) {
				tex_Item4.sprite = player.items.item4.Tex;
			}
			if (player.items.item5.Name != Enum_Item.None) {
				tex_Item5.sprite = player.items.item5.Tex;
			}
			if (player.items.item6.Name != Enum_Item.None) {
				tex_Item6.sprite = player.items.item6.Tex;
			}
		}
		if (op.moneyChange != 0) {
			player.money += op.moneyChange;
			op.moneyChange = 0;
		}
	}

	void MoveControl(){
		if (Input.GetMouseButton (0)) {
			if (Vector3.Distance (Input.mousePosition, handStartPos) < handMoveRange) {
				isMoving = true;
				handControl.transform.position = Input.mousePosition;
			} else if (isMoving) {
				handControl.transform.position = (Input.mousePosition - handStartPos) * handMoveRange / (Vector3.Distance (Input.mousePosition, handStartPos)) + handStartPos;
			}
			moveVector = handControl.position - handStartPos;
			moveVector = new Vector3 (moveVector.x * moveSpeed , 0 , moveVector.y * moveSpeed);
			//			player.transform.Translate(moveVector);
		} else if (Input.GetMouseButtonUp (0)) {
			handControl.transform.position = handStartPos;
			isMoving = false;
		}//MoveControl on phone
	}

	void OnDetailButtonClick(GameObject go){
		if (go == button_DetailButton.gameObject) {
			if (!op.detailPanelOpen) {
				HideAllPanel ();
				detailClickedButton.SetActive (true);
				detailNormalButton.SetActive (false);
				op.gameObject.SetActive (false);
				detailPanel.SetActive (true);
				op.detailPanelOpen = true;
			} 
			else if (op.detailPanelOpen) 
			{
				detailClickedButton.SetActive (false);
				detailNormalButton.SetActive (true);
				op.gameObject.SetActive (true);
				detailPanel.SetActive (false);
				op.detailPanelOpen = false;
			}
		}
	}

	void OnBuyBUttonClick(GameObject go){
		if (go == button_BuyButton.gameObject) {
			if (!op.buyPanelOpen) {
				HideAllPanel ();
				buyPanel.SetActive (true);
				op.buyPanelOpen = true;
			} 
			else if (op.buyPanelOpen) 
			{
				buyPanel.SetActive (false);
				op.buyPanelOpen = false;
			}
		}
	}

	void OnSetButtonClick(GameObject go){
		if (go == button_SettingButton.gameObject) {
			if (!op.setPanelOpen) {
				HideAllPanel ();
				setPanel.SetActive (true);
				op.setPanelOpen = true;
			} 
			else if (op.setPanelOpen) {
				setPanel.SetActive (false);
				op.setPanelOpen = false;
			}
		}
	}

	public void SellItemStep1(int code){
		HideAllSellButton ();
		sellButton [code].gameObject.SetActive (true);
	}

	public void SellItemStep2(int code){
		op.items [code].gameObject.SetActive (false);
		op.haveItemOrNot [code] = false;
		int code2 = Convert.ToInt32(op.items [code].sprite.name) -1;
		player.money += Items.items [code2].Gold;
		sellButton [code].gameObject.SetActive (false);
		op.itemCount--;
		HideAllSellButton ();
		OffItemState (code2);
	}

	void HideAllSellButton(){
		for (int i = 0; i < sellButton.Length; i++) {
			sellButton [i].gameObject.SetActive (false);
		}
	}

	public void HideAllPanel(){
		detailPanel.SetActive (false);
		buyPanel.SetActive (false);
		setPanel.SetActive (false);
		op.detailPanelOpen = false;
		op.buyPanelOpen = false;
		op.setPanelOpen = false;
		detailClickedButton.SetActive (false);
		detailNormalButton.SetActive (true);
	}

	public void AddItemState(int code){
		player.attack_Physical += Items.items [code].PDam;
		player.attack_Magic += Items.items [code].MDam;
		player.HpMax += Items.items [code].HP;
		player.Hp += Items.items [code].HP;
		player.MpMax += Items.items [code].MP;
		player.Mp += Items.items [code].MP;
		player.DefensePhysical += Items.items [code].PDef;
		player.DefenseMagic += Items.items [code].MDef;
		player.attack_Speed *= Items.items [code].AS;
		player.moveSpeed *= Items.items [code].MS;
	}

	public void OffItemState(int code){
		player.attack_Physical -= Items.items [code].PDam;
		player.attack_Magic -= Items.items [code].MDam;
		player.HpMax -= Items.items [code].HP;
		player.Hp -= Items.items [code].HP;
		player.MpMax -= Items.items [code].MP;
		player.Mp -= Items.items [code].MP;
		player.DefensePhysical -= Items.items [code].PDef;
		player.DefenseMagic -= Items.items [code].MDef;
		player.attack_Speed /= Items.items [code].AS;
		player.moveSpeed /= Items.items [code].MS;
	}

	public void OnContinueClick(){
		HideAllPanel ();
	}

	public void OnExitClick(){
		HideAllPanel ();
		if (gc.camp == Role_Camp.Blue) {
			GameObject.Find ("shuiJingBlueRoot/shuiJingBlue").GetComponent<TowerBaseInfo> ().Hp = 0;
		}
		if (gc.camp == Role_Camp.Red) {
			GameObject.Find ("shuiJingRedRoot/shuiJingRed").GetComponent<TowerBaseInfo> ().Hp = 0;
		}
	}
}

public enum MapSelect
{
	oneVSone,
	threeVSthree
}