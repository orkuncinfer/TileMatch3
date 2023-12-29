using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class State_CheckIfBlocked : MonoState
{
    [SerializeField] private LayerMask layerMask;
    
    private BoxCollider2D _boxCollider2D;
    private Vector2 _boxSize = new Vector2(0.9f, 0.9f);
    private float _angle = 0f;
    private DS_Tile _tileData;

    [SerializeField] private EventSignal _onTilesGenerated;
    [SerializeField] private EventSignal _onTileSelected;
    protected override void OnEnter()
    {
        base.OnEnter();
        _boxCollider2D = Owner.GetComponent<BoxCollider2D>();
        _tileData = Owner.GetData<DS_Tile>();

        _tileData.IsBlocked = false;
        _tileData.TileCollider.enabled = true;
        _tileData.IsSelected = false;
        
        Check();
        
        _onTilesGenerated.Register(OnTilesGenerated);
        _onTileSelected.Register(OnTilesGenerated);
    }

    protected override void OnExit()
    {
        base.OnExit();
        _onTilesGenerated.Unregister(OnTilesGenerated);
        _onTileSelected.Unregister(OnTilesGenerated);
    }

    private void OnTilesGenerated()
    {
        Check();
    }
    [Button]
    public void Check()
    {
        if (_tileData.IsSelected) return;
        Vector2 boxCastCenter =  _boxCollider2D.bounds.center;
        
        RaycastHit2D[] hitResults = Physics2D.BoxCastAll(boxCastCenter, _boxSize, _angle, Vector2.zero, 0, layerMask);
        
        foreach (var hit in hitResults)
        {
            if (hit.collider != null)
            {
                if (hit.transform.TryGetComponent(out Actor actor))
                {
                    if (actor.GetData<DS_Tile>().LayerIndex > Owner.GetData<DS_Tile>().LayerIndex)
                    {
                        _tileData.IsBlocked = true;
                        Color currentColor = _tileData.TileSpriteRenderer.color;
                        float H, S, V;
                        Color.RGBToHSV(currentColor, out H, out S, out V);
                        V = 0.3f;
                        _tileData.TileSpriteRenderer.color = Color.HSVToRGB(H, S, V);
                        break;
                    }
                    else
                    {
                        _tileData.IsBlocked = false;
                        _tileData.TileSpriteRenderer.color = new Color(255, 255, 255);
                    }
                }
            }
        }
    }
}
