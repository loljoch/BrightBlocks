using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.TrueRandom;

/// <summary>
/// Spawns the mainBlocks
/// </summary>

public class MainBlockManager : Iinitialize, IupdateMe
{
    public Shape currentShape;

    private float timeIncrease = 1f;
    private float timeNeededToMoveDown = 1f;
    private float currentTime = 0f;

    private List<GridBlock> currentUsedBlocks = new List<GridBlock>();

    private Vector2Int spawnCoordinate;
    private Vector2Int currentMainBlockCoordinate;
    private int totalShapeSpawned = 0;

    private Grid grid;
    private GlowingBlocksManager glowingBlocksManager;
    private ShapeHolder shapeHolder;
    private ShapeSaver shapeSaver;

    private GameController gameController;

    public MainBlockManager(GameController _gameController, Vector2Int _spawnCoordinate)
    {
        gameController = _gameController;
        spawnCoordinate = _spawnCoordinate;
    }

    public void Initialize()
    {
        grid = gameController.grid;
        glowingBlocksManager = gameController.glowingBlocksManager;
        shapeHolder = gameController.shapeHolder;
        shapeSaver = gameController.shapeSaver;

        SpawnShape();
    }

    public void Update()
    {
        if (Timer())
        {
            MoveShapeTowards(Direction.Down);
        }
    }

    public void SpawnShape()
    {
        currentMainBlockCoordinate = spawnCoordinate;
        currentShape = shapeHolder.GetNextShape();
        Grid.allBlocks[spawnCoordinate].AssignMaterial(currentShape.material);
        UpdateCurrentUsedBlocks();
        ColorShapeAroundMainBlock(currentShape.material);

        totalShapeSpawned++;
    }

    public void RespawnShape()
    {
        currentMainBlockCoordinate = spawnCoordinate;
    }

    public void SetCurrentShape(Shape _shape)
    {
        if(_shape.material != null)
        {
            currentShape = _shape;
        } else
        {
            currentShape = shapeHolder.GetNextShape();
        }
    }

    public void MoveShapeTowards(Direction _direction)
    {
        if(currentShape.material == null)
        {
            SpawnShape();
        }

        //Checks if shape can move towards specified direction
        if (grid.CanShapeMove(ShapeCodeProcessor.ShapeCodeToCoordinates(currentShape.shape, currentMainBlockCoordinate), _direction))
        {

            //Resets current shape to base color
            ColorShapeAroundMainBlock(GameController.baseMaterial);

            //Moves the main block down
            MoveMainBlockTowards(grid.DirectionToCoords(_direction, currentMainBlockCoordinate));

            //Updates the list of blocks the shape is made of
            UpdateCurrentUsedBlocks();

            //Colors the shape around the main block
            ColorShapeAroundMainBlock(currentShape.material);
        }

    }

    public void RotateShape()
    {

        //Checks if shape can rotate
        if (grid.AreTheseCoordinatesAvailable(ShapeCodeProcessor.RotateAndGiveCoordinates(currentShape.shape, currentMainBlockCoordinate)))
        {

            //Resets current shape to base color
            ColorShapeAroundMainBlock(GameController.baseMaterial);

            //RotatesShape
            currentShape.shape = ShapeCodeProcessor.RotateShapeCode(currentShape.shape);

            //Updates the list of blocks the shape is made of
            UpdateCurrentUsedBlocks();

            //Colors the shape around the main block
            ColorShapeAroundMainBlock(currentShape.material);
        }

    }

    public void SetShape()
    {
        
        Grid.allBlocks[currentMainBlockCoordinate].SetState(BlockState.Set);
        for (int i = 0; i < currentUsedBlocks.Count; i++)
        {

            if (currentShape.isGlowing)
            {
                currentUsedBlocks[i].SetState(BlockState.Glowing);
            } else
            {
                currentUsedBlocks[i].SetState(BlockState.Set);
            }

            currentUsedBlocks[i].AssignShapeId(totalShapeSpawned);
        }

        if (currentShape.isGlowing)
        {
            glowingBlocksManager.AddGlowingBlocks(currentUsedBlocks);
        }

        glowingBlocksManager.LightUpShapesAroundGlowingBlocks();

        if (gameController.playing)
        {
            SpawnShape();
        }
    }

    public void SetTimerSpeed(float _speedUp)
    {
        timeIncrease = _speedUp;
    }

    private bool Timer()
    {
        currentTime += Time.deltaTime * timeIncrease;
        if(currentTime >= timeNeededToMoveDown)
        {
            currentTime = 0;
            return true;
        }
        return false;
    }

    private void UpdateCurrentUsedBlocks()
    {

        //Clears the list
        currentUsedBlocks.Clear();

        List<Vector2Int> _currentUsedBlocksCoordinates = ShapeCodeProcessor.ShapeCodeToCoordinates(currentShape.shape, currentMainBlockCoordinate);

        //Finds the blocks linked with the coordinates and adds them to the list
        for (int i = 0; i < _currentUsedBlocksCoordinates.Count; i++)
        {

            currentUsedBlocks.Add(Grid.allBlocks[_currentUsedBlocksCoordinates[i]]);
        }
    }

    private void ColorShapeAroundMainBlock(Material _material)
    {

        for (int i = 0; i < currentUsedBlocks.Count; i++)
        {

            currentUsedBlocks[i].AssignMaterial(_material);
        }
    }

    private void MoveMainBlockTowards(Vector2Int _coordinate)
    {

        //Resets current block to base color
        Grid.allBlocks[currentMainBlockCoordinate].AssignMaterial(GameController.baseMaterial);

        //Picks new block
        currentMainBlockCoordinate = _coordinate;

        //Colors the new block
        Grid.allBlocks[currentMainBlockCoordinate].AssignMaterial(currentShape.material);

    }
}
