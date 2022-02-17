using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// places objects from a generated StreetGrid
public class PlaceObjectsFromGrid : MonoBehaviour {
	private Vector3 [,] vertices;
	private int xSize;
	private int zSize;
	[SerializeField] private GameObject streetTile;
	[SerializeField] private GameObject intersectionTile;

	[SerializeField] private GameObject buildingTile;
	[SerializeField] private GameObject treeSelector;

	public void PlaceStreets (Vector3 [,] vertices, int xSize, int zSize) {
		this.vertices = vertices;
		this.xSize = xSize;
		this.zSize = zSize;
		StartCoroutine ("IPlaceStreets");
	}

	IEnumerator IPlaceStreets () {
		// for every tile in the scene
		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < zSize; z++) {
				// if is street tile
				if (vertices [x, z].y == -1) {
					Vector3 toDraw = vertices [x, z]; // where we will draw this tile in worldspace
					toDraw.y = 0.001f; // so street lays on top of grass

					GameObject instance;
					if (HasXNeighbor (x, z) && HasZNeighbor (x, z)) {
						instance = Instantiate (intersectionTile, toDraw, Quaternion.identity); // spawn Intersection
					} else {
						instance = Instantiate (streetTile, toDraw, Quaternion.identity); // spawn normal street
						if (HasXNeighbor (x, z)) {
							instance.transform.Rotate (new Vector3 (0, 90, 0)); // if has a X neighbor, we should rotate to match
						}
					}
					//yield return new WaitForSeconds (.00001f);
				}
			}
			yield return new WaitForSeconds (.00001f);
		}
		StartCoroutine ("IPlaceBuildings");
	}

	IEnumerator IPlaceBuildings () {
		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < zSize; z++) 
			{
				Vector3 toDraw = vertices [x, z]; // where we will draw this tile in worldspace
				// if is not street tile or empty tile!
				if (vertices [x, z].y != -1 && vertices [x, z].y != -2) {
					GameObject instance;
					
					toDraw.y = 0.001f; // so street lays on top of grass

					instance = Instantiate (buildingTile, toDraw, Quaternion.identity); // spawn building

					instance.GetComponentInChildren<GrowMesh> ().GrowScaleY (vertices [x, z].y);
					//yield return new WaitForSeconds (.00001f);
				} else if (vertices [x,z].y == -2) // if is empty tile, TODO: Spawn a tree!
				{
					if (Random.Range(0f, 1f) > .8) // spawn a tree sometimes
					{
						Instantiate(treeSelector, toDraw, Quaternion.identity);
					}
					
				}
			}
			//yield return new WaitForSeconds (.00001f);
		}
		yield return new WaitForSeconds (.00001f);
	}

	// checks if a street has neighbor on the X axis
	private bool HasXNeighbor (int x, int z) {
		int upperCheckIndex = x + 1;
		int lowerCheckIndex = x - 1;

		if ((upperCheckIndex < xSize) && vertices [upperCheckIndex, z].y == -1) { // is a street
			return true;
		} else if ((lowerCheckIndex >= 0) && vertices [lowerCheckIndex, z].y == -1) {
			return true;
		} else {
			return false;
		}
	}

	// checks if a street has neighbor on the X axis
	private bool HasZNeighbor (int x, int z) {
		int upperCheckIndex = z + 1;
		int lowerCheckIndex = z - 1;

		if ((upperCheckIndex < zSize) && vertices [x, upperCheckIndex].y == -1) { // is a street
			return true;
		} else if ((lowerCheckIndex >= 0) && vertices [x, lowerCheckIndex].y == -1) {
			return true;
		} else {
			return false;
		}
	}
}
