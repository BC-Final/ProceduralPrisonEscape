using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HackerMapIcon : MonoBehaviour {
	protected HackerData _data;

	public void SetData (HackerData pData) {
		_data = pData;
	}

	private void OnMouseOver () {
		if (Input.GetMouseButtonDown(1)) {
			DisplayContext();
		}
	}

	protected abstract void DisplayContext ();

	//TODO Stub functions
	protected virtual void DisplayInfo () { Debug.Log("No Info Set"); }
	protected virtual  void StartHack () { Debug.Log("No Hack"); }
	protected virtual void StartAdvHack () { Debug.Log("No Adv Hack"); }
}
