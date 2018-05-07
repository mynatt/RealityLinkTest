using System;
using UnityEngine;

// Reality Link Base Types
// they're all serializable so you can
// serialize them as JSON or whatever you need

namespace RealityLink {
	interface IShadow<T> {
		T ToReal(T t);
		T FromReal(T t);
	}

	[Serializable]
	public class ShadowVector3 : IShadow<Vector3> {
		public float x;
		public float y;
		public float z;

		public Vector3 FromReal(Vector3 vector) {
			x = vector.x;
			y = vector.y;
			z = vector.z;
			return vector;
		}
		public Vector3 ToReal(Vector3 vector) {
			vector.Set (x, y, z);
			return vector;
		}

		new public string ToString() {
			return String.Format ("({0}, {1}, {2})", x, y, z);
		}
	}

	[Serializable]
	public class ShadowQuaternion : IShadow<Quaternion> {
		public float x;
		public float y;
		public float z;
		public float w;

		public Quaternion FromReal(Quaternion quaternion) {
			x = quaternion.x;
			y = quaternion.y;
			z = quaternion.z;
			w = quaternion.w;
			return quaternion;
		}
		public Quaternion ToReal(Quaternion quaternion) {
			quaternion.Set (x, y, z, w);
			return quaternion;
		}

		new public string ToString() {
			return String.Format ("({0}, {1}, {2}, {3})", x, y, z, w);
		}
	}

	[Serializable]
	public class ShadowTransform : IShadow<Transform> {
		public ShadowVector3 position;
		public ShadowVector3 scale;
		public ShadowQuaternion rotation;

		public ShadowTransform() {
			position = new ShadowVector3 ();
			scale = new ShadowVector3 ();
			rotation = new ShadowQuaternion ();
		}

		public Transform FromReal(Transform transform) {
			// Transform parent = transform.parent;
			// transform.parent = null;
			position.FromReal (transform.position);
			scale.FromReal (transform.lossyScale);
			rotation.FromReal (transform.rotation);
			// transform.parent = parent;
			return transform;
		}
		public Transform ToReal(Transform transform) {
			Transform parent = transform.parent;
			transform.parent = null;
			transform.localPosition = position.ToReal (new Vector3 ());
			transform.localScale = scale.ToReal (new Vector3 ());
			transform.localRotation = rotation.ToReal (new Quaternion ());
			transform.parent = parent;
			return transform;
		}

		new public string ToString() {
			return String.Format ("( position: {0}, scale: {1}, rotation: {2} )", 
				position.ToString(), scale.ToString(), rotation.ToString());
		}
	}

	[Serializable]
	public class ShadowGameObject : IShadow<GameObject> {
		public int id;
		public string uid;
		public string name;
		public ShadowTransform transform;

		public ShadowGameObject(int id, string uid) {
			this.id = id;
			this.uid = uid;
			transform = new ShadowTransform ();
		}

		public GameObject FromReal (GameObject gameObject) {
//			name = gameObject.name;
			transform.FromReal (gameObject.transform);
			return gameObject;
		}
		public GameObject ToReal(GameObject gameObject) {
//			gameObject.name = name;
			transform.ToReal (gameObject.transform);
			return gameObject;
		}

		new public string ToString() {
			return String.Format ("GameObject {0}({1}, #{2})", name, uid, id);
		}
	}

	[Serializable] 
	public class ShadowTracker : ShadowTransform {};
}