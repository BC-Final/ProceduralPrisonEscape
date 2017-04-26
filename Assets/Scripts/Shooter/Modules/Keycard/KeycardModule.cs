using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardModule : AbstractModule {
	[SerializeField]
	private Material _neutralMaterial;

	[SerializeField]
	private Material _approvedMaterial;

	[SerializeField]
	private Material _deniedMaterial;

	[SerializeField]
	private float _materialChangeTime;

	[SerializeField]
	private Renderer _cardreaderRenderer;

	private Animator _animator;

	private Timer _materialTimer;

	private void Start() {
		_animator = GetComponentInChildren<Animator>();
		_animator.SetBool("Eject", false);
	
	}

	public override void Interact() {
		base.Interact();

		if (!IsSolved) {
			if (FindObjectOfType<KeycardHolder>().HasKeycard(_objective as KeycardObjective)) {
				_animator.SetTrigger("Insert");
				_cardreaderRenderer.material = _approvedMaterial;
				(_objective as KeycardObjective).Insert();
				//TODO Delay the approved material with an animation event maybe?
			} else {
				_cardreaderRenderer.material = _deniedMaterial;
				_materialTimer = TimerManager.CreateTimer("Cardreader Material Timer", true).SetDuration(_materialChangeTime).AddCallback(() => _cardreaderRenderer.material = _neutralMaterial).Start();
			}
		}
	}
}