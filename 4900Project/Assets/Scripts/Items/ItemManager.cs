﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtility;

public enum rarity {None, Abundant, Common, Uncommon, Rare, Unique}
public enum typetag {Food,Luxury,Medicine,Building_Materials, Tools_And_Parts, Combat,Fuel}

/// <summary>
/// Intermediate class for handling loading items from a CSV file.
/// This should be the class that gets generated, then each should be fed into the Item class.
/// </summary>
public struct ItemCsvData
{
    public string DisplayName;
    public string Tooltip;
    public string Description;
    public float Value;
    public float Weight;
    public string Tier;
    public string Tags;
    public string IconName;
}

public struct Item 
{
    public string DisplayName;
    public string Tooltip;
    public string Description;
    public float Value;
    public float Weight;
    public rarity tier;
    public List<typetag> tags;

    /// <summary>
    /// Creates an item given the values to set.
    /// This can be used for hardcoding items.
    /// </summary>
    /// <param name="name_"></param>
    /// <param name="tooltip_"></param>
    /// <param name="description_"></param>
    /// <param name="value_"></param>
    /// <param name="weight_"></param>
    /// <param name="_tags"></param>
    public Item(string name_, string tooltip_, string description_, float value_, float weight_, List<typetag> _tags)
    {
        DisplayName = name_;
        Tooltip = tooltip_;
        Description = description_;
        Value = value_;
        Weight = weight_;
        tags = _tags;
        //create rarity based on value
        if (Value >= 20)
        {
            tier = rarity.Unique;
        }
        else if (Value < 20 && Value >= 10)
        {
            tier = rarity.Rare;
        }
        else if (Value < 10 && Value >= 5)
        {
            tier = rarity.Uncommon;
        }
        else if (Value < 5 && Value >= 2)
        {
            tier = rarity.Common;
        }
        else
        {
            tier = rarity.Abundant;
        }
    }

    /// <summary>
    /// Creates an Item out of an ItemCsvData class.
    /// This can be used for generating items from CSV.
    /// </summary>
    /// <param name="itemData"></param>
    public Item(ItemCsvData itemData)
    {
        DisplayName = itemData.DisplayName;
        Tooltip = itemData.Tooltip;
        Description = itemData.Description;
        Value = itemData.Value;
        Weight = itemData.Weight;
        tier = UnityHelperMethods.ParseEnum<rarity>(itemData.Tier);
        tags = UnityHelperMethods.ParseCommaSeparatedList<typetag>(itemData.Tags, UnityHelperMethods.ParseEnum<typetag>);
    }

}

public class ItemManager : MonoBehaviour
{
    private static ItemManager _current;
    public static ItemManager Current { get { return _current; } }
    public  Dictionary<string, Item> itemsMaster = new Dictionary<string, Item>();

    void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }

        //create item list

        List<typetag> tempList = new List<typetag>();
        tempList.Add(typetag.Food);
        Item temp = new Item("Rations", "Unappetizing but cheap", "The bare minimum to keep someone running for a day", 1, 1, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Tools_And_Parts); //we will prob want a different tag for player essentials.
        temp = new Item("Fuel", "A neccesity for travel between colonies", "Fuel is the lifeblood of traders, never leave port without first checking your tank. ", 1, 1, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Food);
        tempList.Add(typetag.Luxury);
        temp = new Item("Fresh Fruit", "Tasty, but spoils without proper storage", "A rarity in a good portion of the world, which makes is quite valuable to the right people. ", 5, 1, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Tools_And_Parts);
        temp = new Item("Wrench", "An essential repair tool", "Helps with maintaining machines. Wear and tear means you need a replacement once in a while.", 2, 1, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Tools_And_Parts);
        temp = new Item("Repair Patch", "One use smart repair patch", "Uncommon tech allows these patches to conduct repairs autonomously, but burn out performing the job", 15, 4, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Luxury);
        temp = new Item("2000's Painting", "Collectors love this stuff", "Art from before the world ended. Nostalgic.", 19, 3, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Luxury);
        temp = new Item("Jewelry", "A valuable, if nonfunctional, accessory", "Goes nice with a finely tailored outfit. Good luck finding one", 12, 1, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Building_Materials);
        temp = new Item("Concrete", "Basic building material", "What most things are built out of these days", 3, 10, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Building_Materials);
        tempList.Add(typetag.Tools_And_Parts);
        temp = new Item("Scrap Metal", "General purpose parts", "Good for making something new or fixing something old", 5, 10, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Tools_And_Parts);
        temp = new Item("Generator", "Creates electricity", "Consumes fuel to generate power. Vital for survival.", 40, 25, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Combat);
        temp = new Item("Body Armor", "Keeps you intact", "Armor meant to protect from the typical bandit raid.", 4, 5, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Combat);
        temp = new Item("RPG", "Sends a message", "Let anyone who intends to raid you serverly regret their choices for a whole second.", 20, 2, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Medicine);
        temp = new Item("Medicine", "Patches up wounds.", "General pruopse bandages and medicine to treat injuries and illnesses", 6, 3, tempList);
        itemsMaster.Add(temp.DisplayName, temp);

        tempList = new List<typetag>();
        tempList.Add(typetag.Medicine);
        tempList.Add(typetag.Luxury);
        temp = new Item("Medical Kit", "High end First Aid Kit for dangerous work", "NOt only stocked with the best medical supplies on hand, but even contain stimulants and boosters to avoid injury in the first place", 20, 2, tempList);
        itemsMaster.Add(temp.DisplayName, temp);
    }

    Dictionary<string, Item> GetListofType(List<typetag> types)
    {
        Dictionary<string, Item> values = new Dictionary<string, Item>();
        foreach (typetag targettype in types)
        {
            foreach(KeyValuePair<string,Item> viewed in itemsMaster)
            {
                if(viewed.Value.tags.Contains(targettype) && values.ContainsKey(viewed.Key) == false)
                {
                    values.Add(viewed.Key,viewed.Value);
                }
            }
        }
        return values;
    }

}
