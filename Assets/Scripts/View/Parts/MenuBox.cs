using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuBox : MonoBehaviour
{
    [SerializeField]
    private Button _buttonOpen;
    [SerializeField]
    private Button _buttonTitle;
    [SerializeField]
    private Button _buttonSave;
    [SerializeField]
    private Button _buttonLoad;
    [SerializeField]
    private Animator _animator;

    public void Init(Action<GameState, Dictionary<string, object>> onChangeState,Func<UserData> getUserData)
    {
        bool isMenuOpened = false;
        _buttonOpen.onClick.RemoveAllListeners();
        _buttonOpen.onClick.AddListener(delegate {

            isMenuOpened = !isMenuOpened;

            if (isMenuOpened)
            {
                _animator.SetTrigger("Open");
            }
            else
            {
                _animator.SetTrigger("Close");
            }
            
        });
        _buttonTitle.onClick.RemoveAllListeners();
        _buttonTitle.onClick.AddListener(delegate {
            onChangeState(GameState.InitTitle, new Dictionary<string, object>());
        });



        _buttonSave.onClick.RemoveAllListeners();
        _buttonSave.onClick.AddListener(delegate {
            var userData = getUserData();

            onChangeState(GameState.InitGameSetting,new Dictionary<string, object>() {
                {"settingType",SettingType.Save },
                {"beforeGameState",GameState.UpdateMain },
                {"userData",userData }
            });

        });
        _buttonLoad.onClick.RemoveAllListeners();
        _buttonLoad.onClick.AddListener(delegate {
            onChangeState(GameState.InitGameSetting,new Dictionary<string, object> {
                {"settingType",SettingType.Load },
                {"beforeGameState",GameState.UpdateMain }
            });
        });
    }

    private void OnDestroy()
    {
        _buttonOpen.onClick.RemoveAllListeners();
        _buttonTitle.onClick.RemoveAllListeners();
        _buttonSave.onClick.RemoveAllListeners();
        _buttonLoad.onClick.RemoveAllListeners();
    }

}
