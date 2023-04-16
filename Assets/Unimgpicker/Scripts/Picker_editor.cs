#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Kakera
{
	internal class Picker_editor : IPicker
	{
		public void Show(string title, string outputFileName)
		{
			var path = EditorUtility.OpenFilePanel(title, "","png");
			if (path.Length != 0) {
				string destination = Application.persistentDataPath + "/" + outputFileName;
				if (File.Exists(destination))
					File.Delete(destination);
				File.Copy(path, destination);
				Debug.Log ("PickerOSX:" + destination);
				//var receiver = GameObject.Find("Unimgpicker");
				var receiver = GameObject.FindObjectOfType<Unimgpicker>();
				Debug.Log (receiver);
				if (receiver != null)
				{
					Debug.Log("Send noti on complete");
					receiver.SendMessage("OnComplete", Application.persistentDataPath + "/" + outputFileName);
				}
			}
		}
	}
}
#endif