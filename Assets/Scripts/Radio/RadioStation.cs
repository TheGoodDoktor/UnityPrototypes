using UnityEngine;
using System.Collections;

public class RadioStation : MonoBehaviour {

	// Public Properities
	public int Channel = 1;
	public float Frequency = 0.0f;	// 0 - 1000
	public float FrequencyTolerance = 10;	// amount either side of frequency that clear transmission will be heard
	public float FrequencyFallOff = 10;
	public bool GlobalTransmission = true;	// is transmission global - no range	
	public float TransmissionRange = 50;	// range of transmission 
	public float TransmissionFallOff = 10.0f;	// distance from the edge of transmission range to full static

	public float DirectionalQuality = 1.0f;	// quality factor for when pointing 180 degrees away from source
	
	//public AudioClip StationAudio;
	//private AudioSource _audioSource = null;

	// Use this for initialization
	void Start () {
	
		//RadioManager.Instance.AddRadionStationForChannel(Channel,this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Play()
	{
		Debug.Log ("Station Play NOT Implemented");
		//if(_audioSource == null)
		//	_audioSource = SoundManager.PlaySFX(StationAudio, true);
	}

	public virtual void Stop()
	{
		Debug.Log ("Station Stop NOT Implemented");
		//_audioSource.Stop();
		//_audioSource = null;
	}

	public virtual void SetVolume(float volume)
	{
		Debug.Log ("Station SetVolume NOT Implemented");
		//if(_audioSource!=null)
		//	_audioSource.volume = volume;
	}
}
