using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private string[] _description;
    [SerializeField]
    private string[] _actionDescription;
    [SerializeField]
    private string[] _afterActionDescription;
    [SerializeField]
    private string[] _usedDescription;
    [SerializeField]
    private bool _isInputItem;
    [SerializeField]
    private string _answer;

    [SerializeField][CustomId(nameof(BackGroundData))]
    private long _getItemBackGroundId;
    [SerializeField]
    private bool _isOnce;

    [SerializeField]
    [CustomId(nameof(ItemData))]
    private long[] _conditionItemIds;

    [SerializeField]
    private Sprite _defaultItemSprite;
    [SerializeField]
    private Sprite _usedItemSprite;
    [SerializeField]
    private string _actionButtonString;
    [SerializeField]
    private string _cancelButtonString;

    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }

    public string[] Description { get { return _description; } }
    public string[] ActionDescription { get { return _actionDescription; } }
    public string[] AfterActionDescription { get { return _afterActionDescription; } }
    public string[] UsedDescription { get { return _usedDescription; } }
    public bool IsInputItem { get { return _isInputItem; } }
    public string Answer { get { return _answer; } }

    public long GetItemBackGroundId { get { return _getItemBackGroundId; } }
    public bool IsOnce { get { return _isOnce; } }
    public long[] ConditionItemIds { get { return _conditionItemIds; } }

    public Sprite DefaultItemSprite { get { return _defaultItemSprite; } }
    public Sprite UsedItemSprite { get { return _usedItemSprite; } }

    public string ActionButtonString { get { return _actionButtonString; } }
    public string CancelButtonString { get { return _cancelButtonString; } }
}
