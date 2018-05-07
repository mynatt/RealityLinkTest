using UnityEngine;

public class SECAvatar : MonoBehaviour, ISEComponent {
	public SEAvatar avatar;

	public GameObject objectHead;

	public void SECDecode(string data) {
		JsonUtility.FromJsonOverwrite(data, avatar);
	}
	public string SECEncode() {
		return JsonUtility.ToJson(avatar);
	}
	public void SECUpdate() {
	}
}
