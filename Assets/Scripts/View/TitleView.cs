using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TitleView : MonoBehaviour
{
    [SerializeField]
    private Button _buttonNewGame;
    [SerializeField]
    private Button _buttonLoad;
    [SerializeField]
    private Button _buttonEndingList;
    public void InitView(Action<GameState, Dictionary<string, object>> onChangeState)
    {
        _buttonNewGame.onClick.RemoveAllListeners();
        _buttonNewGame.onClick.AddListener(() => {
            SoundManager.Instance.PlaySe(SeType.OK);
            onChangeState(GameState.InitMain, null);
        });
        _buttonLoad.onClick.RemoveAllListeners();
        _buttonLoad.onClick.AddListener(delegate {
            SoundManager.Instance.PlaySe(SeType.OK);
            onChangeState(GameState.InitGameSetting, new Dictionary<string, object>() {
                { "settingType",SettingType.Load},
                { "beforeGameState",GameState.UpdateTitle},

            });
        });

        _buttonEndingList.onClick.RemoveAllListeners();
        _buttonEndingList.onClick.AddListener(delegate {
            SoundManager.Instance.PlaySe(SeType.OK);
            onChangeState(GameState.InitEndingList, null);
        });
    }
    private void OnDestroy()
    {
        _buttonLoad.onClick.RemoveAllListeners();
        _buttonNewGame.onClick.RemoveAllListeners();
        _buttonEndingList.onClick.RemoveAllListeners();
    }

}
