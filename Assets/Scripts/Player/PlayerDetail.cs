using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetail
{
    public class PlayerInput
    {
        public bool leftButton;
        public bool rightButton;
    }
    public enum PlayerEvent
    {
        NewPlayer,
        IsReadyForRace
    }
    public delegate void Observer(PlayerDetail playerDetail, PlayerEvent playerEvent);
    public Observer observer;
    public PlayerInput input
    {
        get
        {
            return _input;
        }
    }
    public GameObject body
    {
        get
        {
            return _body;
        }
    }
    public int playerId
    {
        get
        {
            return _playerId;
        }
    }
    private int _playerId;
    private PlayerInput _input;
    private GameObject _body;
    private InputCommand inputCommand;

    public PlayerDetail(int playerId, GameObject player, InputCommand inputCommand)
    {
        this._playerId = playerId;
        this._body = player.GetComponentInChildren<PlayerMovement>().gameObject;
        this.inputCommand = inputCommand;
        this._input = new PlayerInput();
    }

    public void Update()
    {
        inputCommand.Execute(this);
    }

    public void SetInputCommand(InputCommand newCommand)
    {
        inputCommand = newCommand;
    }

    public void SetInput(PlayerInput newInput)
    {
        _input = newInput;
    }
}
