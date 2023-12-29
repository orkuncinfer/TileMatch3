using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_MoveTileToSlot : MonoState
{
    private DS_Tile _tileData;
    [SerializeField] private EventSignal _detectMatchedTilesEvent;
    protected override void OnEnter()
    {
        base.OnEnter();
        _tileData = Owner.GetData<DS_Tile>();
    }

    protected override void OnExit()
    {
        base.OnExit();
        _tileData.BoardData.SelectedTiles.Remove(Owner);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        int index = _tileData.BoardData.SelectedTiles.IndexOf(Owner);
        Vector3 position =Owner.transform.position;
        Vector3 targetPosition = _tileData.BoardData.BottomSlotActorList[index].transform.position;
           
        if (Owner.transform.position == targetPosition)
        {
            if (_tileData.IsSettled == false)
            {
                _tileData.IsSettled = true;
                _detectMatchedTilesEvent.Raise();
            }
        }
        Owner.transform.position = Vector3.MoveTowards(position, targetPosition, 50 * Time.deltaTime);
    }
}
