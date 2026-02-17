using UnityEngine;
using BoardAgain.Abilities;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int attack;
    public int defense;
    public Ability[] abilites;
}
