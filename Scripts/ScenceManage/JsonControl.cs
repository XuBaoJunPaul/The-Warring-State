using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System .IO ;
using LitJson;
using System .Text;

public class JsonControl: MonoBehaviour {
	private static JsonControl _instance;
	public static JsonControl Instance{
		get{return _instance;}
	}

	public RoleInfo role;

	// Use this for initialization
	void Start () {
		_instance = this;
	}

	public void ObjectToJsonStream<T>(string path,T o){
		using (FileStream fWrite = new FileStream (path, FileMode.Create, FileAccess.Write)) {
			string infoTemp = JsonMapper.ToJson (o);
			Debug.Log ("111111111");
			byte[] buffer = Encoding.Default.GetBytes (infoTemp);
			fWrite.Write (buffer, 0, buffer.Length);
		}
	}
	public T JsonstreamToObject<T>(string path){
		using (FileStream fRead = new FileStream (path, FileMode.Open, FileAccess.Read)) {
			byte[] buffer = new byte[1024 * 1024];
			StringBuilder sb = new StringBuilder ();
			while (fRead .Read (buffer ,0,buffer .Length )>0) {
				sb.Append (Encoding.Default.GetString (buffer, 0, buffer.Length));
			}
			string sbTemp = sb.ToString ();
			T resoult = JsonMapper.ToObject<T> (sbTemp);
			return resoult;
		}
	}
}
