using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Card : MonoBehaviour
{
    public string m_cardName;
    public string m_cardDescription;

    [SerializeField]
    protected Texture m_texture;
    [SerializeField]
    protected Sprite m_portrait;

    public abstract void OnDrawn();

    public abstract void OnPlayed();

    public abstract void OnAttack();

    public abstract void OnHit();

    public abstract void OnKilled();

    public abstract void OnSacrificed();

    public abstract void OnTurnStart();

    public abstract void OnTurnEnd();
}
