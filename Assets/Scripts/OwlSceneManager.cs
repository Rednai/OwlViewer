using System;
using System.IO;    
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using Dummiesman;
using RuntimeHandle;

public class OwlSceneManager : MonoBehaviour
{
    static public OwlSceneManager instance;
    public Inspector inspector;
    public GameObject modelPrefab;
    public OrbitCamera orbitCamera;
    public RuntimeTransformHandle runtimeTransform;
    public List<OwlViewerModel> selectedModels = new List<OwlViewerModel>();
    private TexturesLoader _texturesLoader;

    public Action<Texture2D> EventUserTextureIsLoaded;
    public Action EventModelUpdated;

    private class ExtensionHandler
    {
        public string extension;
        public Func<String, GameObject> handler;
    }
    private List<ExtensionHandler> extensionHandlers;
    private List<OwlViewerModel> models = new List<OwlViewerModel>();

    private void Awake()
    {
        instance = this;
        _texturesLoader = new TexturesLoader();

        extensionHandlers = new List<ExtensionHandler>() {
            new ExtensionHandler() { extension = ".obj", handler = OpenObj },
            new ExtensionHandler() { extension = ".stl", handler = OpenStl }
        };
    }


    public void ChangeSelectedModelsTexture(int index)
    {
        Texture2D texture = _texturesLoader.GetTextureFromIndex(index);
        
        foreach(OwlViewerModel model in selectedModels)
        {
            model.self.Traverse((Node child) =>
            {
                if (child is null)
                {
                    throw new ArgumentNullException(nameof(child));
                }

                MeshRenderer meshRenderer = child.transform.GetComponent<MeshRenderer>();
                if (meshRenderer)
                    meshRenderer.material.mainTexture = _texturesLoader.GetTextureFromIndex(index);

            });
        }
            
    }

    /// <summary>
    /// Open an 3D object and add it to the scene.
    /// </summary>
    public void OpenObject()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Objects", ".obj", ".stl"));
        FileBrowser.ShowLoadDialog((string[] paths) => {
            // Ask the user for the 3D object path
            string path = paths[0];
            if (path.Length == 0)
                return;

            // Check if the object extension is supported
            string extension = Path.GetExtension(path).ToLower();
            ExtensionHandler extensionHandler = extensionHandlers.Find(i => i.extension == extension);
            if (extensionHandler == null)
            {
                // TODO : ajouter message d'erreur quand le format n'est pas supporté
                return;
            }
            // Load the 3D object
            GameObject obj = extensionHandler.handler(path);
            if (obj == null)
            {
                // TODO : ajouter message d'erreur quand l'objet ne peut pas être ouvert
                return;
            }
            // Add the 3D object to the scene
            AddObjectToScene(obj, Path.GetFileNameWithoutExtension(path));
        }, () => { }, FileBrowser.PickMode.Files);
    }

    /// <summary>
    /// Select or unselect a model
    /// </summary>
    /// <param name="model">Model to select / unselect</param>
    public void SelectModel(OwlViewerModel modelToSelect)
    {
        OwlViewerModel model = selectedModels.Find(i => i.modelName == modelToSelect.modelName);

        if (model == null)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
                selectedModels.Clear();
            selectedModels.Add(modelToSelect);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
            selectedModels.Remove(model);
        else if (selectedModels.Count != 1)
        {
            selectedModels.Clear();
            selectedModels.Add(modelToSelect);
        }
        else
            return;

        if (selectedModels.Count == 1)
        {
            inspector.Init();
            runtimeTransform.gameObject.SetActive(true);
            runtimeTransform.target = selectedModels[0].gameObject.transform;
            
            EventModelUpdated?.Invoke();
        }
        else
        {
            inspector.gameObject.SetActive(false);
            runtimeTransform.gameObject.SetActive(false);
        }
    }

    public List<Texture2D> getAllTextures()
    {
        return _texturesLoader.textures;
    }

    public void GetUserTexture()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Objects", ".DDS", ".TGA", ".BMP", ".PNG", ".JPG", ".CRN"));
        FileBrowser.ShowLoadDialog((string[] paths) => {
            string path = paths[0];
            if (path.Length == 0)
                return;
            Texture2D newTextureLoaded = _texturesLoader.AddNewTexture(path);
            if (!newTextureLoaded)
                return;
            EventUserTextureIsLoaded(newTextureLoaded);
        }, () => { }, FileBrowser.PickMode.Files);
    }

    /// <summary>
    /// Add the opened 3D object to the scene.
    /// </summary>
    /// <param name="obj">3D object to add</param>
    private void AddObjectToScene(GameObject obj, string name)
    {
        GameObject model = Instantiate(modelPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        obj.transform.parent = model.transform;
        obj.transform.position = new Vector3(0, 0, 0);

        model.GetComponent<OwlViewerModel>().modelName = VerifyName(name);

        // Combine children meshes for the mesh collider to detect mouse click
        MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Mesh mesh = new Mesh();

        for (int i = 0;  i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        mesh.CombineMeshes(combine);
        model.GetComponent<MeshCollider>().sharedMesh = mesh;

        models.Add(model.GetComponent<OwlViewerModel>());

        // Focus the camera on the new model
        orbitCamera.target = model;
        orbitCamera.UpdateCamera(true);

        // Open the new model in the inspector and select it
        SelectModel(model.GetComponent<OwlViewerModel>());
    }

    /// <summary>
    /// Check if a model has the same name and return an unique name
    /// </summary>
    /// <param name="name">Model name</param>
    /// <returns>Return an unique name</returns>
    private string VerifyName(string name, int index = 0)
    {
        string nameToTest = name;

        if (index != 0)
            nameToTest += index;

        foreach (OwlViewerModel model in models)
        {
            if (model.modelName == nameToTest)
            {
                return VerifyName(name, index + 1);
            }
        }

        return nameToTest;
    }

    /// <summary>
    /// Load a .obj file with the given path.
    /// </summary>
    /// <param name="path">Path to the .obj file</param>
    /// <returns>Return a GameObject representing the .obj file.</returns>
    private GameObject OpenObj(string path)
    {
        return new OBJLoader().Load(path);
    }

    /// <summary>
    /// Load a .stl file with the given path.
    /// </summary>
    /// <param name="path">Path to the .stl file</param>
    /// <returns>Return a GameObject representing the .stl file.</returns>
    private GameObject OpenStl(string path)
    {
        GameObject importedObj = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        MeshFilter meshFilter = importedObj.AddComponent(typeof(MeshFilter)) as MeshFilter;

        Mesh[] meshes = Parabox.Stl.Importer.Import(path, smooth: true, indexFormat: UnityEngine.Rendering.IndexFormat.UInt32);
        CombineInstance[] combine = new CombineInstance[meshes.Length];

        for (int i = 0; i < meshes.Length; i++)
            combine[i].mesh = meshes[i];

        meshFilter.mesh = new Mesh();
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh.CombineMeshes(combine, true, false);

        importedObj.name = Path.GetFileNameWithoutExtension(path);
        if (!importedObj.GetComponent<MeshRenderer>())
        {
            MeshRenderer meshRenderer = importedObj.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            meshRenderer.material = new Material(Shader.Find("Diffuse"));
        }

        return importedObj;
    }

    /// <summary>
    /// Destroy all the object
    /// </summary>
    public void Clear()
    {
        foreach (OwlViewerModel model in models)
            Destroy(model.gameObject);
        selectedModels.Clear();
        models.Clear();
        inspector.gameObject.SetActive(false);
        runtimeTransform.gameObject.SetActive(false);
        orbitCamera.target = gameObject;
        orbitCamera.UpdateCamera(true);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && selectedModels.Count == 1)
        {
            orbitCamera.target = selectedModels[0].gameObject;
            orbitCamera.UpdateCamera(true);
        }
    }
}
