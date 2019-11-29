using System.IO;
using UnityEngine;

public class TextureCamera : MonoBehaviour
{         
    private bool capture = false;
    
    public void PaletteTransformInitialize()
    {        
        //paletteTransform.position = new Vector3(Screen.width / 4 * 3, Screen.height / 3 * 1, 0);
    }
    public void SetCapture(bool b)
    {
        capture = b;
    }
    private void OnPostRender()
    {
        if (capture)
        {           
            Rect rect = new Rect(Screen.width / 100 * 48, Screen.height / 100 * 34, Screen.width / 100 * 48, Screen.height / 768 * 485 );//Wheel         
            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
            
            tex.ReadPixels(rect, 0, 0);
            tex.Apply();            
            byte[] bytes = tex.EncodeToPNG();
           
            Destroy(tex);           
            File.WriteAllBytes("Palette.png", bytes);
            capture = false;
        }
    }
}
