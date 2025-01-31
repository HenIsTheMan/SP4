﻿using Photon.Pun;
using UnityEngine;

namespace IdolFever {
    internal sealed class RetrievePlayerColorsRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void RetrievePlayerColors() {
            int colorsArrLen = PlayerUniversal.Colors.Length;
            Vector3[] vecs = new Vector3[colorsArrLen];
            for(int i = 0; i < colorsArrLen; ++i) {
                Color color = PlayerUniversal.Colors[i];
                vecs[i] = new Vector3(color.r, color.g, color.b);
            }

            PhotonView.Get(this).RPC("SetPlayerColors", RpcTarget.Others, vecs);
        }
    }
}