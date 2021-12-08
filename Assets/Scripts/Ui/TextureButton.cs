using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextureButton : MonoBehaviour
{

    public Text text;
    public Button self;
    public Color actived;
    public Color desactived;

    private Texture2D _associatedTexture;
    private Texture _parent;
    private bool isClicked = false;
    private int _idx = 0;
    // Start is called before the first frame update

    public void ModifyText(string newText)
    {
        text.text = newText;
    }

    public void Init(Texture2D texture, int idx, Texture parent)
    {
        _associatedTexture = texture;
        _idx = idx;
        _parent = parent;
        self.onClick.AddListener(OnClick);
        ModifyText(texture.name);
    }

    public void ToggleColor()
    {
        text.color = (isClicked == true) ? desactived : actived;
        isClicked = !isClicked;
    }


    public void OnClick( )
    {
        _parent.ChildIsClicked(_idx);
    }

    public string GetName()
    {
        return this._associatedTexture.name;
    }
    

    // Update is called once per frame
}
