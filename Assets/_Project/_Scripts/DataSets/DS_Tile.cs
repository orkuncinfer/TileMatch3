using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class DS_Tile : Data
{
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;
    
    [SerializeField] private GenericKey _tileType;
    public GenericKey TileType  
    {
        get => _tileType;
        set => _tileType = value;
    }
    
    [SerializeField] private int _layerIndex;
    public int LayerIndex  
    {
        get => _layerIndex;
        set => _layerIndex = value;
    }
    
    [SerializeField] private bool _isBlocked;
    public bool IsBlocked  
    {
        get => _isBlocked;
        set => _isBlocked = value;
    }
    
    [SerializeField] private GameObject _blockedTint;
    public GameObject BlockedTint  
    {
        get => _blockedTint;
        set => _blockedTint = value;
    }
    
    [SerializeField] private Collider2D _tileCollider;
    public Collider2D TileCollider  
    {
        get => _tileCollider;
        set => _tileCollider = value;
    }
    
    [SerializeField] private SpriteRenderer _tileSpriteRenderer;
    public SpriteRenderer TileSpriteRenderer  
    {
        get => _tileSpriteRenderer;
        set => _tileSpriteRenderer = value;
    }
    
    [ReadOnly][SerializeField] private bool _isSelected;
    public bool IsSelected  
    {
        get => _isSelected;
        set => _isSelected = value;
    }
    
    [ReadOnly][SerializeField] private bool _isSettled;
    public bool IsSettled  
    {
        get => _isSettled;
        set => _isSettled = value;
    }
    
    [ReadOnly][SerializeField] private bool _isMatched;
    public bool IsMatched  
    {
        get => _isMatched;
        set => _isMatched = value;
    }
    
    private DS_TileBoard _boardData;
    public DS_TileBoard BoardData  
    {
        get => _boardData;
        set => _boardData = value;
    }
}