using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class State_GeneratingTiles : MonoState
{
    [SerializeField] private EventSignal _onTilesGenerated;
    
    private DS_TileBoard _boardData;
    
    private List<Actor> _generatedTileActors = new List<Actor>();
    
    [ShowInInspector] private Dictionary<string, int> typePool = new Dictionary<string, int>();
    
    protected override void OnEnter()
    {
        base.OnEnter();
        _boardData = Owner.GetData<DS_TileBoard>();
        _generatedTileActors.Clear();
        _boardData.BoardTiles.Clear();
        _boardData.CellDictionary.Clear();
        _boardData.BottomSlotActorList.Clear();
        typePool.Clear();
        _boardData.TileHolder.transform.localScale = Vector3.one;
        
        GenerateGrid();
        CenterTileHolder();
        ScaleBoardHolderAccordingToScreenSize();
        
        _onTilesGenerated.Raise();
        CheckoutExit();
    }

    IEnumerator delayed()
    {
        yield return null;
        _onTilesGenerated.Raise();
    }
    
    void GenerateGrid()
    {
        LevelDataSO levelData = _boardData.GetCurrentLevelData();

        int sortingOrder = 200;
        int maxTileCount = levelData.TileCount ;
        int remainder = levelData.TileCount % 3;
        if (remainder != 0)
        {
            maxTileCount -= remainder;
        }

        int totalPack = maxTileCount / 3;
        int eachPack = totalPack / levelData.LevelDropTypeKeys.Count;
        int packRemainder = totalPack % levelData.LevelDropTypeKeys.Count;
        
        int tileCountToSpawn = maxTileCount;
        
        for (int i = 0; i < levelData.LevelDropTypeKeys.Count; i++)
        {
            typePool.Add(levelData.LevelDropTypeKeys[i].ID,eachPack * 3);
            if (packRemainder > 0)
            {
                typePool[levelData.LevelDropTypeKeys[i].ID] += 3;
                packRemainder--;
            }
        }
        
        for (int layerIndex = 0; layerIndex < 99; layerIndex++)
        {
            for (int x = 0; x < levelData.BoardWidth - layerIndex; x++)
            {
                for (int y = 0; y < levelData.BoardHeight - layerIndex; y++)
                {
                    int spawn = Random.Range(0, 2);
                    if(spawn == 0) continue;
                    if(layerIndex % 2 == 0 && (x % 2 != 0 || y % 2 != 0)) continue;
                    if(layerIndex % 2 != 0 && (x % 2 == 0 || y % 2 == 0)) continue;
                    //GenericKey tileKey = levelData.BoardCellsDictionary.Get(new Vector2Int(x, y));

                    int randomTypeIndex = 0;
                    bool foundUniqueType = false;
                    while (!foundUniqueType && typePool.Count > 0)
                    {
                        randomTypeIndex = Random.Range(0,levelData.LevelDropTypeKeys.Count);
                        if (typePool.ContainsKey(levelData.LevelDropTypeKeys[randomTypeIndex].ID))
                        {
                            foundUniqueType = true;
                            typePool[levelData.LevelDropTypeKeys[randomTypeIndex].ID] -= 1;
                            if( typePool[levelData.LevelDropTypeKeys[randomTypeIndex].ID] == 0) typePool.Remove(levelData.LevelDropTypeKeys[randomTypeIndex].ID);
                        }
                    }
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    Vector3 worldPosition = new Vector3(x/ 2f * _boardData.Spacing, y/2f * _boardData.Spacing, -layerIndex);
                
                    GameObject tileInstance = GOPoolProvider.Retrieve(_boardData.DropPrefab, worldPosition, Quaternion.identity, _boardData.TileHolder);
                    Actor tileActor = tileInstance.GetComponent<Actor>();
                    
                    sortingOrder--;
                    int addition = layerIndex * _boardData.Width * _boardData.Height;
                    tileActor.GetData<DS_Tile>().TileSpriteRenderer.sortingOrder = sortingOrder + addition;
                    tileActor.GetData<DS_Tile>().LayerIndex = layerIndex;
                    tileActor.GetData<DS_Tile>().TileType = levelData.LevelDropTypeKeys[randomTypeIndex];
                    tileActor.GetData<DS_Tile>().BoardData = _boardData;
                    tileActor.StartIfNot(Owner);
               
                    tileInstance.name = "Tile" +  "_" + x + "_" + y + "_layer:" + layerIndex;
                    _generatedTileActors.Add(tileActor);
                    _boardData.BoardTiles.Add(tileActor);
                    _boardData.CellDictionary[gridPosition] = tileActor;
                    tileCountToSpawn--;
                    
                    if(tileCountToSpawn == 0) break;
                }
                if(tileCountToSpawn == 0) break;
            }
            if(tileCountToSpawn == 0) break;
        } // Generate board tiles randomly.
        
        for (int x = 1; x < 8; x++) // Generate bottom slots
        {
            
            int y = -4;
            Vector2Int gridPosition = new Vector2Int(x, y);
            Vector3 worldPosition = new Vector3((x -0.5f) * _boardData.Spacing, y * _boardData.Spacing, 0);
                
            GameObject slotInstance = GOPoolProvider.Retrieve(_boardData.BottomSlotPrefab, worldPosition, Quaternion.identity, _boardData.TileHolder);
            Actor slotActor = slotInstance.GetComponent<Actor>();
            
            sortingOrder--;
            slotActor.GetData<DS_Tile>().TileSpriteRenderer.sortingOrder = sortingOrder;
            
            slotActor.StartIfNot(Owner);
            _boardData.BottomSlotActorList.Add(slotActor);
            slotInstance.name = "Slot_"   + x ;
            _generatedTileActors.Add(slotActor);
            _boardData.CellDictionary[gridPosition] = slotActor;
        }
        
    }
    void ScaleBoardHolderAccordingToScreenSize()
    {
        float boardPixelWidth = _boardData.Width / 2f + (_boardData.Width - 1) * _boardData.Spacing / 10;
        
        _boardData.TileHolder.localScale = Vector3.one * (GetScreenToWorldWidth / boardPixelWidth) * _boardData.FillScreenPercentage;
    }

    void CenterTileHolder()
    {
        float boardPixelWidth = _boardData.Width / 2f +(_boardData.Width - 0) *  (_boardData.Spacing - 1f);
        float boardPixelHeight = _boardData.Height /2f  + (_boardData.Height - 0) * (_boardData.Spacing - 1f);
        Vector2 centerPos = new Vector2(boardPixelWidth / 2, boardPixelHeight / 2);
        _boardData.TileHolder.SetParent(null);
       
        Transform[] children = _boardData.TileHolder.GetComponentsInChildren<Transform>();
        _boardData.TileHolder.DetachChildren();
        
        _boardData.TileHolder.position = centerPos;

        foreach (Actor tileActor in _generatedTileActors)
        {
            tileActor.transform.SetParent(_boardData.TileHolder);
        }
        
        _boardData.TileHolder.position = new Vector3(0,3,0);
    }
    
    public static float GetScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2;
            return width;
        }
    }
}