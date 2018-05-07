using UnityEngine;
using UnityEditor;
using RealityLink;

[CustomEditor(typeof(Controller<TransportSocketIO>), true)]

public class ControllerEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		Controller<TransportSocketIO> controller = (Controller<TransportSocketIO>)target;
		if (GUILayout.Button("Connect")) {
			controller.DoConnect ();
		}
		if (GUILayout.Button("Disconnect")) {
			controller.DoDisconnect ();
		}
	}
}