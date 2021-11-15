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

	[SerializeField] private int xSize = 40;
	[SerializeField] private int zSize = 40;
	[SerializeField] private int gridScale = 1;
	[SerializeField] private int streetEveryWhatVertices = 3;

	[SerializeField] PlaceObjectsFromGrid gridPlacer;

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

		gridPlacer.PlaceStreets (this.vertices, this.xSize, this.zSize); // uses this matrix to place objects

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
