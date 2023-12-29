using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Levels/NewLevel", fileName = "NewLevel")]
public class LevelDataSO : ScriptableObject
{
    public int BoardWidth;
    public int BoardHeight;
    public int TileCount;
    public  List<GenericKey> LevelDropTypeKeys = new List<GenericKey>();
}