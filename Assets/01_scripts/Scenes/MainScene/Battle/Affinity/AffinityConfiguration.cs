using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BlobSetting
{
    public BlobTypes type = BlobTypes.MAX;
    public List<BlobAffinities> affinities = new List<BlobAffinities>();
}

[System.Serializable]
public class AffinityRelationship
{
    public BlobAffinities affinity = BlobAffinities.MAX;
    public float percentage = 100.0f;
}

[System.Serializable]
public class AffinitySetting
{
    public BlobAffinities affinity = BlobAffinities.MAX;
    public List<AffinityRelationship> affinities = new List<AffinityRelationship>();
}

[System.Serializable]
public class AffinityConfiguration : ScriptableObject
{
    public List<BlobSetting> blobsSetting = new List<BlobSetting>();
    public List<AffinitySetting> affinitySetting = new List<AffinitySetting>();    
}