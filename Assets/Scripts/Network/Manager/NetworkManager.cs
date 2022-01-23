using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using com.SluggaGames.MiniShooter.Utils;
using com.SluggaGames.MiniShooter.PlayerSystem;

namespace com.SluggaGames.MiniShooter.Manager
{
  public class NetworkManager : NetworkSingleton<NetworkManager>
  {
    #region Private Fields
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    #endregion

    #region MB Callbacks
    void Start()
    {
      if (playerPrefab == null)
      {
        Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);

      }
      else
      {
        if (PlayerManager.LocalPlayerInstance == null)
        {

          Debug.Log($"We are Instantaiting LocalPlayer from {SceneManagerHelper.ActiveSceneName}");
          PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity);
        }
        else
        {
          Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
      }

    }
    #endregion

    #region PUN Callbacks
    public override void OnLeftRoom()
    {
      SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
      Debug.LogFormat($"{newPlayer.NickName} has joined the room!");
      if (PhotonNetwork.IsMasterClient)
      {
        var partyLeader = PhotonNetwork.MasterClient.NickName;

        Debug.Log($"PUN NetworkManager: Joined Room. You are the Party Leader");
        LoadRoom();
      }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      Debug.Log($"{otherPlayer.NickName} Has left the room ");
      if (PhotonNetwork.IsMasterClient)
      {
        Debug.Log($"Party Leader {PhotonNetwork.MasterClient.NickName} Left room");
        LoadRoom();
      }
    }
    #endregion

    #region Public Method
    public void LeaveRoom()
    {
      PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private Methods
    void LoadRoom()
    {
      if (!PhotonNetwork.IsMasterClient)
      {
        Debug.LogError("PUN Network: Trying to load level but not the master client");

      }
      Debug.Log($"PUN Network: Loading Level {PhotonNetwork.CurrentRoom.PlayerCount}");
      PhotonNetwork.LoadLevel($"Room for {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
    #endregion

  }
}
