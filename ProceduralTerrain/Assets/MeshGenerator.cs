using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
public class MeshGenerator : MonoBehaviour {
	// Start is called before the first frame update
	Mesh mesh;

	Vector3 [] vertices;
	int [] triangles;

	[SerializeField] private int xSize = 40;
	[SerializeField] private int zSize = 40;

	private int newNoise;

	void Start () {
		mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = this.mesh;
		StartCoroutine ("CreateShape");
		UpdateMesh ();
	}

	private void Update () {
		UpdateMesh ();
	}

	// creates the shape of the mesh
	IEnumerator CreateShape () {
		this.newNoise = Random.Range (0, 10000);
		vertices = new Vector3 [(xSize + 1) * (zSize + 1)];

		for (int z = 0, i = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++) {
				float y = Mathf.PerlinNoise (newNoise + (x * .1f), newNoise + (z * .1f)) * 3f;
				vertices [i] = new Vector3 (x, y, z);
				i++;
			}
		}

		int vert = 0;
		int tris = 0;
		triangles = new int [(xSize * zSize) * 6];
		for (int z = 0; z < zSize; z++) {
			for (int x = 0; x < xSize; x++) {

				triangles [tris + 0] = vert + 0;
				triangles [tris + 1] = vert + xSize + 1;
				triangles [tris + 2] = vert + 1;

				triangles [tris + 3] = vert + 1;
				triangles [tris + 4] = vert + xSize + 1;
				triangles [tris + 5] = vert + xSize + 2;
				vert++;
				tris += 6;

			}
			yield return new WaitForSeconds (.000001f);
			vert++;
		}

		if (tris == triangles.Length) {
			yield return new WaitForSeconds (1f);
			StartCoroutine ("CreateShape");
		}

	}

	// updates the mesh in unity
	private void UpdateMesh () {
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals (); // for lighting
	}

	private void OnDrawGizmos () {
		if (vertices == null) {
			return;
		}

		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere (vertices [i], 0.1f);
		}
	}

}
