namespace StateFramework {
	public abstract class AbstractState {
		public AbstractState () { }

		public abstract void Enter ();
		public abstract void Step ();
		public abstract void Exit ();
	}
}