using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To create a grid and safe the position of the blocks within the created grid
/// </summary>

public class Grid : Iinitialize
{
    private const int INVISIBLE_ROWS = 4;

    public static Dictionary<Vector2Int, GridBlock> allBlocks = new Dictionary<Vector2Int, GridBlock>();

    private Vector2Int gridSize;
    private GameObject blockPrefab;

    private LayerMask invisibleLayer;

    private Demonstration_MeshProcessing meshOutliner;
    private GlowingBlocksManager glowingBlocksManager;
    private MainBlockManager mainBlockManager;

    private GameController gameController;

    public Grid(GameController _gameController, GameObject _blockPrefab, Vector2Int _gridSize, LayerMask _invisibleLayer, Demonstration_MeshProcessing _meshOutliner)
    {
        gameController = _gameController;
        blockPrefab = _blockPrefab;
        gridSize = _gridSize;
        invisibleLayer = _invisibleLayer;
        meshOutliner = _meshOutliner;
    }

    public void Initialize()
    {
        glowingBlocksManager = gameController.glowingBlocksManager;
        mainBlockManager = gameController.mainBlockManager;
        CreateGrid();
    }

    public bool CanShapeMove(List<Vector2Int> _fromCoordinates, Direction _direction)
    {

        List<Vector2Int> _potentialCoordinates = new List<Vector2Int>();

        //Adds all potential coordinates to a list
        for (int i = 0; i < _fromCoordinates.Count; i++)
        {

            _potentialCoordinates.Add(DirectionToCoords(_direction, _fromCoordinates[i]));
        }



        return AreTheseCoordinatesAvailable(_potentialCoordinates, _direction);
    }

    public bool AreTheseCoordinatesAvailable(List<Vector2Int> _coordinates, Direction _direction = Direction.Left)
    {

        //Checks if the potential coordinates exist within the grid and are not already occupied by colored blocks
        for (int i = 0; i < _coordinates.Count; i++)
        {

            if (!allBlocks.ContainsKey(_coordinates[i]) || allBlocks[_coordinates[i]].state > BlockState.None)
            {

                //Checks if the block has reached the bottom or a colored block
                if (_coordinates[i].y < 0 || (_direction == Direction.Down && allBlocks[_coordinates[i]].state > BlockState.None))
                {

                    //Delegate potential
                    mainBlockManager.SetShape();
                    List<GridBlock> _madeBlocks = GetAllMadeLines();
                    if (_madeBlocks.Count >= gridSize.x)
                    {
                        ClearBlocks(_madeBlocks);
                        MoveAllUsedBlocksDown(_madeBlocks.Count / gridSize.x, GetLowestYBlock(_madeBlocks));
                    }
                    if (CheckIfGameOver())
                    {
                        gameController.EndGame();
                    }
                    return false;
                }
                return false;
            }
        }

        return true;
    }

    public bool CheckIfGameOver()
    {
        List<GridBlock> _blocks = GetBlocksAtY(gridSize.y - INVISIBLE_ROWS);
        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i].state > BlockState.None)
            {
                return true;
            }
        }

        return false;
    }

    public Vector2Int DirectionToCoords(Direction _direction, Vector2Int _coordinate)
    {

        Vector2Int _newCoordinate = Vector2Int.zero;

        switch (_direction)
        {
            case Direction.Down:
                _newCoordinate = new Vector2Int(_coordinate.x, _coordinate.y - 1);
                break;
            case Direction.Left:
                _newCoordinate = new Vector2Int(_coordinate.x - 1, _coordinate.y);
                break;
            case Direction.Right:
                _newCoordinate = new Vector2Int(_coordinate.x + 1, _coordinate.y);
                break;
            default:
                break;
        }

        return _newCoordinate;
    }

    public static List<GridBlock> BlocksAroundCoordinate(Vector2Int _coordinate)
    {
        Vector2Int[] _coordsAroundCoord = new Vector2Int[] {
            new Vector2Int(_coordinate.x, _coordinate.y + 1),
            new Vector2Int(_coordinate.x, _coordinate.y - 1),
            new Vector2Int(_coordinate.x + 1, _coordinate.y),
            new Vector2Int(_coordinate.x - 1, _coordinate.y) };

        List<GridBlock> _blocks = new List<GridBlock>();
        for (int i = 0; i < _coordsAroundCoord.Length; i++)
        {
            if(allBlocks.ContainsKey(_coordsAroundCoord[i]))
            {
                _blocks.Add(allBlocks[_coordsAroundCoord[i]]);
            }
        }

        return _blocks;
    }

    public List<GridBlock> FindAllGlowingBlocks()
    {
        List<GridBlock> _blocks = new List<GridBlock>();

        for (int i = 0; i < gridSize.y; i++)
        {
            List<GridBlock> _yBlocks = GetBlocksAtY(i);
            int _amountOfNoneBlocksInRow = 0;
            for (int b = 0; b < _yBlocks.Count; b++)
            {
                switch (_yBlocks[b].state)
                {
                    case BlockState.None:
                        _amountOfNoneBlocksInRow++;
                        break;
                    case BlockState.Emmissive:
                        _yBlocks[b].SetState(BlockState.Set);
                        break;
                    case BlockState.Glowing:
                        _blocks.Add(_yBlocks[b]);
                        break;
                    default:
                        break;
                }
            }

            //Stops searching for glowing blocks if they meet an empty row
            if(_amountOfNoneBlocksInRow == _yBlocks.Count)
            {
                break;
            }

        }

        return _blocks;
    }

    public void Destroy()
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            List<GridBlock> _blocks = GetBlocksAtY(i);
            for (int b = 0; b < _blocks.Count; b++)
            {
                if(_blocks[b].GetComponent<Rigidbody>() == null)
                {
                    _blocks[b].gameObject.AddComponent<Rigidbody>().velocity = new Vector3(Random.Range(-6, 6), Random.Range(-6, 6), Random.Range(-6, 6));
                }
            }
        }
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
                GridBlock _newBlock = Object.Instantiate(blockPrefab, new Vector3(x, y), Quaternion.identity, gameController.transform).AddComponent<GridBlock>();

                //Initializes the newBlock with the Vector2Ints
                _newBlock.Initialize(_currentCoordinate);
                _newBlock.AssignMaterial(GameController.baseMaterial);

                allBlocks.Add(_currentCoordinate, _newBlock);

                if(y < gridSize.y - 3)
                {
                    meshOutliner.OutlineMesh(_newBlock.gameObject);
                } else
                {
                    //FIXME
                    _newBlock.gameObject.layer = LayerMask.NameToLayer("Invisible");
                }
                
            }
        }
    }

    private void ClearBlocks(List<GridBlock> _blocks)
    {

        for (int i = 0; i < _blocks.Count; i++)
        {

            _blocks[i].SetState(BlockState.None);
        }
    }

    private void MoveAllUsedBlocksDown(int _linesToMove, int _lowestYCoordinate)
    {
        for (int i = 0; i < _linesToMove; i++)
        {
            for (int y = _lowestYCoordinate; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {

                    allBlocks[new Vector2Int(x, y)].TransferColorDown();
                }
            }
        }

        glowingBlocksManager.UpdateGlowingBlocks(FindAllGlowingBlocks());
    }

    //TODO only check the lines a shape has interacted with
    //Check if all the blocks in a horizontal lines have the isSet bool as true
    private List<GridBlock> GetAllMadeLines()
    {

        List<GridBlock> _allBlocksInMadeLines = new List<GridBlock>();

        for (int y = 0; y < gridSize.y; y++)
        {
            if (CheckIfBlocksAreSet(GetBlocksAtY(y)))
            {
                _allBlocksInMadeLines.AddRange(GetBlocksAtY(y));
            }
        }

        return _allBlocksInMadeLines;

    }

    private bool CheckIfBlocksAreSet(List<GridBlock> _blockList)
    {
        for (int i = 0; i < _blockList.Count; i++)
        {
            if (_blockList[i].state < BlockState.Emmissive)
            {
                return false;
            }
        }

        return true;
    }

    private List<GridBlock> GetBlocksAtY(int _y)
    {

        List<GridBlock> _blocks = new List<GridBlock>();

        for (int x = 0; x < gridSize.x; x++)
        {
            _blocks.Add(allBlocks[new Vector2Int(x, _y)]);
        }

        return _blocks;
    }

    private int GetLowestYBlock(List<GridBlock> _blockList)
    {
        int _lowestY = gridSize.y + 2;

        for (int i = 0; i < _blockList.Count; i++)
        {
            if (_lowestY > _blockList[i].coordinate.y)
            {

                _lowestY = _blockList[i].coordinate.y;
            }
        }

        return _lowestY;
    }

}
