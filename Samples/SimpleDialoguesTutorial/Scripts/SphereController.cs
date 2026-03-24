using System;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public bool IsBlue;
    private Renderer rend;
    
    public static SphereController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        rend.material.color = IsBlue ? Color.red : Color.blue;
        IsBlue = !IsBlue;
    }
    
    
}
