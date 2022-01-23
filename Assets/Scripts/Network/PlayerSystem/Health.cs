using UnityEngine;
using Photon.Pun;
using com.SluggaGames.MiniShooter.Manager;
namespace com.SluggaGames.MiniShooter.PlayerSystem
{
  /// <summary>
  ///  Manages the health of the player
  /// </summary>
  public class Health : MonoBehaviourPunCallbacks, IPunObservable
  {
    #region Public Fields

    public float PlayerHealth = 1f;
    #endregion

    #region  MB Callbacks
    private void Update()
    {
      if (photonView.IsMine)
      {
        if (PlayerHealth <= 0)
        {
          NetworkManager.Instance.LeaveRoom();
        }
      }

    }


    /// <summary>
    /// affect health of player if the collider is a beam
    /// </summary>
    /// <param name="other">the beam hitting us</param>
    void OnTriggerEnter(Collider other)
    {
      if (!photonView.IsMine)
      {
        return;
      }

      // only want Beams so looking for Beam tag if not a beam then return
      if (!other.gameObject.tag.Contains("Beam"))
      {
        return;
      }
      // it is a beam hitting so do initial damage
      PlayerHealth -= 0.1f;
    }

    void OnTriggerStay(Collider other)
    {
      // if not the local player do nothing
      if (!photonView.IsMine)
      {
        return;
      }
      if (other.gameObject.tag.Contains("Beam"))
      {
        return;
      }
      // beam is still on target do damage to target over time.
      PlayerHealth -= 0.1f * Time.deltaTime;

    }
    #endregion
    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      if (stream.IsWriting)
      {
        stream.SendNext(PlayerHealth);
      }else{
        // Network player, recieve data
        this.PlayerHealth = (float)stream.ReceiveNext();
      }
    }
    #endregion
  }
}
