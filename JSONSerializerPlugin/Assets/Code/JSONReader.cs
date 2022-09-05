/*
 * Programmer: Hunter Goodin 
 * Date Created: 09/05/2022 @ 3:00 PM
 * Description: Reads a JSON file provided in editor 
 */

using UnityEngine;

public class JSONReader : MonoBehaviour
{
	[Header("Files")] 
	[SerializeField] TextAsset jsonFile;
	MyJSON myJSON;

	[Header("Variables")] 
	[SerializeField] bool myBool;
	[SerializeField] int myInt;
	[SerializeField] float myFloat; 
	[SerializeField] string myStr;

	[SerializeField] bool[] myBoolArr;
	[SerializeField] int[] myIntArr;
	[SerializeField] float[] myFloatArr;
	[SerializeField] string[] myStrArr;

	void Start()
	{
		ParseJSON(); 
	}

	void ParseJSON()
	{
		Debug.Log($"Parsing {jsonFile.name}.json\n{jsonFile.text}");

		myJSON = JsonUtility.FromJson<MyJSON>(jsonFile.text); 

		myBool = myJSON.myBool; 
		myInt = myJSON.myInt; 
		myFloat = myJSON.myFloat; 
		myStr = myJSON.myStr; 

		myBoolArr = myJSON.myBoolArr;
		myIntArr = myJSON.myIntArr;
		myFloatArr = myJSON.myFloatArr;
		myStrArr = myJSON.myStrArr;
	}
}

//[System.Serializable]
//public class MyJSON
//{
//	public bool myBool;
//	public int myInt;
//	public float myFloat;
//	public string myStr;

//	public bool[] myBoolArr;
//	public int[] myIntArr;
//	public float[] myFloatArr;
//	public string[] myStrArr;
//}