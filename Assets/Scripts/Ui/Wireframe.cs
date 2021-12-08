using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wireframe : MonoBehaviour
{
    private bool isWireframeOn = false;

    private void OnPreRender()
    {
        GL.wireframe = isWireframeOn;
    }
    private void OnPostRender()
    {
        GL.wireframe = false;
    }
    
    /// <summary>
    /// Toggle wireframe mode
    /// </summary>
    public void ToggleWireframe()
    {
        isWireframeOn = !isWireframeOn;
    }
}
