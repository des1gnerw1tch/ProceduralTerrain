using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {
	// Start is called before the first frame update
	Mesh mesh;
	[SerializeField] private MeshFilter meshFilter;

	[SerializeField] Vector3 [] vertices;
	int [] triangles;

	[SerializeField] private int xSize = 20;
	[SerializeField] private int zSize = 20;

	void Start () {
		mesh = new Mesh ();
		meshFilter.mesh = this.mesh;

		CreateShape ();
		//UpdateMesh ();
	}


	// creates the shape of the mesh
	private void CreateShape () {
		vertices = new Vector3 [(xSize + 1) * (zSize + 1)];


		for (int z = 0, i = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++) {
				//vertices [(z * 4) + x] = new Vector3 (x, 0, z);
				vertices [i] = new Vector3 (x, 0, z);
				i++;
			}
		}
	}

	// updates the mesh in unity
	private void UpdateMesh () {
		throw new NotImplementedException ();
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
