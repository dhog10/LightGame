﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInventoryItem : BaseInteractable {

    public InventoryItemType itemType;

    protected override void Start()
    {
        base.Start();

        if (!textMeshObject)
        {
            textMeshObject = new GameObject("TextMesh");
            textMeshObject.transform.SetParent(transform);
            textMesh = textMeshObject.AddComponent<TextMesh>();
            textMesh.characterSize = 0.05f;
            textMesh.fontSize = 45;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
        }
    }

    protected override void OnInteractableStart(GameObject invokerObject)
    {
        base.OnInteractableStart(invokerObject);

        Character player = GameManager.instance.playerCharacter;

        itemType.instance = gameObject;
        if (player.InventoryPickup(itemType))
        {
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.Notify("Inventory full!", 2);
        }
    }

    protected override string GetHoverText(GameObject invokerObject)
    {
        if(itemType == null) { return "Pickup NULL"; }
        return "Pickup " + itemType.name;
    }
}