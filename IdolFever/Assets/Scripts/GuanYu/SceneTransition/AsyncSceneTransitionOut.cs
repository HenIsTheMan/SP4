﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IdolFever {
    internal sealed class AsyncSceneTransitionOut: MonoBehaviour {
        #region Fields

        [SerializeField] private AsyncSceneTransitionOutTypes.AsyncSceneTransitionOutType type;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioListener audioListener;
        [SerializeField] private GameObject myInterest;
        [SerializeField] private Image img;
        [SerializeField] private string sceneName;
        [SerializeField] private string startAnimName;

        #endregion

        #region Properties

        public string SceneName {
            get {
                return sceneName;
            }
            private set { //Not used
                sceneName = value;
            }
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void ChangeScene() {
            _ = StartCoroutine(MyStartCoroutine(sceneName));
        }

        private System.Collections.IEnumerator MyStartCoroutine(string sceneName) {
            img.fillAmount = 0.0f;

            animator.SetTrigger("Start");

            float animLen = -1.0f;
            foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
                if(clip.name == startAnimName) {
                    animLen = clip.length;
                }
            }

            if(animLen >= 0.0f) {
                yield return new WaitForSeconds(animLen);
            } else {
                UnityEngine.Assertions.Assert.IsTrue(false);
            }

            SceneTracker.prevSceneName = SceneManager.GetActiveScene().name;

            switch(type) {
                case AsyncSceneTransitionOutTypes.AsyncSceneTransitionOutType.AddSingle: {
                    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                    while(!operation.isDone) {
                        img.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
                        yield return null;
                    }

                    break;
                }
                case AsyncSceneTransitionOutTypes.AsyncSceneTransitionOutType.AddAdditive: {
                    myInterest.SetActive(false);
                    SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
                        audioListener.enabled = false;
                    };

                    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                    while(!operation.isDone) {
                        img.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
                        yield return null;
                    }

                    break;
                }
                case AsyncSceneTransitionOutTypes.AsyncSceneTransitionOutType.RemoveAdditive:
                    break;
                default:
                   UnityEngine.Assertions.Assert.IsTrue(false);
                   break;
            }
        }

        public AsyncSceneTransitionOut() {
            type = AsyncSceneTransitionOutTypes.AsyncSceneTransitionOutType.AddSingle;
            animator = null;
            audioListener = null;
            myInterest = null;
            img = null;
            sceneName = string.Empty;
            startAnimName = string.Empty;
        }
    }
}