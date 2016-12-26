using UnityEngine;
using System.Collections;

public class VoiceSequenceStation : RadioStation {

	public AutomatedVoiceDef	Voice;
	public string				Message;

	private AudioSource _audioSource = null;
	private float 		_volume = 1.0f;
	private AudioClip[]	_sequenceClips;
	private bool		_playing = false;
	private float		_sequenceTimer = 0;
	private AudioClip	_itemClip = null;
	private int			_sequenceIndex = 0;

	// Use this for initialization
	void Start () {
		RadioManager.Instance.AddRadioStationForChannel(Channel,this);

		_sequenceClips = new AudioClip[Message.Length];
		// Store AudioCLips
		for(int i=0;i<Message.Length;i++)
		{
			_sequenceClips[i] = Voice.GetClipForChar(Message[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(_playing)
		{
			_sequenceTimer += Time.deltaTime;

			if(_sequenceTimer > _itemClip.length)
			{
				_sequenceIndex++;

				if(_sequenceIndex > _sequenceClips.Length-1)
				{
					_sequenceIndex = -1;
					_itemClip = Voice.GapSound;
				}
				else
				{
					_itemClip = _sequenceClips[_sequenceIndex];
				}
				//TODO: _audioSource = SoundManager.PlaySFX(_itemClip, false);
				_audioSource.volume = _volume;
				_sequenceTimer = 0;
			}
		}
	}

	public override void Play()
	{
		if(_playing == false)
		{
			_sequenceIndex = 0;
			_sequenceTimer = 0;

			_itemClip = _sequenceClips[_sequenceIndex];
			//TODO: _audioSource = SoundManager.PlaySFX(_itemClip, false);
			_audioSource.volume = _volume;
			_playing = true;
		}

	}
	
	public override void Stop()
	{
		if(_playing == true)
		{
			_playing = false;
		}
	}
	
	public override void SetVolume(float volume)
	{
		_volume = volume;
		if(_audioSource!=null)
			_audioSource.volume = volume;
	}
}
