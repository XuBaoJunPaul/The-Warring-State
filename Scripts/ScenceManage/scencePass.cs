using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scencePass : MonoBehaviour {
	public static bool IsGameOver;
	public static bool redWin;
	public static bool blueWin;
	public static Record_Role [] role_Red;
	public static Record_Role [] role_Blue;
	public static Role_Camp camp;
	public static MapSelect mapSelect;
	// Use this for initialization
	void Start () {
		IsGameOver = false;
		GameObject.DontDestroyOnLoad (this .gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (IsGameOver) {
			redWin = GameCount.Instance.redWin;
			blueWin = GameCount.Instance.blueWin;
			role_Red = GameCount.Instance.roles_Red;
			role_Blue = GameCount.Instance.roles_Blue;
			camp = GameCount.Instance.camp;
			mapSelect = UIManager.instance.mapSelect;
		}
	}
}
