using UnityEngine;

public class GameOverState : IState
{
    public void Enter()
    {
        Debug.Log("Entering Game Over State");
    }

    public void Exit()
    {
        Debug.Log("Exiting Game Over State");
    }

    public void Update()
    {
        // Game Over update logic here
    }
}
