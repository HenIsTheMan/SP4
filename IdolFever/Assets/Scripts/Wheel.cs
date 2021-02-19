﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IdolFever.Character;

namespace IdolFever.UI
{
    public class Wheel : MonoBehaviour
    {
        private bool _isStarted;
        private float[] _sectorsAngles;
        private float _finalAngle;
        private float _startAngle = 0;
        private float _currentLerpRotationTime;
        public Button TurnButton;
        public GameObject Circle;           // Rotatable Object with rewards
        public TMP_Text reward_txt;         // Pop-up text with card gotten/Turn Cost
        public TMP_Text CurrentGemsText;        // Pop-up text with curr gems
        public int TurnCost = 100;          // How much coins user waste when turn whe wheel
        public int CurrentGemsAmount = 1000;    // Started gems amount.
        public int PreviousGemsAmount;      // For wasted coins animation
                                            //public GameObject Panel;

        public TMP_Text Rg_txt;         // Pop-up text with card gotten/Turn Cost
        public TMP_Text Rb_txt;         // Pop-up text with card gotten/Turn Cost
        public TMP_Text SRb_txt;         // Pop-up text with card gotten/Turn Cost
        public TMP_Text SRg_txt;         // Pop-up text with card gotten/Turn Cost
        public TMP_Text SSRb_txt;         // Pop-up text with card gotten/Turn Cost
        public TMP_Text SSRg_txt;         // Pop-up text with card gotten/Turn Cost
        //public Image[] img;
        //public float[] values;

        private void Awake()
        {
            PreviousGemsAmount = CurrentGemsAmount;
            CurrentGemsText.text = CurrentGemsAmount.ToString();
        }

        //private void Start()
        //{
        //    SetValues(values);
        //}

        public void TurnWheel()
        {
            // Player has enough money to turn the wheel
            if (CurrentGemsAmount >= TurnCost)
            {
                _currentLerpRotationTime = 0f;

                // Fill the necessary angles 6 angle
                //_sectorsAngles = new float[] { 60, 120, 180, 240, 300, 360 };

                //RG (45%) ,RB (45%) , SRG (4%) ,SRB (4%) , SRG (1%) , SRB (1%) 
                _sectorsAngles = new float[] { 90, 120, 150, 210, 300, 360 };

                int fullCircles = 5;
                float randomFinalAngle = _sectorsAngles[UnityEngine.Random.Range(0, _sectorsAngles.Length)];

                // Here we set up how many circles our wheel should rotate before stop
                _finalAngle = -(fullCircles * 360 + randomFinalAngle);
                _isStarted = true;

                //Panel.gameObject.SetActive(false);

                PreviousGemsAmount = CurrentGemsAmount;

                // Decrease money for the turn
                CurrentGemsAmount -= TurnCost;

                // Show wasted coins
                reward_txt.text = "Turn Cost: " + TurnCost;
                reward_txt.gameObject.SetActive(true);

                // Animate coins
                StartCoroutine(HideGemsDelta());
                StartCoroutine(UpdateGemsAmount());
            }
        }

        private void GiveAwardByAngle()
        {
            // Here you can set up rewards for every sector of wheel
            switch ((int)_startAngle)
            {
                case 0:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SSR_CHARACTER_GIRL0;
                        Debug.Log("You got " + c.ToString());
                        StaticDataStorage.SSR_Girl = true;
                        RewardGems(1000);
                        Rg_txt.text = "SSR_Girl";
                    }
                    break;
                case -300:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SR_CHARACTER_GIRL0;
                        Debug.Log("You got " + c.ToString());
                        StaticDataStorage.SR_Girl = true;
                        RewardGems(300);
                        SRg_txt.text = "SR_Girl";
                    }
                    break;
                case -210:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.R_CHARACTER_GIRL0;
                        Debug.Log("You got " + c.ToString());
                        StaticDataStorage.R_Girl = true;
                        RewardGems(100);
                        Rg_txt.text = "R_Girl";
                    }
                    break;
                case -150:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SR_CHARACTER_BOY0;
                        Debug.Log("You got " + c.ToString());
                        StaticDataStorage.SR_Boy = true;
                        RewardGems(900);
                        SRb_txt.text = "SR_Boy";
                    }
                    break;
                case -120:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SSR_CHARACTER_BOY0;
                        Debug.Log("You got " + c.ToString());
                        StaticDataStorage.SSR_Boy = true;
                        RewardGems(200);
                        Rb_txt.text = "SSR_Boy";
                    }
                    break;
                case -90:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.R_CHARACTER_BOY0;
                        Debug.Log("You got " + c.ToString());
                        StaticDataStorage.R_Boy = true;
                        RewardGems(300);
                        Rb_txt.text = "R_Boy";                  
                    }
                    break;
                default:
                    {
                        CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.R_CHARACTER_GIRL0;
                        Debug.Log("You got default. : " + c.ToString());
                        StaticDataStorage.R_Girl = true;
                        RewardGems(100);
                    }
                    break;
            }
            //        // Here you can set up rewards for every sector of wheel
            //        switch ((int)_startAngle)
            //{
            //    case 0://ssr
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SSR_CHARACTER_GIRL0;
            //            Debug.Log("You got " + c.ToString());
            //            StaticDataStorage.SSR_Girl = true;
            //            RewardGems(1000);
            //            Rg_txt.text = "SSR_Girl";
            //        }
            //        break;
            //    case -342:
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SSR_CHARACTER_BOY0;
            //            Debug.Log("You got " + c.ToString());
            //            StaticDataStorage.SSR_Boy = true;
            //            RewardGems(200);
            //            Rb_txt.text = "SSR_Boy";
            //        }
            //        break;
            //    case -324:
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SR_CHARACTER_GIRL0;
            //            Debug.Log("You got " + c.ToString());
            //            StaticDataStorage.SR_Girl = true;
            //            RewardGems(300);
            //            SRg_txt.text = "SR_Girl";
            //        }
            //        break;
            //    case -288:
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.SR_CHARACTER_BOY0;
            //            Debug.Log("You got " + c.ToString());
            //            StaticDataStorage.SR_Boy = true;
            //            RewardGems(900);
            //            SRb_txt.text = "SR_Boy";
            //        }
            //        break;
            //    case -252:
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.R_CHARACTER_GIRL0;
            //            Debug.Log("You got " + c.ToString());
            //            StaticDataStorage.R_Girl = true;
            //            RewardGems(100);
            //            Rg_txt.text = "R_Girl";
            //        }
            //        break;
            //    case -126:
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.R_CHARACTER_BOY0;
            //            Debug.Log("You got " + c.ToString());
            //            StaticDataStorage.R_Boy = true;
            //            RewardGems(300);
            //            Rb_txt.text = "R_Boy";
            //        }
            //        break;
            //    default:
            //        {
            //            CharacterFactory.eCHARACTER c = CharacterFactory.eCHARACTER.R_CHARACTER_GIRL0;
            //            Debug.Log("You got default. : " + c.ToString());
            //            StaticDataStorage.R_Girl = true;
            //            RewardGems(100);
            //        }
            //        break;
            //}
        }

        void Update()
        {
            // Make turn button non interactable if user has not enough money for the turn
            if (_isStarted || CurrentGemsAmount < TurnCost)
            {
                TurnButton.interactable = false;
                TurnButton.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            }
            else
            {
                TurnButton.interactable = true;
                TurnButton.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            }

            if (!_isStarted)
                return;

            float maxLerpRotationTime = 4f;

            // increment timer once per frame
            _currentLerpRotationTime += Time.deltaTime;
            if (_currentLerpRotationTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle)
            {
                _currentLerpRotationTime = maxLerpRotationTime;
                _isStarted = false;
                _startAngle = _finalAngle % 360;

                GiveAwardByAngle();
                StartCoroutine(HideGemsDelta());
                //tartCoroutine(ShowPanel());
            }

            // Calculate current position using linear interpolation
            float t = _currentLerpRotationTime / maxLerpRotationTime;

            // This formulae allows to speed up at start and speed down at the end of rotation.
            // Try to change this values to customize the speed
            t = t * t * t * (t * (6f * t - 15f) + 10f);

            float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
            Circle.transform.eulerAngles = new Vector3(0, 0, angle);
        }

        private void RewardGems(int awardGems)
        {
            CurrentGemsAmount += awardGems;
            reward_txt.text = "Gotten: +" + awardGems;
            reward_txt.gameObject.SetActive(true);
            StartCoroutine(UpdateGemsAmount());
        }

        private IEnumerator HideGemsDelta()
        {
            yield return new WaitForSeconds(1f);
            reward_txt.gameObject.SetActive(false);
        }

        private IEnumerator UpdateGemsAmount()
        {
            // Animation for increasing and decreasing of coins amount
            const float seconds = 0.5f;
            float elapsedTime = 0;

            while (elapsedTime < seconds)
            {
                CurrentGemsText.text = Mathf.Floor(Mathf.Lerp(PreviousGemsAmount, CurrentGemsAmount, (elapsedTime / seconds))).ToString();
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            PreviousGemsAmount = CurrentGemsAmount;
            CurrentGemsText.text = CurrentGemsAmount.ToString();
        }

        //public void SetValues(float[] valueToSet)
        //{
        //    float totalvalues = 0;
        //    for (int i = 0; i < img.Length; ++i)
        //    {
        //        totalvalues += FindPercentage(valueToSet, 1);
        //        img[i].fillAmount = totalvalues;
        //    }
        //}

        //private float FindPercentage(float[] valueToSet,int index)
        //{
        //    float totalAmt = 0;
        //    for (int i = 0; i < valueToSet.Length; ++i)
        //    {
        //        totalAmt += valueToSet[i];              
        //    }
        //    return valueToSet[index] / totalAmt;
        //}
        //public bool WheelStop
        //{
        //    get { return wheelStop; }
        //    set { wheelStop = value; }
        //}

        //public void CheckStop()
        //{
        //    if (wheelStop == true) 
        //    {
        //        obj.gameObject.SetActive(true);
        //    }
        //}


        //private IEnumerator ShowPanel()
        //{
        //    yield return new WaitForSeconds(1f);
        //    Panel.gameObject.SetActive(true);
        //}

        //public IEnumerator HidePanel()
        //{
        //    yield return new WaitForSeconds(1f);
        //    Panel.gameObject.SetActive(false);
        //}
    }
}
