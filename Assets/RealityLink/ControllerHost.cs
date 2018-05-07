using System.Collections.Generic;
using UnityEngine;

namespace RealityLink {
	public class ControllerHost 
		: Controller<TransportSocketIO>
	{
		public string username = "Unity Host";
		public float connectDelay = 0.0f;

		protected PacketUpdate packetUpdate;

		void Start () {
			SetLinked ();
			packetUpdate = new PacketUpdate ();
			packetUpdate.SetObjects (objects);
			packetUpdate.SetTracker (tracker);
			Invoke("DoConnect", connectDelay);
		}

		protected override void DoBindings() {
			connection.Bind ("connect", EventConnect);
			connection.Bind ("disconnect", EventDisconnect);
			connection.Bind ("session started", EventStarted);
			connection.Bind ("session ready", EventReady);
		}

		private void DoStart() {
			Log ("Starting \"{0}\"...", session);
			tracker.InitHost ();
			objects.InitHost ();
			PacketStart packet = new PacketStart (username, session, objects, tracker); //, objects.GetShadow(), tracker.GetShadow());
			string data = packet.Encode();
			Log ("Using tracker: {0}", packet.tracker.ToString ());
			Log ("Using objects: {0}", packet.objects.ToString ());

			connection.Emit ("session start", data);
		}

		protected override void EventConnect() {
			Log ("Connected.");
			DoStart ();
		}
		protected override void EventDisconnect() {
			Log("Disconnected.");
			sessionActive = false;
		}
		private void EventStarted() {
			Log ("Started \"{0}\".", session);
			TryActive ();
		}
		private void EventReady() {
			Log ("The server is ready.");
		}

		protected override void Update() {
			if (connection != null) {
				connection.Update ();
				if (sessionActive) {
					tracker.UpdateHost ();
					objects.UpdateHost ();
					packetUpdate.SetTracker (tracker);
					packetUpdate.SetObjects (objects);
					connection.Emit ("update all", packetUpdate.Encode());
					packetUpdate.Reset ();
				}
			}

		}

		public override void Log(string message) {
			Debug.Log ("[Host] " + message);
		}

		public override void Log(string format, params object[] args) {
			Debug.LogFormat ("[Host] " + format, args);
		}
	}
}