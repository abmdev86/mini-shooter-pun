
using UnityEngine;
using Photon.Pun;





namespace com.SluggaGames.MiniShooter.PlayerSystem
{
  /// <summary>
  /// Handles firing input and beams
  /// </summary>
  public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
  {


    #region Private Fields
    [Tooltip("The Beams Gameobject to control")]
    [SerializeField]
    GameObject beams;
    [Tooltip("The local player health instance. Use this to know if the local player is represented in the Scene")]
    [SerializeField]
    Health playerHealth;
    //true when firing
    bool isFiring;
    #endregion

    #region Public Fields
    public static GameObject LocalPlayerInstance;
    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUIPrefab;

    #endregion

    #region  MB Callbacks
    void Awake()
    {
      if (photonView.IsMine)
      {
        PlayerManager.LocalPlayerInstance = this.gameObject;
      }
      if (beams == null)
      {
        Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference", this);
      }
      else
      {
        beams.SetActive(false);
      }
      DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
      playerHealth = GetComponent<Health>();
      if (PlayerUIPrefab != null)
      {
        GameObject _uiGo = Instantiate(PlayerUIPrefab);
        PlayerUI _playerUI = _uiGo.GetComponent<PlayerUI>();
        if (photonView.IsMine)
        {
          _playerUI.SetTextNameColor(Color.green);
        }
        else
        {
          _playerUI.SetTextNameColor(Color.red);
        }

        _uiGo.SendMessage("SetTarget", playerHealth, SendMessageOptions.RequireReceiver);

      }
      else
      {
        Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
      }
      SetCamera();


#if UNITY_5_4_OR_NEWER
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
    }

    private void SetCamera()
    {
      CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

      if (_cameraWork != null)
      {
        if (photonView.IsMine)
        {
          _cameraWork.OnStartFollowing();
        }
      }
      else
      {
        Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
      }
    }

    void Update()
    {
      if (photonView.IsMine)
      {
        ProcessInputs();

      }

      if (beams != null && isFiring != beams.activeInHierarchy)
      {
        beams.SetActive(isFiring);
      }
    }

#if !UNITY_5_4_OR_NEWER
    void OnLevelWasLoaded(int level)
    {
      this.CalledOnLevelWasLoaded(level);

    }
#endif

    void CalledOnLevelWasLoaded(int level)
    {
      GameObject _uiGo = Instantiate(this.PlayerUIPrefab);
      _uiGo.SendMessage("SetTarget", this.playerHealth, SendMessageOptions.RequireReceiver);
      if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
      {
        transform.position = new Vector3(0, 5f, 0);
      }

    }

#if UNITY_5_4_OR_NEWER
    public override void OnDisable()
    {
      base.OnDisable();
      UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
#endif
    #endregion
    #region Private Methods
#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
      this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
#endif
    #endregion

    #region Custom
    void ProcessInputs()
    {
      if (Input.GetButtonDown("Fire1"))
      {
        if (!isFiring)
        {
          isFiring = true;
        }
      }
      if (Input.GetButtonUp("Fire1"))
      {
        if (isFiring)
        {
          isFiring = false;
        }
      }
    }
    #endregion
    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      if (stream.IsWriting)
      {
        stream.SendNext(isFiring);
      }
      else
      {
        this.isFiring = (bool)stream.ReceiveNext();
      }
    }
    #endregion
  }
}
