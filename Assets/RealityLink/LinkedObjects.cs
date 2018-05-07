using UnityEngine;
using System;
using System.Collections.Generic;

namespace RealityLink {
	public class LinkedObjects
		: LinkedComponent/*<List<ShadowGameObject>>*/ //, ISEComponent {
	{
		public List<ShadowGameObject> objects = new List<ShadowGameObject>();
		public LinkedPrefabs linkedPrefabs;

		private int idCounter = 0;
		private Dictionary<int, LinkedObject> linkedObjects;
		private Dictionary<string, GameObject> prefabs;

		public void Start() {
//			objects = new List<ShadowGameObject>();
			linkedObjects = new Dictionary<int, LinkedObject> ();
			prefabs = new Dictionary<string, GameObject> ();
		}
		public override void InitGuest() {
			linkedPrefabs.CreateLookup (prefabs);
			linkedObjects.Clear ();

			initialized = true;
			UpdateGuest ();
		}
		public override void UpdateGuest() {
			foreach (ShadowGameObject shadowObject in objects) {
				if (linkedObjects.ContainsKey(shadowObject.id)) {
					// update
					LinkedObject linkedObject = linkedObjects [shadowObject.id];
					shadowObject.ToReal (linkedObject.gameObject);

//					LogGuest("Updated {0}", shadowObject.ToString());
				}
				else if (prefabs.ContainsKey(shadowObject.uid)) {
					// spawn
					GameObject gameObj = (GameObject)Instantiate (prefabs[shadowObject.uid], transform);
					shadowObject.ToReal (gameObj);
					LinkedObject linkedObject = gameObj.GetComponent<LinkedObject> ();
					if (linkedObject == null) {
						linkedObject = gameObj.AddComponent<LinkedObject>();
					}
					linkedObject.uid = shadowObject.uid;
					linkedObjects.Add (shadowObject.id, linkedObject);

					LogGuest ("Spawned {0}", shadowObject.ToString());
				}
				else {
					LogGuest ("No object {0} or prefab {1}", shadowObject.id, shadowObject.uid);
				}
			}
//			shadow.ToReal (this.transform);
		}
		public override void InitHost() {
			linkedPrefabs.CreateLookup (prefabs);
			linkedObjects.Clear ();
			idCounter = 0;
			foreach (LinkedObject linkedObject in GetComponentsInChildren<LinkedObject>()) {
				ShadowGameObject shadowObject = new ShadowGameObject (idCounter++, linkedObject.uid);
				linkedObject.shadowObject = shadowObject;
				objects.Add (shadowObject);
				linkedObjects.Add (shadowObject.id, linkedObject);

				LogHost ("Spawned {0}", shadowObject.ToString());
			}

			initialized = true;
			UpdateHost ();
//				if (gameObjects.ContainsKey(linkedObject.id)) {
//					// update
//					GameObject gameObject = gameObjects [shadowObject.id];
//					shadowObject.ToReal (gameObject);
//				}
//				else if (prefabs.ContainsKey(linkedObject.uid)) {
//					// spawn
//					GameObject gameObject = (GameObject)Instantiate (prefabs[shadowObject.uid]);
//					LinkedObject linkedObject = gameObject.AddComponent<LinkedObject>();
//					linkedObject.id = shadowObject.id;
//					linkedObject.uid = shadowObject.uid;
//					gameObjects.Add (shadowObject.id, gameObject);
//				}
//			} 
		}
		public override void UpdateHost() {
			objects.Clear ();
			foreach (LinkedObject linkedObject in linkedObjects.Values) {
				linkedObject.shadowObject.FromReal (linkedObject.gameObject);
				objects.Add (linkedObject.shadowObject);
//				LogHost ("Updated {0}", linkedObject.shadowObject.ToString());
			}
//			foreach (LinkedObject linkedObject in GetComponentsInChildren<LinkedObject>()) {
//				ShadowGameObject shadowObject = new ShadowGameObject (linkedObject.id, linkedObject.uid);
//				shadowObject.FromReal (linkedObject.gameObject);
//
//				GameObject gameObject = linkedObject.gameObject;
//				gameObjects.Add (shadowObject.id, gameObject);
//			}
//			foreach (LinkedObject linkedObject in GetComponentsInChildren<LinkedObject>()) 
//				if (gameObjects.ContainsKey(linkedObject.id)) {
//					// update
//					GameObject gameObject = gameObjects [shadowObject.id];
//					shadowObject.ToReal (gameObject);
//				}
//				else if (prefabs.ContainsKey(linkedObject.uid)) {
//					// spawn
//					GameObject gameObject = (GameObject)Instantiate (prefabs[shadowObject.uid]);
//					LinkedObject linkedObject = gameObject.AddComponent<LinkedObject>();
//					linkedObject.id = shadowObject.id;
//					linkedObject.uid = shadowObject.uid;
//					gameObjects.Add (shadowObject.id, gameObject);
//				}
//			}
//			shadow.FromReal (this.transform);
		}


//			foreach (ShadowGameObject shadowGameObject in GetComponentsInChildren<ShadowGameObject>()) {
				//				shadowGameObject.id = id++;
				////				shadowGameObject.name = shadowGameObject.;
				//				objects.Add (shadowGameObject);
				//			}
				//			int id = 0;
				//			foreach (Transform transform in GetComponentInChildren<Transform>()) {
				//				GameObject gameObject = transform.gameObject;
				//				gameObject.AddComponent<ShadowGameObject> ();
				//			}
//		private bool ObjectAdd(GameObject gameObject) {
//			
//		}
	}
}