using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.SluggaGames.MiniShooter.PlayerSystem
{
  public class CameraWork : MonoBehaviour
  {
    #region Private Fields
    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    float distance = 7.0f;
    [Tooltip("The Height we want the camera to be above the target")]
    [SerializeField]
    float height = 3.0f;
    [Tooltip("Allow the camera to be offsetted. More Scenerey vs viewing ground")]
    [SerializeField]
    Vector3 centerOffset = Vector3.zero;
    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField]
    bool followOnStart = false;
    [Tooltip("The smoothing of the camera to follow the target")]
    [SerializeField]
    float smoothSpeed = 0.125f;

    Transform cameraTransform;

    bool isFollowing;

    Vector3 cameraOffset = Vector3.zero;

    #endregion

    #region MB Callbacks
    void Start()
    {
      if (followOnStart)
      {
        OnStartFollowing();
      }
    }

    void LateUpdate()
    {
      // Check if main camera is different on scene load and reconnect
      if (cameraTransform == null && isFollowing)
      {
        OnStartFollowing();
      }

      // only follow is explicitly set
      if (isFollowing)
      {
        Follow();
      }
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Raises the Start following event.
    /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
    /// </summary>
    public void OnStartFollowing()
    {
      cameraTransform = Camera.main.transform;
      isFollowing = true;
      Cut();
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Follow the target smoothly
    /// </summary>
    void Follow()
    {
      cameraOffset.z = -distance;
      cameraOffset.y = height;

      cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position +
      this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);
      cameraTransform.LookAt(this.transform.position + cameraOffset);
    }

    void Cut()
    {
      cameraOffset.z = -distance;
      cameraOffset.y = height;

      cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

      cameraTransform.LookAt(this.transform.position + centerOffset);
    }
    #endregion
  }
}
