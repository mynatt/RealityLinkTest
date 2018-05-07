using UnityEngine;

public class SECSpace : MonoBehaviour, ISEComponent {
	public SESpace space;

	public void SECDecode(string data) {
		
		JsonUtility.FromJsonOverwrite(data, space);
//		SESpace testSpace = JsonUtility.FromJson<SESpace> (data);
//		Debug.Log(JsonUtility.ToJson(space));
//		Debug.Log(JsonUtility.ToJson(testSpace));
	}
	public string SECEncode() {
		return JsonUtility.ToJson(space);
	}
	public void SECUpdate() {
//		Debug.Log (SECEncode ());
		space.apply(this.transform);
//		Debug.Log (SECEncode ());
	}
}
