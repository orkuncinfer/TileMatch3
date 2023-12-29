using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Tile : ActorStateMachine
{
    protected override MonoState _initialState => _initialize;

    [SerializeField] private MonoState _initialize;
    [SerializeField] private MonoState _clickable;
    [SerializeField] private MonoState _blocked;
    [SerializeField] private MonoState _selected;
    [SerializeField] private MonoState _matched;

    private DS_Tile _tileData;

    protected override void OnEnter()
    {
        base.OnEnter();
        _tileData = Owner.GetData<DS_Tile>();
    }

    public override void OnRequireAddTransitions()
    {
        AddTransition(_initialize,_clickable, InitializeToClickable);
        AddTransition(_clickable,_blocked, ClickableToBlocked);
        AddTransition(_blocked,_clickable,BlockedToClickable);
        AddTransition(_clickable,_selected,ClickableToSelected);
        AddTransition(_selected,_matched,SelectedToMatched);
    }

    private bool SelectedToMatched()
    {
        return _tileData.IsMatched && _tileData.IsSettled;
    }

    private bool ClickableToSelected()
    {
        return _tileData.IsSelected;
    }

    private bool BlockedToClickable()
    {
        return !_tileData.IsBlocked;
    }

    private bool ClickableToBlocked()
    {
        return _tileData.IsBlocked;
    }

    private bool InitializeToClickable()
    {
        return _initialize.IsFinished;
    }
    
}
