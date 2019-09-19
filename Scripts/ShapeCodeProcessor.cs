using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ShapeCodeProcessor
{

    //Does both functions
    public static List<Vector2Int> RotateAndGiveCoordinates(int[] _shapeCode, Vector2Int _fromCoordinate)
    {

        return ShapeCodeToCoordinates(RotateShapeCode(_shapeCode), _fromCoordinate);
    }

    //Transforms shapeCode to coordinates based on the given coordinate
    public static List<Vector2Int> ShapeCodeToCoordinates(int[] _shapeCode, Vector2Int _fromCoordinate)
    {

        if (_shapeCode.Length == 8)
        {
            List<Vector2Int> _coordList = new List<Vector2Int>();

            for (int i = 0; i < _shapeCode[0]; i++)
            {

                _coordList.Add(new Vector2Int(_fromCoordinate.x, _fromCoordinate.y + i));                       //UP
            }

            for (int i = 0; i < _shapeCode[1]; i++)
            {
                _coordList.Add(new Vector2Int(_fromCoordinate.x + i, _fromCoordinate.y));                       //RIGHT
            }

            for (int i = 0; i < _shapeCode[2]; i++)
            {

                _coordList.Add(new Vector2Int(_fromCoordinate.x, _fromCoordinate.y - i));                       //DOWN
            }

            for (int i = 0; i < _shapeCode[3]; i++)
            {
                _coordList.Add(new Vector2Int(_fromCoordinate.x - i, _fromCoordinate.y));                       //LEFT
            }

            //Hoping this saves perfomance
            if (_shapeCode[4] + _shapeCode[5] + _shapeCode[6] + _shapeCode[7] != 0)
            {
                for (int i = 0; i < _shapeCode[4]; i++)
                {

                    _coordList.Add(new Vector2Int(_fromCoordinate.x - i, _fromCoordinate.y + i));       //UP-LEFT
                }

                for (int i = 0; i < _shapeCode[5]; i++)
                {
                    _coordList.Add(new Vector2Int(_fromCoordinate.x + i, _fromCoordinate.y + i));       //UP-RIGHT
                }

                for (int i = 0; i < _shapeCode[6]; i++)
                {

                    _coordList.Add(new Vector2Int(_fromCoordinate.x + i, _fromCoordinate.y - i));       //DOWN-RIGHT
                }

                for (int i = 0; i < _shapeCode[7]; i++)
                {
                    _coordList.Add(new Vector2Int(_fromCoordinate.x - i, _fromCoordinate.y - i));       //DOWN-LEFT
                }
            }

            return _coordList;
        } else
        {
            Debug.LogError("You have used a ShapeCode which does not have an array length of 8");
            return null;
        }

    }

    //Rotates the shapeCode one to the right
    public static int[] RotateShapeCode(int[] _shapeCode)
    {

        int[] _newShapeCode = new int[] { _shapeCode[3], _shapeCode[0], _shapeCode[1], _shapeCode[2], _shapeCode[7], _shapeCode[4], _shapeCode[5], _shapeCode[6] };

        return _newShapeCode;
    }

}

