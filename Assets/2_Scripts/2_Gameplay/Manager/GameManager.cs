using LSH.Core;
using UnityEngine;

public class GameManager : Singleton<GameManager>,IBootable
{

    public void Init()
    {

    }


    public void Exit()
    {
        Application.Quit();
    }
}
