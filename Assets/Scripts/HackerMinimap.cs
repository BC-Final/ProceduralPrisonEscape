using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerMinimap : MonoBehaviour {
	public static void ProcessPacket(NetworkPacket.GameObjects.Minimap.Creation pPacket) {
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(pPacket.bytes);
		FindObjectOfType<HackerMinimap>().GetComponent<Renderer>().material.mainTexture = tex;
	}
}
