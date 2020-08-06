﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

// Behaviour for the game object in the Map Scene corresponding to a node

public class MapNode : MonoBehaviour
{


    public OverworldMap.LocationNode NodeData { get; set; }
    EncounterNode encounterData;
    TravelPanel panel;
    Camera cam;
    Vector3 offset = new Vector3(0, 60, 0);




    public void Init(OverworldMap.LocationNode Node){
        NodeData = Node;
        cam = Camera.main;
    
        // Appearance is determined by node type
        if (NodeData.Type == OverworldMap.LocationType.TOWN)
        {
            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);

            Town t = DataTracker.Current.TownManager.GetTownById(NodeData.LocationId);

            if (t.HasTag("Farm"))
            {
                transform.Find("farm").gameObject.SetActive(true);
            }
            else if (t.Size == Town.Sizes.Small)
            {
                transform.Find("smallTown").gameObject.SetActive(true);

            }
            else if (t.Size == Town.Sizes.Medium)
            {
                transform.Find("town").gameObject.SetActive(true);

            }
            else if (t.Size == Town.Sizes.Large)
            {
                transform.Find("largeTown").gameObject.SetActive(true);
            }
            //transform.Find("Indicator").gameObject.SetActive(true);


        }
        else if (NodeData.Type == OverworldMap.LocationType.POI)
        {
            transform.Find("EncounterMark").gameObject.SetActive(true);
        }
        else
        {
            encounterData = new EncounterNode();
            encounterData.Init();
            encounterData.SampleTexture(NodeData.PosX, NodeData.PosY);
            transform.Find("Icon").gameObject.SetActive(true);
            SpriteRenderer sprite =  transform.Find("Icon").GetComponent<SpriteRenderer>();
            
            // gradient stuff
            Gradient g = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;
            colorKey = new GradientColorKey[2];
            colorKey[0].color = GameObject.Find("Map").GetComponent<OverworldMapUI>().color1 / 255.0f;
            colorKey[0].time = 0.0f;
            colorKey[1].color = GameObject.Find("Map").GetComponent<OverworldMapUI>().color2 / 255.0f;
            colorKey[1].time = 1.0f;
        
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            g.SetKeys(colorKey, alphaKey);

            switch (encounterData.p)
            {
                case "Low":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/hexagon");
                    sprite.color = g.Evaluate(0.25f);
                break;
                case "Medium":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/diamond");
                    sprite.color = g.Evaluate(0.5f);
                break;
                case "High":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/triangle");
                    sprite.color = g.Evaluate(0.75f);
                break;
                case "Very High":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/cross");
                    sprite.color = g.Evaluate(1.0f);
                break;
                default:
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/circle");
                    sprite.color = g.Evaluate(0.0f);
                break;
            }
        }
    }

    public bool RollEncounter(){
        return encounterData.RollEncounter();
    }

    /// <summary>
    /// Update the info panel's position so it is always on top of the node
    /// </summary>
    private void Update()
    {
        if (panel)
        {
            if (panel.gameObject.activeInHierarchy)
            {
                panel.transform.position = cam.WorldToScreenPoint(gameObject.transform.position) + offset;
            }
        }
    }

    /// <summary>
    /// Assosiate an info panel with this node
    /// </summary>
    /// <param name="obj">The info panel game object</param>
    public void setPanel(GameObject obj)
    {
        if (panel) return;
        bool adjacent = DataTracker.Current.WorldMap.HasEdge(NodeData.Id, DataTracker.Current.currentLocationId);
        if (NodeData.Type == OverworldMap.LocationType.NONE && ! adjacent) return;

        panel = obj.GetComponent<TravelPanel>();
        panel.SetNode(this);
        obj.SetActive(true);

        if (DataTracker.Current.WorldMap.HasEdge(NodeData.Id, DataTracker.Current.currentLocationId)){
            panel.SetTravelInfo(MapTravel.GetFuelCost(this), MapTravel.timeRate);
        }

        OverworldMap.LocationNode node;
        DataTracker.Current.WorldMap.GetNode(NodeData.Id, out node);

        if (NodeData.Type == OverworldMap.LocationType.TOWN) {
            Town t = DataTracker.Current.TownManager.GetTownById(node.LocationId);
            panel.SetName(t.Name);
            string details = "";
            if (t.Tags.Count > 0)
            {
                foreach (var tag in t.Tags)
                {
                    if (tag.Name == "Small" || tag.Name == "Medium" || tag.Name == "Large")
                    {
                        continue;
                    }
                    details += tag.Name + " ";
                }
            }

            switch (t.Size)
            {
                case Town.Sizes.Small:
                    details += "Hamlet";
                    break;
                case Town.Sizes.Medium:
                    details += "Town";
                    break;
                case Town.Sizes.Large:
                    details += "City";
                    break;
                default:
                    details += "Town";
                    break;
            }
            panel.SetDetails(details);
        }
        else if (NodeData.Type == OverworldMap.LocationType.EVENT) {
            panel.SetDetails("Unknown Event");

        }
        else if (NodeData.Type == OverworldMap.LocationType.POI) {
            panel.SetDetails("Point of Interest");
        }

        obj.transform.position = cam.WorldToScreenPoint(gameObject.transform.position) + offset;
        panel.onNodeHover();
    }

    /// <summary>
    /// Invoke an event on mouse over
    /// This tells the map UI to assign this node an info panel
    /// </summary>
    public void OnMouseEnter()
    {
        int xMod = Mathf.RoundToInt(Mathf.Lerp(0, 800, (NodeData.PosX + 1) / 2.0f));
        int yMod = Mathf.RoundToInt(Mathf.Lerp(0, 600, (NodeData.PosY + 1) / 2.0f));
        Debug.Log($"({xMod}, {yMod})");
        if (panel) return;
        EventManager.Instance.OnNodeMouseEnter.Invoke(this);
    }

    /// <summary>
    /// Close the panel when the mouse leaves, unless the node has been selected
    /// </summary>
    private void OnMouseExit()
    {
        if (panel)
        {
            panel.onNodeLeave();
        }
    }

    /// <summary>
    /// On click, select the node.
    /// </summary>
    private void OnMouseDown()
    {
        // Only allow clicks on adjacent nodes
        if (DataTracker.Current.WorldMap.HasEdge(NodeData.Id, DataTracker.Current.currentLocationId))
        {
            if (panel)
            {
                EventManager.Instance.OnNodeMouseDown.Invoke(this);
                panel.onNodeClick();
            }
        }

    }

    /// <summary>
    /// Remove the info panel
    /// </summary>
    public void Close()
    {
        panel = null;
    }



}
