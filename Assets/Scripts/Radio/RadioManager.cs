using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadioManager : MonoBehaviour {
	
	private static RadioManager _instance = null;	// instance for singleton

	public static RadioManager Instance {
		get{
			if(_instance == null)
			{
				GameObject manager = new GameObject("RadioManager");
				_instance = manager.AddComponent<RadioManager>();
			}

			return _instance;
		}
	}

	// private members
	private Dictionary<int, ArrayList> _stationList = new Dictionary<int, ArrayList>();

	private ArrayList GetStationListForChannel(int channel)
	{
		if(_stationList.ContainsKey(channel) == false)
			return null;

		return _stationList[channel];
	}

	// public methods
	public void AddRadioStationForChannel(int channel, RadioStation station)
	{
		if(_stationList.ContainsKey(channel) == false)
			_stationList.Add(channel, new ArrayList());
		_stationList[channel].Add(station);
	}

	public RadioStation GetClosestRadioStation(int channel, float frequency)
	{
		ArrayList stationList = GetStationListForChannel(channel);
		if(stationList == null)
			return null;

		RadioStation closestStation = null;
		float closestDist = 9999.0f;

		foreach(RadioStation station in stationList)
		{
			if(station == null)
				continue;

			float dist = Mathf.Abs(frequency - station.Frequency);
			if(dist < closestDist)
			{
				closestDist = dist;
				closestStation = station;
			}
		}

		return closestStation;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
