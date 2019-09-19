using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is a block
/// </summary>
public class Block : MonoBehaviour
{
    public Vector2Int coordinate;

    protected Renderer rend;

    public void Initialize(Vector2Int _coordinate)
    {
        rend = GetComponent<Renderer>();
        coordinate = _coordinate;
    }

    public void AssignMaterial(Material _material)
    {
        rend.material = _material;
    }
}
