using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace SpaceShooter
{
    public class LivesIndicator: MonoBehaviour
    {
        [SerializeField] private Text m_Text;
        [SerializeField] private Image m_Icon;

        private int lastLives;

        private void Start()
        {
            m_Icon.sprite = Player.Instance.ActiveShip.PreviewImage;
        }

        private void Update()
        {
            int lives = Player.Instance.NumLives;

            if (lastLives != lives)
            {
                m_Text.text = lives.ToString();
                lastLives = lives;
            }
        }
    }

}

   
