/*
 * Programmer: Hunter Goodin 
 * Date Created: 09/19/2022 @ 10:00 PM
 * Description: This class is for JSON objects that will persist on disk to the persistentDataPath even if the app is deleted. 
 */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JSON
{
    public class PersistentJSON : JSON
    {
		public PersistentJSON() : base() { } 
		
		public PersistentJSON(TextAsset jsonFile) : base(jsonFile) { } 

		/// <summary> Test </summary> 
		public override void WriteToFile()
		{
			if (this.jsonFile == null)
			{
				Debug.LogError("ERROR: JSON TextAsset file never set!");
				return;
			}

			File.WriteAllText(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json"), ToString());
			Debug.Log(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json")); 
		}

		/// <summary> If the TextAsset was given, them also write to the TextAsset </summary> 
		public override void WriteToFile(TextAsset jsonFile)
		{
			if (this.jsonFile != null && this.jsonFile != jsonFile)
			{
				Debug.LogError($"ERROR: Object already has a JSON TextAsset file: '{this.jsonFile.name}.json'");
				return;
			}

			base.WriteToFile(this.jsonFile);

			File.WriteAllText(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json"), ToString());
			Debug.Log(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json"));
		}

		protected override void ParseJSON(TextAsset jsonFile)
		{
			// If there is a persistent data path, read from that instead 
			if (File.Exists(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json"))) 
			{
				File.ReadAllText(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json"));

				boolList = new List<JSONBoolean>();
				intList = new List<JSONInteger>();
				floatList = new List<JSONFloat>();
				stringList = new List<JSONString>();
				this.jsonFile = jsonFile;

				string[] lines = File.ReadAllText(Path.Combine(Application.persistentDataPath, $"{jsonFile.name}.json")).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
				lines[lines.Length - 2] = (lines[lines.Length - 2].EndsWith(",")) ? lines[lines.Length - 2] : lines[lines.Length - 2] + ",";

				for (int i = 1; i < lines.Length - 1; i++)
				{
					string[] lineSplit = lines[i].Split(new string[] { ":" }, StringSplitOptions.None);
					lineSplit[0] = lineSplit[0].Replace("    \"", "");
					lineSplit[0] = lineSplit[0].Replace("\t", "");
					lineSplit[0] = lineSplit[0].Replace("\"", "");
					lineSplit[1] = lineSplit[1].Substring(1, lineSplit[1].Length - 2);

					// If it's a boolean
					if (lineSplit[1] == "true" || lineSplit[1] == "false")
					{
						AddBool(lineSplit[0], Convert.ToBoolean(lineSplit[1]));
					}

					// If it's an int 
					if (int.TryParse(lineSplit[1], out int num))
					{
						AddInt(lineSplit[0], num);
					}

					// If it's a float 
					if (lineSplit[1].Contains(".") && float.TryParse(lineSplit[1], out float fl))
					{
						AddFloat(lineSplit[0], fl);
					}

					// If it's a string 
					if (lineSplit[1].StartsWith("\""))
					{
						lineSplit[1] = lineSplit[1].Replace("\"", "");
						AddString(lineSplit[0], lineSplit[1]);
					}
				}
			}
			else
			{
				base.ParseJSON(jsonFile);
			}
		}
	}
}