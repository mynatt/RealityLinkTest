using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealityLink
{
	[CreateAssetMenu]
	public class LinkedPrefabs : ScriptableObject
	{
		[Serializable]
		public class Item {
			public string uid;
			public GameObject prefab;
		}
		public List<Item> items = new List<Item>();
		public void CreateLookup(Dictionary<string, GameObject> dictionary) {
			dictionary.Clear ();
			foreach (Item item in items) {
				dictionary.Add (item.uid, item.prefab);
			}
		}
	}
}

