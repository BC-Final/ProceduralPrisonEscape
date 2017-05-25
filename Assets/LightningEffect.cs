using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightningEffect : MonoBehaviour {
	[SerializeField]
	private float _visibleDuration;

	[SerializeField]
	private float _fadeDuration;

	[SerializeField]
	private Color _color1;

	[SerializeField]
	private Color _color2;

	[SerializeField]
	private float _inaccuracy;

	[SerializeField]
	private float _arcLength;

	[SerializeField]
	private float _arcVariation;

	[SerializeField]
	private int _maxSegments = 1000;

	[SerializeField]
	private LineRenderer _line;

	private Transform _target;
	private Vector3 _lastTargetPos;
	private bool _active;

	private void Awake() {
		_line.material = new Material(Shader.Find("Particles/Alpha Blended"));
	}

	private void Update() {
		if (_active && _arcLength != 0.0f) {
			redrawLine();
		}
	}

	public void Display(Transform pTarget) {
		_active = true;
		_target = pTarget;
		_line.enabled = true;
		TimerManager.CreateTimer("Lightning Visible", true).SetDuration(_visibleDuration).AddCallback(startFade).Start();
	}

	private void startFade() {
		Color c1 = _line.startColor;
		c1.a = 0.0f;

		Color c2 = _line.endColor;
		c2.a = 0.0f;

		_active = false;

		_line.DOColor(new Color2(_line.startColor, _line.endColor), new Color2(c1, c2), _fadeDuration).OnComplete(() => Destroy(this.gameObject));
	}

	private void redrawLine() {
		_line.startColor = Random.value > 0.5f ? _color1 : _color2;
		_line.endColor = Random.value > 0.5f ? _color1 : _color2;

		_line.SetPosition(0, transform.position);

		Vector3 lastPoint = transform.position;
		Vector3 forward = Vector3.zero;
		int counter = 1;

		if (_target != null) {
			_lastTargetPos = _target.position;
		}

		while (Vector3.Distance(_lastTargetPos, lastPoint) > 0.5f) {
			_line.positionCount = counter + 1;
			forward = ((_lastTargetPos - lastPoint).normalized + Random.insideUnitSphere * _inaccuracy).normalized * Random.Range(_arcLength * _arcVariation, _arcLength) + lastPoint;
			_line.SetPosition(counter, forward);
			lastPoint = forward;
			counter++;

			if (counter > _maxSegments) return;
		}
	}

	//function RedrawLine() {
	//	var colorStart = color1;
	//	var colorEnd = color1;

	//	if (Random.value > .5)
	//		colorStart = color2;
	//	if (Random.value > .5)
	//		colorEnd = color2;
	//	LR.SetColors(colorStart, colorEnd);
	//	var existsList = Array();

	//	for (var i = 0; i < targets.length; i++) {
	//		//var exists = targets[i];
	//		//if (exists)
	//		if (targets[i])
	//			existsList.Add(targets[i]);
	//	}

	//	targets = existsList;
	//	LR.SetPosition(0, transform.position);
	//	var lastPoint = transform.position;
	//	var j = 1;
	//	var fwd : Vector3;
	//	if (targets.length == 0) {
	//		LR.SetVertexCount(2);
	//		var target = transform.position + transform.TransformDirection(Randomize(Vector3.forward, inaccuracy / 4)) * Random.Range(jumpRadius * arcVariation, jumpRadius) * 1.5;
	//		while (Vector3.Distance(target, lastPoint) > .5) {
	//			LR.SetVertexCount((j + 1));
	//			fwd = target - lastPoint;
	//			fwd.Normalize();
	//			fwd = Randomize(fwd, inaccuracy);
	//			fwd *= Random.Range(arcLength * arcVariation, arcLength);
	//			fwd += lastPoint;
	//			LR.SetPosition(j, fwd);
	//			j++;
	//			lastPoint = fwd;
	//		}
	//		return;
	//	} else {
	//		for (i = 0; i < targets.length; i++) {
	//			while (Vector3.Distance(targets[i].position, lastPoint) > .5) {
	//				LR.SetVertexCount((j + 1));
	//				fwd = targets[i].position - lastPoint;
	//				fwd.Normalize();
	//				fwd = Randomize(fwd, inaccuracy);
	//				fwd *= Random.Range(arcLength * arcVariation, arcLength);
	//				fwd += lastPoint;
	//				LR.SetPosition(j, fwd);
	//				j++;
	//				lastPoint = fwd;
	//			}
	//		}
	//	}
	//}
}
