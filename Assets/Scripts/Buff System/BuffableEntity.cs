﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Game
{
    public class BuffableEntity : MonoBehaviour
    {
        readonly Dictionary<BuffData, Buff> _buffs = new Dictionary<BuffData, Buff>();

        public event Action<Buff> OnNewBuffAdd = delegate { };

        void Update()
        {
            // Iterate through all buffs and update them
            foreach (var buff in _buffs.Values.ToList())
            {
                buff.Tick(Time.deltaTime);
                if (buff.IsFinished)
                {
                    _buffs.Remove(buff.BuffData);
                }
            }
        }

        public void AddBuff(Buff buff)
        {
            if (_buffs.ContainsKey(buff.BuffData))
            {
                // Restart buff
                _buffs[buff.BuffData].Activate();
            }
            else
            {
                _buffs.Add(buff.BuffData, buff);
                buff.Activate();

                OnNewBuffAdd?.Invoke(buff);
            }
        }

        public void RemoveBuff(BuffData buff)
        {
            if (_buffs.ContainsKey(buff))
            {
                _buffs[buff].End();
                _buffs.Remove(buff);
            }
        }

        public bool Contains(BuffData buff)
        {
            return _buffs.ContainsKey(buff);
        }
    }
}
