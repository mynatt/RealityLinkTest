using UnityEngine;
using System;
using System.Collections.Generic;

namespace RealityLink {
	public class LinkedObject
		: LinkedComponent/*<List<ShadowGameObject>>*/ //, ISEComponent {
	{
		public string uid;
		public ShadowGameObject shadowObject;

		public override void UpdateGuest() { }
		public override void UpdateHost() { }
	}
}