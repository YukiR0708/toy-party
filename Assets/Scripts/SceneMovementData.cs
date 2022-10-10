using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable][CreateAssetMenu(fileName = "SceneMovementData", menuName = "CreateSceneMovementData")]
public class SceneMovementData : ScriptableObject
{
    public enum SceneType
    {
        WorldMap,
        Stage1,
        Stage1toWorldMap
    }

    [SerializeField] SceneType _sceneType;

    public void OnEnable()
    {
        _sceneType = SceneType.WorldMap;
    }

    public void SetSceneType(SceneType scene)
    {
        _sceneType = scene;
    }

    public SceneType GetSceneType()
    {
        return _sceneType;
    }

}
