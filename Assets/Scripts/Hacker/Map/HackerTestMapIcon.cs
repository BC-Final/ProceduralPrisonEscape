using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerTestMapIcon : HackerMapIcon {
	protected override void DisplayContext () {
		//TODO Instantiate a new ContextWindow or make it Singleton?
		HackerContextMenu hcm = (Instantiate(Resources.Load("pfb_context"), FindObjectOfType<Canvas>().transform) as GameObject).GetComponent<HackerContextMenu>();

		hcm.AddOption("View Info", () => DisplayInfo());
		hcm.AddOption("Hack", () => StartHack());
		hcm.AddOption("Adv Hack", () => StartAdvHack());

		hcm.Display();
	}

	protected override void DisplayInfo () {
		GameObject g = Instantiate(Resources.Load("pfb_window_testObject"), FindObjectOfType<HackerWindowContainer>().transform) as GameObject;
		g.GetComponent<HackerWindow>().SetData(_data);
	}
}
