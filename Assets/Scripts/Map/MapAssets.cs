using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapAssets", menuName = "ScriptableObjects/MapAssets", order = 1)]
public class MapAssets : ScriptableObject
{
    [System.Serializable]
    struct NodeAsset
    {
        public int Type;
        public GameObject Prefab;
        public GameObject ZoomedPrefab;
    }

    [SerializeField]
    List<NodeAsset> _nodeAssets = null;

    public GameObject GetNodePrefab(int typeId)
    {
        foreach(NodeAsset asset in _nodeAssets)
        {
            if(asset.Type == typeId)
            {
                return asset.Prefab;
            }
        }

        return null;
    }

    public GameObject GetZoomedNodePrefab(int typeId)
    {
        foreach(NodeAsset asset in _nodeAssets)
        {
            if(asset.Type == typeId)
            {
                return asset.ZoomedPrefab;
            }
        }

        return null;
    }
}
