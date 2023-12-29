using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class State_DetectMatchedTiles : MonoState
{
    private DS_TileBoard _boardData;
    private List<Actor> _matchedTiles = new List<Actor>();

    [SerializeField] private EventSignal _requestFailEvent;
    [SerializeField] private EventSignal _requestCompleteEvent;
    [SerializeField] private EventSignal _detectMatchedTilesEvent;
    protected override void OnEnter()
    {
        base.OnEnter();
        _boardData = Owner.GetData<DS_TileBoard>();
        _detectMatchedTilesEvent.Register(OnDetectMatchedTiles);
    }

   

    protected override void OnExit()
    {
        base.OnExit();
        _detectMatchedTilesEvent.Unregister(OnDetectMatchedTiles);
    }
    private void OnDetectMatchedTiles()
    {
        Check();
    }


    public void Check()
    {
        bool hasMatch = false;
        for (int i = 0; i < _boardData.SelectedTiles.Count; i++)
        {
            string key = _boardData.SelectedTiles[i].GetData<DS_Tile>().TileType.ID;
            
            for (int j = i; j < _boardData.SelectedTiles.Count; j++)
            {
                if (_boardData.SelectedTiles[j].GetData<DS_Tile>().TileType.ID == key && _matchedTiles.Count < 3)
                {
                    if(_boardData.SelectedTiles[j].GetData<DS_Tile>().IsMatched ) continue;
                    _matchedTiles.Add(_boardData.SelectedTiles[j]);
                }    
            }

            bool allSettled = true;
            if (_matchedTiles.Count == 3)
            {
                hasMatch = true;
                foreach (Actor tileActor in _matchedTiles) // check if all tiles are settled on slots.
                {
                    if (!tileActor.GetData<DS_Tile>().IsSettled)
                    {
                        allSettled = false;
                    }
                }
                if (allSettled) // all tiles are settled on slots so we can match them.
                {
                    foreach (Actor tileActor in _matchedTiles)
                    {
                        tileActor.GetData<DS_Tile>().IsMatched = true;
                        _boardData.BoardTiles.Remove(tileActor);
                    }
                }
            }
            _matchedTiles.Clear();
        }
        
        if (!hasMatch && _boardData.SelectedTiles.Count == _boardData.BottomSlotActorList.Count)
        {
            _requestFailEvent.Raise();
        }
        if ( _boardData.BoardTiles.Count == 0)
        {
            _requestCompleteEvent.Raise();
        }
    }
}