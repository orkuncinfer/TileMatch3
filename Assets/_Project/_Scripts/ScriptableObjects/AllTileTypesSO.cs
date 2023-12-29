using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "AllTileTypesAsset", fileName = "AllTileTypesAsset")]
public class AllTileTypesSO : ScriptableObject
{
    public List<TileDropType> TileDropTypes;
    public Dictionary<GenericKey, TileDropType> TileDropTypesDict = new Dictionary<GenericKey, TileDropType>();
    
    public TileDropType GetTileDropType(GenericKey genericKey)
    {
        if (TileDropTypesDict.Count != TileDropTypes.Count)
        {
            TileDropTypesDict.Clear();
            for (int i = 0; i < TileDropTypes.Count; i++)
            {
                TileDropTypesDict.Add(TileDropTypes[i].TileTypeKey,TileDropTypes[i]);
            }
        }

        if (TileDropTypesDict.ContainsKey(genericKey))
        {
            return TileDropTypesDict[genericKey];
        }
        else
        {
            return null;
        }
        
    }
}

[System.Serializable]
public class TileDropType
{
    public GenericKey TileTypeKey;
    [PreviewField]
    public Sprite Visual;
}