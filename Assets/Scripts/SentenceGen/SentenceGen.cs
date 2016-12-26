using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization;

// Token Expander Base Class
public abstract class TokenExpander
{
    protected string _token;

    public virtual bool InitFromXML(XmlNode node)
    {
        _token = node.Attributes.GetNamedItem("token").Value;
        return true;
    }

    public abstract string Expand(SentenceGen generator);
}

// Get a random variation for a token
public class RandomToken : TokenExpander
{
    private List<string> _variations = new List<string>();

    public override bool InitFromXML(XmlNode node)
    {
        if (base.InitFromXML(node) == false)
            return false;

        foreach (XmlNode childNode in node.ChildNodes)
        {
            if (childNode.Name == "variation")
            {
                _variations.Add(childNode.Attributes.GetNamedItem("text").Value);
            }
        }

        return true;
    }

    public override string Expand(SentenceGen generator)
    {
        int variationNo = Random.Range(0, _variations.Count);
        return _variations[variationNo];
    }
}

// This will pick a variation based on a value retrieved from a selector
public class FixedToken : TokenExpander
{
    private string _selector;
    private List<string> _variations = new List<string>();

    public override bool InitFromXML(XmlNode node)
    {
        if (base.InitFromXML(node) == false)
            return false;

        _selector = node.Attributes.GetNamedItem("selector").Value;

        foreach (XmlNode childNode in node.ChildNodes)
        {
            if (childNode.Name == "variation")
            {
                _variations.Add(childNode.Attributes.GetNamedItem("text").Value);
            }
        }

        return true;
    }

    public override string Expand(SentenceGen generator)
    {
        int variationNo = generator.GetRandomNumberForSelector(_selector) % _variations.Count;
        return _variations[variationNo];
    }
}

// This will pick a variation based on a value retrieved from a selector
public class RangeToken : TokenExpander
{
    private string _selector;
    private int _rangeMin, _rangeMax;

    public struct RangeVariation
    {
        public string text;
        public int min;
        public int max;
    };

    private List<RangeVariation> _variations = new List<RangeVariation>();

    public override bool InitFromXML(XmlNode node)
    {
        if (base.InitFromXML(node) == false)
            return false;

        _selector = node.Attributes.GetNamedItem("selector").Value;
        _rangeMin = int.MaxValue;
        _rangeMax = int.MinValue;

        foreach (XmlNode childNode in node.ChildNodes)
        {
            if (childNode.Name == "variation")
            {
                RangeVariation variation = new RangeVariation();
                variation.text = childNode.Attributes.GetNamedItem("text").Value;
                variation.min = int.Parse(childNode.Attributes.GetNamedItem("min").Value);
                variation.max = int.Parse(childNode.Attributes.GetNamedItem("max").Value);
                _variations.Add(variation);

                if (variation.min < _rangeMin)
                    _rangeMin = variation.min;
                if (variation.max > _rangeMax)
                    _rangeMax = variation.max;
            }
        }

        return true;
    }

    public override string Expand(SentenceGen generator)
    {
        int value = generator.GetNumberForSelector(_selector);
        if (value == -1)
        {
            value = Random.Range(_rangeMin, _rangeMax);
        }

        foreach (RangeVariation variation in _variations)
        {
            if (value >= variation.min && value <= variation.max)
                return variation.text;
        }

        return string.Format("[{0} rangeNotFound]",_token);
    }
}



// Sentence generator class
public class SentenceGen 
{	
	// Private members

    // Preset dictionary - this can be used to set preset values for token from an external system, 
    // this can be used for example by a character generator
    private Dictionary<string, string> _presetTokenDictionary = new Dictionary<string, string>();
    
    // Token expander dictionary
    // This contains the various token expanders which will procedurally expand a token based on its rules
    private Dictionary<string, TokenExpander> _tokenExpanderDictionary = new Dictionary<string, TokenExpander>();

    // Random dictionary
    // this is a persistant store of numbers which can be referred to many times to return the same value
	private Dictionary<string, int>	_numberDictionary = new Dictionary<string, int>();



	// recursive string expansion
	private string ExpandString(string inputString)
	{
		string outputString = "";
		string tokenString = null;

		// scan through string looking for token
		for(int i=0;i<inputString.Length;i++)
		{
			char inChar = inputString[i];

			if(inChar == '[')	// open token
			{
				tokenString = "";
			}
			else if(inChar == ']') // close token
			{
                if (_presetTokenDictionary.ContainsKey(tokenString))
                {
                    outputString += _presetTokenDictionary[tokenString];    // add preset
                }
                if (_tokenExpanderDictionary.ContainsKey(tokenString))// Expand token
				{
                    outputString += ExpandString(_tokenExpanderDictionary[tokenString].Expand(this));
				}
				else
					outputString += "[" + tokenString + "]";    // token not found so output as debug

				tokenString = null;
			}
			else if(tokenString != null)	// writing to token
			{
				tokenString += inChar;
			}
			else // writing to string
			{
				outputString += inChar;
			}
		}

		return outputString;
	}

    // Public Methods

    // returns a value for a given selector - generate random number if not found
    const int kTokenRandRange = 1000;
    public int GetRandomNumberForSelector(string token)
    {
        if (_numberDictionary.ContainsKey(token))
        {
            return _numberDictionary[token];
        }
        else
        {   // selector doesn't exist - generate one
            int randomNo = Random.Range(0, kTokenRandRange);
            _numberDictionary[token] = randomNo;
            return randomNo;
        }
    }

    // Return a number for a given selector from the number dictionary
    // If the number doesn't exist return the supplied default
    public int GetNumberForSelector(string token, int defaultVal = -1)
    {
        if (_numberDictionary.ContainsKey(token))
            return _numberDictionary[token];
        else
            return defaultVal;
    }

    public void SetNumberForSelector(string token, int value)
    {
        _numberDictionary[token] = value;
    }

    // set the preset
    public void SetPreset(string token, string value)
    {
        _presetTokenDictionary[token] = value;
    }

    // Load & parse XML data
	public void LoadAndParseTokenData(string tokenData)
	{
		XmlDocument XMLDocument = new XmlDocument();
		
		// Load Quiz XML File
		XMLDocument.LoadXml(tokenData);
		XmlNodeList tokenExpanders  = XMLDocument.GetElementsByTagName("tokenexpander");
		
		//_Questions = _XMLDocument.ChildNodes;
		Debug.Log(string.Format("Loaded {0} Token Expanders",tokenExpanders.Count));

		foreach(XmlNode node in tokenExpanders)
		{
			string nodeType = node.Attributes.GetNamedItem("type").Value;
			string nodeToken = node.Attributes.GetNamedItem("token").Value;
			TokenExpander expander = null;

			if(nodeType == "fixed")
				expander = new FixedToken();
			else if(nodeType == "random")
				expander = new RandomToken();
            else if (nodeType == "range")
                expander = new RangeToken();

			expander.InitFromXML(node);
            _tokenExpanderDictionary[nodeToken] = expander;
		}
	}

	public void Reset()
	{
		_numberDictionary.Clear();
	}

    // Generate the string
    public string GenerateString(string inString, int seed = 0)
    {
        if (seed != 0)
            Random.seed = seed;
        return ExpandString(inString);
    }
}
