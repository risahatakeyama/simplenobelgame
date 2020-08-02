using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class EventPanel : MonoBehaviour
{
    [SerializeField]
    private string[] _descriptions;
    [SerializeField]
    private Button _buttonTapArea;
    [SerializeField]
    private Text _textDescription;
    public virtual void InitPanel(Action<bool,bool> onGameEnd)
    {
        var index = 0;

        _textDescription.text = _descriptions[0];

        //ゲームの説明が入る
        _buttonTapArea.onClick.RemoveAllListeners();
        _buttonTapArea.onClick.AddListener(delegate {
            index++;

            if (_descriptions.Length - 1 < index)
            {
                index = 0;

                _buttonTapArea.gameObject.SetActive(false);


            }
            else if (_descriptions.Length - 1 == index)
            {
               // SetChoiceBox(phraseData.ChoiceDatas);

            }

            _textDescription.text = _descriptions[index];
        });
    }
    protected virtual void OnDestroy()
    {
        _buttonTapArea.onClick.RemoveAllListeners();
    }
}
