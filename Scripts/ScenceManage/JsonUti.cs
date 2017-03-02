using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System .IO ;
using System .Text;

public class JsonUti{
	
//		转化为objcet的使用
//		Data_Rloe role=JsonstreamToObject<Data_Rloe>(Application.dataPath + "/Resources/InitializeInfo/Ahri.text");
//
//		转化为jsonText的使用
//		if (Input .GetKeyDown (KeyCode .P )) {     //保存数据时使用
//			string nameData;
//			nameData ="MoZi.text";
//			string path=Application.dataPath + "/Resources/InitializeInfo/"+nameData ; ;
//			ObjectToJsonStream <Data_Rloe> (path, role);
//		}
	static int count=0;
	public static void ObjectToJsonStream<T>(string path,T o){
		string jsonTemp = JsonUtility.ToJson (o);
		using (FileStream fWrite = new FileStream (path, FileMode.Create, FileAccess.Write)) {
			byte[] buffer = Encoding.Default.GetBytes (jsonTemp);
			fWrite.Write(buffer, 0, buffer.Length); 
		}
	}

	public static void LoginDotaTOJsonStream<T>(string path,T o)
	{
		string jsonTemp	 = JsonUtility.ToJson (o);
		using (FileStream fWrite = new FileStream (path, FileMode.Append, FileAccess.Write)) {
			byte[] buffer = Encoding.Default.GetBytes (jsonTemp);
			fWrite.Write (buffer, 0, buffer.Length);
		}
	}

	public static T JsonstreamToObject<T>(string path){
		Debug.Log ("开始读取文件"+(count++));
		using (FileStream fRead = new FileStream (path, FileMode.Open, FileAccess.Read)) {
			byte[] buffer = new byte[1024 * 1024];
			StringBuilder sb = new StringBuilder ();
			while (fRead .Read (buffer ,0,buffer .Length )>0) {
				sb.Append (Encoding.Default.GetString (buffer, 0, buffer.Length));
			}
			string sbTemp = sb.ToString ();
			T resoult = JsonUtility.FromJson<T> (sbTemp);
			//JsonUtility.FromJsonOverwrite (sbTemp, resoult);
			return resoult;
		}
	}
}


