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
        [SerializeField] List<Condition> conditions;
        public override void Using(Player.Player _player, Player.Inventory.AimController _aim, Vector3 _handPos, Quaternion _handRot)
        {
            foreach (Condition c in conditions)
            {
                if (!c.InCondition(_player))
                {
                    return;
                }
            }
            if (currentUseTimer < timeToUse)
            {
                return;
            }
            foreach (ItemEffect i in effects)
            {
                _player.Effect(i);
            }
            _aim.UseItem();
            currentUseTimer = 0;
        }
    }

    [Serializable]
    public struct ItemEffect
    {
        public StatusController.Effect effect;
        public System.ArithmeticOperations operation;
        public int value;
    }


    [Serializable]
    public class Condition
    {
        [SerializeField] StatusController.Effect effect;
        [SerializeField] System.LogicOperations operation;
        [SerializeField] int value;

        public bool InCondition(Player.Player _player)
        {
            switch (effect)
            {
                case StatusController.Effect.Life:
                    return System.Compare.CompareValue(operation, _player.LifeBar.CurrentValue, value);
                case StatusController.Effect.Food:
                    return System.Compare.CompareValue(operation, _player.HungryBar.CurrentValue, value);
                case StatusController.Effect.Water:
                    return System.Compare.CompareValue(operation, _player.WaterBar.CurrentValue, value);
                case StatusController.Effect.Hapiness:
                    return System.Compare.CompareValue(operation, _player.HappinessBar.CurrentValue, value);
            }
            return false;
        }
    }
}

namespace Game.StatusController
{
    [Serializable]
    public enum Effect
    {
        Life,
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
    public enum ArithmeticOperations
    {
        Plus,
        Minus,
        SetValue,
        SetTrue,
        SetFalse
    }
    [Serializable]
    public enum LogicOperations
    {
        Equal,
        GreaterThan,
        LessTham,
        Different,
    }

    public static class Compare
    {
        public static bool CompareValue(LogicOperations _operation, int _A, int _B)
        {
            switch (_operation)
            {
                case LogicOperations.Different:
                    return _A != _B;
                case LogicOperations.Equal:
                    return _A == _B;
                case LogicOperations.GreaterThan:
                    return _A > _B;
                case LogicOperations.LessTham:
                    return _A < _B;
                default: return false;
            }
        }
    }
}