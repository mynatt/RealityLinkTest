using UnityEngine;

public class SECObject : MonoBehaviour, ISEComponent {
	public SEObject obj;

	public void SECDecode(string data) {
		JsonUtility.FromJsonOverwrite(data, obj);
	}
	public string SECEncode() {
		return JsonUtility.ToJson(obj);
	}
	public void SECUpdate() {
		obj.apply(gameObject);
	}
}
