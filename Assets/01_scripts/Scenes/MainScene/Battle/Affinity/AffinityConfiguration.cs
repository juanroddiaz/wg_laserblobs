using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BlobSetting
{
    public BlobTypes type = BlobTypes.MAX;
    public List<BlobAffinities> affinities = new List<BlobAffinities>();
}

[System.Serializable]
public class DamageConfiguration
{
    public BlobDamageHierarchy damageHierarchy = BlobDamageHierarchy.Max;
    public float percentage = 100.0f;
}

[System.Serializable]
public class AffinityRelationship
{
    public BlobAffinities affinity = BlobAffinities.MAX;
    public BlobDamageHierarchy damage = BlobDamageHierarchy.Max;
}

[System.Serializable]
public class AffinitySetting
{
    public BlobAffinities affinity = BlobAffinities.MAX;
    public List<AffinityRelationship> affinities = new List<AffinityRelationship>();
}

/// <summary>
/// Main Scriptable Object to edit
/// </summary>
[System.Serializable]
public class AffinityConfiguration : ScriptableObject
{
    [Tooltip("Damage configuration for all the six possible resistance/weakness relationships in game")]
    public List<DamageConfiguration> damageConfiguration = new List<DamageConfiguration>();

    [Tooltip("Blob type configuration for all the possible affinities a blob instance can have")]
    public List<BlobSetting> blobsSetting = new List<BlobSetting>();

    [Tooltip("Affinity configuration with all the damage relationships it can have")]
    public List<AffinitySetting> affinitySetting = new List<AffinitySetting>();    
}