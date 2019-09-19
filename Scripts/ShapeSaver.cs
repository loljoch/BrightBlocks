using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSaver : MonoBehaviour
{
    private Shape savedShape;
    [SerializeField] private Vector2Int mainBlockCoordinate;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Vector2Int gridSize;
    private List<Vector2Int> gridCoordinates = new List<Vector2Int>();
    private Dictionary<Vector2Int, Block> allBlocks = new Dictionary<Vector2Int, Block>();

    private void Start()
    {
        CreateGrid();
        transform.Rotate(Vector3.up * 30);
        ClearGrid();
    }

    public Shape SwapSavedShape(Shape _shape)
    {
        Shape _savedShape = savedShape;
        savedShape = _shape;
        ShowSavedShape();
        return _savedShape;
    }

    private void CreateGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {

                //Saves the current coordinates for later use
                Vector2Int _currentCoordinate = new Vector2Int(x, y);

                //Creates a block, then adds the Block script to the object
                Block _newBlock = Instantiate(blockPrefab, new Vector3(transform.position.x + x, transform.position.y + y), Quaternion.identity, transform).AddComponent<Block>();

                //Initializes the newBlock with the Vector2Ints
                _newBlock.Initialize(_currentCoordinate);
                _newBlock.AssignMaterial(GameController.baseMaterial);

                gridCoordinates.Add(_currentCoordinate);
                allBlocks.Add(_currentCoordinate, _newBlock);
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = 0; i < gridCoordinates.Count; i++)
        {
            allBlocks[gridCoordinates[i]].gameObject.SetActive(false);
        }
    }

    private void ShowSavedShape()
    {
        ClearGrid();
        List<Vector2Int> _blocks = ShapeCodeProcessor.ShapeCodeToCoordinates(savedShape.shape, mainBlockCoordinate);

        for (int i = 0; i < _blocks.Count; i++)
        {
            allBlocks[_blocks[i]].AssignMaterial(savedShape.material);
            allBlocks[_blocks[i]].gameObject.SetActive(true);
        }
    }
}
