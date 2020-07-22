﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Tooltip tooltip;
    // Start is called before the first frame update
    void Awake()
    {
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string name = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            int index = name.IndexOf("(");
            if (index > 0)
            {
                name = name.Substring(0, index - 1);
            }
            Debug.Log(name);
            Item temp;
            ItemManager.Current.itemsMaster.TryGetValue(name, out temp);
            tooltip.GenerateDetailedTooltip(temp);
        }
        else
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        int index = name.IndexOf("(");
        if(index > 0)
        {
            name = name.Substring(0, index-1);
        }
        Debug.Log(name);
        Item temp;
        ItemManager.Current.itemsMaster.TryGetValue(name, out temp);
        tooltip.GenerateTooltip(temp);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
