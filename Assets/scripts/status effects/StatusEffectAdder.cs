using System;
using System.Collections.Generic;
using UnityEngine;

namespace statusEffects
{


    public class StatusEffectAdder : MonoBehaviour
    {
        public static StatusEffectAdder current;
        public GameObject player;

        public List<GameObject> statusEffects;
        public List<GameObject> statusEffect_displays;

        public void Awake()
        {
            current = this;
        }

        public void addStatusEffect(GameObject target, int effect_number)
        {
            GameObject effect = Instantiate(statusEffects[effect_number]);
            effect.transform.SetParent(target.transform);
        }

        [ContextMenu("buff 3 test")]
        public void test_buff_3()
        {
            addStatusEffect(player, 3);
        }
    }
}