using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetGrid : MonoBehaviour {
	// Start is called before the first frame update
	Mesh mesh;

	/*
	 * For our vertices, a y value of -1 is STREET
	 * y value of 0-1 is height of building
	 */
	Vector3 [,] vertices;
	int [] triangles;

	[SerializeField] private int xSize = 40;
	[SerializeField] private int zSize = 40;
	[SerializeField] private int gridScale = 1;
	[SerializeField] private int streetEveryWhatVertices = 3;
	[SerializeField] private GameObject streetTile;
	[SerializeField] private GameObject intersectionTile;

	void Start () {
		mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = this.mesh;
		StartCoroutine ("CreateGrid");
	}

	// creates the grid of the street, with streets and buildings
	IEnumerator CreateGrid () {
		vertices = new Vector3 [xSize, zSize];


		int seed = Random.Range (0, streetEveryWhatVertices); // for offsetting so street placement is different every time
		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < zSize; z++) {
				int y = 0;
				if (((x + seed) % streetEveryWhatVertices == 0) || ((z + seed) % streetEveryWhatVertices == 0)) {
					y = -1; // is a street
				}
				vertices [x, z] = new Vector3 (gridScale * x, y, gridScale * z);
				yield return new WaitForSeconds (.00001f);
			}
		}

		StartCoroutine ("PlaceStreets");
	}

	// places streets prefabs onto scene
	IEnumerator PlaceStreets () {
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
					yield return new WaitForSeconds (.00001f);
				}
			}
		}

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

	private void OnDrawGizmos () {


		if (vertices == null) {
			return;
		}

		for (int x = 0; x < xSize; x++) {

			for (int z = 0; z < zSize; z++) {
				if (vertices [x, z].y == -1) {
					Gizmos.color = Color.black;
				} else {
					Gizmos.color = Color.white;
				}
				Vector3 toDraw = vertices [x, z];
				toDraw.y = 0;
				Gizmos.DrawSphere (toDraw, 0.1f);
			}

		}
	}

}
