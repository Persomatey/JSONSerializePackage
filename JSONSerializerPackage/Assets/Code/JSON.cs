/*
 * Programmer: Hunter Goodin 
 * Date Created: 09/11/2022 @ 1:00 PM
 * Description: This class holds the data of the json object and controls the serializing of that object. 
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace JSON
{
	[Serializable]
	public class JSON
	{
		List<JSONBoolean> boolList;
		List<JSONInteger> intList;
		List<JSONFloat> floatList;
		List<JSONString> stringList;

		TextAsset jsonFile = null;

		public JSON()
		{
			boolList = new List<JSONBoolean>();
			intList = new List<JSONInteger>();
			floatList = new List<JSONFloat>();
			stringList = new List<JSONString>();
		}

		public JSON(TextAsset jsonFile)
		{
			boolList = new List<JSONBoolean>();
			intList = new List<JSONInteger>();
			floatList = new List<JSONFloat>();
			stringList = new List<JSONString>();
			this.jsonFile = jsonFile;

			ParseJSON(jsonFile);
		}

		public override string ToString()
		{
			List<string> list = new List<string>();

			// Print bools 
			for (int i = 0; i < boolList.Count; i++)
			{
				list.Add($"    \"{boolList[i].name}\": {boolList[i].value.ToString().ToLower()}");
			}

			// Print ints 
			for (int i = 0; i < intList.Count; i++)
			{
				list.Add($"    \"{intList[i].name}\": {intList[i].value}");
			}

			// Print floats 
			for (int i = 0; i < floatList.Count; i++)
			{
				list.Add($"    \"{floatList[i].name}\": {floatList[i].value}");
			}

			// Print strings 
			for (int i = 0; i < stringList.Count; i++)
			{
				list.Add($"    \"{stringList[i].name}\": \"{stringList[i].value}\"");
			}

			// Make string 
			string str = "{\n";

			for (int i = 0; i < list.Count; i++)
			{
				str += (i == list.Count - 1) ? list[i] + "\n" : list[i] + ",\n";
			}

			str += "}";

			return str;
		}

		void ParseJSON(TextAsset jsonFile)
		{
			boolList = new List<JSONBoolean>();
			intList = new List<JSONInteger>();
			floatList = new List<JSONFloat>();
			stringList = new List<JSONString>();
			this.jsonFile = jsonFile;

			string[] lines = jsonFile.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
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

		public void WriteToFile()
		{
			if (jsonFile == null)
			{
				Debug.LogError("ERROR: JSON TextAsset file never set!"); 
				return; 
			}

			File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), ToString());
			EditorUtility.SetDirty(jsonFile);
		}

		public void WriteToFile(TextAsset jsonFile)
		{
			if (jsonFile != null)
			{
				Debug.LogError($"ERROR: Object already has a JSON TextAsset file: '{jsonFile.name}.json'"); 
				return;
			}

			File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), ToString());
			EditorUtility.SetDirty(jsonFile);
		}

		#region Get Funcitons

		public bool GetBool(string variableName)
		{
			bool found = false;
			bool ret = false;

			for (int i = 0; i < boolList.Count; i++)
			{
				if (boolList[i].name == variableName)
				{
					found = true;
					ret = boolList[i].value;
				}
			}

			if (found)
			{
				return ret;
			}
			else
			{
				Debug.LogError($"ERROR: No variable named {variableName} found! returning 'false' instead");
				return false;
			}
		}

		public int GetInt(string variableName)
		{
			bool found = false;
			int ret = 0;

			for (int i = 0; i < intList.Count; i++)
			{
				if (intList[i].name == variableName)
				{
					found = true;
					ret = intList[i].value;
				}
			}

			if (found)
			{
				return ret;
			}
			else
			{
				Debug.LogError($"ERROR: No variable named {variableName} found! returning '0' instead");
				return 0;
			}
		}

		public float GetFloat(string variableName)
		{
			bool found = false;
			float ret = 0.0f;

			for (int i = 0; i < floatList.Count; i++)
			{
				if (floatList[i].name == variableName)
				{
					found = true;
					ret = floatList[i].value;
				}
			}

			if (found)
			{
				return ret;
			}
			else
			{
				Debug.LogError($"ERROR: No variable named '{variableName}' found! returning '0.0f' instead");
				return 0.0f;
			}
		}

		public string GetString(string variableName)
		{
			bool found = false;
			string ret = null;

			for (int i = 0; i < stringList.Count; i++)
			{
				if (stringList[i].name == variableName)
				{
					found = true;
					ret = stringList[i].value;
				}
			}

			if (found)
			{
				return ret;
			}
			else
			{
				Debug.LogError($"ERROR: No variable named '{variableName}' found! returning NULL instead");
				return null;
			}
		}

		#endregion Get Functions 

		#region Set Funcitons

		public void SetBool(string variableName, bool newBool)
		{
			bool found = false;

			for (int i = 0; i < boolList.Count; i++)
			{
				if (boolList[i].name == variableName)
				{
					found = true;
					boolList[i].value = newBool;
				}
			}

			if (!found)
			{
				Debug.LogError($"ERROR: No variable named {variableName} found!");
			}
		}

		public void SetInt(string variableName, int newInt)
		{
			bool found = false;

			for (int i = 0; i < intList.Count; i++)
			{
				if (intList[i].name == variableName)
				{
					found = true;
					intList[i].value = newInt;
				}
			}

			if (!found)
			{
				Debug.LogError($"ERROR: No variable named {variableName} found!");
			}
		}

		public void SetFloat(string variableName, float newFloat)
		{
			bool found = false;

			for (int i = 0; i < floatList.Count; i++)
			{
				if (floatList[i].name == variableName)
				{
					found = true;
					floatList[i].value = newFloat;
				}
			}

			if (!found)
			{
				Debug.LogError($"ERROR: No variable named {variableName} found!");
			}
		}

		public void SetString(string variableName, string newString)
		{
			bool found = false;

			for (int i = 0; i < stringList.Count; i++)
			{
				if (stringList[i].name == variableName)
				{
					found = true;
					stringList[i].value = newString;
				}
			}

			if (!found)
			{
				Debug.LogError($"ERROR: No variable named {variableName} found!");
			}
		}

		#endregion Get Functions 

		#region Add Functions 

		public void AddBool(string newItemName, bool newItemValue)
		{
			for (int i = 0; i < boolList.Count; i++)
			{
				if (boolList[i].name == newItemName)
				{
					Debug.LogError($"ERROR: There is already a boolean named {newItemValue} in {jsonFile.name}.json!");
					return; 
				}
			}

			boolList.Add(new JSONBoolean(newItemName, newItemValue));
		}

		public void AddInt(string newItemName, int newItemValue)
		{
			for (int i = 0; i < intList.Count; i++)
			{
				if (intList[i].name == newItemName)
				{
					Debug.LogError($"ERROR: There is already a integer named {newItemValue} in {jsonFile.name}.json!");
					return;
				}
			}

			intList.Add(new JSONInteger(newItemName, newItemValue));
		}

		public void AddFloat(string newItemName, float newItemValue)
		{
			for (int i = 0; i < floatList.Count; i++)
			{
				if (floatList[i].name == newItemName)
				{
					Debug.LogError($"ERROR: There is already a float named {newItemValue} in {jsonFile.name}.json!");
					return;
				}
			}

			floatList.Add(new JSONFloat(newItemName, newItemValue));
		}

		public void AddString(string newItemName, string newItemValue)
		{
			for (int i = 0; i < stringList.Count; i++)
			{
				if (stringList[i].name == newItemName)
				{
					Debug.LogError($"ERROR: There is already a string named {newItemValue} in {jsonFile.name}.json!");
					return;
				}
			}

			stringList.Add(new JSONString(newItemName, newItemValue));
		}

		#endregion Add Functions 
		
		#region Remove Functions 

		public void RemoveBool(string variableName)
		{
			for (int i = 0; i < boolList.Count; i++)
			{
				if (boolList[i].name == variableName)
				{
					boolList.Remove(boolList[i]);
					return; 
				}
			}

			Debug.LogError($"ERROR: No variable named {variableName} found!");
		}

		public void RemoveInt(string variableName)
		{
			for (int i = 0; i < intList.Count; i++)
			{
				if (intList[i].name == variableName)
				{
					intList.Remove(intList[i]);
					return;
				}
			}

			Debug.LogError($"ERROR: No variable named {variableName} found!");
		}

		public void RemoveFloat(string variableName)
		{
			for (int i = 0; i < floatList.Count; i++)
			{
				if (floatList[i].name == variableName)
				{
					floatList.Remove(floatList[i]);
					return;
				}
			}

			Debug.LogError($"ERROR: No variable named {variableName} found!");
		}

		public void RemoveString(string variableName)
		{
			for (int i = 0; i < stringList.Count; i++)
			{
				if (stringList[i].name == variableName)
				{
					stringList.Remove(stringList[i]);
					return;
				}
			}

			Debug.LogError($"ERROR: No variable named {variableName} found!");
		}

		#endregion Remove Functions 
	}

	#region JSON Variables

	[Serializable]
	public class JSONBoolean
	{
		public string name;
		public bool value;

		public JSONBoolean(string name, bool value)
		{
			this.name = name;
			this.value = value;
		}
	}

	[Serializable]
	public class JSONInteger
	{
		public string name;
		public int value;

		public JSONInteger(string name, int value)
		{
			this.name = name;
			this.value = value;
		}
	}

	[Serializable]
	public class JSONFloat
	{
		public string name;
		public float value;

		public JSONFloat(string name, float value)
		{
			this.name = name;
			this.value = value;
		}
	}

	[Serializable]
	public class JSONString
	{
		public string name;
		public string value;

		public JSONString(string name, string value)
		{
			this.name = name;
			this.value = value;
		}
	}

	#endregion JSON Variables 
}