using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace com.SluggaGames.MiniShooter.PlayerSystem
{
  public class PlayerUI : MonoBehaviour
  {
    #region Private Fields

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    TMP_Text playerNameText;

    [Tooltip("UI Slider to display player Health")]
    [SerializeField]
    Slider playerHealthSlider;

    Health target;
    PlayerManager playerTarget;
    #endregion

    #region Public Fields
    [Tooltip("Pixel offset from the player target")]
    [SerializeField]
    Vector3 screenOffset = new Vector3(0f, 30f, 0f);
    float characterControllerHeight = 0f;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup _canvasGroup;
    Vector3 targetPosition;
    #endregion

    #region MB Callbacks
    private void Awake()
    {
      this.transform.SetParent(GameObject.Find("UI").transform, false);
      _canvasGroup = this.GetComponent<CanvasGroup>();

    }
    private void Update()
    {
      if (target == null)
      {
        Destroy(this.gameObject);
        return;
      }
      if (playerHealthSlider != null)
      {
        playerHealthSlider.value = target.PlayerHealth;
      }
    }

    private void LateUpdate()
    {
      // Do not show UI if we are not Visible to the camera
      if (targetRenderer != null)
      {
        this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;

        // Important
        // Follow target Gameobject on screen
        if (targetTransform != null)
        {
          targetPosition = target.GetComponent<PlayerManager>().transform.position;
          targetPosition.y += characterControllerHeight;
          this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
      }
    }
    #endregion

    #region Public Methods
    public void SetTarget(Health _target)
    {
      if (_target == null)
      {
        Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
        return;
      }
      // cache health reference for efficiency
      target = _target;
      playerTarget = _target.GetComponent<PlayerManager>();
      targetTransform = this.playerTarget.GetComponentInChildren<Transform>();
      targetRenderer = this.playerTarget.GetComponentInChildren<Renderer>();
      CharacterController characterController = _target.GetComponent<CharacterController>();

      //Get data from the Player that wont change during the lifetime of this Component
      if (characterController != null)
      {
        characterControllerHeight = characterController.height;
      }

      if (playerNameText != null)
      {
        playerNameText.text = this.playerTarget.photonView.Owner.NickName;
      }
    }

    public void SetTextNameColor(Color color)
    {
      playerNameText.color = color;
    }
    #endregion
  }
}
