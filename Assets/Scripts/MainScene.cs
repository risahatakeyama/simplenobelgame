using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    [SerializeField]
    private Transform _mainCanvas;
    [SerializeField]
    private AudioSource _bgmSource;
    [SerializeField]
    private AudioSource _seSource;
    [SerializeField]
    private AudioSource _voiceSource;


    private GameState _gameState;
    private Dictionary<string, object> _param;

    private TitleView _titleView;
    private MainStoryView _mainStoryView;
    private EndingView _endingView;
    private EventView _eventView;
    private GameDataSettingView _gameDataSettingView;
    private EndingListView _endingListView;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.Init(_bgmSource, _seSource, _voiceSource);
        ChangeState(GameState.InitTitle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState)
        {
            case GameState.InitTitle:
                OnInitTitleProcess();
                break;
            case GameState.UpdateTitle:
                break;
            case GameState.InitMain:
                OnInitMainProcess();
                break;

            case GameState.UpdateMain:

                OnUpdateMainProcess();

                break;
            case GameState.InitEnding:
                OnInitEndingProcess();
                break;
            case GameState.UpdateEnding:
                break;
            case GameState.InitEvent:
                OnInitEventProcess();
                break;
            case GameState.UpdateEvent:
                break;
            case GameState.InitGameSetting:
                OnInitGameSettingProcess();
                break;
            case GameState.UpdateGameSetting:
                break;
            case GameState.InitEndingList:
                OnInitEndingListProcess();
                break;
            case GameState.UpdateEndingList:
                break;

        }
    }
    private void OnInitTitleProcess()
    {
        
        SoundManager.Instance.PlayBgm(BgmType.Title);
        if (_titleView == null)
        {
            _titleView = ResourceManager.Instance.InstantiateView<TitleView>(_mainCanvas);
        }
        HideAllView();
        _titleView.gameObject.SetActive(true);
        _titleView.InitView(ChangeState);

        ChangeState(GameState.UpdateTitle);
    }
    private void OnInitMainProcess()
    {


        if (_mainStoryView == null)
        {
            _mainStoryView = ResourceManager.Instance.InstantiateView<MainStoryView>(_mainCanvas);
        }
        var phraseId = default(long);
        UserData userData = null;
        if (_param!=null)
        {
            if (_param.ContainsKey("phraseId"))
            {
                phraseId = (long)_param["phraseId"];
            }
            if (_param.ContainsKey("userData"))
            {
                userData = (UserData)_param["userData"];
            }
            
        }

        HideAllView();
        _mainStoryView.gameObject.SetActive(true);
        _mainStoryView.InitView(ChangeState,phraseId,userData);

        ChangeState(GameState.UpdateMain);
    }


    private void OnUpdateMainProcess()
    {



    }
    private void OnInitEndingProcess()
    {

        if (_endingView == null)
        {
            _endingView = ResourceManager.Instance.InstantiateView<EndingView>(_mainCanvas);
        }
        long endingId = default(long);
        UserData userData = null;
        if (_param != null)
        {
            if (_param.ContainsKey("endingId"))
            {
                endingId = (long)_param["endingId"];
            }
            if (_param.ContainsKey("userData"))
            {
                userData = (UserData)_param["userData"];
            }
        }

        HideAllView();
        _endingView.gameObject.SetActive(true);
        _endingView.InitView(endingId,ChangeState,null,userData);
        ChangeState(GameState.UpdateEnding);
    }
    private void OnInitEventProcess()
    {
        if (_eventView == null)
        {
            _eventView = ResourceManager.Instance.InstantiateView<EventView>(_mainCanvas);
        }

        var eventId = (long)_param["eventId"];
        HideAllView();
        _eventView.gameObject.SetActive(true);
        _eventView.InitView(eventId,ChangeState);
        ChangeState(GameState.UpdateEvent);
    }

    private void OnInitGameSettingProcess()
    {
        if (_gameDataSettingView == null)
        {
            _gameDataSettingView = ResourceManager.Instance.InstantiateView<GameDataSettingView>(_mainCanvas);
        }

        UserData currentUserData = null;
        SettingType settingType = SettingType.None;
        GameState beforeGameState = GameState.InitTitle;
        if (_param != null)
        {

            if (_param.ContainsKey("settingType"))
            {
                settingType = (SettingType)_param["settingType"];
            }
            if (_param.ContainsKey("beforeGameState"))
            {
                beforeGameState = (GameState)_param["beforeGameState"];
            }


            if (_param.ContainsKey("userData"))
            {
                currentUserData = (UserData)_param["userData"];
            }
        }
        _gameDataSettingView.gameObject.transform.SetAsLastSibling();
        _gameDataSettingView.InitView(ChangeState,settingType,beforeGameState,currentUserData);

        ChangeState(GameState.UpdateGameSetting);
    }
    private void OnInitEndingListProcess()
    {
        if (_endingListView == null)
        {
            _endingListView = ResourceManager.Instance.InstantiateView<EndingListView>(_mainCanvas);

        }
        //HideAllView();
        _endingListView.InitView(ChangeState);
        ChangeState(GameState.UpdateEndingList);
    }
    private void HideAllView()
    {
        if (_titleView != null)
        {
            _titleView.gameObject.SetActive(false);
        }

        if (_mainStoryView != null)
        {
            _mainStoryView.gameObject.SetActive(false);
        }

        if (_endingView != null)
        {
            _endingView.gameObject.SetActive(false);
        }

        if (_eventView != null)
        {
            _eventView.gameObject.SetActive(false);
        }
        if (_gameDataSettingView != null)
        {
            _gameDataSettingView.gameObject.SetActive(false);
        }
        if(_endingListView!=null)
        {
            _endingListView.gameObject.SetActive(false);
        }
    }
    private void ChangeState(GameState state,Dictionary<string,object> param=null)
    {
        _gameState = state;
        _param = param;
    }
}
