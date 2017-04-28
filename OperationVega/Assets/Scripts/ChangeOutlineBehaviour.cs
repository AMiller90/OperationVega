using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOutlineBehaviour : MonoBehaviour {


    
    Material material;
    Color defaultColor;
    public Color targetColor = Color.green;

    static class Uniforms { internal static readonly int colorUniform = Shader.PropertyToID("_OutlineColor"); }

    void Start()
    {
        material = GetComponentInChildren<Renderer>().materials[0];
        defaultColor = material.GetColor(Uniforms.colorUniform);
    }
    

    public void ChangeOutlineColor(bool on)
    {
        
        material.SetColor(Uniforms.colorUniform, targetColor);
        print("color");

       if(on == false)
        {
            material.SetColor(Uniforms.colorUniform, Color.black);
        }
        
    }
    
  
    
}
