using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetGrid : MonoBehaviour {
	// Start is called before the first frame update
	Mesh mesh;

	/*
	 * For our vertices, a y value of -1 is STREET
	 * y value of > 1 is height of building
	 * y value of -2 is an EMPTY SPACE, no street and no building, maybe TODO: Tree?
	 */
	Vector3 [,] vertices;

	[SerializeField] private int xSize = 40;
	[SerializeField] private int zSize = 40;
	[SerializeField] private int gridScale = 1; // would we like to make our grid bigger? (dont change this)
	[SerializeField] private int streetEveryWhatVertices = 3; // 1 street every what amount of vertices? 

	[SerializeField] PlaceObjectsFromGrid gridPlacer; // this is how we place items onto the grid

	[Header ("Areas")]

	[Range (0.0f, 1f)]
	[SerializeField] private float percentCityDowntown; // what percentage of city to be downtown? 
	[SerializeField] private float downtownBuildingSizeMultiplier; // how much larger should downtown buildings be then normal buldings?

	[Range (0.0f, 1f)]
	[SerializeField] private float percentCitySuburb; // what percentage city to be a suburb? (smaller buildings)
	[SerializeField] private float suburbBuildingSizeMultiplier; // how much smaller should suburb buildings be than normal buildings?
	[Range (0.0f, 1f)]
	[SerializeField] private float suburbanFullness; // how full is this suburb with buildings?

	[Header ("Perlin Noise Generator")]
	[SerializeField] private float scale; // scale of this perlin noise, how close you want data to be
	[SerializeField] private float amplitude; // how much we would like to amplify this noise value by
	[SerializeField] private int perlinSeed;
	[SerializeField] private bool randomizeSeed; // should I randomize this seed? 


	void Start () {
		mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = this.mesh;
		StartCoroutine ("CreateGrid");
	}

	// creates the grid of the street, with streets and buildings
	IEnumerator CreateGrid () {
		vertices = new Vector3 [xSize, zSize];
		if (randomizeSeed) {
			perlinSeed = Random.Range (1, 10000); // seed for perlin noise generated
		}

		Random.InitState (perlinSeed); // our random number generator will have the same seed as our perlin value
		int seed = Random.Range (0, streetEveryWhatVertices); // for offsetting so street placement is different every time
		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < zSize; z++) {

				float y; // what the y value of our vertice will be
				float perlin = 0; // value got from perlin
				if (((x + seed) % streetEveryWhatVertices == 0) || ((z + seed) % streetEveryWhatVertices == 0)) {
					y = -1; // is a street
				} else {
					perlin = Mathf.PerlinNoise (perlinSeed + (x * scale), perlinSeed + (z * scale));
					y = Mathf.PerlinNoise (perlinSeed + (x * scale), perlinSeed + (z * scale)) * amplitude;
				}

				if (perlin > 1 - percentCityDowntown) { // if building is in the upper % of sizes, make it DOWNTOWN, even bigger
					y *= downtownBuildingSizeMultiplier;
				} else if (perlin < percentCitySuburb && perlin > 0) { // if building is in the lower 20%, make them extra small
					if (Random.Range (0f, 1f) > suburbanFullness) {
						y = -2;
					} else {
						y *= suburbBuildingSizeMultiplier;
					}

				}

				vertices [x, z] = new Vector3 (gridScale * x, y, gridScale * z);
			}
			yield return new WaitForSeconds (.00001f);
		}

		gridPlacer.PlaceStreets (this.vertices, this.xSize, this.zSize); // uses this matrix to place objects

	}

	// so you can see dots on scene
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
