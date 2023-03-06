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
		protected List<JSONBoolean> boolList;
		protected List<JSONInteger> intList;
		protected List<JSONFloat> floatList;
		protected List<JSONString> stringList;

		protected List<JSONBooleanArray> boolArrList; 
		//protected List<JSONIntegerArray> intArrList; 
		//protected List<JSONFloatArray> floatArrList; 
		//protected List<JSONStringArray> stringArrList;

		protected TextAsset jsonFile = null;

		public string[] debugLines; 

		public JSON()
		{
			boolList = new List<JSONBoolean>();
			intList = new List<JSONInteger>();
			floatList = new List<JSONFloat>();
			stringList = new List<JSONString>();

			boolArrList = new List<JSONBooleanArray>(); 
		}

		public JSON(TextAsset jsonFile)
		{
			boolList = new List<JSONBoolean>();
			intList = new List<JSONInteger>();
			floatList = new List<JSONFloat>();
			stringList = new List<JSONString>();

			boolArrList = new List<JSONBooleanArray>();

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

			Debug.Log("boolArrList.Count = " + boolArrList.Count); 

			// Print bool arrays 
			for (int i = 0; i < boolArrList.Count; i++)
			{
				Debug.Log("Printing " + boolArrList[i].name); 
				list.Add($"    \"{boolArrList[i].name}\": {boolArrList[i].value}"); 
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

		protected virtual void ParseJSON(TextAsset jsonFile)
		{
			boolList = new List<JSONBoolean>();
			intList = new List<JSONInteger>();
			floatList = new List<JSONFloat>();
			stringList = new List<JSONString>();
			this.jsonFile = jsonFile;
			string jsonText = jsonFile.text; 

			// Check if the json is a one line json. If so, format. 
			if (lineCount(jsonText) == 1) 
			{
				jsonText = formatString(jsonText); 
			}

			string[] lines = jsonText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			lines[lines.Length - 2] = (lines[lines.Length - 2].EndsWith(",")) ? lines[lines.Length - 2] : lines[lines.Length - 2] + ",";

			debugLines = lines; 

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

				// If it's an integer 
				if (int.TryParse(lineSplit[1], out int num))
				{
					AddInt(lineSplit[0], num);
				}

				// If it's a floating point 
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

				// If it's an array 
				if (lineSplit[1].Contains("["))
				{
					// Check if it's a one line array -- if so, format it 
					if (lineSplit[1].Contains("]"))
					{
						lineSplit[1] = formatArray(lineSplit[1]);
					}

					// Separate out values of array 
					string[] arrSplit = lineSplit[1].Split(new string[] { "\n" }, StringSplitOptions.None);
					string[] arr = new string[arrSplit.Length - 2];

					for (int j = 0; j < arr.Length; j++)
					{
						arr[j] = arrSplit[j + 1];
						arr[j] = arr[j].Replace(",", ""); 
						arr[j] = arr[j].Replace(" ", "");
					}

					// If it's an array of booleans 
					if (arr[0] == "true" || arrSplit[1] == "false")
					{
						bool[] boolArr = new bool[arr.Length]; 
						for (int j = 0; j < arr.Length; j++)
						{
							boolArr[j] = Convert.ToBoolean( arr[j] );
						}

						AddBoolArray(lineSplit[0], boolArr); 
					}
				}
			}
		}

		void ReadOutArray(string[] arr)
		{
			string str = ""; 
			for (int i = 0; i < arr.Length; i++)
			{
				str += arr[i] + "\n"; 
			}
			Debug.Log(str); 
		}

		int lineCount(string str)
		{
			int ret = 1;

			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '\n')
				{
					ret++;
				}
			}

			return ret; 
		} 

		string formatString(string str)
		{
			string ret = "";
			bool midQuote = false; 

			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '"')
				{
					midQuote = !midQuote; 
				}
				if (str[i] == '{')
				{
					ret += str[i] + "\n"; 
				}
				else if (str[i] == '}')
				{
					ret += "\n" + str[i];
				}
				else if (str[i] == ',' && !midQuote)
				{
					ret += str[i] + "\n";
				}
				else if (str[i] == '[')
				{
					ret += str[i] + "\n"; 
				}
				else if (str[i] == ']')
				{
					ret += "\n" + str[i]; 
				}
				else
				{
					ret += str[i];
				}
			}

			ret = ret.Replace("\n ", "\n");

			return ret; 
		}

		string formatArray(string str)
		{
			string ret = "";

			// Loop through it 
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '[') 
				{
					ret += str[i] + "\n"; 
				}
				else if (str[i] == ']')
				{
					ret += "\n" + str[i]; 
				}
				else if (str[i] == ',')
				{
					ret += str[i] + "\n"; 
				}
				else
				{
					ret += str[i]; 
				}
			}

			ret = ret.Replace("\n ", "\n");

			return ret;
		}

		public virtual void WriteToFile()
		{
			if (jsonFile == null)
			{
				Debug.LogError("ERROR: JSON TextAsset file never set!"); 
				return; 
			}

			File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), ToString());
			EditorUtility.SetDirty(jsonFile);
		}

		public virtual void WriteToFile(TextAsset jsonFile)
		{
			if (this.jsonFile != null && this.jsonFile != jsonFile)
			{
				Debug.LogError($"ERROR: Object already has a JSON TextAsset file: '{this.jsonFile.name}.json'"); 
				return;
			}

			File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), ToString());
			EditorUtility.SetDirty(this.jsonFile);
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

		public void AddBoolArray(string newItemName, bool[] newItemValue)
		{
			for (int i = 0; i < boolList.Count; i++)
			{
				if (boolList[i].name == newItemName)
				{
					Debug.LogError($"ERROR: There is already a array named {newItemValue} in {jsonFile.name}.json!");
					return; 
				}
			}

			boolArrList.Add(new JSONBooleanArray(newItemName, newItemValue));
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

	// Arrays 

	[Serializable]
	public class JSONBooleanArray
	{
		public string name;
		public bool[] value;

		public JSONBooleanArray(string name, bool[] value)
		{
			this.name = name;
			this.value = value;
		}
	}

	#endregion JSON Variables 
}