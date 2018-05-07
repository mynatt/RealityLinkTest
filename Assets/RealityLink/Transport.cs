using System;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

// basic prototype is only equipped to 

namespace RealityLink {
	public class Transport
	{
		public class Callback
		{
			private Action fn = null;
			private Action<object> fnData = null;
			public Callback(Action fn) {
				this.fn = fn;
			}
			public Callback(Action<object> fnData) {
				this.fnData = fnData;
			}
			public void Call(params object[] args) {
				if (fn != null) {
					if (args.Length == 0) {
						fn ();
					} else {
						throw new Exception (string.Format ("invalid callback: expected 0 arguments, got {0}", args.Length));
					}
				} else if (fnData != null) {
					if (args.Length == 1) {
						fnData (args[0]);
					} else {
						throw new Exception (string.Format ("invalid callback: expected 1 argument, got {0}", args.Length));
					}
				} else {
					throw new Exception("invalid callback: no action specified, somehow.");
				}
			}
		}

		public class Event
		{
			public readonly string name;
			public readonly object data;
			public Event(string name) {
				this.name = name;
				this.data = null;
			}
			public Event(string name, object data) {
				this.name = name;
				this.data = data;
			}
		}

		public enum Status {
			Disconnected,
			Connecting,
			Connected,
		};
	}

	public interface ITransport
	{
		void Initialize (string url);
		void Connect();
		void Disconnect();
		void Update(); 	// main thread updater
		void Bind(string eventName, Action callback);
		void Bind(string eventName, Action<object> callback);
		void Emit(string eventName);
		void Emit(string eventName, object data);
		void Log(string message);
		void Log(string format, params string[] args);
	}
}