using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hierarchy : MonoBehaviour
{
    public void displayHierarchy()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
