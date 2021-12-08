using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Texture : MonoBehaviour
{
    public Transform scrollableTextureMenu;
    public GameObject prefabs;
    public Text title;

    private int lastClicked = -1;
    private List<TextureButton> _buttons = new List<TextureButton>();


    public void Start()
    {
        OwlSceneManager owlSceneManagerInstance = OwlSceneManager.instance;
        owlSceneManagerInstance.EventUserTextureIsLoaded += NewTextureIsLoaded;
        owlSceneManagerInstance.EventModelUpdated += ClearChoices;

        List<Texture2D> textures = owlSceneManagerInstance.getAllTextures();

        int idx = 0;
        foreach (Texture2D texture in textures)
        {
            TextureButton newButton = CreateNewButtonForTexture(texture, idx);
            _buttons.Add(newButton);
            idx += 1;
        }
    }

    private TextureButton CreateNewButtonForTexture(Texture2D texture, int idx)
    {
        GameObject textureButton = Instantiate(prefabs, scrollableTextureMenu);
        textureButton.transform.SetAsFirstSibling();

        TextureButton button = textureButton.GetComponentInChildren<TextureButton>();
        button.Init(texture, idx, this);
        return button;
    }

    private void _ChangeButtonTextColor(int idx)
    {
        if (lastClicked != -1)
            _buttons[lastClicked].ToggleColor();
        _buttons[idx].ToggleColor();

        lastClicked = idx;
    }

    public void ChildIsClicked(int idx)
    {
        if (lastClicked == idx)
            return;
        OwlSceneManager.instance.ChangeSelectedModelsTexture(idx);
        title.text = _buttons[idx].GetName();
        this._ChangeButtonTextColor(idx);
    }

    public void ClearChoices()
    {
        if (lastClicked == -1)
            return;
        title.text = "basic.png";
        _buttons[lastClicked].ToggleColor();
        lastClicked = -1;
    }

    public void NewTextureIsLoaded(Texture2D texture)
    {
        int idx = _buttons.Count;
        TextureButton newButton = CreateNewButtonForTexture(texture, idx);
        _buttons.Add(newButton);
    }


}
