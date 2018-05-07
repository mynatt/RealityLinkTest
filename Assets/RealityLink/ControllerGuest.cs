using System.Collections.Generic;
using UnityEngine;

namespace RealityLink {
	public class ControllerGuest
		: Controller<TransportSocketIO> 
	{
		public string username = "Unity Guest";
		public float connectDelay = 1.0f;

		protected PacketUpdate packetUpdate;

		void Start () {
			SetLinked ();
			packetUpdate = new PacketUpdate ();
			packetUpdate.SetTracker (tracker);
			packetUpdate.SetObjects (objects);
			Invoke("DoConnect", connectDelay);
		}

		protected override void DoBindings() {
			connection.Bind ("connect", EventConnect);
			connection.Bind ("disconnect", EventDisconnect);
			connection.Bind ("session joined", EventJoined);
			connection.Bind ("session ready", EventReady);
			connection.Bind ("provide objects", EventGotObjects);
			connection.Bind ("provide tracker", EventGotTracker);
			connection.Bind ("tick", EventTick);
		}

		private void DoJoin() {
			Log ("Joining \"{0}\"...", session);
			PacketJoin packet = new PacketJoin(username, session);
			string data = packet.Encode();
			connection.Emit ("session join", data);
		}
			
		protected override void EventConnect() {
			Log ("Connected.");
			DoJoin ();
		}
//		private override void EventDisconnect() {
//			Log("Disconnected.");
//		}
		private void EventJoined() {
			Log ("Joined \"{0}\".", session);
		}
		private void EventReady() {
			Log ("The server is ready.");
			connection.Emit ("request space");
			connection.Emit ("request objects");
			connection.Emit ("request tracker");
		}
//		private void EventGotSpace(object data) {
//			Log("Got space.");
//
////			string dataStr = data.ToString ();
////			space.SECDecode(dataStr);
////			queueComponents.Enqueue(space);
//		}
		private void EventGotObjects(object data) {
			packetUpdate.Decode (data);
			Log("Got objects: {0}", packetUpdate.objects.Count.ToString());
			objects.InitGuest ();
			TryActive ();
		}
		private void EventGotTracker(object data) {
			packetUpdate.Decode (data);
			Log("Got tracker: {0}", packetUpdate.tracker.ToString ());
			tracker.InitGuest ();
			TryActive ();
		}

		private void EventTick(object data) {
			if (sessionActive) {
//				Log ("tick: {0}", data);
				packetUpdate.Decode (data);
//				Log("Got tracker: {0}", packetUpdate.tracker.ToString ());
//				Log("Got objects: {0}", packetUpdate.objects.Count.ToString());
				tracker.UpdateGuest ();
				objects.UpdateGuest ();
			} else {
//				Log ("waiting for active");
			}
		}
			
		protected override void Update () {
			if (connection != null) {
				connection.Update ();
			}
		}

		public override void Log(string message) {
			Debug.Log ("[Guest] " + message);
		}

		public override void Log(string format, params object[] args) {
			Debug.LogFormat ("[Guest] " + format, args);
		}
	}
}