using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ResourceManager : SingletonMonobihavior<ResourceManager>
{

    public TView InstantiateView<TView>(Transform objectParent) where TView : MonoBehaviour
    {
        return InstantiateCanvasObj<TView>($"View/{typeof(TView).Name}", objectParent);
    }
    public TViewParts InstantiateViewParts<TViewParts>(Transform objectParent) where TViewParts : MonoBehaviour
    {
        return InstantiateCanvasObj<TViewParts>($"View/Parts/{typeof(TViewParts).Name}",objectParent);
    }
    public TEventPanel InstantiateEventPanel<TEventPanel>(Transform objectParent,long eventId)where TEventPanel:EventPanel
    {
        return InstantiateCanvasObj<TEventPanel>($"View/EventPanel/{nameof(EventPanel)}-{eventId}",objectParent);
    }
    public SearchPanel InstantiateSearchPanel(Transform objectParent,long backGroundId)
    {
        return InstantiateCanvasObj<SearchPanel>($"View/SearchPanel/{ nameof(SearchPanel) }-{backGroundId}", objectParent);
    }
    public Sprite LoadScreenShot(string shotName)
    {
        return Resources.Load<Sprite>($"ScreenShots/{shotName}");
    }

    private TCanvasObject InstantiateCanvasObj<TCanvasObject>(string path,Transform objectParent) where TCanvasObject : MonoBehaviour
    {
        
        Debug.Log($"[InstantiatePrefabPath]:{path}");
        var viewPref = Resources.Load<GameObject>(path);

        if (viewPref != null)
        {
            var viewObj = Instantiate(viewPref, objectParent, false);
            var view = viewObj.GetComponent(typeof(TCanvasObject)) as TCanvasObject;
            return view;
        }
        return null;
    }
    public TActor InstantiateActor<TActor>(Vector3 insPos) where TActor:MonoBehaviour
    {
        var pref = Resources.Load<GameObject>($"Actor/{ nameof(TActor)}");
        return InstantiateObj<TActor>(pref, insPos);
    }
    //script返したいやつ
    private TScript InstantiateObj<TScript>(GameObject prefab,Vector3 insPos) where TScript:MonoBehaviour
    {
        if (prefab != null)
        {
            var obj = Instantiate(prefab,insPos,Quaternion.identity);
            var tScript = obj.GetComponent(typeof(TScript)) as TScript;
            return tScript;
        }
        
        return null;
    }


    private const string ScriptableObjectPath = "MasterBook";


    //MasterBook


    public TData LoadMasterData<TData, TDataObjectList>(long id)
        where TData : ScriptableObject, IMasterBookData
        where TDataObjectList : DataObjectList<TData>
    {
        var dataObject = Resources.Load<TDataObjectList>($"{ScriptableObjectPath}/{typeof(TDataObjectList).Name}");
        return dataObject.dataObjectList.FirstOrDefault(data => data.Id == id);
    }
    public List<TData> LoadMasterDataAll<TData, TDataObjectList>()
        where TData : ScriptableObject, IMasterBookData
        where TDataObjectList : DataObjectList<TData>
    {
        return Resources.Load<TDataObjectList>($"{ScriptableObjectPath}/{typeof(TDataObjectList).Name}").dataObjectList;
    }



}
