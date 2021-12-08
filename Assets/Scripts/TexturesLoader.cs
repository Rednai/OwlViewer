using Dummiesman;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TexturesLoader
{
    public List<Texture2D> textures = new List<Texture2D>();

    public TexturesLoader()
    {

        var texturesArray = Resources.LoadAll("Textures/OwlViewer", typeof(Texture2D)).Cast<Texture2D>().ToArray();
        foreach (var t in texturesArray)
        {
            this.textures.Add(t);
        }
    }
    
    public Texture2D AddNewTexture(string path)
    {
        Texture2D newTexture = ImageLoader.LoadTexture(path);

        if (!newTexture)
        {
            // Show to user can't load texture
            return null;
        }
        textures.Add(newTexture);
        return newTexture;
    }

    public Texture2D GetTextureFromIndex(int index)
    {
        if (index >= textures.Count || index < 0)
        {
            return null;
        }
        return textures[index];
    }

}
