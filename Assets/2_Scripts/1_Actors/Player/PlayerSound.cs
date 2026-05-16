using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    


    public void PlayJump()
    {
        SoundManager.Instance.PlaySFX(SFXID.Player_Jump);
    }
}
