using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


namespace com.SluggaGames.MiniShooter.Network
{
  [RequireComponent(typeof(TMP_InputField))]
  public class PlayerNameInputField : MonoBehaviour
  {
    #region Private Constants

    const string playerNamPrefKey = "PlayerName";
   
    #endregion
    # region Private Fields
    TMP_InputField _inputField;
    string defaultName;
    #endregion

    #region MB Callbacks
    private void Start()
    {

      _inputField = GetComponent<TMP_InputField>();
      defaultName = GetPlayerPrefsPlayerName();
      _inputField.text = defaultName;
      if (string.IsNullOrEmpty(PlayerPrefs.GetString(playerNamPrefKey)))
      {
        _inputField.gameObject.SetActive(true);
      }
      else
      {
        _inputField.gameObject.SetActive(true);

      }
      PhotonNetwork.NickName = defaultName;
    }
    /// <summary>
    ///  set the InputField and
    /// </summary>
    /// <returns>The PlayerPrefs PlayerName Key's Value</returns>
    private string GetPlayerPrefsPlayerName()
    {
      string playerName = string.Empty;

      if (_inputField != null)
      {
        if (PlayerPrefs.HasKey(playerNamPrefKey))
        {
          playerName = PlayerPrefs.GetString(playerNamPrefKey);

        }
      }

      return playerName;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets the PlayerPrefs String PlayerName = value. Called when Input field is done editing.
    /// </summary>
    /// <param name="value">Name of the Player</param>
    public void SetPlayerName(string value)
    {
      // TODO name filtering for bad names
      if (string.IsNullOrEmpty(value))
      {
        Debug.LogError("Player Name is null or empty");

        return;
      }
      PhotonNetwork.NickName = value;

      PlayerPrefs.SetString(playerNamPrefKey, value);
    }
    #endregion
  }
}
