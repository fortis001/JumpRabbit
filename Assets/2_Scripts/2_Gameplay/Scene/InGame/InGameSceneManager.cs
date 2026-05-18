using JumpRabbit.Actors;
using JumpRabbit.Core;
using JumpRabbit.GamePlay.InGame;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;



namespace JumpRabbit.GamePlay
{
    public class InGameSceneManager : MonoBehaviour
    {

        [Header("Bootable")]
        [SerializeField] PlayerActor _player;
        [SerializeField] PlatformManager _platformManager;
        [SerializeField] InGameCameraController _cameraController;
        [SerializeField] ScoreManager _scoreManager;
        [SerializeField] InGameUIManager _uiManager;

        [Header("GameSettings")]
        [SerializeField] float _gameOverY = -6f;

        private InputAction _escAction;

        private void Start()
        {
            SoundManager.Instance.PlayBGM(BGMID.InGame);

            InputManager.Instance.SetActionMap(InputMapName.InGame);
            _escAction = InputManager.Instance.GetAction(InputActionName.Esc);
            _escAction.performed += HandleEscPerformed;

            _platformManager.Init(_cameraController, _player.transform);
            _player.Init(InputManager.Instance, _gameOverY);
            _player.OnPlayerFalled += HandlePlayerFalled;

            _cameraController.Init(_player.transform, InputManager.Instance);
            _scoreManager.Init(_platformManager);
            _uiManager.Init(_scoreManager, _player.transform);
        }

        private void HandleEscPerformed(InputAction.CallbackContext context)
        {
            if (!TimeManager.Instance.IsGamePaused)
            {
                TimeManager.Instance.PauseGame();
                _uiManager.ShowPauseMenu();
            }
            else
            {
                TimeManager.Instance.ResumeGame();
                _uiManager.HidePauseMenu();
            }
        }

        private void HandlePlayerFalled()
        {
            _uiManager.ShowPauseMenu();
        }

        private void OnDestroy()
        {
            _escAction.performed -= HandleEscPerformed;
        }
    }
}

