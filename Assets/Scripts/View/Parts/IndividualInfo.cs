using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class IndividualInfo : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField][CustomId(nameof(ItemData))]
    private long _itemId;//ものの説明が入る//表示する画像なども設定
    //個体識別番号
    [SerializeField]
    private long _guid;

    public void SetId(long itemId,long guid)
    {
        _itemId=itemId;
        _guid = guid;
    }
    public void RemoveId()
    {
        _itemId = default(long);
        _guid = default(long);
    }
    public void SetIndividualInfo(Action<long,long> callBack)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(delegate {

            callBack?.Invoke(_itemId,_guid);
        });

    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

}
