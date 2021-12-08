using UnityEngine;
using TMPro;

public class Inspector : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Position position;
    public Rotation rotation;
    public Scale scale;

    private OwlViewerModel model;

    /// <summary>
    /// When the inspector is enable, we get the selected model from the scene manager
    /// </summary>
    public void Init()
    {
        gameObject.SetActive(true);

        model = OwlSceneManager.instance.selectedModels[0];
        position.UpdateModel();
        rotation.UpdateModel();
        scale.UpdateModel();

        title.text = model.modelName;
    }
}
