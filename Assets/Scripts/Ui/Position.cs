using UnityEngine;
using UnityEngine.UI;

public class Position : MonoBehaviour
{
    public InputField x;
    public InputField y;
    public InputField z;
    public OrbitCamera orbitCamera;

    private OwlViewerModel model = null;

    private void Start()
    {
        x.onEndEdit.AddListener(SetNewPosition);
        y.onEndEdit.AddListener(SetNewPosition);
        z.onEndEdit.AddListener(SetNewPosition);
    }

    public void UpdateModel()
    {
        model = OwlSceneManager.instance.selectedModels[0];
    }

    private void UpdateModelPosition()
    {
        if (model == null)
            return;
        if (!x.isFocused)
            x.text = model.transform.position.x.ToString("0.00");
        if (!y.isFocused)
            y.text = model.transform.position.y.ToString("0.00");
        if (!z.isFocused)
            z.text = model.transform.position.z.ToString("0.00");
    }

    private void SetNewPosition(string value)
    {
        float fValue;
        if (float.TryParse(value, out fValue))
        {
            Vector3 pos = new Vector3(float.Parse(x.text), float.Parse(y.text), float.Parse(z.text));
            model.transform.position = pos;
            orbitCamera.UpdateCamera(true);
        }
    }

    private void FixedUpdate()
    {
        UpdateModelPosition();
    }
}
