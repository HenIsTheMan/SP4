﻿using IdolFever;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SkillProgressBarUI: MonoBehaviour {
    #region Fields

    [SerializeField] private float maxValue;
    [SerializeField] private float minValue;
    [SerializeField] private Image image;

    #endregion

    #region Properties

    public float MaxValue {
        get {
            return maxValue;
        }
        set {
            maxValue = value;
        }
    }

    public float MinValue {
        get {
            return minValue;
        }
        set {
            minValue = value;
        }
    }

    #endregion

    private void Start() {
        image = GetComponent<Image>();
    }

    void Update() {
        if(PauseScreen.isPaused && !PhotonNetwork.IsConnected) {
            return;
        }

        minValue -= Time.deltaTime;

        transform.localScale = new Vector2(minValue / maxValue, transform.localScale.y);

        float factor = minValue / maxValue * 0.5f + 0.8f;
        if(factor > 1.0f) {
            factor -= 1.0f;
        }

        image.color = Color.HSVToRGB(factor, 1.0f, 1.0f);
    }
}