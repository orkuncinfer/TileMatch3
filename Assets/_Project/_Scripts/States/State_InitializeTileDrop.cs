
using UnityEngine;

public class State_InitializeTileDrop : MonoState
{
    private DS_Tile _tileData;
    [SerializeField] private AllTileTypesSO _allTiles;
    protected override void OnEnter()
    {
        base.OnEnter();
        _tileData = Owner.GetData<DS_Tile>();

        int randomTypeIndex = Random.Range(0, _allTiles.TileDropTypes.Count);
       // _tileData.TileType = _allTiles.TileDropTypes[randomTypeIndex].TileTypeKey;
        if (_tileData.TileType != null)
        {
            _tileData.TileSpriteRenderer.sprite = _allTiles.GetTileDropType(_tileData.TileType).Visual;
        }
       // Owner.transform.name = _tileData.TileCoordinates + "_" + _tileData.DropTypeKey.ID;

       _tileData.IsSettled = false;
       _tileData.IsMatched = false;
       _tileData.IsBlocked = false;
        CheckoutExit();
    }
}