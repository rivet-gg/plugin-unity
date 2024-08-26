using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Modules.Lobbies;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace Examples.LobbiesServers
{
    public class GameManager : NetworkBehaviour
    {
        public float MoveSpeed = 3.0f;

        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 5, -5);
        [SerializeField] private float cameraSmoothTime = 0.25f;
        [SerializeField] private float cameraAngle = 45f; // Angle of the camera from horizontal

        public Player LocalPlayer;
        private Vector3 cameraVelocity;

        private void Awake()
        {
            Debug.Log("awake");
        }

        private void LateUpdate()
        {
            if (LocalPlayer != null)
            {
                UpdateCameraPosition();
            }
        }

        private void UpdateCameraPosition()
        {
            var mainCamera = Camera.main;

            // Move
            Vector3 targetPosition = LocalPlayer.transform.position + cameraOffset;
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothTime);
            
            // Look at the player
            mainCamera.transform.LookAt(mainCamera.transform.position - cameraOffset);
        }
    }
}