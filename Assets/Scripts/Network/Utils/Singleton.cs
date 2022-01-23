
using UnityEngine;
using Photon.Pun;
using System;

namespace com.SluggaGames.MiniShooter.Utils
{
  public abstract class Singleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
  {
    static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

    public static T Instance => LazyInstance.Value;
    

    static T CreateSingleton()
    {
      var ownerObject = new GameObject($"{typeof(T).Name} (MBPunCB Singleton");
      var instance = ownerObject.AddComponent<T>();
      // DontDestroyOnLoad(ownerObject);
      return instance;
    }
  }
}
