using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SECObjectContainer : MonoBehaviour, ISEComponent {
	public SEObjectContainer objectContainer;
	public List<SEPrefabIndexEntry> sePrefabIndex = new List<SEPrefabIndexEntry>();

	private Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();
	private Dictionary<string, GameObject> prefabIndex = new Dictionary<string, GameObject> ();
	private bool init = false;

	void Start() {
		foreach (SEPrefabIndexEntry seEntry in sePrefabIndex) {
			prefabIndex.Add (seEntry.uid, seEntry.prefab);
		}
	}

	public void SECDecode(string data) {
		JsonUtility.FromJsonOverwrite(data, objectContainer);
		if (!init) {
			SECInit ();
		}
	}
	public string SECEncode() {
		return objectContainer.encode();
	}
	public void SECUpdate() {
		if (init) {
			objectContainer.apply (gameObjects);
		}
	}
	public void SECInit() {
		init = true;
		Debug.Log ("initializing objects: " + SECEncode());
		foreach (SEObject seObject in objectContainer.objects) {
			Debug.Log ("- initializing " + seObject.name + "(" + seObject.uid + ", " + seObject.id + "): " + seObject.encode());
			if (prefabIndex.ContainsKey(seObject.uid) && !gameObjects.ContainsKey(seObject.id)) {
				GameObject gameObject = Instantiate<GameObject>(
					prefabIndex [seObject.uid],
					this.transform
				) as GameObject;
				seObject.init(gameObject);
				gameObjects.Add (seObject.id, gameObject);
			}
		}
	}
	public void SECDecodeDelta(string data) {
		if (init) {
//			Debug.Log (data);
			SECDecode (data);
		}
	}
}
