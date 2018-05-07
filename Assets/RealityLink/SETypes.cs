using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple types shadow a real type they can overwrite and return back
interface ISEType<Shadow> {
	Shadow apply(Shadow shadow);
}

// Complex types can hold several primitive or shadow types and don't return any of them
interface ISETypeComplex<A> {
	void apply(A a);
}
interface ISETypeComplex<A, B> {
	void apply(A a, B b);
}
interface ISETypeComplex<A, B, C> {
	void apply(A a, B b, C c);
}

// components must be able to decode their contents
// into themselves and encode themselves into a string,
// as well as cause updates using their types

interface ISEComponent {
	void SECDecode(string data);
	string SECEncode();
	void SECUpdate();
}

// SIMPLE TYPES

[Serializable]
public class SEVector3 : ISEType<Vector3> {
	public float x;
	public float y;
	public float z;
	public Vector3 apply(Vector3 vector) {
//		string before = vector.ToString ();
		vector.Set(x, y, z);
//		string after = vector.ToString ();
//		Debug.Log ("SEVector3.apply: " + before + " => " + after);
		return vector;
	}
	new public string ToString() {
		return "(" + x + ", " + y + ", " + z + ")";
	}
}

[Serializable]
public class SEQuaternion : ISEType<Quaternion> {
	public float x;
	public float y;
	public float z;
	public float w;
	public Quaternion apply(Quaternion quaternion) {
//		string before = quaternion.ToString ();
		quaternion.Set(x, y, z, w);
//		string after = quaternion.ToString ();
//		Debug.Log ("SEQuaternion.apply: " + before + " => " + after);
		return quaternion;
	}
	new public string ToString() {
		return "(" + x + ", " + y + ", " + z + ", " + w + ")";
	}
}

[Serializable]
public class SETransform : ISEType<Transform> {
	public SEVector3 position;
	public SEVector3 scale;
	public SEQuaternion rotation;
	public Transform apply(Transform transform) {
//	 	string before = "{ " + transform.localPosition.ToString () + " " + transform.localScale.ToString () + " " + transform.localRotation.ToString () + " }";
		transform.localPosition = position.apply(new Vector3());
		transform.localScale = scale.apply(new Vector3());
		transform.localRotation = rotation.apply(new Quaternion());
//		string after = "{ " + transform.localPosition.ToString () + " " + transform.localScale.ToString () + " " + transform.localRotation.ToString () + " }";
//		Debug.Log ("SETransform.apply: " + before + " => " + after);
		return transform;
	}
	new public string ToString() {
		return "( " + position.ToString () + " " + scale.ToString () + " " + rotation.ToString () + " )";
	}
}

// COMPLEX TYPES

// spaces map a { space } to a transform
[Serializable]
public class SESpace : ISETypeComplex<Transform> {
	public SETransform space;
	public void apply(Transform transform) {
		space.apply(transform);
	}
	public string encode() {
		return JsonUtility.ToJson(this);
	}
}

// trackers map a { tracker } to a transform
[Serializable]
public class SETracker: ISETypeComplex<Transform> {
	public SETransform tracker;
	public void apply(Transform transform) {
		tracker.apply(transform);
	}
	public string encode() {
		return JsonUtility.ToJson(this);
	}
}

// avatars apply themselves to their disparate parts
[Serializable]
public class SEAvatar : ISETypeComplex<Transform, Transform, Transform> {
	public SETransform head;
	public SETransform handLeft;
	public SETransform handRight;
	public void apply(Transform t_head, Transform t_handLeft, Transform t_handRight) {
		head.apply(t_head.transform);
		handLeft.apply(t_handLeft.transform);
		handRight.apply(t_handRight.transform);
	}
	public string encode() {
		return JsonUtility.ToJson(this);
	}
}

// objects apply their name and transform to a gameobject
[Serializable]
public class SEObject : ISETypeComplex<GameObject> {
	public int id;
	public string uid;
	public string name;
	public SETransform transform;
	public void apply(GameObject gameObject) {
		transform.apply(gameObject.transform);
	}
	public void init(GameObject gameObject) {
		gameObject.name = name;
		apply(gameObject);
	}
	public string encode() {
		return JsonUtility.ToJson(this);
	}
}

// an objectcontainer applys an unordered list of { id, name, transform } objects
// to a dictionary of gameobjects
[Serializable]
public class SEObjectContainer : ISETypeComplex<Dictionary<int, GameObject>> {
	public List<SEObject> objects;
	public void apply(Dictionary<int, GameObject> gameObjects) {
		foreach (SEObject seObject in objects) {
			if (gameObjects.ContainsKey(seObject.id)) {
				seObject.apply(gameObjects[seObject.id]);
			}
		}
	}
	public string encode() {
		return JsonUtility.ToJson(this);
	}
}

[Serializable]
public class SEPrefabIndexEntry {
	public string uid;
	public GameObject prefab;
}

// [Serializable]
// public struct SEReady : ISETypeComplex<GameObject, Dictionary<int, GameObject>> {

// }
