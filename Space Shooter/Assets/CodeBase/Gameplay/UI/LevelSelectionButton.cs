using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private LevelProperties m_LevelProperties;
        [SerializeField] private Text m_LevelTile;
        [SerializeField] private Image m_PreviewImage;

        private void Start()
        {
            if (m_LevelProperties) return;

            m_PreviewImage.sprite = m_LevelProperties.PreviewImage;
            m_LevelTile.text = m_LevelProperties.Title;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }

}

   
