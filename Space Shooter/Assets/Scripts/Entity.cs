
using UnityEngine;

/// <summary>
/// Base class of all game interactive objects
/// </summary>
/// 
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Object name for the user
    /// </summary>
    /// 
    [SerializeField] private string m_Nickname;
    public string Nickname => m_Nickname;
}
