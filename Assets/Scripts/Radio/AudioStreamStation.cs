using UnityEngine;
using System.Collections;

public class AudioStreamStation : RadioStation {

	public AudioClip StationAudio;

	private AudioSource _audioSource = null;

	// Use this for initialization
	void Start () {
		RadioManager.Instance.AddRadioStationForChannel(Channel,this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Play()
	{
		//TODO: Fixup
		//if(_audioSource == null)
		//	_audioSource = SoundManager.PlaySFX(StationAudio, true);
	}
	
	public override void Stop()
	{
		_audioSource.Stop();
		_audioSource = null;
	}
	
	public override void SetVolume(float volume)
	{
		if(_audioSource!=null)
			_audioSource.volume = volume;
	}
}
