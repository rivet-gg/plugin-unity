using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Logging;
using UnityEngine;
using FishNet.Object;

namespace Examples.LobbiesServers
{
    public class Player : NetworkBehaviour
    {
        private CharacterController _cc;
        private GameManager _gm;

        void Start()
        {
            _cc = GetComponent<CharacterController>();
            _gm = FindObjectOfType<GameManager>();
        }

        void Update()
        {
            Move();
        }

        [Client(Logging = LoggingType.Off, RequireOwnership = true)]
        void Move()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            var offset = new Vector3(horizontalInput, Physics.gravity.y, verticalInput) * _gm.MoveSpeed * Time.deltaTime;
            _cc.Move(offset);
        }
    }
}