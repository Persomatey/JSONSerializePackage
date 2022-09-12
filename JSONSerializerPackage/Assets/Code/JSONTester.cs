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

		public void ReadExample()
		{
			JSON readJSON = new JSON(readFile);

			Debug.Log($"Data read from {readFile.name}.json:\n" + readJSON.ToString());
			Debug.Log($"Read myStr: {readJSON.GetString("myStr")}"); 
		}

		public void WriteExample()
		{
			JSON writeJSON = new JSON();

			writeJSON.AddBool("myBool", false);
			writeJSON.AddInt("myInt", 987);
			writeJSON.AddFloat("myFloat", 654.321f);
			writeJSON.AddString("myStr", "What's up, World!");

			writeJSON.WriteToFile(writeFile);
		}

		public void LabelFiller(TextAsset file)
		{
			GameObject.Find("JSONReadout").GetComponent<Text>().text = $"{file.text}";
		}
	}
}