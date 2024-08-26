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
        [SerializeField] private float rotationSpeed = 10f;
        private Vector3 _lastMovementDirection;

        void Start()
        {
            _cc = GetComponent<CharacterController>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            _gm = FindObjectOfType<GameManager>();
            if (IsOwner) _gm.LocalPlayer = this;
        }

        void Update()
        {
            if (IsOwner)
            {
                Move();
            }
            UpdateRotation();
        }

        [Client(Logging = LoggingType.Off, RequireOwnership = true)]
        void Move()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            _lastMovementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            
            var offset = new Vector3(horizontalInput, Physics.gravity.y, verticalInput) * _gm.MoveSpeed * Time.deltaTime;
            _cc.Move(offset);
        }

        void UpdateRotation()
        {
            if (_lastMovementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_lastMovementDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}