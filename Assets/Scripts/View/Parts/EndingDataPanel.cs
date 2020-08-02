using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class EndingDataPanel : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Text _textTitle;
    [SerializeField]
    private Text _textDescription;
    [SerializeField]
    private Button _buttonPanel;
    public void SetPanel(EndingData endingData,bool isUnlock,Action<long> callBack)
    {
        _buttonPanel.onClick.RemoveAllListeners();
        if (isUnlock)
        {
            var backGroundData=ResourceManager.Instance.LoadMasterData<BackGroundData, BackGroundDataObjectList>(endingData.BackGroundId);
            _image.sprite = backGroundData.Sprites[0];

            _textTitle.text = $"End{endingData.EndingNumber}:{endingData.Memo}";

            _textDescription.text = endingData.Description;

            _buttonPanel.onClick.AddListener(delegate {
                callBack?.Invoke(endingData.Id);
            });
        }
        else
        {
            _textTitle.text =$"End{endingData.EndingNumber}:???????";
            _textDescription.text = string.Empty;

        }

    }
    private void OnDestroy()
    {
        _buttonPanel.onClick.RemoveAllListeners();
    }
}
