using UnityEngine;

public struct Shape
{

    public Shape(int[] _shapeOfShape, Material _materialOfShape, bool _isGlowing)
    {

        material = _materialOfShape;
        shape = _shapeOfShape;
        isGlowing = _isGlowing;
    }

    public Material material;
    public int[] shape;
    public bool isGlowing;
}
