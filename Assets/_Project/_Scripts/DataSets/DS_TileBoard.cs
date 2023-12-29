using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DS_TileBoard : Data
{
     private int _width;
    public int Width => GetCurrentLevelData().BoardWidth;
    
    private int _height;
    public int Height => GetCurrentLevelData().BoardHeight;
    
    [SerializeField] private float _spacing;
    public float Spacing => _spacing;
    
    [Range(0,1)][SerializeField]private float _fillScreenPercentage = 0.9f;
    public float FillScreenPercentage => _fillScreenPercentage;
    
    [SerializeField] private Transform _tileHolder;
    public Transform TileHolder => _tileHolder;
    
    [SerializeField] private GameObject _dropPrefab;
    public GameObject DropPrefab => _dropPrefab;
    
    [SerializeField] private GameObject _bottomSlotPrefab;
    public GameObject BottomSlotPrefab => _bottomSlotPrefab;
    
    [SerializeField] private AllLevelsDataSO _allLevels;
    public AllLevelsDataSO AllLevels => _allLevels;
  
   [SerializeField] private IntVar _currentLevelIndex;
   public IntVar CurrentLevelIndex => _currentLevelIndex;

    private Dictionary<Vector2Int, Actor> _cellDictionary = new Dictionary<Vector2Int, Actor>();
    public Dictionary<Vector2Int, Actor> CellDictionary
    {
        get => _cellDictionary;
        set => _cellDictionary = value;

    }
    
    private List<Actor> _matchedDrops = new List<Actor>();
    public List<Actor> MatchedDrops
    {
        get => _matchedDrops;
        set => _matchedDrops = value;
    }

    private bool _hasMatch;
    public bool HasMatch
    {
        get => _hasMatch;
        set => _hasMatch = value;
    }
    
    [SerializeField] private bool _bottomTilesSettled;
    public event Action<bool, bool> onTilesSettledChanged;
    public bool BottomTilesSettled
    {
        get => _bottomTilesSettled;
        set
        {
            bool oldValue = _bottomTilesSettled;
            bool isChanged = _bottomTilesSettled != value;
            _bottomTilesSettled = value;
            if (isChanged)
            {
                onTilesSettledChanged?.Invoke(oldValue, value);
            }
        }
    }
    
    private int _mustBeFilledCellCount;
    public int MustBeFilledCellCount
    {
        get => _mustBeFilledCellCount;
        set => _mustBeFilledCellCount = value;
    }
    [SerializeField] private List<Actor> _bottomSlotActorList = new List<Actor>();
    public List<Actor> BottomSlotActorList
    {
        get => _bottomSlotActorList;
        set => _bottomSlotActorList = value;
    }
    [SerializeField] private List<Actor> _selectedTiles = new List<Actor>();
    public List<Actor> SelectedTiles
    {
        get => _selectedTiles;
        set => _selectedTiles = value;
    }
    
    [SerializeField] private List<Actor> _boardTiles = new List<Actor>();
    public List<Actor> BoardTiles
    {
        get => _boardTiles;
        set => _boardTiles = value;
    }
    

    public LevelDataSO GetCurrentLevelData()
    {
        if (CurrentLevelIndex.Value >= AllLevels.Levels.Count)
        {
            int mod = _currentLevelIndex.Value % AllLevels.Levels.Count;
            return AllLevels.Levels[mod];
        }
        LevelDataSO levelData = AllLevels.Levels[CurrentLevelIndex.Value];
        return levelData;
    }
    
    public event Action<Actor> OnTileSelected;

    Dictionary<string, int> typeOrder = new Dictionary<string, int>();
    public void NotifyTileSelected(Actor actor)
    {
        OnTileSelected?.Invoke(actor);
       
        SelectedTiles.Add(actor);

        typeOrder.Clear();
        int order = 0;

        // Assign order to each type
        foreach (var tile in SelectedTiles)
        {
            if (!typeOrder.ContainsKey(tile.GetData<DS_Tile>().TileType.ID))
            {
                typeOrder[tile.GetData<DS_Tile>().TileType.ID] = order++;
            }
        }
        SelectedTiles = SelectedTiles.OrderBy(t => typeOrder[t.GetData<DS_Tile>().TileType.ID]).ToList();
    }
}