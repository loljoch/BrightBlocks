using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material BaseMaterial;
    public static Material baseMaterial;
    [SerializeField] private Material blueMaterial, orangeMaterial, cyanMaterial, greenMaterial, redMaterial, yellowMaterial, magentaMaterial, glowingMaterial;
    [SerializeField] private Material outlineMaterial;

    [Space()]

    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private LayerMask invisibleLayer;

    [SerializeField] private Vector2Int spawnCoordinate;

    [SerializeField] private float spedUpTimeIncrease = 12f;

    [Space()]
    [Header("Sound")]
    [SerializeField] private AudioClip moveShapeAudio;
    [SerializeField] private AudioClip setBlockAudio;
    [SerializeField] private AudioClip clearLineAudio;

    public MainBlockManager mainBlockManager;
    public SoundManager soundManager;
    public GlowingBlocksManager glowingBlocksManager;
    public PlayerInput playerInput;
    public ShapeHolder shapeHolder;
    public ShapeSaver shapeSaver;
    public Grid grid;

    public bool playing = true;

    private void Awake()
    {
        baseMaterial = BaseMaterial;
        InitiateScripts();

        playerInput.SpeedChange += mainBlockManager.SetTimerSpeed;

        playerInput.MoveShape += soundManager.PlayMoveShapeClip;

        playerInput.RotateShape += mainBlockManager.RotateShape;
        playerInput.RotateShape += soundManager.PlayMoveShapeClip;
    }

    private void InitiateScripts()
    {
        mainBlockManager = new MainBlockManager(this, spawnCoordinate);
        playerInput = new PlayerInput(this, spedUpTimeIncrease);
        grid = new Grid(this, blockPrefab, gridSize, invisibleLayer, new Demonstration_MeshProcessing(outlineMaterial));
        soundManager = new SoundManager(moveShapeAudio, setBlockAudio, clearLineAudio);

        glowingBlocksManager = new GlowingBlocksManager();
        shapeSaver = FindObjectOfType<ShapeSaver>();
        shapeHolder = new ShapeHolder(blueMaterial, orangeMaterial, cyanMaterial, greenMaterial, redMaterial, yellowMaterial, magentaMaterial, glowingMaterial);

        shapeHolder.Initialize();
        grid.Initialize();
        mainBlockManager.Initialize();
    }

    public void EndGame()
    {
        if (playing == true)
        {
            playing = false;
            Debug.Log("You lost!");
            grid.Destroy();
        }
    }

    private void Update()
    {
        playerInput.Update();
        mainBlockManager.Update();
    }

    private void OnDisable()
    {
        playerInput.SpeedChange -= mainBlockManager.SetTimerSpeed;

        playerInput.MoveShape -= soundManager.PlayMoveShapeClip;

        playerInput.RotateShape -= mainBlockManager.RotateShape;
        playerInput.RotateShape -= soundManager.PlayMoveShapeClip;
    }



}
