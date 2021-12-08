using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.Stl;

public class StlFromFile : MonoBehaviour
{
    private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        
        Mesh[] meshes = Parabox.Stl.Importer.Import("C:\\Users\\hugob\\Desktop\\DeLorean.STL");
        //Mesh[] meshes = Parabox.Stl.Importer.Import("C:\\Users\\hugob\\Downloads\\Eiffel_tower_sample.STL");
        //Mesh[] meshes = Parabox.Stl.Importer.Import("C:\\Users\\hugob\\Desktop\\teapot.stl");
        CombineInstance[] combine = new CombineInstance[meshes.Length];

        Debug.Log("Length : " + meshes.Length);

        for (int i = 0; i < meshes.Length; i++)
        {
            Debug.Log("vertices : " + meshes[i].vertices.Length);
            Debug.Log("triangles : " + meshes[i].triangles.Length);
        
            combine[i].mesh = meshes[i];
            //combine[i].transform = meshFilter.transform.localToWorldMatrix;
        }

        meshFilter.mesh = new Mesh();
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh.CombineMeshes(combine, true, false);
    }
}
