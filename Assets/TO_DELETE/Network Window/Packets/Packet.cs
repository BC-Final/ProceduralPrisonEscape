//using UnityEngine;
//using System.Collections.Generic;
//using DG.Tweening;

//public class Packet : MonoBehaviour {
//	[SerializeField]
//	private float _moveSpeed;

//	private List<AbstractNode> _path;
//	private Tweener tween;

//	private AbstractNode _start;
//	private AbstractNode _target;

//	public void Send(AbstractNode pStart, AbstractNode pTarget) {
//		_start = pStart;
//		_target = pTarget;

//		_path = AstarPathFinder.CalculatePath(pStart, pTarget);
//	}

//	protected virtual void Update() {
//		if (_path != null && _path.Count != 0) {
//			if (tween == null || !tween.IsPlaying()) {
//				tween = transform.DOMove(_path[0].transform.position, Vector3.Distance(transform.position, _path[0].transform.position) / _moveSpeed);
//				_path.RemoveAt(0);
//				tween.SetEase(Ease.Flash);
//			}
//		} else if(tween == null || !tween.IsPlaying()) {
//			_target.ReceivePacket(_start, this);
//			Destroy(gameObject);
//		}
//	}
//}
