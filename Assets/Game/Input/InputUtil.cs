using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Util
{
    public static class InputUtil
    {
        public static Dictionary<KeyCode, bool> GetIfKeysHeld(KeyCode[] keysToCheck)
        {
            Dictionary<KeyCode, bool> valuePairs = new Dictionary<KeyCode, bool>();

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
    }
}