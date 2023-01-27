using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewConsumableItem", menuName = "New Item/New Consumable Item")]
    public class ConsumableItem : ItemScriptable
    {
        [SerializeField] List<ItemEffect> effects;
        public override void Using(Game.Player.Player _player, Player.Inventory.AimController _aim, Vector3 _handPos, Quaternion _handRot)
        {
            if (currentUseTimer < timeToUse)
            {
                return;
            }
            foreach (ItemEffect i in effects)
            {
                _player.Effect(i);
                _aim.UseItem();
            }
            currentUseTimer = 0;
        }
    }

    [Serializable]
    public struct ItemEffect
    {
        public StatusController.Effect effect;
        public System.Operation operation;
        public int value;
    }
}

namespace Game.StatusController
{
    [Serializable]
    public enum Effect
    {
        Hapiness,
        Water,
        Food,
        Stoned,
        Drunk
    }
}

namespace Game.System
{
    [Serializable]
    public enum Operation
    {
        Plus,
        Minus,
        SetValue,
        SetTrue,
        SetFalse
    }
}