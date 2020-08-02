using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ChoicePanel : MonoBehaviour
{
    [SerializeField]
    private Text _textChoice;
    [SerializeField]
    private Button _buttonSelect;
    public void SetPanel(ChoiceInfo choiceInfo,Action<long,int> onUpgradeRating)
    {
        _textChoice.text = choiceInfo.selectionMessage;
        _buttonSelect.onClick.RemoveAllListeners();
        _buttonSelect.onClick.AddListener(()=> {
            SoundManager.Instance.PlaySe(SeType.Tap);
            onUpgradeRating?.Invoke(choiceInfo.targetCharacterId,choiceInfo.updradeRating);

        });
    }
    private void OnDestroy()
    {
        _buttonSelect.onClick.RemoveAllListeners();
    }
}
