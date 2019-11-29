using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private InputField[] textField = null;
    [SerializeField] private Image colorImage = null;
    [SerializeField] private Material colorsMTL = null;
    [SerializeField] private Material wheelMTL = null;
    [SerializeField] private TextureCamera textureCamera = null;
    [SerializeField] private ColorPicker picker = null;

    private static Color selectColor;
    private List<Color> colors;
    private const int colorNum = 36;
    private int idx = 0;
    private static bool modify = false;
    private Vector2 paletteCenter;
    private float paletteRadius = 144f;
    private int selectIdx = 0;
    void Start ()
    {
        UIInitializate();
        textureCamera.PaletteTransformInitialize();        
    }
    public void UIInitializate()
    {
        colors = new List<Color>();
        for (int i = 0; i < colorNum; i++)
            colors.Add(Color.black);
        paletteCenter = new Vector2(628,255);
    }
	public void UIUpdate()
    {
        try
        {
            if (modify)//Set text field
            {               
                textField[0].text = (int)(selectColor.r * 255) + "";
                textField[1].text = (int)(selectColor.g * 255) + "";
                textField[2].text = (int)(selectColor.b * 255) + "";
                textField[3].text = (int)(selectColor.a * 255) + "";               
                modify = false;
            }
            else
            {
                selectColor = new Color(float.Parse(textField[0].textComponent.text) / 255, float.Parse(textField[1].textComponent.text) / 255, float.Parse(textField[2].textComponent.text) / 255, float.Parse(textField[3].textComponent.text) / 255);
            }
            colorImage.color = selectColor;
        }
        catch (FormatException e)
        {
            colorImage.color = Color.black;
            print(e);
        }

        if (Input.GetMouseButtonDown(0))//Left button click
        {
            Vector3 mpos = Input.mousePosition;            
            if (mpos.x > 390 && mpos.x < 866)
            {
                if (mpos.y > 20 && mpos.y < 490)
                {
                    float d = Vector2.Distance(mpos, paletteCenter);
                    if (d > paletteRadius)
                    {                        
                        float x = (mpos.x - paletteCenter.x);
                        float y = (mpos.y - paletteCenter.y);
                        float theta = Mathf.Atan2(y,x) * Mathf.Rad2Deg;
                        if (theta < 0)
                            theta = 360 + theta;
                        selectIdx = (int)(theta / (360f / (idx)));
                        SetSelectColor(colors[selectIdx]);                        
                    }
                }
            }
        }
    }
    public void AddColors()
    {
        if(idx < 36)
        {
            colors[idx] = selectColor;
            colorsMTL.SetInt("_Count", ++idx);
            colorsMTL.SetColorArray("_Color", colors.ToArray());
            wheelMTL.SetInt("_Count", idx);
            wheelMTL.SetColorArray("_Color", colors.ToArray());
        }
    }
    public void RemoveColor()
    {
        if(idx < 36)
        {
            if (idx > 0)
            {
                colors.RemoveAt(selectIdx);
                idx--;
                colors.Add(Color.black);
                colorsMTL.SetColorArray("_Color", colors.ToArray());
                wheelMTL.SetColorArray("_Color", colors.ToArray());
                colorsMTL.SetInt("_Count", idx);
                wheelMTL.SetInt("_Count", idx);
            }
        }        
    }
    
    public void GeneratePalette()
    {        
        textureCamera.SetCapture(true);
    }
    public void ShowColorPicker()
    {        
        picker.ShowColorPicker();
    }
    public void SetSelectColor(Color c)
    {
        selectColor = c;        
        modify = true;
    }
    void Update ()
    {
        UIUpdate();       
    }
    //class ColorPicker : EditorWindow
    //{
    //    Color c = Color.white;
    //    public static void Init()
    //    {
    //        EditorWindow window = GetWindow(typeof(ColorPicker));
    //        window.Show();
    //    }
    //    void OnGUI()
    //    {           
    //        c = EditorGUILayout.ColorField("Color", selectColor);
            
    //        if (GUILayout.Button("Select"))
    //        {
    //            selectColor = c;
    //            modify = true;
    //        }
    //    }        
    //}
}