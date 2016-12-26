using UnityEngine;
using System.Collections;

public class MorseStation : RadioStation {

	public AudioClip 			DotSound;
	public AudioClip			DashClip;
	public string				Message;

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
	}
	
	public override void Stop()
	{
	}
	
	public override void SetVolume(float volume)
	{
	}
}
