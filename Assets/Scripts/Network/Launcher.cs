
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace com.SluggaGames.MiniShooter.Network
{

  public class Launcher : MonoBehaviourPunCallbacks
  {
    #region private Serializable Fields
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField] private byte maxPlayersPerRoom = 4;
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField] GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField] GameObject progressLabel;
    bool isConnecting;
    #endregion

    #region private Fields
    string _gameVersion = "1";
    #endregion

    #region MB callbacks
    private void Awake()
    {
      PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {

      progressLabel.SetActive(false);
      controlPanel.SetActive(true);
    }




    #endregion

    #region Public Methods

    /// <summary>
    /// Start the connection process.
    /// - If already Connected, we attempt to join a random room
    /// - If not yet connected, Connect this application instance to Photon Cloud Network.
    /// </summary>
    public void Connect()
    {
      progressLabel.SetActive(true);
      controlPanel.SetActive(false);
      if (PhotonNetwork.IsConnected)
      {

        PhotonNetwork.JoinRandomRoom();
      }
      else
      {
        isConnecting = PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = _gameVersion;
      }
    }

    public void QuitTheGame()
    {
      Application.Quit();
    }
    #endregion
    #region PUN MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
      if (isConnecting)
      {
        PhotonNetwork.JoinRandomRoom();
        isConnecting = false;
      }

      Debug.Log("Pun Network/Launcher: OnConnectedToMaster() was called.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
      isConnecting = false;
      progressLabel.SetActive(false);
      controlPanel.SetActive(true);
      Debug.LogWarningFormat("Pun Newtwork/Launcher: OnDisconnected() was called.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
      Debug.Log($"PUN Network/Launcher: {returnCode} \n {message} \n Failed joining room. Calling PhotonNetwork.CreateRoom() ");
      PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }


    public override void OnJoinedRoom()
    {
      // IMPORTANT Only load with Unity if  1 player otherwise sync other players
      if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
      {

        Debug.Log("Loading Single Player");
        //Important
        PhotonNetwork.LoadLevel("Room for 1");
      }

      Debug.Log("PUN Network/Launcher: OnJoinRoom Called by PUN  Connected to Room!");
    }
    #endregion
  }
}
