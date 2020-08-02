using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class ChairSelectionGame : EventPanel
{

    [SerializeField]
    private Text _textTurn;

    [SerializeField]
    private Button _buttonChairKing;
    [SerializeField]
    private Image _imageSelectKing;

    [SerializeField]
    private Button _buttonChairAristocrat;
    [SerializeField]
    private Image _imageSelectAristocrat;

    [SerializeField]
    private Button _buttonChairCommon;
    [SerializeField]
    private Image _imageSelectCommon;

    [SerializeField]
    private Button _buttonChairSlave;
    [SerializeField]
    private Image _imageSelectSlave;


    [SerializeField]
    private Button _buttonDecision;

    private enum ClassType {

        King,
        Aristocrat,
        Common,
        Slave
    }
    private Dictionary<ClassType, int> _classDict = new Dictionary<ClassType, int>() {

        { ClassType.King,1000},
        {ClassType.Aristocrat,500 },
        {ClassType.Common,100 },
        {ClassType.Slave,10 }
    };
    private List<ClassType> _myChooseableList = new List<ClassType>() {
        ClassType.King,ClassType.Aristocrat,ClassType.Common,ClassType.Slave
    };

    private List<ClassType> _opponentChooseableList = new List<ClassType>() {
        ClassType.King,ClassType.Aristocrat,ClassType.Common,ClassType.Slave
    };
    private bool _isMyTurn;
    private ClassType _currentSelectClassType;
    private int _remainMatchesCount;

    private ClassType _opponentSelectClassType;
    private int _myPoint;
    private int _opponentPoint;

    public override void InitPanel(Action<bool,bool> onGameEnd)
    {

        base.InitPanel(onGameEnd);
        _remainMatchesCount = 6;
        _isMyTurn = true;//自分がどの椅子と対話するか選ぶ番。奴隷以外は何も起きない。
        _textTurn.text = _isMyTurn ? "あなたのターン" : "相手のターン";

        SetChairButton(_buttonChairKing, ClassType.King);
        SetChairButton(_buttonChairAristocrat, ClassType.Aristocrat);
        SetChairButton(_buttonChairCommon, ClassType.Common);
        SetChairButton(_buttonChairSlave, ClassType.Slave);


        _buttonDecision.onClick.RemoveAllListeners();
        _buttonDecision.onClick.AddListener(delegate {

            

            //相手の椅子を選ぶ処理。どのようにロジックを組むか。
            _opponentSelectClassType = SelectOpponentChair(_isMyTurn);

            if (_isMyTurn)
            {
                
                if (_currentSelectClassType == _opponentSelectClassType)
                {
                    if (_currentSelectClassType == ClassType.Slave)
                    {
                        //相手を処刑
                        //gameend
                        onGameEnd(true, false);
                    }
                }
                else
                {
                    if (_currentSelectClassType == ClassType.King&&_opponentSelectClassType==ClassType.Slave)
                    {
                        //相手にポイント加算
                        _opponentPoint += _classDict[_currentSelectClassType];
                    }
                    else
                    {
                        //相手に通常ポイント
                        _opponentPoint += _classDict[_opponentSelectClassType];
                    }

                }
                //自分の椅子選択可能リスト更新
                _myChooseableList.Remove(_currentSelectClassType);
                SetActiveChair(_opponentChooseableList);
            }
            else
            {
                if(_currentSelectClassType == _opponentSelectClassType)
                {
                    if (_opponentSelectClassType == ClassType.Slave)
                    {
                        //自分が処刑される
                        //gameend
                        onGameEnd(false, false);
                    }
                }
                else
                {
                    if (_opponentSelectClassType == ClassType.King && _currentSelectClassType == ClassType.Slave)
                    {
                        //自分にポイント加算
                        _myPoint += _classDict[_opponentSelectClassType];
                    }
                    else
                    {
                        //自分に通常ポイント加算
                        _myPoint += _classDict[_currentSelectClassType];
                    }
                }
                _opponentChooseableList.Remove(_opponentSelectClassType);
                SetActiveChair(_myChooseableList);
                
            }
            _remainMatchesCount--;

            if (_remainMatchesCount <= 0)
            {
                //gameend
                onGameEnd(_myPoint > _opponentPoint, false);
            }
            _buttonDecision.gameObject.SetActive(false);
            _isMyTurn = !_isMyTurn;
            _textTurn.text = _isMyTurn ? "あなたのターン" : "相手のターン";
        });




    }

    private void SetActiveChair(List<ClassType> chooseableList)
    {
        foreach (ClassType classType in Enum.GetValues(typeof(ClassType)))
        {
            var button = GetChairButton(classType);
            button.gameObject.SetActive(chooseableList.Contains(classType));
        }
    }
    private void SetActiveSelectImage(ClassType classType)
    {
        foreach (ClassType type in Enum.GetValues(typeof(ClassType)))
        {
            var image = GetChairSelectImage(type);
            image.gameObject.SetActive(type==classType);
        }
    }
    private void SetChairButton(Button button,ClassType classType)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate {
            _currentSelectClassType = classType;
            SetActiveSelectImage(classType);

            _buttonDecision.gameObject.SetActive(true);
        });
    }
    private Button GetChairButton(ClassType classType)
    {
        Button button = null;
        switch (classType)
        {
            case ClassType.Aristocrat:
                button = _buttonChairAristocrat;
                break;
            case ClassType.Common:
                button = _buttonChairCommon;
                break;
            case ClassType.King:
                button = _buttonChairKing;
                break;
            case ClassType.Slave:
                button = _buttonChairSlave;
                break;
        }
        return button;
    }
    private Image GetChairSelectImage(ClassType classType)
    {
        Image image = null;
        switch (classType)
        {
            case ClassType.Aristocrat:
                image = _imageSelectAristocrat;
                break;
            case ClassType.Common:
                image = _imageSelectCommon;
                break;
            case ClassType.King:
                image = _imageSelectKing;
                break;
            case ClassType.Slave:
                image = _imageSelectSlave;
                break;
        }
        return image;
    }
    private ClassType SelectOpponentChair(bool isMyTurn)
    {
        var classType = ClassType.Slave;
        var maxPoint = 0;
        if (isMyTurn)
        {
            
            foreach(var type in _myChooseableList)
            {
                if (maxPoint <= _classDict[type])
                {
                    maxPoint = _classDict[type];
                    classType = type;
                }
            }
        }
        else
        {
            foreach(var type in _opponentChooseableList)
            {
                if (maxPoint <= _classDict[type])
                {
                    maxPoint = _classDict[type];
                    classType = type;
                }
            }
        }


        return classType;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();


        _buttonChairSlave.onClick.RemoveAllListeners();
        _buttonChairAristocrat.onClick.RemoveAllListeners();
    }
}

