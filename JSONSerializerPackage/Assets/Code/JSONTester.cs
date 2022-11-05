/*
 * Programmer: Hunter Goodin 
 * Date Created: 09/11/2022 @ 3:30 PM 
 * Description: An example script showing how you can use this package to read from and write to provided .json files.m 
 */

using UnityEngine;
using UnityEngine.UI;

namespace JSON 
{
	public class JSONTester : MonoBehaviour
	{
		[Header("Files")]
		[SerializeField] TextAsset readFile;
		[SerializeField] TextAsset writeFile;

		[Header("Debug Stuff")]
		public string[] debugLines; 

		private void Start()
		{
			//JSON tempJSON = new PersistentJSON(readFile);
			//tempJSON.Test();
			//tempJSON.Test(true); 
		}

		public void ReadExample()
		{
			JSON readJSON = new JSON(readFile);

			debugLines = readJSON.debugLines;

			Debug.Log($"Data read from {readFile.name}.json\n" + readJSON.ToString());
		}

		public void WriteExample()
		{
			JSON writeJSON = new JSON();

			writeJSON.AddBool("myBool", true);
			writeJSON.AddInt("myInt", 987);
			writeJSON.AddFloat("myFloat", 654.321f);
			writeJSON.AddString("myStr", "What's up, World?");

			writeJSON.SetBool("myBool", false);

			writeJSON.WriteToFile(writeFile);
		}

		public void LabelFiller(TextAsset file)
		{
			GameObject.Find("JSONReadout").GetComponent<Text>().text = $"{file.text}";
		}
	}
}