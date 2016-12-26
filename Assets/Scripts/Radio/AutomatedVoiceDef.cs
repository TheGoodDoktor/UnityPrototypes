using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[System.Serializable]
public class CharacterSound
{
	public string Character;
	public AudioClip Sound;
}

public class AutomatedVoiceDef : ScriptableObject {

	public AudioClip			GapSound;
	public CharacterSound[] 	CharacterSounds;

	public AudioClip GetClipForChar(char voiceChar)
	{
		foreach(CharacterSound charSound in CharacterSounds)
		{
			if(charSound.Character[0] == voiceChar)
				return charSound.Sound;
		}

		return null;
	}

	/*#if UNITY_EDITOR
	[MenuItem("Assets/Create/AutomatedVoiceDef")]
	public static void CreateAsset ()
	{
		AssetUtility.CreateAsset<AutomatedVoiceDef>();
	}
	#endif*/
}
