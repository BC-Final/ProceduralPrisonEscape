using System;
using System.Collections.Generic;

namespace StateFramework {
	public class StateMachine<T> where T : AbstractState {
		private T _state = null;
		private Dictionary<Type, T> _stateCache = new Dictionary<Type, T>();

		public StateMachine () { }

		public void AddState (T pState) {
			_stateCache[pState.GetType()] = pState;
		}

		public void SetState<P> () where P : T {
			if (_state != null) {
				_state.Exit();
			}

			_state = _stateCache[typeof(P)];
			_state.Enter();
		}

		public T GetState () {
			return _state;
		}

		public void Step () {
			_state.Step();
		}
	}
}
