using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Util
{
    public static class InputUtil
    {
        /// <summary>
        /// Checks an array of <see cref="KeyCode"/> for any currently held keys
        /// </summary>
        /// <param name="keysToCheck">The <see cref="KeyCode"/> to check</param>
        /// <returns>A dic of the <see cref="KeyCode"/> and <see cref="true"/> if held</returns>
        public static IDictionary<KeyCode, bool> GetIfKeysHeld(KeyCode[] keysToCheck)
        {
            IDictionary<KeyCode, bool> valuePairs = new Dictionary<KeyCode, bool>();

            foreach (KeyCode item in keysToCheck)
            {
                if (Input.GetKey(item))
                {
                    valuePairs.Add(item, true);
                }
                else
                {
                    valuePairs.Add(item, false);
                }
            }

            return valuePairs;
        }

        public static IList<Direction> ConvertKeyToDirection(IDictionary<Direction, KeyCode> translationDic, IDictionary<KeyCode, bool> heldKeys)
        {
            IList<Direction> directions = new List<Direction>();
            foreach (var item in translationDic)
            {
                if (heldKeys[item.Value])
                {
                    directions.Add(item.Key);
                }
            }
            return directions;
        }

        public enum Direction
        {
            forward,
            backward,
            left,
            right
        }
    }
}