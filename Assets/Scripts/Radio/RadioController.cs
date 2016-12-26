#if UNITY_4_6
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioController : MonoBehaviour {

	public Text	ChannelText;
	public Text	FrequencyText;
	public Image TuningKnob;
	public Image LevelNeedle;
	public AudioClip StaticSound;
	public float TuneScale = 1.0f;
	
	// private members
	private int _minChannel = 1;
	private int _maxChannel = 5;
	private int _channelNo = 1;

	private float _frequency = 0.0f;	// 0 - 1, will be formatted for display purposes
	private float _dialRotationScale = 0.5f;

	private bool _mouseDown = false;
	private Vector3 _oldMousePos;

	private AudioSource _staticSfx;
	//private AudioSource _stationAudio = null;

	private RadioStation _lastStation = null;

	// Use this for initialization
	void Start () {

		//TODO: _staticSfx = SoundManager.PlaySFX(StaticSound, true);
	
		FrequencyText.text = string.Format("{0}", _frequency);

	}
	
	// Update is called once per frame
	void Update () {
	
		//if(_frequency < 600.0f)
		//	ChangeTuning(1.0f);

		if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("Pressed left click.");
			_mouseDown = true;
			_oldMousePos = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0))
		{
			Debug.Log("Released left click.");
			_mouseDown = false;
		}

		if(_mouseDown)
		{
			float dist = Input.mousePosition.x - _oldMousePos.x;
			ChangeTuning(dist * TuneScale);
			_oldMousePos = Input.mousePosition;
		}


		//TuningKnob.rectTransform.Rotate(new Vector3(0,0,1.0f));

		RadioStation closestStation = RadioManager.Instance.GetClosestRadioStation(_channelNo,_frequency);

		// Update Audio
		float staticVolume = 1.0f;
		float signalAmount = 0.0f;

		if(closestStation != null)
		{
			float dist = Mathf.Abs (_frequency - closestStation.Frequency);

			if(dist > closestStation.FrequencyTolerance + closestStation.FrequencyFallOff)	// fully off
			{
				staticVolume = 1.0f;
				signalAmount = 0.0f;
			}
			else if(dist < closestStation.FrequencyTolerance)	// fully on
			{
				staticVolume = 0.0f;
				signalAmount = 1.0f;
			}
			else
			{
				staticVolume = (dist - closestStation.FrequencyTolerance) / closestStation.FrequencyFallOff;
				signalAmount = 1.0f - staticVolume;
			}

			if((_lastStation != closestStation) && (signalAmount > 0.0f))	// new station
			{
				if(_lastStation != null)
					_lastStation.Stop();

				//_stationAudio = SoundManager.PlaySFX(closestStation.StationAudio, true);
				closestStation.Play();
				_lastStation = closestStation;
			}
		}

		// Set Volumes
		if(_staticSfx != null)
		{
			_staticSfx.volume = staticVolume;
		}

		if(closestStation != null)
		{
			closestStation.SetVolume(signalAmount);
		}

		// TODO: Position needle
		Quaternion needleRotation = Quaternion.Euler(0,0,25.0f - (signalAmount * 50.0f));
		LevelNeedle.rectTransform.rotation = needleRotation;
	}

	public void OnChannelUp()
	{
		if(_channelNo < _maxChannel)
			_channelNo++;
		ChannelText.text = string.Format("{0}", _channelNo);
	}

	public void OnChannelDown()
	{
		if(_channelNo > _minChannel)
			_channelNo--;
		ChannelText.text = string.Format("{0}", _channelNo);
	}

	public void ChangeTuning(float amount)
	{
		float oldFrequency = _frequency;
		// apply change
		_frequency = Mathf.Clamp(_frequency + amount,0.0f,1000.0f);
		FrequencyText.text = string.Format("{0}", _frequency);// + (_channelNo * 1000));

		TuningKnob.rectTransform.Rotate(new Vector3(0,0,-(_frequency - oldFrequency) * _dialRotationScale));
	}
}
#endif
