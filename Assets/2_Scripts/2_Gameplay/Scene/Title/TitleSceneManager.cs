using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] TitleUIManager _uiManager;


    private void Start()
    {
        SoundManager.Instance.PlayBGM(BGMID.Title);
        InputManager.Instance.SetActionMap(InputMapName.Title);

        _uiManager.Init(InputManager.Instance);
    }
}
