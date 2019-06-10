using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class Gray : MonoBehaviour
{
    private static Queue<Material> materials = new Queue<Material>();

    [SerializeField, HideInInspector]
    private bool isGray = false;
    private Material material;
    private Graphic graphic;

    public void EnableGray()
    {
        isGray = true;
        material.EnableKeyword("UNITY_UI_GRAYSCALE");
    }
    public void DisableGray()
    {
        isGray = false;
        material.DisableKeyword("UNITY_UI_GRAYSCALE");
    }

    public void Init()
    {
        if (material == null)
        {
            if (materials.Count > 0)
                material = materials.Dequeue();
            else
                material = new Material(Shader.Find("UI/UIGray"));
        }
        if (graphic == null)
            graphic = GetComponent<Graphic>();
        graphic.material = material;
        if (isGray)
            material.EnableKeyword("UNITY_UI_GRAYSCALE");
        else
            material.DisableKeyword("UNITY_UI_GRAYSCALE");
    }
    public void Clear()
    {
        if (material != null)
        {
            materials.Enqueue(material);
            material = null;
        }
        if (graphic != null)
        {
            graphic.material = null;
            graphic = null;
        }
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        Clear();
    }
}
