using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SearchPanel : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _roomLayoutObjList;
    [SerializeField]
    private List<IndividualInfo> _individualInfoList;
    public void InitRoomLayout(int index,Action<long,long> callBack)
    {
        foreach(var info in _individualInfoList)
        {
            info.SetIndividualInfo(callBack);
        }
        //特定のindexのみのobject群をオンにする
        SetRoomLayout(index);
    }
    public void SetRoomLayout(int index)
    {
        var roomIndex = 0;
        foreach(var roomObj in _roomLayoutObjList)
        {

            roomObj.SetActive(roomIndex == index);
            roomIndex++;
        }
    }
}
