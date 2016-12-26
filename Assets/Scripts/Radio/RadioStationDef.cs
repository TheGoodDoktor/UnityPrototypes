using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class RadioStationDef : ScriptableObject 
{
	public float Frequency = 0.0f;	// 0 - 1000
	public float FrequencyTolerance = 10;	// amount either side of frequency that clear transmission will be heard
	public float TransmissionRange = 0;	// range of transmission - 0 is global
	public float TransmissionFallOff = 10.0f;	// distance from the edge of transmission range to full static
	public float DirectionalQuality = 1.0f;	// quality factor for when pointing 180 degrees away from source

	public AudioClip StationAudio;

	/*#if UNITY_EDITOR
	[MenuItem("Assets/Create/RadioStationDef")]
	public static void CreateAsset ()
	{
		AssetUtility.CreateAsset<RadioStationDef>();
	}
	#endif*/
}