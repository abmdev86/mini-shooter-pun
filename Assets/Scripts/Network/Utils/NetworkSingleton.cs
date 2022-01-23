using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.SluggaGames.MiniShooter.Utils
{
  public abstract class NetworkSingleton<T> : MonoBehaviourPunCallbacks where T : NetworkSingleton<T>
  {
    static T _instance;
    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = FindObjectOfType<T>();
          if (_instance == null)
          {
            // gameobject not in scene so create it
            GameObject go = new GameObject(typeof(T).ToString() + " singleton");
            go.tag = "NetworkManager";
            _instance = go.AddComponent<T>();
            return _instance;
          }
          Debug.LogError(typeof(T).ToString() + " is null and no gameobject created");
        }
        return _instance;
      }
    }

    protected virtual void Awake()
    {
      _instance = this as T;
      InitiateNetworkSingletonBase();
    }

    public virtual void InitiateNetworkSingletonBase()
    {
      Debug.Log(typeof(T).ToString() + " Initiation method ran");
    }
  }
}
