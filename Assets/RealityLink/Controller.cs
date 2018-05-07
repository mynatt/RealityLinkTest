using UnityEngine;

namespace RealityLink {
	public class Controller<Transport> 
		: MonoBehaviour where Transport : class, ITransport, new() 
	{
		public string session = "test";
		protected bool sessionActive = false;
		public string bundle = "test";

		protected string userID = null;
		protected Transport connection = null;

		public LinkedObjects objects = null;
		public LinkedTracker tracker = null;

		public string serverURL = "http://localhost:3000";

		public virtual void SetLinked() {
			if (tracker == null) {
				tracker = GetComponentInChildren<LinkedTracker> ();
			}
			if (objects == null) {
				objects = GetComponentInChildren<LinkedObjects> ();
			}
		}

		public virtual void DoConnect() {
			if (connection == null) {
				connection = new Transport ();
				connection.Initialize (serverURL);
				connection.Connect ();
				DoBindings ();
			}
		}
		protected virtual void DoBindings () {
			connection.Bind ("connect", EventConnect);
			connection.Bind ("disconnect", EventDisconnect);
		}
		public virtual void DoDisconnect() {
			if (connection != null) {
				connection.Disconnect ();
				connection = null;
			}
		}

		protected virtual void Update() {
			if (connection != null) {
				connection.Update ();
			}
		}

		protected virtual void EventConnect() {
			Log ("Connected.");
		}
		protected virtual void EventDisconnect() {
			Log("Disconnected.");
		}

		protected virtual void OnDestroy() {
			DoDisconnect ();
		}
		protected virtual void OnDisable() {
			DoDisconnect ();
		}

		protected void TryActive() {
			if (tracker.initialized && objects.initialized) {
				sessionActive = true;
			}
		}
			
		public virtual void Log(string message) {
			Debug.Log (message);
		}

		public virtual void Log(string format, params object[] args) {
			Debug.LogFormat (format, args);
		}

		//	IEnumerator Reconnect() {
		//		if (connection == Status.Connected) {
		//			reconnectCoroutine = null;
		//			yield break;
		//		}
		//
		//		if (++reconnectAttempts > reconnectAttemptsMax) {
		//			ChatMessage("<b>Failed to reconnect.</b>");
		//			connection = Status.Disconnected;
		//			reconnectCoroutine = null;
		//			yield break;
		//		}
		//		if (socket == null) {
		//			ChatMessage("<b>Attempting connection.</b>");
		//			DoOpen();
		//		} else {
		//			ChatMessage("<b>Attempting reconnection. (" + reconnectAttempts + ")</b>");
		//			socket.Connect();
		//		}
		//
		//		// ChatMessage("Waiting to reattempt.");
		//		yield return new WaitForSeconds(5.0f);
		//		reconnectCoroutine = StartCoroutine(Reconnect());
		//	}
		//
		//	IEnumerator WaitConnect() {
		//		yield return new WaitForSeconds(1.0f);
		//		reconnectCoroutine = null;
		//	}
	}
}
