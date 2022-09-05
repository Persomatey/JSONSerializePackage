/*
 * Programmer: Hunter Goodin 
 * Date Created: 09/05/2022 @ 3:00 PM
 * Description: Writes to a JSON file provided in editor 
 */

using System.IO;
using UnityEditor;
using UnityEngine;

public class JSONWriter : MonoBehaviour
{
	[Header("Files")]
	[SerializeField] TextAsset jsonFile;

	[Header("Variables")]
	[SerializeField] bool myBool;
	[SerializeField] int myInt;
	[SerializeField] float myFloat;
	[SerializeField] string myStr;

	[SerializeField] bool[] myBoolArr;
	[SerializeField] int[] myIntArr;
	[SerializeField] float[] myFloatArr;
	[SerializeField] string[] myStrArr;

	private void Start()
	{
		WriteToJSON(); 
	}

	void WriteToJSON()
	{
		Debug.Log($"Writing to {jsonFile.name}.json");

		MyJSON newJSON = new MyJSON(myBool, myInt, myFloat, myStr, myBoolArr, myIntArr, myFloatArr, myStrArr);
		string str = JsonUtility.ToJson(newJSON, true);

		File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), str);
		EditorUtility.SetDirty(jsonFile);
	}
}
