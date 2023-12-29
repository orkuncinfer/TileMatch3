using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TileMatched : MonoState
{
    private DS_Tile _tileData;

    protected override void OnEnter()
    {
        base.OnEnter();
        _tileData = Owner.GetData<DS_Tile>();
        Owner.StopIfNot();
    }
}
