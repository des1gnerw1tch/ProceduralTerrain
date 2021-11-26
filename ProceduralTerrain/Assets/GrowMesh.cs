using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grows a mesh taller by changing its Y scale and adjusting position so it just looks like it grows
public class GrowMesh : MonoBehaviour {
	[SerializeField] private Window [] windows;
	[SerializeField] private float windowSeperation;
	[SerializeField] private bool growWindows;
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
		if (growWindows) {
			PlaceNewWindows ();
		}

	}

	// Places windows on a building
	//TODO: Have this be affected by seed as well!
	void PlaceNewWindows () {
		for (float i = windowSeperation; i < gainedHeight * 1.5; i += windowSeperation) {
			foreach (Window window in windows) {
				GameObject newWindow = Instantiate (window.gameObject, this.transform.parent);
				Vector3 pos = window.transform.position;
				pos.y += i;
				newWindow.transform.position = pos;
				newWindow.GetComponent<Window> ().RandomizeState ();
			}
		}
	}
}
