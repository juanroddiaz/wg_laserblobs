using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Blob wave configuration 
/// </summary>
[System.Serializable]
public class BlobsWaveConfig
{
    // a random pool will pick any blob from the pool with a normal random selection. A non random will pick the next blob by list sorting
    public bool isRandomPool;
    // hoy many random entries this wave will have. For a non random wave, the entries will be as same as the list amount
    public int randomIterations = 0;
    // available blob types for this wave configuration
    public List<BlobTypes> availableTypes = new List<BlobTypes>();
}

public class GameplayConfiguration : ScriptableObject
{
    // amount of blobs killed to get another blob as reward from availableTypes pool
    public int amountForBlobEarning = 5;
    // amount of blobs killed to get a difficulty increment
    public int amountForDifficultIncreasing = 5;
    // enemy's laser force increment per difficulty step
    public float enemyDiffIncForceStep = 0.0005f;
    // player's laser force increment per difficulty step
    public float playerDiffIncForceStep = 0.0005f;
    // starting blob types
    public List<BlobTypes> startingTypes = new List<BlobTypes>();
    // list of blob waves configuration for gameplay
    public List<BlobsWaveConfig> blobsWaveConfig = new List<BlobsWaveConfig>();
}