using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_Result;
        [SerializeField] private Text m_ButtonNextText;

        private bool m_LevelPassed = false;



        private void Start()
        {
            gameObject.SetActive(false);
            LevelController.Instance.LevelLost += OnLevelLost;
            LevelController.Instance.LevelPassed += OnLevelPassed;
        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelLost -= OnLevelLost;
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }

        private void OnLevelPassed()
        {
            gameObject.SetActive(true);

            m_LevelPassed = true;

            FillLevelStatistics();

            m_Result.text = "Passed";

            if (LevelController.Instance.HasNextLevel == true)
            {
                m_ButtonNextText.text = "Next";
            }
            else
            {
                m_ButtonNextText.text = "Main menu";
            }
        }



        private void OnLevelLost()
        {
            gameObject.SetActive(true);

            FillLevelStatistics();

            m_Result.text = "Lose";

            m_ButtonNextText.text = "Restart";
        }

        

        private void FillLevelStatistics()
        {
            m_Kills.text = "Kills : " + Player.Instance.NumKills.ToString();
            m_Score.text = "Scores : " + Player.Instance.Score.ToString();
            m_Time.text = "Time : " + LevelController.Instance.LevelTime.ToString("F0");
            
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            if (m_LevelPassed == true)
            {
                LevelController.Instance.LoadNextLevel();
            }

            else
            {
                LevelController.Instance.RestartLevel();
            }
        }




    }
}

    
