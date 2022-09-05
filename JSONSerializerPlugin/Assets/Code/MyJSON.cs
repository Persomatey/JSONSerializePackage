/*
 * Programmer: Hunter Goodin 
 * Date Created: 09/05/2022 @ 3:00 PM
 * Description: This class is the JSON object that gets written 
 *				It can also be included inside another class' file 
 *				(An example is commented out of JSONReader.cs)
 */

// This class can also be written inside another class 
// An example is commented out of JSONReader.cs  

[System.Serializable]
public class MyJSON
{
	public bool myBool;
	public int myInt;
	public float myFloat;
	public string myStr;

	public bool[] myBoolArr;
	public int[] myIntArr;
	public float[] myFloatArr;
	public string[] myStrArr;

	public MyJSON (bool myBool, int myInt, float myFloat, string myStr, bool[] myBoolArr, int[] myIntArr, float[] myFloatArr, string[] myStrArr)
	{
		this.myBool = myBool; 
		this.myInt = myInt; 
		this.myFloat = myFloat; 
		this.myStr = myStr;

		this.myBoolArr = myBoolArr;
		this.myIntArr = myIntArr;
		this.myFloatArr = myFloatArr;
		this.myStrArr = myStrArr;
	}
}
