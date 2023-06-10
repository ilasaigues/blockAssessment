
using System.Collections.Generic;

public interface IBlockDataProvider
{
    public BlockData GetBlockByID(int id);

    public Dictionary<string, List<BlockData>> GetAllBlocks();

    public bool HasLoaded { get; }
}
