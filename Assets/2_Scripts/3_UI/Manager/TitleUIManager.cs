using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleUIManager : MonoBehaviour
{

    private InputAction _escAction;

    public void Init(InputManager inputManager)
    {
        _escAction = inputManager.GetAction(InputActionName.Esc);

        _escAction.performed += HandleEscPerformed;
    }

    private void HandleEscPerformed(InputAction.CallbackContext context)
    {
        GameManager.Instance.Exit();
    }

    public void HandleStartBtnClicked()
    {
        TransitionManager.Instance.LoadNextScene(SceneName.InGame);
    }

    public void HandleExitBtnClicked()
    {
        GameManager.Instance.Exit();
    }

    private void OnDestroy()
    {
        _escAction.performed -= HandleEscPerformed;
    }
}
