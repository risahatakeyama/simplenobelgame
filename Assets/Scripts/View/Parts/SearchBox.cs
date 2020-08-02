using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class SearchBox : MonoBehaviour
{



    [SerializeField]
    private Image _imageBackGround;



    [SerializeField]
    private GameObject _objSearchAction;

    [SerializeField]
    private GameObject _objSelectionBox;
    [SerializeField]
    private Button _buttonAction;
    [SerializeField]
    private Text _textButtonAction;

    [SerializeField]
    private Button _buttonCancel;
    [SerializeField]
    private Text _textButtonCancel;


    [SerializeField]
    private InputField _input;//buttonActionが化ける


    [SerializeField]
    private Text _textDescription;
    [SerializeField]
    private Image _imageSearchItem;
    //[SerializeField]
    //private GameObject _objSearchItem;
    [SerializeField]
    private Button _tapArea;



    [SerializeField]
    private RectTransform _searchPanelContent;

    [SerializeField]
    private Button _buttonRight;
    [SerializeField]
    private Button _buttonLeft;


    [SerializeField]
    private IndividualInfo _buttonHaveItem1;
    [SerializeField]
    private Image _haveItem1;
    [SerializeField]
    private IndividualInfo _buttonHaveItem2;
    [SerializeField]
    private Image _haveItem2;

    private int _backGroundIndex=0;
    private BackGroundData _backGroundData;
    private ItemData _itemData;
    private long _currentGuid;
    private UserData _userData;
    private Func<KeyValuePair<long, long>, KeyValuePair<long, long>, UserData> _onUpdateHaveItems;
    private Func<KeyValuePair<long, long>, UserData> _onUpdateUsedItem;
    private long _backGroundId;

    public void SetSearchBox(long backGroundId,UserData userData,
        Func<KeyValuePair<long, long>,KeyValuePair<long,long>,UserData> onUpdateHaveItems,
        Func<KeyValuePair<long, long>, UserData> onUpdateUsedItem)
    {
        _onUpdateHaveItems = onUpdateHaveItems;
        _onUpdateUsedItem = onUpdateUsedItem;

        _userData = userData;
        _backGroundData = ResourceManager.Instance.LoadMasterData<BackGroundData, BackGroundDataObjectList>(backGroundId);
        ChangeBackGroundForIndex(_backGroundIndex);
        //searchPanel生成
        ClearBox();
        var searchPanel = ResourceManager.Instance.InstantiateSearchPanel(_searchPanelContent, backGroundId);
        if (searchPanel != null)
        {
            searchPanel.InitRoomLayout(_backGroundIndex, OnSearchPanelTapped);
        }
        

        _objSearchAction.SetActive(false);

        _backGroundId = backGroundId;

        _buttonCancel.onClick.RemoveAllListeners();
        _buttonCancel.onClick.AddListener(delegate {
            _objSearchAction.SetActive(false);
        });

        _input.onValueChanged.RemoveAllListeners();
        _input.onValueChanged.AddListener(delegate {

            if (_itemData.Answer == _input.text)
            {
                UseItem(_onUpdateHaveItems, _onUpdateUsedItem);
                //action後の説明があれば表示する
                if (_itemData.AfterActionDescription.Length > 0)
                {
                    _textDescription.text = _itemData.AfterActionDescription[0];
                    _imageSearchItem.sprite = _itemData.UsedItemSprite;
                    _objSelectionBox.SetActive(false);
                    _objSearchAction.SetActive(true);
                    SetTapArea(_itemData.AfterActionDescription);

                }
                else
                {
                    _objSearchAction.SetActive(false);
                    _tapArea.gameObject.SetActive(false);
                }
            }

        });
        _buttonHaveItem1.SetIndividualInfo(OnItemButtonTapped);
        _buttonHaveItem2.SetIndividualInfo(OnItemButtonTapped);


        SetRightAndLeftButton(searchPanel);



    }
    private void SetChoiceRemoveItemButton()
    {
        _buttonAction.onClick.RemoveAllListeners();
        _buttonAction.onClick.AddListener(delegate {
            _objSelectionBox.SetActive(false);
            RemoveItem(_currentGuid, _onUpdateHaveItems);
            _objSearchAction.SetActive(false);
            _tapArea.gameObject.SetActive(false);
        });
    }
    private void SetChoiceActionButton()
    {

        _buttonAction.onClick.RemoveAllListeners();
        _buttonAction.onClick.AddListener(delegate {
            _objSelectionBox.SetActive(false);

            if (_itemData.GetItemBackGroundId == _backGroundId)
            {
                //所持できるのであれば所持する
                AddItem(_onUpdateHaveItems);
            }

            //使えるのであれば使う
            if (_itemData.ConditionItemIds.Length > 0)
            {
                UseItem(_onUpdateHaveItems, _onUpdateUsedItem);
            }

            if (_itemData.UsedItemSprite != null)
            {
                _imageSearchItem.sprite = _itemData.UsedItemSprite;
            }

            //action後の説明があれば表示する
            if (_itemData.AfterActionDescription.Length > 0)
            {
                _textDescription.text = _itemData.AfterActionDescription[0];

                SetTapArea(_itemData.AfterActionDescription);

            }
            else
            {
                _objSearchAction.SetActive(false);
                _tapArea.gameObject.SetActive(false);
            }



        });
    }
    private void SetRightAndLeftButton(SearchPanel searchPanel)
    {
        _buttonLeft.onClick.RemoveAllListeners();
        _buttonLeft.onClick.AddListener(delegate {
            if (_backGroundData.Sprites.Length - 1 > _backGroundIndex)
            {
                _backGroundIndex++;
            }
            else if (_backGroundData.Sprites.Length - 1 <= _backGroundIndex)
            {
                _backGroundIndex = 0;
            }

            ChangeBackGroundForIndex(_backGroundIndex);
            searchPanel?.SetRoomLayout(_backGroundIndex);
        });
        _buttonRight.onClick.RemoveAllListeners();
        _buttonRight.onClick.AddListener(delegate {
            if (_backGroundIndex > 0)
            {
                _backGroundIndex--;
            }
            else if (_backGroundIndex <= 0)
            {
                _backGroundIndex = _backGroundData.Sprites.Length - 1;
            }

            ChangeBackGroundForIndex(_backGroundIndex);
            searchPanel?.SetRoomLayout(_backGroundIndex);
        });
    }

    private void ClearBox()
    {
        if (_searchPanelContent.childCount > 0)
        {
            foreach (Transform child in _searchPanelContent)
            {
                Destroy(child.gameObject);
            }
        }

    }
    private void ChangeBackGroundForIndex(int index)
    {
        if (_backGroundData != null)
        {
            if (_backGroundData.Sprites[index] == null)
            {
                index = 0;
            }
            _imageBackGround.sprite = _backGroundData.Sprites[index];
            _imageBackGround.color = _backGroundData.SpriteColor;

            _backGroundIndex = index;
        }
    }
    private void OnSearchPanelTapped(long itemId,long guid)
    {
        //所持とかしていなかったら表示する
        if (!_userData.haveItem1.Key.Equals(guid)&&!_userData.haveItem2.Key.Equals(guid))
        {
            _itemData = ResourceManager.Instance.LoadMasterData<ItemData, ItemDataObjectList>(itemId);
            _currentGuid = guid;
            string[] descriptions = _userData.usedItemDict.ContainsKey(guid) ? _itemData.UsedDescription : _itemData.Description;
            _textDescription.text = descriptions[0];
            _imageSearchItem.sprite = _userData.usedItemDict.ContainsKey(guid)?_itemData.UsedItemSprite:_itemData.DefaultItemSprite;
            _objSelectionBox.SetActive(false);
            _objSearchAction.SetActive(true);


            SetTapArea(descriptions);
            
        }

        
    }
    private void OnItemButtonTapped(long itemId,long guid)
    {
        _currentGuid = guid;
        var itemData = ResourceManager.Instance.LoadMasterData<ItemData, ItemDataObjectList>(itemId);
        if (itemData == null)
        {
            return;
        }
        string[] removeDescriptions = new string[] { "このアイテムはまだ使用できます。","アイテムを外しますか？" };
        _textDescription.text = removeDescriptions[0];
        _imageSearchItem.sprite = itemData.DefaultItemSprite;
        _objSelectionBox.SetActive(false);
        _objSearchAction.SetActive(true);

        SetTapArea(removeDescriptions, true);
        

    }
    private void AddItem(Func<KeyValuePair<long, long>, KeyValuePair<long,long>,UserData> onUpdateHaveItemIds)
    {
        if (_itemData.GetItemBackGroundId!=default(long) && (_userData.haveItem1.Value == default(long) || _userData.haveItem2.Value == default(long)))
        {
            

            if (_userData.haveItem1.Value == default(long))
            {
                _userData.haveItem1 = new KeyValuePair<long, long>(_currentGuid,_itemData.Id);
                _buttonHaveItem1.SetId(_itemData.Id, _currentGuid);
                _haveItem1.sprite = _itemData.DefaultItemSprite;
            }
            else if (_userData.haveItem2.Value == default(long))
            {
                _userData.haveItem2 = new KeyValuePair<long, long>(_currentGuid,_itemData.Id);
                _buttonHaveItem2.SetId(_itemData.Id, _currentGuid);
                _haveItem2.sprite = _itemData.DefaultItemSprite;
            }
        }
        _userData=onUpdateHaveItemIds?.Invoke(_userData.haveItem1,_userData.haveItem2);

    }
    private void UseItem(Func<KeyValuePair<long, long>, KeyValuePair<long, long>, UserData> onUpdateHaveItemIds,
        Func<KeyValuePair<long, long>,UserData> onUpdateUsedItem)
    {
        ItemData haveItemData = null;

        if (_itemData.ConditionItemIds.Contains(_userData.haveItem1.Value))
        {
            haveItemData = ResourceManager.Instance.LoadMasterData<ItemData, ItemDataObjectList>(_userData.haveItem1.Value);

            if (haveItemData.IsOnce)
            {

                
                _userData.haveItem1 = new KeyValuePair<long, long>();
                _buttonHaveItem1.RemoveId();
                _haveItem1.sprite = null;
            }

        }
        if (_itemData.ConditionItemIds.Contains(_userData.haveItem2.Value))
        {
            haveItemData = ResourceManager.Instance.LoadMasterData<ItemData, ItemDataObjectList>(_userData.haveItem2.Value);
            if (haveItemData.IsOnce)
            {
                
                _userData.haveItem2 = new KeyValuePair<long, long>();
                _buttonHaveItem2.RemoveId();
                _haveItem2.sprite = null;
            }

        }
        _userData = onUpdateUsedItem(new KeyValuePair<long, long>(_currentGuid, _itemData.Id));
        _userData = onUpdateHaveItemIds?.Invoke(_userData.haveItem1, _userData.haveItem2);



    }


    private void RemoveItem(long removeGuid, Func<KeyValuePair<long, long>, KeyValuePair<long, long>, UserData> onUpdateHaveItemIds)
    {
        if (_userData.haveItem1.Key.Equals(removeGuid))
        {
            _userData.haveItem1 = new KeyValuePair<long, long>();
            _buttonHaveItem1.RemoveId();
            _haveItem1.sprite = null;
        }

        if (_userData.haveItem2.Key.Equals(removeGuid))
        {
            _userData.haveItem2 = new KeyValuePair<long, long>();
            _buttonHaveItem2.RemoveId();
            _haveItem2.sprite = null;
        }

        _userData = onUpdateHaveItemIds?.Invoke(_userData.haveItem1, _userData.haveItem2);
    }

    private void SetTapArea(string[] descriptions,bool isItemRemove=false)
    {
        _tapArea.gameObject.SetActive(true);
        int index = 0;
        _tapArea.onClick.RemoveAllListeners();
        _tapArea.onClick.AddListener(delegate {

            index++;

            if (descriptions.Length - 1 < index)
            {
                index = 0;

                var haveItemIds = new long[] { _userData.haveItem1.Value, _userData.haveItem2.Value };
                bool isContain = true;
                foreach(var itemId in _itemData.ConditionItemIds)
                {
                    if (!haveItemIds.Contains(itemId))
                    {
                        isContain = false;
                    }
                }

                if (EqualDescriptions(descriptions, _itemData.Description) &&
                (_itemData.IsInputItem || _itemData.GetItemBackGroundId!=default(long)|| isContain) && 
                !isItemRemove&&
                _itemData.ActionDescription.Length>0)//必要アイテムがあれば次へ進む
                {
                    descriptions = _itemData.ActionDescription;
                }
                else
                {
                    _objSearchAction.SetActive(false);
                    _objSelectionBox.SetActive(false);
                    _tapArea.gameObject.SetActive(false);
                    return;
                }



            }
            if (descriptions.Length - 1 == index)
            {
                if (isItemRemove)
                {
                    SetChoiceRemoveItemButton();
                    SetSelectionBox("はい", "いいえ", false);
                }else if (EqualDescriptions(descriptions, _itemData.ActionDescription))
                {
                    SetChoiceActionButton();
                    SetSelectionBox(_itemData.ActionButtonString,_itemData.CancelButtonString,_itemData.IsInputItem);
                }




            }

            if (descriptions.Length - 1>=index)
            {
                _textDescription.text = descriptions[index];
            }
           

        });
    }

    private bool EqualDescriptions(string[] compDescriptions1,string[] compDescriptions2)
    {
        bool isEqual = true;

        if (object.ReferenceEquals(compDescriptions1, compDescriptions2))
        {
            isEqual = true;
        }
        else if (compDescriptions1 == null || compDescriptions2 == null
            || compDescriptions1.Length != compDescriptions2.Length)
        {
            isEqual = false;
        }
        else
        {
            for (int i = 0; i < compDescriptions1.Length; i++)
            {
                if (!compDescriptions1[i].Equals(compDescriptions2[i]))
                {
                    isEqual = false;
                    break;
                }
            }
        }
        return isEqual;
    }

    private void SetSelectionBox(string actionButtonString,string cancelButtonString,bool isInputItem)
    {
        _objSelectionBox.SetActive(true);
        _textButtonAction.text = actionButtonString;
        _textButtonCancel.text = cancelButtonString;

        _input.gameObject.SetActive(isInputItem);
        _buttonAction.gameObject.SetActive(!isInputItem);

        _tapArea.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _buttonLeft.onClick.RemoveAllListeners();
        _buttonRight.onClick.RemoveAllListeners();

        _tapArea.onClick.RemoveAllListeners();

        _buttonAction.onClick.RemoveAllListeners();
        _buttonCancel.onClick.RemoveAllListeners();

        _onUpdateHaveItems = null;
    }
}
