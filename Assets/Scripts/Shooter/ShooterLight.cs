using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
//TODO Make Networked
public class ShooterLight : MonoBehaviour, IDamageable {
	[SerializeField]
	private float _distractRange;

	private bool _broken = false;

	private Color _defaultColor;

	//FIX One light might have multiple light sources???
	private Light _light;

	//TODO Add Packet processing

	private void Start() {
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;

		_light = GetComponentInChildren<Light>(true);
		_defaultColor = _light.color;
	}

	public void SetState(bool pTurnedOn) {
		if (!_broken) {
			_light.enabled = pTurnedOn;
		}
	}

	private void blowOut(bool pDistractEnemies) {
		ShooterAlarmManager.Instance.OnAlarmChange -= onAlarmChange;
		_light.DOKill();
		_broken = true;

		if (pDistractEnemies) {
			Sequence breakSequence = DOTween.Sequence()
				.Append(_light.DOIntensity(_light.intensity * 1.5f, 0.35f))
				.Append(_light.DOIntensity(0.0f, 0.0f));
			//ADD Display explode effects
			//TODO Distract nearby enemies
		} else {
			Sequence breakSequence = DOTween.Sequence()
				.Append(_light.DOIntensity(0.0f, 0.0f))
				.AppendInterval(0.25f)
				.Append(_light.DOIntensity(_light.intensity, 0.0f))
				.AppendInterval(0.15f)
				.Append(_light.DOIntensity(0.0f, 0.0f))
				.AppendInterval(0.1f)
				.Append(_light.DOIntensity(_light.intensity, 0.0f))
				.AppendInterval(0.075f)
				.Append(_light.DOIntensity(0.0f, 0.0f));
		}
		
	}

	private void onAlarmChange() {
		if (ShooterAlarmManager.Instance.AlarmIsOn) {
			//ADD Make this modifiable
			Sequence alarmSequence = DOTween.Sequence()
				.Append(_light.DOColor(Color.red, 0.75f))
				.Append(_light.DOColor(_defaultColor, 0.75f))
				.SetLoops(-1)
				.SetTarget(_light);
		} else {
			_light.DOKill();
			_light.DOColor(_defaultColor, 0.5f);
		}
	}


	public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
		blowOut(false);
	}

	public Faction Faction { get { return Faction.Neutral; } }
	public GameObject GameObject { get { return gameObject; } }
}
