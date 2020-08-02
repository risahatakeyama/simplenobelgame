using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;
public class GameDataSettingView : MonoBehaviour
{
    [SerializeField]
    private Text _textViewName;
    [SerializeField]
    private GameObject _objConfirmation;
    [SerializeField]
    private Button _buttonOK;
    [SerializeField]
    private Button _buttonCancel;
    [SerializeField]
    private Text _textConfirmation;
    [SerializeField]
    private Transform _dataPanelContents;
    [SerializeField]
    private Button _buttonReturn;

    private List<GameDataPanel> _gameDataPanelList;
    private SettingType _currentSettingType;
    private List<UserData> _userDataList;
    private long _currentSelectSlot;
    private UserData _currentUserData;
    private Action<GameState, Dictionary<string, object>> _onChangeState;
    private Sprite cacheScreenShot;
    private Dictionary<string, Sprite> _gameDataScreenShotDic=new Dictionary<string, Sprite>();


    private const string KEY_USER_DATA_LIST = "keyUserDataList";
    private const string KEY_SAVE_CONFIRMATION_MESSAGE = "ここに上書きしますか？";
    private const string KEY_LOAD_CONFIRMATION_MESSAGE = "このデータをロードしますか？";
    private const string TEST_SCREEN_SHOT_NAME = "Test";
    private const string RESOURCES_SCREEN_SHOT_PATH = "Assets/Resources/ScreenShots/";
    private const string VIEW_NAME_SAVE="Save";
    private const string VIEW_NAME_LOAD = "Load";
    public void InitView(Action<GameState, Dictionary<string, object>> onChangeState, SettingType settingType,GameState beforeGameState,UserData userData)
    {
        _onChangeState = onChangeState;
        _currentUserData = userData;
        _textViewName.text = settingType == SettingType.Save ? VIEW_NAME_SAVE : VIEW_NAME_LOAD;
        var userDatas=PlayerPrefsUtil.GetValue<UserData[]>(KEY_USER_DATA_LIST);

        if (userDatas == null||userDatas.Length<=0)
        {
            _userDataList = new List<UserData>();
            for(int i = 0; i < 5; i++)
            {
                var index = i;
                _userDataList.Add(new UserData(index + 1));
            }
        }
        else
        {
            _userDataList = userDatas.ToList();
        }

        ScreenCapture.CaptureScreenshot($"{RESOURCES_SCREEN_SHOT_PATH} {TEST_SCREEN_SHOT_NAME}.png");
        cacheScreenShot = ResourceManager.Instance.LoadScreenShot(TEST_SCREEN_SHOT_NAME);
        


        gameObject.SetActive(true);
        _currentSettingType = settingType;

        if (_gameDataPanelList == null)
        {
            _gameDataPanelList = new List<GameDataPanel>();
        }


        UpdateGameDataPanel();


        _buttonOK.onClick.RemoveAllListeners();
        _buttonOK.onClick.AddListener(delegate {
            OnClickOkButton();
        });

        _buttonCancel.onClick.RemoveAllListeners();
        _buttonCancel.onClick.AddListener(delegate {
            _objConfirmation.SetActive(false);
        });
        _buttonReturn.onClick.RemoveAllListeners();
        _buttonReturn.onClick.AddListener(delegate {
            gameObject.SetActive(false);
            _onChangeState(beforeGameState,new Dictionary<string, object>());
        });
    }
    private void UpdateGameDataPanel()
    {
        for (int i = 0; i < _userDataList.Count; i++)
        {
            GameDataPanel panel = null;
            if (_gameDataPanelList.Count > i)
            {
                panel = _gameDataPanelList[i];
            }
            else
            {
                panel = ResourceManager.Instance.InstantiateViewParts<GameDataPanel>(_dataPanelContents);
                _gameDataPanelList.Add(panel);
            }

            panel.Set(_userDataList[i], OnPanelTapped);
        }
    }
    private void OnClickOkButton()
    {        
        switch (_currentSettingType)
        {
            case SettingType.Save:

                var dateTimeOffset = new DateTimeOffset(DateTime.Now, new TimeSpan(+09, 00, 00));
                _currentUserData.time = dateTimeOffset.ToUnixTimeSeconds();

                for (int i = 0; i < _userDataList.Count; i++)
                {
                    if (_userDataList[i].slotId == _currentSelectSlot)
                    {
                        _userDataList[i] = _currentUserData;


                        break;
                    }
                }
                var png=CreateReadabeTexture2D(cacheScreenShot.texture).EncodeToPNG();
                File.WriteAllBytes(RESOURCES_SCREEN_SHOT_PATH+_currentSelectSlot.ToString(), png);

                PlayerPrefsUtil.SetValue<UserData[]>(KEY_USER_DATA_LIST, _userDataList.ToArray());
                UpdateGameDataPanel();

                break;
            case SettingType.Load:

                var loadUserData = _userDataList.FirstOrDefault(data=>data.slotId==_currentSelectSlot);

                _onChangeState(GameState.InitMain, new Dictionary<string, object>() {
                    { "userData",loadUserData}
                });

                break;
        }
        _objConfirmation.gameObject.SetActive(false);
    }
    Texture2D CreateReadabeTexture2D(Texture2D texture2d)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
                    texture2d.width,
                    texture2d.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(texture2d, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D readableTextur2D = new Texture2D(texture2d.width, texture2d.height);
        readableTextur2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTextur2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);
        return readableTextur2D;
    }
    private void OnPanelTapped(long slotId)
    {
        Debug.Log(_currentSettingType);
        //確認メッセージを変える
        switch (_currentSettingType)
        {
            case SettingType.Save:

                _textConfirmation.text = KEY_SAVE_CONFIRMATION_MESSAGE;

                break;
            case SettingType.Load:
                _textConfirmation.text = KEY_LOAD_CONFIRMATION_MESSAGE;
                break;
        }

        _objConfirmation.SetActive(true);
        _currentSelectSlot = slotId;


    }
    private void OnDestroy()
    {
        _buttonOK.onClick.RemoveAllListeners();
        _buttonCancel.onClick.RemoveAllListeners();
        _buttonReturn.onClick.RemoveAllListeners();
    }

}
