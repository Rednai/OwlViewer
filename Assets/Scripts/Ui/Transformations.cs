using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using RuntimeHandle;

public class Transformations : MonoBehaviour
{
    public RuntimeTransformHandle runtimeTransform;

    private ToggleGroup toggleGroup;

    private void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void OnTransformationChange()
    {
        Toggle toggle = toggleGroup.ActiveToggles().First();

        switch (toggle.name)
        {
            case "Position":
                runtimeTransform.type = HandleType.POSITION;
                break;
            case "Rotation":
                runtimeTransform.type = HandleType.ROTATION;
                break;
            case "Scale":
                runtimeTransform.type = HandleType.SCALE;
                break;
            default:
                break;
        }
    }
}
