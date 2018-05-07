using UnityEngine;

namespace RealityLink {
	public class SECTracker : MonoBehaviour, ISEComponent {
		public ShadowTransform tracker;

		public void SECDecode(string data) {
			JsonUtility.FromJsonOverwrite(data, tracker);
		}
		public string SECEncode() {
			Debug.Log (JsonUtility.ToJson (tracker));
			return JsonUtility.ToJson(tracker);
		}
		public void SECUpdate() {
			tracker.ToReal(this.transform);
		}
		public void UpdateHost() {
			tracker.FromReal(this.transform);
		}
	}
}