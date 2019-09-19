using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockState
{
    None,
    Set,
    Emmissive,
    Glowing
}

/// <summary>
/// Is a block in the grid
/// </summary>
public class GridBlock : Block
{

    public BlockState state = BlockState.None;
    public int id = 0;

    public void SetState(BlockState _state)
    {
        state = _state;
        switch (state)
        {
            case BlockState.None:
                AssignShapeId(0);
                AssignMaterial(GameController.baseMaterial);
                break;
            case BlockState.Set:
                Color _color = rend.material.GetColor("_BaseColor");
                _color.a = 1;
                rend.material.SetColor("_BaseColor", _color);
                rend.material.DisableKeyword("_EMISSION");
                break;
            case BlockState.Emmissive:
                rend.material.EnableKeyword("_EMISSION");
                break;
            case BlockState.Glowing:
                rend.material.EnableKeyword("_EMISSION");
                break;
            default:
                Debug.LogErrorFormat("Wrong state has been used on block");
                break;
        }
    }

    public void TransferColorDown()
    {
        if (coordinate.y - 1 >= 0 && state > BlockState.None && Grid.allBlocks[new Vector2Int(coordinate.x, coordinate.y - 1)].state == BlockState.None)
        {
            Vector2Int _newCoordinate = new Vector2Int(coordinate.x, coordinate.y - 1);
            Grid.allBlocks[_newCoordinate].AssignMaterial(rend.material);
            Grid.allBlocks[_newCoordinate].SetState(state);
            Grid.allBlocks[_newCoordinate].AssignShapeId(id);
            SetState(BlockState.None);
        }
    }

    public void AssignShapeId(int _id)
    {
        id = _id;

    }

    public void LightUpShape()
    {
        SetState(BlockState.Emmissive);

        List<GridBlock> _surroundingBlocks = Grid.BlocksAroundCoordinate(coordinate);
        for (int i = 0; i < _surroundingBlocks.Count; i++)
        {
            if (_surroundingBlocks[i].id == id && _surroundingBlocks[i].state < BlockState.Emmissive)
            {
                _surroundingBlocks[i].LightUpShape();
            }
        }
    }
}
