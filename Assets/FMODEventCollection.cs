using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventCollection : MonoBehaviour {
	[SerializeField]
	private string[] _events;

	private Dictionary<string, FMOD.Studio.EventInstance> _instances = new Dictionary<string, FMOD.Studio.EventInstance>();

	private void Awake() {
		foreach (string s in _events) {
			_instances.Add(s, FMODUnity.RuntimeManager.CreateInstance(s));
		}
	}

	public void Play(int pIndex) {
		_instances[_events[pIndex]].start();
		//_instances[_events[pIndex]].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform, GetComponent<Rigidbody>()));
		FMODUnity.RuntimeManager.AttachInstanceToGameObject(_instances[_events[pIndex]], transform, GetComponent<Rigidbody>());
	}

	public void Stop(int pIndex) {
		_instances[_events[pIndex]].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	public void SetParameter(int pIndex, string pName, float pValue) {
		_instances[_events[pIndex]].setParameterValue(pName, pValue);
	}
}
