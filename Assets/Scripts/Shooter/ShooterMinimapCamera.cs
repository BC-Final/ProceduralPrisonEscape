using UnityEngine;
using System.Collections;

public class ShooterMinimapCamera : MonoBehaviour {
	private Camera _camera;

	private void Awake () {
		_camera = GetComponent<Camera>();
	}

	public void SendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Minimap(getMinimapData()));
	}

	private byte[] getMinimapData () {
		RenderTexture rt = new RenderTexture(2048, 2048, 24);

		_camera.targetTexture = rt;
		_camera.Render();

		RenderTexture.active = rt;
		Texture2D tex2d = new Texture2D(2048, 2048, TextureFormat.RGB24, false);
		tex2d.ReadPixels(new Rect(0, 0, 2048, 2048), 0, 0);

		RenderTexture.active = null;
		_camera.targetTexture = null;

		byte[] bytes;
		bytes = tex2d.EncodeToPNG();

		return bytes;
	}
}
