using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.TrueRandom;

public class ShapeHolder : Iinitialize
{
    private const int QUEUE_SIZE = 15;
    private const int SHAPE_LIST_SIZE = 12;
    private const int AMOUNT_OF_GLOWING_BLOCKS = 3;


    private Queue<int> sequenceQueue = new Queue<int>(QUEUE_SIZE);
    private List<Shape> availableShapes = new List<Shape>(SHAPE_LIST_SIZE);

    private Material blueMaterial, orangeMaterial, cyanMaterial, greenMaterial, redMaterial, yellowMaterial, magentaMaterial, glowingMaterial;

    public ShapeHolder(Material _blueMaterial, 
        Material _orangeMaterial, 
        Material _cyanMaterial,
        Material _greenMaterial, 
        Material _redMaterial, 
        Material _yellowMaterial, 
        Material _magentaMaterial, 
        Material _glowingMaterial)
    {
        blueMaterial = _blueMaterial;
        orangeMaterial = _orangeMaterial;
        cyanMaterial = _cyanMaterial;
        greenMaterial = _greenMaterial;
        redMaterial = _redMaterial;
        yellowMaterial = _yellowMaterial;
        magentaMaterial = _magentaMaterial;
        glowingMaterial = _glowingMaterial;
    }

    public void Initialize()
    {
        AddShapes();
        TRManager.GenerateSequence(0, SHAPE_LIST_SIZE -1, QUEUE_SIZE);
    }

    public Shape GetNextShape()
    {
        
        if (sequenceQueue.Count < 1)
        {
            return availableShapes[Random.Range(0, SHAPE_LIST_SIZE - 1)];
        } else
        {
            //Adds ints to the sequence if low
            if (sequenceQueue.Count <= 5)
            {
                TRManager.GenerateSequence(0, SHAPE_LIST_SIZE - 1, QUEUE_SIZE - sequenceQueue.Count);
            }
            return availableShapes[sequenceQueue.Dequeue()];
        }
    }

    private void OnEnable()
    {
        TRManager.OnGenerateSequenceStart += RandomSequenceStartMethod;
        TRManager.OnGenerateSequenceFinished += RandomSequenceFinishedMethod;
    }

    private void OnDestroy()
    {
        TRManager.OnGenerateIntegerStart -= RandomSequenceStartMethod;
        TRManager.OnGenerateIntegerFinished -= RandomSequenceFinishedMethod;
    }

    private void RandomSequenceStartMethod(string _id)
    {

    }

    private void RandomSequenceFinishedMethod(List<int> _result, string _id)
    {
        for (int i = 0; i < _result.Count; i++)
        {
            sequenceQueue.Enqueue(_result[i]);
        }
    }

    private void AddShapes()
    {
        availableShapes.Add(new Shape(new int[] { 0, 2, 2, 0, 0, 0, 2, 0 }, yellowMaterial, false));
        availableShapes.Add(new Shape(new int[] { 2, 2, 0, 2, 0, 0, 0, 0 }, magentaMaterial, false));
        availableShapes.Add(new Shape(new int[] { 3, 2, 0, 0, 0, 0, 0, 0 }, orangeMaterial, false));
        availableShapes.Add(new Shape(new int[] { 3, 0, 0, 2, 0, 0, 0, 0 }, blueMaterial, false));
        availableShapes.Add(new Shape(new int[] { 4, 0, 0, 0, 0, 0, 0, 0 }, cyanMaterial, false));
        availableShapes.Add(new Shape(new int[] { 0, 2, 2, 0, 0, 2, 0, 0 }, redMaterial, false));
        availableShapes.Add(new Shape(new int[] { 2, 2, 0, 0, 0, 0, 2, 0 }, greenMaterial, false));

        availableShapes.Add(new Shape(new int[] { 0, 0, 0, 0, 0, 0, 2, 2 }, glowingMaterial, true));
        availableShapes.Add(new Shape(new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, glowingMaterial, true));
        availableShapes.Add(new Shape(new int[] { 0, 0, 0, 0, 0, 0, 2, 0 }, glowingMaterial, true));
        availableShapes.Add(new Shape(new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, glowingMaterial, true));
        availableShapes.Add(new Shape(new int[] { 0, 0, 0, 0, 0, 0, 2, 0 }, glowingMaterial, true));
    }
}
