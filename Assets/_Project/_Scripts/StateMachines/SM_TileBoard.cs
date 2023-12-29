using UnityEngine;

public class SM_TileBoard : ActorStateMachine
{
    protected override MonoState _initialState => _generatingCellsState;

    [SerializeField] private MonoState _generatingCellsState;
    [SerializeField] private MonoState _playingState;
    [SerializeField] private MonoState _matchHappenedState;

    private DS_TileBoard _boardData;

    protected override void OnEnter()
    {
        base.OnEnter();
        _boardData = Owner.GetData<DS_TileBoard>();
    }

    public override void OnRequireAddTransitions()
    {
        AddTransition(_generatingCellsState,_playingState,GeneratingCellsToDropsCondition);
        AddTransition(_playingState,_matchHappenedState, PlayingToMatchedCondition);
        AddTransition(_matchHappenedState,_playingState, MatchedToGravityState);
    }
    private bool MatchedToGravityState()
    {
        return !_matchHappenedState.IsRunning;
    }
    private bool PlayingToMatchedCondition()
    {
        return _boardData.HasMatch || _boardData.MatchedDrops.Count > 0;
    }
    private bool GeneratingCellsToDropsCondition()
    {
        return _generatingCellsState.IsFinished;
    }
}