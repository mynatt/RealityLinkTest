using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler
{
	new protected virtual void OnTrackingFound()
	{
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Enable rendering:
		foreach (var component in rendererComponents)
			component.enabled = true;

		// Enable colliders:
		foreach (var component in colliderComponents)
			component.enabled = true;

		// Enable canvas':
		foreach (var component in canvasComponents)
			component.enabled = true;
	}

	new protected virtual void OnTrackingLost()
	{
//		var rendererComponents = GetComponentsInChildren<Renderer>(true);
//		var colliderComponents = GetComponentsInChildren<Collider>(true);
//		var canvasComponents = GetComponentsInChildren<Canvas>(true);
//
//		// Disable rendering:
//		foreach (var component in rendererComponents)
//			component.enabled = false;
//
//		// Disable colliders:
//		foreach (var component in colliderComponents)
//			component.enabled = false;
//
//		// Disable canvas':
//		foreach (var component in canvasComponents)
//			component.enabled = false;
	}
}
