# JSON Serializer Package 
<i>A JSON serializer package for Unity which supports reading from and writing to JSON files. </i>

Check out [releases](https://github.com/Persomatey/JSONSerializePackage/releases) to download the unity package. 

<img src="https://raw.githubusercontent.com/Persomatey/JSONSerializerPlugin/main/Logo/JSONSerializerPluginLogo.png" width="318"/>

## Tutorial 

### Setup
Since everything in this plugin is wrapped in a JSON namespace, you'll need to add it at the top of your script. 
```
using JSON; 
```

Because .json files are read as TextAssets in Unity, you'll need a reference to the TextAsset in your script like so: 
```
[SerializeField] TextAsset file;
public TextAsset file;
```

### Creating a new JSON object from a file 
Call the constructor, using the TextAsset you want read in the parameters. 
```
JSON myJSON = new JSON(file);
```
There is also a constructor for if you don't have a parameter. 
```
JSON myJSON = new JSON();
```

### Serializing JSON object to a file 
You can serialize the JSON object to the provided TextAsset file. 
```
writeJSON.WriteToFile();
```
If no parameter was provided when object was created, you'll need to include the TextAsset in the parameters of `WriteToFile()`
```
writeJSON.WriteToFile(file);
```

### Reading and writing JSON variables  
To read a specific variable (`GetBool`, `GetInt`, `GetFloat`, `GetString`): 
```
myJSON.GetString("myStr"); 
```
To change a specific variable (`SetBool`, `SetInt`, `SetFloat`, `SetString`): 
```
myJSON.SetInt("myInt", 246); 
```
To add a variable to the JSON (`AddBool`, `AddInt`, `AddFloat`, `AddString`): 
```
myJSON.AddFloat("myFloat", 24.68); 
```

## Details 

<details>
<summary>Specs</summary>
<blockquote>
	
Unity 2020.3.32f1
- Windows: https://download.unity3d.com/download_unity/12f8b0834f07/UnityDownloadAssistant-2020.3.32f1.exe 
- Mac: https://download.unity3d.com/download_unity/12f8b0834f07/UnityDownloadAssistant-2020.3.32f1.dmg 
- Unity HUB: unityhub://2020.3.32f1/12f8b0834f07 

SLN solution in Visual Studio Community 2019 Preview 
https://visualstudio.microsoft.com/vs/community/
	
</blockquote>
</details> 
