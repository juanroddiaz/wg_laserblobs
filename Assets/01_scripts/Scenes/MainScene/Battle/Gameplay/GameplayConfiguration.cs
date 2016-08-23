using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BlobsWaveConfig
{
    public int  startTurn;
    public bool isRandomPool;
    public List<BlobTypes> availableTypes = new List<BlobTypes>();
}

public class GameplayConfiguration : ScriptableObject
{
    public int amountForBlobEarning = 5;
    public int amountForDifficultIncreasing = 5;
    public List<BlobsWaveConfig> blobsWaveConfig = new List<BlobsWaveConfig>();
}
