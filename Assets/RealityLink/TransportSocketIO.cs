using System;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

namespace RealityLink
{
	public class TransportSocketIO
		: Transport, ITransport
	{
		private Socket socket;

		public Status status { get; private set; }
		public string serverURL;

		private Dictionary<string, Callback> callbacks;

		private object mutexEvents;
		private Queue<Event> events;
		private Queue<Event> eventsMain; // main thread

		//		private int eventCounter = 0;
		//		private int updateCounter = 0;

		public TransportSocketIO () {
			callbacks = new Dictionary<string, Callback> ();
			mutexEvents = new object ();
			events = new Queue<Event> ();
			eventsMain= new Queue<Event> ();
			status = Status.Disconnected;
		}

		public void Initialize(string url) {
			Log ("bound to \"{0}\"", url);
			serverURL = url;
		}

		public void Connect() {
			//			Log("Connect()");

			if (socket != null) {
				//				Log ("already open");

				if (status == Status.Disconnected) {
					Log ("connecting...");
					status = Status.Connecting;

					socket.Open ();
					socket.Connect ();

				} else if (status == Status.Connecting) {
					Log ("can't connect: already connecting");
				} else if (status == Status.Connected) {
					Log ("can't connect: already connected");
				}
			} else {
				//				Log ("opening");

				Log ("connecting...");
				status = Status.Connecting;

				socket = IO.Socket (serverURL);
				socket.On ("connect", () => {
					Log("connected");
					status = Status.Connected;
				});
				socket.On ("disconnect", () => {
					Log("disconnected");

					if (callbacks.ContainsKey("disconnect")) {
						callbacks ["disconnect"].Call ();
					}

					status = Status.Disconnected;
				});
			}
		}

		public void Disconnect() {
			//			Log ("Disconnect()");

			if (socket != null) {
				if (status != Status.Disconnected) {
					Log ("disconnecting...");

					socket.Disconnect ();
					socket.Close ();
				}
				else {
					Log ("can't disconnect: already disconnected");
				}
			} else {
				Log ("can't disconnect; no socket");
			}
		}

		public void Update() {
			lock (mutexEvents) {
				while (events.Count > 0) {
					eventsMain.Enqueue(events.Dequeue ());
				}
			}
			while (eventsMain.Count > 0) {
				var e = eventsMain.Dequeue ();
				if (callbacks.ContainsKey (e.name)) {
					var callback = callbacks[e.name];
					if (e.data != null) {
						//							Log ("Update(\"{0}\",[data]) {1}", e.name, (updateCounter++).ToString());
						callback.Call(e.data);
					} else {
						//							Log ("Update(\"{0}\") {1}", e.name, (updateCounter++).ToString());
						callback.Call();
					}
				} else {
					Log ("can't update: no callback for \"{0}\" found", e.name);
				}
			}
		}

		public void Bind(string eventName, Action callback) {
			//			Log ("Bind(\"{0}\")", eventName);

			if (!callbacks.ContainsKey (eventName)) {
				socket.On(eventName, () => {
					var e = new Event (eventName);
					//					Log("Event(\"{0}\") {1}", eventName, (eventCounter++).ToString());
					lock (mutexEvents) {
						events.Enqueue(e);
					}
				});
				callbacks [eventName] =  new Callback(callback);
			} else {
				Log ("can't bind: callback exists");
			}
		}

		public void Bind(string eventName, Action<object> callback) {
			//			Log ("Bind(\"{0}\") (with data)", eventName);

			if (!callbacks.ContainsKey (eventName)) {
				socket.On(eventName, (data) => {
					var e = new Event (eventName, data);
					//					Log("Event(\"{0}\", [data]) {1}", eventName, (eventCounter++).ToString());
					lock (mutexEvents) {
						events.Enqueue(e);
					}
				});
				callbacks [eventName] =  new Callback(callback);
			} else {
				Log ("can't bind: callback exists");
			}
		}

		public void Emit(string eventName) {
			//			Log ("Emit(\"{0}\")", eventName);

			if (socket != null) {
				if (status == Status.Connected) {
					socket.Emit (eventName);
				} else {
					Log ("can't emit: not connected");
				}
			} else {
				Log ("can't emit; no socket");
			}
		}

		public void Emit(string eventName, object data) {
			//			Log ("Emit(\"{0}\", ...)", eventName);

			if (socket != null) {
				if (status == Status.Connected) {
					socket.Emit (eventName, data);
				} else {
					Log ("can't emit: not connected");
				}
			} else {
				Log ("can't emit; no socket");
			}
		}

		public void Log(string message) {
			Debug.Log ("[SocketIO] " + message);
		}

		public void Log(string format, params string[] args) {
			Debug.LogFormat ("[SocketIO] " + format, args);
		}
	}
}

