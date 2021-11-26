using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
// A window on a building... will randomize to whether this window is turned on or not
/// </summary>
public class Window : MonoBehaviour {
	[SerializeField] private Material onMaterial;
	[SerializeField] private Material offMaterial;
	[Range (0, 1)]
	[SerializeField] private float percentWindowsOn;

	// Either turns a window on or off
	public void RandomizeState () {
		MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		if (Random.Range (0f, 1f) < percentWindowsOn) {
			meshRenderer.material = onMaterial;
		} else {
			meshRenderer.material = offMaterial;
		}
	}
}
