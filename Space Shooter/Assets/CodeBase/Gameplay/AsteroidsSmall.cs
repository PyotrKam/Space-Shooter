using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class AsteroidsSmall : MonoBehaviour
    {
        [SerializeField] private GameObject m_prefab;

        private void OnDestroy()
        {            
            if (!IsSceneClosing())
            {
                Instantiate(m_prefab, transform.position, Quaternion.identity);
            }
        }

        private bool IsSceneClosing()
        {
            
            return SceneManager.GetActiveScene().isLoaded == false;
        }
    }

}


