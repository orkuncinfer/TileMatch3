using UnityEngine;
using UnityEngine.Serialization;

public class State_DetectSelection : MonoState
{
    [SerializeField] private EventSignal _onTileSelectedEvent;
    [SerializeField] private GenericKey _tileTag;
    
    private DS_TileBoard _boardData;
    private Actor _draggedTileDrop;
    private Actor _targetTileDrop;

    private Vector2 _currentTouchPosition;

    protected override void OnEnter()
    {
        base.OnEnter();
        _boardData = Owner.GetData<DS_TileBoard>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider == null) return;
            if (hit.transform.TryGetComponent(out Actor hitActor))
            {
                if(!hitActor.ContainsTag(_tileTag.ID)) return;
                if (hitActor.GetData<DS_Tile>().IsSelected) return;
                if(_boardData.BottomSlotActorList.Count == _boardData.SelectedTiles.Count) return;
                if (hitActor == null) return;
                if(hitActor.GetData<DS_Tile>().IsBlocked) return;
                
                
                _boardData.NotifyTileSelected(hitActor);
                hitActor.GetData<DS_Tile>().TileCollider.enabled = false;
                hitActor.GetData<DS_Tile>().TileSpriteRenderer.sortingOrder = 15555;
                hitActor.GetData<DS_Tile>().IsSelected = true;
                _onTileSelectedEvent.Raise();
            }
        }
    }
}