using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Owl : MonoBehaviour
{
    public GameObject settings;
    private Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<Dropdown>();
    }

    public void OnValueChanged()
    {
        if (dropdown.value == dropdown.options.Count - 1)
            return;

        switch (dropdown.options[dropdown.value].text)
        {
            case "Open":
                OwlSceneManager.instance.OpenObject();
                break;
            case "Clear":
                OwlSceneManager.instance.Clear();
                break;
            case "Settings":
                settings.SetActive(!settings.activeSelf);
                break;
            case "Exit":
                Application.Quit();
                break;
            default:
                break;
        }

        dropdown.value = dropdown.options.Count - 1;
    }
}
