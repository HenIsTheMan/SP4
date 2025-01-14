﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace IdolFever {
    internal sealed class RoomListEntry: MonoBehaviour {
        #region Fields

        private string roomName;

        [SerializeField] private Text RoomNameText;
        [SerializeField] private Text RoomPlayersText;
        [SerializeField] private Button JoinRoomButton;

        #endregion

        #region Properties
        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            JoinRoomButton.onClick.AddListener(() => {
                if(PhotonNetwork.InLobby) {
                    PhotonNetwork.LeaveLobby();
                }
                PhotonNetwork.JoinRoom(roomName);
            });
        }

        #endregion
        
        public RoomListEntry() {
            RoomNameText = null;
            RoomPlayersText = null;
            JoinRoomButton = null;
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers) {
            roomName = name;
            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        }
    }
}