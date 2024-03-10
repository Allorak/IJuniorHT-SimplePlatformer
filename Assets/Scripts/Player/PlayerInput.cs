using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            _player.Move(Directions.Right);
        else if (Input.GetKey(KeyCode.A))
            _player.Move(Directions.Left);
        else
            _player.Stop();

        if (Input.GetKey(KeyCode.Space))
            _player.Jump();

        if (Input.GetKey(KeyCode.Mouse0))
            _player.Attack();
    }
}
