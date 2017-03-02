using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .UI ;


/// <summary>
/// Hp show:显示挂载这个脚本的hp值
/// </summary>
public class HpShow : MonoBehaviour {
	public RoleInfo info;
	private Vector3 hpScreenPos;
	public GameObject hpPrefabs;
	public Transform hpTrans;  //hp在世界坐标中的位置
	public GameObject hpInstance;
	public Transform hpParent;
	public Slider HpSilider;
	public Type_Allrole type;
	public bool canShow = true;

	// Use this for initialization
	void Start () {
		type = transform.GetComponent <RoleInfo> ().type_Allrole;
		info = GetComponent <RoleInfo> ();
		hpTrans = transform.Find ("HpPos");
		hpParent=GameObject .Find ("UI/HpParent").transform;
		hpPrefabs = Resources.Load<GameObject> ("UI/Prefabs/Hp");
		hpInstance = Instantiate (hpPrefabs, hpTrans.position, Quaternion.identity)as GameObject;
		HpSilider = hpInstance.GetComponent <Slider> ();
		HpSilider.maxValue = info.HpMax;
		HpSilider.value = info.Hp;
	}
	
	// Update is called once per frame
	void Update () {
		if (hpInstance !=null ) {
			HpSilider.value = info.Hp;
			hpInstance.transform.position = Camera.main.WorldToScreenPoint (hpTrans.position);
			hpInstance.transform.SetParent (hpParent, true);
			if (HpSilider .value <=0.5) {
				if (type == Type_Allrole.role) {
					canShow = false;
				} else {
					Destroy (hpInstance);
				}
			}
			if (canShow) {
				hpInstance.SetActive (true);
			} else {
				hpInstance.SetActive (false);
			}
		}
	}
}
