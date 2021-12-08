using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
    public InputField x;
    public InputField y;
    public InputField z;
    
    private OwlViewerModel model = null;

    private void Start()
    {
        x.onEndEdit.AddListener(SetNewScale);
        y.onEndEdit.AddListener(SetNewScale);
        z.onEndEdit.AddListener(SetNewScale);
    }

    public void UpdateModel()
    {
        model = OwlSceneManager.instance.selectedModels[0];
    }

    private void UpdateModelScale()
    {
        if (model == null)
            return;
        if (!x.isFocused)
            x.text = model.transform.localScale.x.ToString("0.00");
        if (!y.isFocused)
            y.text = model.transform.localScale.y.ToString("0.00");
        if (!z.isFocused)
            z.text = model.transform.localScale.z.ToString("0.00");
    }

    private void SetNewScale(string value)
    {
        float fValue;
        if (float.TryParse(value, out fValue))
        {
            Vector3 scale = new Vector3(float.Parse(x.text), float.Parse(y.text), float.Parse(z.text));
            model.transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        UpdateModelScale();
    }
}
