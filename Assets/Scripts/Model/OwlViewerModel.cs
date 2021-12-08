using UnityEngine;

public class OwlViewerModel : MonoBehaviour
{
    private Euler _rotation;

    public Node self;
    public string modelName = "";

    public void Awake()
    {
        _rotation = new Euler();
    }

    public void Start()
    {
        self = new Node(this.transform);

    }

    public void rotate(float x = 0, float y = 0, float z = 0)
    {
        _rotation.x += x;
        _rotation.y += y;
        _rotation.z += z;
        transform.eulerAngles = _rotation;
    }

    public void translate(float x = 0, float y = 0, float z = 0)
    {
        transform.Translate(x, y, z);
    }

    public void scale(float x = 0, float y = 0, float z = 0)
    {
        transform.localScale += new Vector3(x, y, z);
    }

    public void shear()
    {
        MeshFilter[] MeshFilterOfChildren = this.transform.GetComponentsInChildren<MeshFilter>();

        foreach(MeshFilter meshFilter in MeshFilterOfChildren)
        {
            Vector3[] vertices = meshFilter.mesh.vertices;

            Vector3 shear = new Vector3(10, 0, 10);

            for (int i = 0; i < vertices.Length; i++)
            {
                meshFilter.mesh.vertices[i] = meshFilter.mesh.vertices[i] + shear * meshFilter.mesh.vertices[i].y;
            }

            meshFilter.mesh.RecalculateBounds();
            meshFilter.mesh.RecalculateNormals();
        }

    }

    /// <summary>
    /// Select or unselect the model
    /// </summary>
    private void OnMouseDown()
    {
        OwlSceneManager.instance.SelectModel(this);
    }
}
