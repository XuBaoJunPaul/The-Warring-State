using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public Ray ray;
	public Transform cam_pos;
	public Transform player;
	public Role_Main player_RoleMain;
	public RaycastHit hit;
	public LayerMask layerScence;
	public LayerMask layerPlayer;
	public float camMoveSpeed;
	public UIManager uimanager;
	// Use this for initialization
	void Start () {
		uimanager = GameObject.Find ("UI").GetComponent<UIManager> ();
		if (uimanager.mapSelect==MapSelect.oneVSone) {
			player = Scence1v1_Intialize.Instance.role_Player.transform ;
			player_RoleMain = Scence1v1_Intialize.Instance.role_Player.GetComponent<Role_Main> ();
		}
		if (uimanager.mapSelect==MapSelect.threeVSthree) {
			player = Scence3v3_Intialize.Instance.role_Players [0].transform;
			player_RoleMain = Scence3v3_Intialize.Instance.role_Players [0].GetComponent <Role_Main> ();
		}
		cam_pos = Camera.main.transform;
		Vector3 camTemp2 =player.position ;
		camTemp2.y +=10f;
		camTemp2.z -= 10f;
		camTemp2.x -= 4f;
		cam_pos.position = camTemp2;	

		layerScence = LayerMask.GetMask ("Scence");
		layerPlayer = player_RoleMain.EnemyLayer;
	}
	
	// Update is called once per frame
	void Update () {
		CameraMoveControl (Input.mousePosition, camMoveSpeed);
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if ( Physics.Raycast (ray, out hit, 2000f,layerScence)) {
			Debug.DrawLine (cam_pos.position,hit.point ,Color .red );
			if (Input.GetMouseButtonDown (1)) {
				player_RoleMain.SetMoveTarget (hit.point);        //角色移动调用
				player_RoleMain.target =null ;
				player_RoleMain.CanAtkNormal = false;

			}
		}
		if ( Physics.Raycast (ray, out hit, 200f, layerPlayer)) {
			Debug.DrawLine (cam_pos.position,hit.point ,Color .blue);
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log (""+hit.transform.gameObject.name);
				player_RoleMain .SetTarget(hit.transform);

			}
		}
		Vector3 temp =hit.point-player .position; 
		temp.y = 0f;
		player_RoleMain.skillLookDir = temp;
	} 
	void OnGUI(){
		//GUILayout.Label ("鼠标位置：" + Input.mousePosition);
	}

	/// <summary>
	/// Cameras the move control.
	/// </summary>
	/// 按空格键则将摄像机初始化在角色上
	/// 鼠标中键可以控制视图的缩放；
	/// 在屏幕边缘则会向相应的方向移动
	/// by 牧
	/// <param name="mousePose">Mouse pose.</param>
	/// <param name="speed">Speed.</param>
	void CameraMoveControl(Vector3 mousePose,float speed){    
		if (Input.GetKey (KeyCode.Space )) {
			Vector3 camTemp1 =player.position ;
			camTemp1.y = cam_pos.position.y;
			camTemp1.z -= 10f;
			camTemp1.x -= 4f;
			cam_pos.position = camTemp1;	 
		}

		if (Input .GetAxis ("Mouse ScrollWheel")!=0f) {
			Vector3 camTemp2 = cam_pos.position;
			camTemp2 .y +=(Input.GetAxis ("Mouse ScrollWheel"))*2f;
			camTemp2.y = Mathf.Clamp (camTemp2.y, player.position.y + 2f, player.position.y + 15f);
			cam_pos.position = camTemp2;
		}


		Vector3 camTemp = cam_pos.position;
		if (mousePose.x -Screen.width >= -3 ) {
			camTemp.x = Mathf.Lerp (camTemp.x, camTemp.x + speed, Time.deltaTime);
		}
		if (mousePose .y -Screen .height >=-3) {
			camTemp.z = Mathf.Lerp (camTemp.z , camTemp.z  +speed, Time.deltaTime);
		}
		if (mousePose .x <=3) {
			camTemp.x = Mathf.Lerp (camTemp.x, camTemp.x - speed, Time.deltaTime);
		}
		if (mousePose .y <=3) {
			camTemp.z = Mathf.Lerp (camTemp.z , camTemp.z -speed, Time.deltaTime); 
		}
		//camTemp.x = Mathf.Clamp (camTemp.x, 44f, 110f);
		cam_pos.position = camTemp;
	}
}
