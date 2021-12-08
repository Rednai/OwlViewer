using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public InputField x;
    public InputField y;
    public InputField z;

    private OwlViewerModel model = null;

    private void Start()
    {
        x.onEndEdit.AddListener(SetNewRotation);
        y.onEndEdit.AddListener(SetNewRotation);
        z.onEndEdit.AddListener(SetNewRotation);
    }

    public void UpdateModel()
    {
        model = OwlSceneManager.instance.selectedModels[0];
    }

    private void UpdateModelRotation()
    {
        if (model == null)
            return;
        if (!x.isFocused)
            x.text = model.transform.eulerAngles.x.ToString("0.00");
        if (!y.isFocused)
            y.text = model.transform.eulerAngles.y.ToString("0.00");
        if (!z.isFocused)
            z.text = model.transform.eulerAngles.z.ToString("0.00");
    }

    private void SetNewRotation(string value)
    {
        float fValue;
        if (float.TryParse(value, out fValue))
        {
            Vector3 rotation = new Vector3(
                float.Parse(x.text.Replace('.', ',')),
                float.Parse(y.text.Replace('.', ',')),
                float.Parse(z.text.Replace('.', ','))
                );
            model.transform.eulerAngles = rotation;
        }
    }

    private void FixedUpdate()
    {
        UpdateModelRotation();
    }
}
