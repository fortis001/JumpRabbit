using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;


namespace JumpRabbit.GamePlay.InGame
{
    public class InGameCameraController : MonoBehaviour
    {

        [SerializeField] CameraController _cameraController;
        [SerializeField] private Vector2 _playerFocusOffset = new Vector2(3.5f,0);


        private Transform _player;
        private InputManager _inputManager;
        private InputAction _zoomAction;

        public void Init(Transform player, InputManager inputManager)
        {
            _player = player;
            _inputManager = inputManager;
            FocusToPlayer();

            _zoomAction = _inputManager.GetAction(InputActionName.Zoom);

            if (_zoomAction != null)
            {
                _zoomAction.performed -= HandleZoomPerformed;
                _zoomAction.performed += HandleZoomPerformed;
            }

        }

        public void FocusToPlayer()
        {
            if (_player == null)
                return;

            Vector3 focusPosition = _player.position + new Vector3(
                _playerFocusOffset.x,
                _playerFocusOffset.y,
                0f);

            _cameraController.FocusOn(focusPosition);
        }

        private void HandleZoomPerformed(InputAction.CallbackContext context)
        {
            Vector2 scroll = context.ReadValue<Vector2>();

            if (Mathf.Abs(scroll.y) <= 0.01f)
                return;

            _cameraController.Zoom(scroll.y);
        }

        private void OnDestroy()
        {
            if (_zoomAction != null)
            {
                _zoomAction.performed -= HandleZoomPerformed;
            }
        }

    }
}

