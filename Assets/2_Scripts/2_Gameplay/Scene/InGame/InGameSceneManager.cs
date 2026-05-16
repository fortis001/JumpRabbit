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
        [SerializeField] ScoreManager _scoreManager;
        [SerializeField] InGameUIManager _uiManager;
        void Start()
        {
            //testcode
            InputManager.Instance.Init();
            SoundManager.Instance.Init();
            SoundManager.Instance.PlayBGM(BGMID.InGame);
            //testcode
            InputManager.Instance.SetActionMap(InputMapName.InGame);
            _platformManager.Init(_cameraController, _player.transform);
            _player.Init(InputManager.Instance);
            _cameraController.Init(_player.transform, InputManager.Instance);
            _scoreManager.Init(_platformManager);
            _uiManager.Init(_scoreManager, _player.transform);
        }

        
    }
}

