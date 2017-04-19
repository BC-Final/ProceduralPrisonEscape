using System;
using System.Collections.Generic;
using System.Linq;

namespace StateFramework {
	public class StateMachine<T> where T : AbstractState {
		private T _state = null;
		private Dictionary<Type, T> _stateCache = new Dictionary<Type, T>();

		public List<T> States {
			get {
				return _stateCache.Values.ToList();
			}
		}

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

		public void SetState (Type pType) {
			if (_state != null) {
				_state.Exit();
			}

			_state = _stateCache[pType];
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
