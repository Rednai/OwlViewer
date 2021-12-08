using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
