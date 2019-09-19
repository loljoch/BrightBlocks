using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingBlocksManager
{
    private List<GridBlock> glowingBlocks = new List<GridBlock>();

    public void AddGlowingBlocks(List<GridBlock> _glowingBlocks)
    {
        glowingBlocks.AddRange(_glowingBlocks);
    }

    public void UpdateGlowingBlocks(List<GridBlock> _glowingBlocks)
    {
        glowingBlocks.Clear();
        AddGlowingBlocks(_glowingBlocks);
        LightUpShapesAroundGlowingBlocks();
    }

    public void LightUpShapesAroundGlowingBlocks()
    {
        for (int i = 0; i < glowingBlocks.Count; i++)
        {
            List<GridBlock> _blocksAroundBlock = Grid.BlocksAroundCoordinate(glowingBlocks[i].coordinate);
            for (int c = 0; c < _blocksAroundBlock.Count; c++)
            {
                if(_blocksAroundBlock[c].state > BlockState.None && _blocksAroundBlock[c].state < BlockState.Glowing)
                {
                    _blocksAroundBlock[c].LightUpShape();
                }
            }
        }
    }
}
