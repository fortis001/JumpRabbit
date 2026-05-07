using JumpRabbit.Actors;
using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;

public class InGameSceneManager : MonoBehaviour
{

    [SerializeField] PlayerActor _player;

    void Start()
    {
        //testcode
        InputManager.Instance.Init();
        InputManager.Instance.SetActionMap(InputMapName.InGame);
        _player.Init(InputManager.Instance);
        //testcode
    }

}
