using JumpRabbit.Actors;
using JumpRabbit.Core;
using JumpRabbit.GamePlay.InGame;
using LSH.Core;
using UnityEngine;



namespace JumpRabbit.GamePlay
{
    public class InGameSceneManager : MonoBehaviour
    {

        [SerializeField] PlayerActor _player;
        [SerializeField] PlatformManager _platformManager;
        [SerializeField] InGameCameraController _cameraController;

        void Start()
        {
            //testcode
            InputManager.Instance.Init();
            //testcode
            InputManager.Instance.SetActionMap(InputMapName.InGame);
            _platformManager.Init(_cameraController);
            _player.Init(InputManager.Instance);
            _cameraController.Init(_player.transform, InputManager.Instance);
        }

        
    }
}

