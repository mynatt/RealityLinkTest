using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealityLink
{
	interface IPacket<T> {
		T Encode ();
		void Decode (object data);
		void Decode (T data);
	}

	public class PacketJSON : IPacket<string> {
		public virtual string Encode() {
			return JsonUtility.ToJson(this);
		}
		public virtual void Decode(object data) {
			Decode (data.ToString ());
		}
		public virtual void Decode(string data) {
			JsonUtility.FromJsonOverwrite(data, this);
		}
	}

	// public class PacketReady : Packet {
	// 	public SESpace space;
	// 	public SEObjectContainer objects;
	// 	// public SEObjects objects;
	// }

	// public class PacketInitSpace : SESpace {
	// 	public SESpace payload;
	// }

//	[Serializable]
//	public class PacketChat : PacketJSON {
//		public string id;
//		public string msg;
//	}
//
	[Serializable]
	public class PacketJoin : PacketJSON {
		public string name; // username
		public string id; // session
		public PacketJoin(string name, string id) {
			this.name = name;
			this.id = id;
		}
	}

	[Serializable]
	public class PacketStart : PacketJSON {
		public string name; // username
		public string id; // session
//		public ShadowVector3 space;
		public List<ShadowGameObject> objects;
		public ShadowTracker tracker;
		public PacketStart(string name, string id, LinkedObjects linkedObjects, LinkedTracker linkedTracker)
		{
			this.name = name;
			this.id = id;
			this.objects = linkedObjects.objects;
			this.tracker = linkedTracker.tracker;
		}
	}

	[Serializable]
	public class PacketUpdate : PacketJSON {
		public List<ShadowGameObject> objects;
		public ShadowTracker tracker;
		public void SetTracker(LinkedTracker linkedTracker) {
			this.tracker = linkedTracker.tracker;
		}
		public void SetObjects(LinkedObjects linkedObjects) {
			this.objects = linkedObjects.objects;
		}
		public void Reset() { 
			this.objects = null;
			this.tracker = null;
		}
	}
}