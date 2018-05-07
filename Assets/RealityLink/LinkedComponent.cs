using UnityEngine;

namespace RealityLink {
	interface ILinkedComponent/*<T>*/ {
//		T GetShadow();
//		string Encode ();
//		void Decode(string data);
		void InitGuest();
		void UpdateGuest();
		void InitHost();
		void UpdateHost();
	}

	public abstract class LinkedComponent/*<T>*/
		: MonoBehaviour, ILinkedComponent/*<T> where T : new()*/
	{
//		protected T shadow;
		public bool initialized = false;

//		protected virtual void Start() {
//			shadow = new T ();
//		}

//		public virtual T GetShadow() {
//			return shadow;
//		}

		// there should be a way to separate this into a packet
		// but i don't know how to do it right now.
//		public virtual string Encode() {
//			return JsonUtility.ToJson(this);
//		}
//		public virtual void Decode(string data) {
//			JsonUtility.FromJsonOverwrite(data, this);
//		}

		public virtual void InitGuest() {
			initialized = true;
			UpdateGuest ();
		}
		public abstract void UpdateGuest();

		public virtual void InitHost() {
			initialized = true;
			UpdateHost ();
		}
		public abstract void UpdateHost();

		public void LogGuest(string message) {
			Debug.Log ("[Guest] " + message);
		}

		public void LogGuest(string format, params object[] args) {
			Debug.LogFormat ("[Guest] " + format, args);
		}

		public void LogHost(string message) {
			Debug.Log ("[Host] " + message);
		}

		public void LogHost(string format, params object[] args) {
			Debug.LogFormat ("[Host] " + format, args);
		}
	}
}