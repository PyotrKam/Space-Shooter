using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    [CreateAssetMenu]

    public class LevelProperties : ScriptableObject
    {
        [SerializeField] private string m_Title;
        [SerializeField] private string m_SceneName;
        [SerializeField] private Sprite m_PreviewImage;
        [SerializeField] private LevelProperties m_NextLevel;

        public string Title => m_Title;
        public string SceneName => m_SceneName;
        public Sprite PreviewImage => m_PreviewImage;
        public LevelProperties NextLevel => m_NextLevel;




    }


}

 
