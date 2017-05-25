using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerFOW : MonoBehaviour {
	[SerializeField]
	private Color _seenColor;

	[SerializeField]
	private int _frameDelay = 3;

	[SerializeField]
	private int _seeRadius = 20;

	[SerializeField]
	private int _textureSize = 512;

	private Texture2D _fowTex;
	private Transform _trans;
	private Collider _coll;

	private void Awake() {
		_fowTex = new Texture2D(_textureSize, _textureSize, TextureFormat.ARGB32, false);

		for (int i = 0; i < _textureSize; ++i) {
			for (int j = 0; j < _textureSize; ++j) {
				_fowTex.SetPixel(i, j, Color.black);
			}
		}

		_fowTex.Apply();

		GetComponent<Renderer>().material.mainTexture = _fowTex;
	}

	private void Start() {
		_coll = GetComponent<Collider>();

		//_coll.enabled = false;
	}

	int xSym, ySym;
	int centerX, centerY;
	int frames = 0;

	private void Update() {

		frames++;

		if (frames == _frameDelay) {
			frames = 0;

			if (_trans == null) {
				PlayerMapIcon i = FindObjectOfType<PlayerMapIcon>();
				if (i != null) {
					_trans = i.transform;
				}
			} else {
				RaycastHit hit;
				_coll.enabled = true;

				if (Physics.Raycast(new Vector3(_trans.position.x, _trans.position.y, transform.position.z - 1), Vector3.forward, out hit, 2.0f, LayerMask.GetMask("FogOfWar"))) {
					StartCoroutine(calcFOW(hit.textureCoord.x, hit.textureCoord.y));
				}

				_coll.enabled = false;
			}
		}
	}

	private IEnumerator calcFOW(float pX, float pY) {
		centerX = (int)(pX * _textureSize);
		centerY = (int)(pY * _textureSize);

		for (int x = centerX - _seeRadius; x <= centerX; x++) {
			for (int y = centerY - _seeRadius; y <= centerY; y++) {
				if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= _seeRadius * _seeRadius) {
					xSym = centerX - (x - centerX);
					ySym = centerY - (y - centerY);

					_fowTex.SetPixel(x, y, _seenColor);
					_fowTex.SetPixel(x, ySym, _seenColor);
					_fowTex.SetPixel(xSym, y, _seenColor);
					_fowTex.SetPixel(xSym, ySym, _seenColor);
				}
			}
		}

		_fowTex.Apply();

		yield return null;
	}
}
