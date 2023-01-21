using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Itens
{
    
    [CreateAssetMenu(fileName = "NewCraftableItem", menuName = "New Item/New Craftable Item")]
    public class CraftableItens : ItemScriptable
    {
        public List<ItemScriptable> craftingResources;
    }
}