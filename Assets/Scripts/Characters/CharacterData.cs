using UnityEngine;
using BoardAgain.Abilities;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int attack;
    public int defense;

    [Header("Ability Library Reference")]
    public AbilityLibrary abilityLibrary;

    [Header("Ability Slots")]
    public Ability[] abilities;

    [Header("Ability Slot Filters")]
    public AbilityTag[] abilityTags;

    [Header("Bonus Slot")]
    public Ability bonusAbility;
}
