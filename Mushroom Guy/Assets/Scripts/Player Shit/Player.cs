using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum PlayerState { Idle, Moving, Jumping }

[RequireComponent(typeof(PlayerController))]
public class Player: MonoBehaviour
{
    public PlayerController controller { get; private set; }

    public bool canPlaySound { get; set; }

    public PlayerState State
    {
        get { return state; }
        set
        {
            state = value;
            StateChange();
        }
    }
    private PlayerState state;

    public void StateChange()
    {
        switch (State)
        {
            case PlayerState.Idle:
                controller.enabled = true;
                break;
            case PlayerState.Moving:
                controller.enabled = true;
                break;
            case PlayerState.Jumping:
                controller.enabled = true;
                break;
        }
    }
}
