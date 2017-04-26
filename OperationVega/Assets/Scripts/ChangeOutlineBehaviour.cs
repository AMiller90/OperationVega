using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOutlineBehaviour : MonoBehaviour {


    int colorUniform;
    Material material;
    Color defaultColor;
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        defaultColor = material.GetColor(colorUniform);
    }
    static class Uniforms { internal static readonly int colorUniform = Shader.PropertyToID("_OutlineColor"); }

    public void ChangeOutlineColor(bool on)
    {
        
        material.SetColor("_OutlineColor", Color.green);
        print("color");

       if(on == false)
        {
            material.SetColor("_OutlineColor", Color.black);
        }
        
    }
    
  
    
}
