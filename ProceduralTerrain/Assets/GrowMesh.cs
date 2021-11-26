using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grows a mesh taller by changing its Y scale and adjusting position so it just looks like it grows
public class GrowMesh : MonoBehaviour {
	[SerializeField] private Window [] windows;
	[SerializeField] private float windowSeperation;
	private float gainedHeight = 0;

	// Grows the Y scale of this object bottom up
	public void GrowScaleY (float x) {
		Vector3 scale = this.transform.localScale;
		scale.y += x;

		Vector3 pos = this.transform.position;
		pos.y += x / 2; // from my experiments
		this.transform.localScale = scale;
		this.transform.position = pos;
		gainedHeight = x;
		PlaceNewWindows ();
	}

	// Places windows on a building
	void PlaceNewWindows () {
		for (float i = windowSeperation; i < gainedHeight; i += windowSeperation) {
			foreach (Window window in windows) {
				GameObject newWindow = Instantiate (window.gameObject);
				Vector3 pos = window.transform.position;
				pos.y += i;
				newWindow.transform.position = pos;
			}
		}
	}
}
