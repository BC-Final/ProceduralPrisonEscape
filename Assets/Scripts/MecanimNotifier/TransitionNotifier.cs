using UnityEngine;
using System.Collections;

public class TransitionNotifier : StateMachineBehaviour {
	[SerializeField]
	private NotificationType _type;

	[SerializeField]
	private string _message;

	private IMecanimNotifiable _behaviour;

	private enum NotificationType {
		Enter,
		Exit
	}

	public void SetBehavior(IMecanimNotifiable pBehaviour) {
		_behaviour = pBehaviour;
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (_behaviour != null && _type == NotificationType.Enter) {
			_behaviour.Notify(_message);
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (_behaviour != null && _type == NotificationType.Exit) {
			_behaviour.Notify(_message);
		}
	}
}
