using UnityEngine;


public class ColorPicker : MonoBehaviour
{
    [SerializeField] private UIController uiController = null;
    [SerializeField] private bool useDefinedPosition = false;
    [SerializeField] private int positionLeft = 0;
    [SerializeField] private int positionTop = 0;
    // the solid texture which everything is compared against
    [SerializeField] private Texture2D colorPicker;        
    // the color that has been chosen
    [SerializeField] private Color setColor;

    private Color lastSetColor;
    // the picker being displayed
    private Texture2D displayPicker;
    [SerializeField] private bool useDefinedSize = false;
    [SerializeField] private int textureWidth = 360;
    [SerializeField] private int textureHeight = 120;

    private float saturationSlider = 0.0F;
    private Texture2D saturationTexture;

    private Texture2D styleTexture;

    [SerializeField] private bool showPicker = false;

    public void ShowColorPicker()
    {
        showPicker = true;
    }

    void Awake()
    {
        if (!useDefinedPosition)
        {
            positionLeft = (Screen.width / 2) - (textureWidth / 2);
            positionTop = (Screen.height / 2) - (textureHeight / 2);
        }

        // if a default color picker texture hasn't been assigned, make one dynamically
        if (!colorPicker)
        {
            colorPicker = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
            ColorHSV hsvColor;
            for (int i = 0; i < textureWidth; i++)
            {
                for (int j = 0; j < textureHeight; j++)
                {
                    hsvColor = new ColorHSV((float)i, (1.0f / j) * textureHeight, 1.0f);
                    colorPicker.SetPixel(i, j, hsvColor.ToColor());
                }
            }
        }
        colorPicker.Apply();
        displayPicker = colorPicker;

        if (!useDefinedSize)
        {
            textureWidth = colorPicker.width;
            textureHeight = colorPicker.height;
        }

        float v = 0.0F;
        float diff = 1.0f / textureHeight;
        saturationTexture = new Texture2D(20, textureHeight);
        for (int i = 0; i < saturationTexture.width; i++)
        {
            for (int j = 0; j < saturationTexture.height; j++)
            {
                saturationTexture.SetPixel(i, j, new Color(v, v, v));
                v += diff;
            }
            v = 0.0F;
        }
        saturationTexture.Apply();

        // small color picker box texture
        styleTexture = new Texture2D(1, 1);
        styleTexture.SetPixel(0, 0, setColor);
    }

    void OnGUI()
    {
        if (!showPicker) return;

        GUI.Box(new Rect(positionLeft - 3, positionTop - 3, textureWidth + 60, textureHeight + 60), "");

        if (GUI.RepeatButton(new Rect(positionLeft, positionTop, textureWidth, textureHeight), displayPicker))
        {
            int a = (int)Input.mousePosition.x;
            int b = Screen.height - (int)Input.mousePosition.y;

            setColor = displayPicker.GetPixel(a - positionLeft, -(b - positionTop));
            lastSetColor = setColor;
        }

        saturationSlider = GUI.VerticalSlider(new Rect(positionLeft + textureWidth + 3, positionTop, 10, textureHeight), saturationSlider, 1, -1);
        setColor = lastSetColor + new Color(saturationSlider, saturationSlider, saturationSlider);
        GUI.Box(new Rect(positionLeft + textureWidth + 20, positionTop, 20, textureHeight), saturationTexture);

        if (GUI.Button(new Rect(positionLeft + textureWidth - 60, positionTop + textureHeight + 10, 60, 25), "Select"))
        {
            setColor = styleTexture.GetPixel(0, 0);
            uiController.SetSelectColor(setColor);
            // hide picker
            showPicker = false;
        }

        // color display
        GUIStyle style = new GUIStyle();
        styleTexture.SetPixel(0, 0, setColor);
        styleTexture.Apply();

        style.normal.background = styleTexture;
        GUI.Box(new Rect(positionLeft + textureWidth + 10, positionTop + textureHeight + 10, 30, 30), new GUIContent(""), style);
    }

}