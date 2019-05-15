using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon, 
    Resource,
    KeyItem,
    Food,
    Equipment
}

public enum effectType
{
    nothing,
    poison,
    bleeding,
    instaKill,
    slowed
}

public enum equipmentSlotType
{
    Head,
    Legs,
    Torso,
    Arms,
    Weapon
}

[CreateAssetMenu(fileName = "Item", menuName = "Items/ItemData", order = 51)]
public class Item : ScriptableObject 
{
    [Tooltip("The name that will display in the game- make sure this is a unique name , else it wont work")]
    public string title;
    [TextArea]
    [Tooltip("A short description of the object, not used at the moment")]
    public string description;
    [Tooltip("What icon will be displayed for this object in menus")]
    public Sprite icon;
    [Tooltip("How the inventory will treat this object. Make sure it is set right, else it wont work")]
    public ItemType type;

}


[CreateAssetMenu(fileName = "Food", menuName = "Items/FoodData", order = 52)]
public class Food : Item
{
    [Tooltip("How much health this item will restore")]
    public int healthRestore;
    [Tooltip("How much stamina this item will restore")]
    public float staminaRestore;

}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/EquipmentData", order = 52)]
public class Equipment : Item
{
    [Tooltip("What this item looks like when worn")]
    public Mesh mesh;
    [Tooltip("What equipment slot this occupies")]
    public equipmentSlotType equipmentSlot;
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/WeaponData", order = 52)]
public class Weapon : Equipment
{
    [Tooltip("How much damage this weapon does")]
    public int attackDamage;
    [Tooltip("Does this item have a special effect?")]
    public SpecialEffect[] effect;
}


[CreateAssetMenu(fileName = "SpEffect", menuName = "OtherData/SpEffect", order = 52)]
public class SpecialEffect : ScriptableObject
{
    [Tooltip("What type of effect is this")]
    public effectType thisEffectType;
    [Tooltip("How severe is this effect")]
    public int value;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueLine", order = 52)]
public class DialogueLine : ScriptableObject
{
    public string speakerName;
    public string lineToSay;
    public Sprite speakerImage;

}



