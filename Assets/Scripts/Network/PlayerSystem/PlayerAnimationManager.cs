
using UnityEngine;
using Photon.Pun;
namespace com.SluggaGames.MiniShooter.PlayerSystem
{
  public class PlayerAnimationManager : MonoBehaviourPunCallbacks
  {
    #region Private Fields
    Animator animator;
    [Range(0.25f, 5)]
    [SerializeField] float directionDampTime = 0.25f;


    #endregion
    #region MB Callbacks
    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
      if (!animator)
      {
        Debug.LogError("PlayerAnimatorManager is Missing Animator Component.", this);
      }
    }

    // Update is called once per frame
    void Update()
    {
      if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
      {
        return;
      }
      if (!animator)
      {
        return;
      }
      AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
      // only allow to jump if running
      if (stateInfo.IsName("Base Layer.Run"))
      {
        if (Input.GetButtonDown("Fire2"))
        {
          animator.SetTrigger("Jump");
        }
      }
      float h = Input.GetAxis("Horizontal");
      float v = Input.GetAxis("Vertical");
      if (v < 0)
      {
        v = 0;
      }
      animator.SetFloat("Speed", h * h + v * v);
      animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }
    #endregion
  }
}
