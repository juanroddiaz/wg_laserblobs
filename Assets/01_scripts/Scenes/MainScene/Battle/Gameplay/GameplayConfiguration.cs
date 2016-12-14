using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BlobsWaveGroupConfig
{
    // list of blobs array settings to create a whole wave
    public List<BlobsWaveConfig> waveInstancesList = new List<BlobsWaveConfig>();
}

/// <summary>
/// Blob wave list instance configuration
/// Each time a wave passes, a blob is earned
/// </summary>
[System.Serializable]
public class BlobsWaveConfig
{
    // blob list description
    public string listDescription = "";
    // enemy's laser force increment for this wave
    public float enemyLaserForce = 0.0005f;
    // wave difficulty
    public IBlobDifficulty difficulty = IBlobDifficulty.VeryEasy;
    // a random pool will pick any blob from the pool with a normal random selection. A non random will pick the next blob by list sorting
    public bool isRandomPool;
    // hoy many random entries this wave will have. For a non random wave, the entries will be as same as the list amount
    public int randomIterations = 0;
    // available blob types for this wave configuration
    public List<BlobTypes> availableTypes = new List<BlobTypes>();
    // available blob types for this wave configuration
    public List<BlobTypes> rewardsTypes = new List<BlobTypes>();
}

public class GameplayConfiguration : ScriptableObject
{
    // starting blob types
    public List<BlobTypes> startingTypes = new List<BlobTypes>();
    // list of blob waves configuration for gameplay
    public BlobsWaveGroupConfig blobWavesConfig;
}