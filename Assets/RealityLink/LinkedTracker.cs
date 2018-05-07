using UnityEngine;

namespace RealityLink {
	public class LinkedTracker
		: LinkedComponent/*<ShadowTracker>*/ //, ISEComponent {
	{
		public ShadowTracker tracker = new ShadowTracker();

		public void Start() {
//			tracker 
		}
//		public override void InitGuest() {
//			UpdateGuest ();
//		}
		public override void UpdateGuest() {
			tracker.ToReal (this.transform);
		}
//		public override void InitHost() {
//			UpdateHost ();
//		}
		public override void UpdateHost() {
			tracker.FromReal (this.transform);
		}
	}
}