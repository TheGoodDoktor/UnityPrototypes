using UnityEngine;
using System.Collections;

public class SentenceGenTest : MonoBehaviour 
{
	public int Seed = 0;
	public string InputString;
	public TextAsset[] ConfigFiles;

	private SentenceGen	_sentenceGenerator = new SentenceGen();
	private string _displayString;
	
	// Use this for initialization
	void Start () 
	{
	
		foreach(TextAsset configFile in ConfigFiles)
		{
			_sentenceGenerator.LoadAndParseTokenData(configFile.text);
		}

		_displayString = _sentenceGenerator.GenerateString(InputString, Seed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() 
	{
		GUI.Label(new Rect(10, 50, 600, 200), _displayString);

		if (GUI.Button(new Rect(10, 10, 50, 30), "Regen"))
		{
			_sentenceGenerator.Reset();
			_displayString = _sentenceGenerator.GenerateString(InputString);
		}
	}
}
