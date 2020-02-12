using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Items {
    /// <summary>
    /// A singleton instance of a player's inventory. Uses storage logic from parent class.
    /// 
    /// It also provides a delegate to subscribe to, if you want something
    /// specific to happen when the inventory is updated (very useful for UI updates).
    /// </summary>
    public class Inventory : ItemStorage {
        // a singleton instance of this class to be used across all of the game
        private static Inventory _instance;

        /// Event that will trigger upon the inventory being changed
        public event Action OnInventoryChange = delegate { };

        // Useful for dumping what's in the inventory from within the inspector
        // will be quickly toggled off right after logging
        [Tooltip("Toggle this to quickly dump/log what's in the inventory.")]
        [SerializeField]
        private bool _dumpContents;

        private void Awake() {
            // the very first time this script is loaded, we want to save this instance
            // as the singleton for this class
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                // otherwise if it tries to instantiate itself again,
                // we want to destroy that object!
            } else {
                Destroy(gameObject);
            }
        }

        private void Update() {
            if (_dumpContents) {
                inventoryDump();
                _dumpContents = false;
            }

            // this will call all methods that are listening for inventory updates
            updateInventoryIfNeeded();
        }

        /// <summary>
        /// Gives you the current instance of the inventory!
        /// </summary>
        public static Inventory GetInstance() {
            return _instance;
        }

        /// <summary>
        /// If the inventory has been updated through something,
        /// then this method will call all methods attached to it's
        /// OnInventoryChange delegate list.
        /// TODO: consider moving this into the base storage, since that's where
        /// the variable is (and other storage systems may want that too).
        /// </summary>
        private void updateInventoryIfNeeded() {
            if (inventoryUpdated) {
                inventoryUpdated = false;
                OnInventoryChange();
            }
        }

        /***** UTILITIES AND USEFUL METHODS ******/

        /// <summary>
        /// See what's currently in your inventory.
        /// </summary>
        public void inventoryDump() {
            Debug.Log("**********Starting Inventory Dump**********");
            string inventoryDump = "[";
            foreach (string itemName in itemCountStorage.Keys) {
                inventoryDump = inventoryDump + "{" + itemName + ": " + itemCountStorage[itemName] + "},";
            }

            inventoryDump = inventoryDump.Remove(inventoryDump.Length - 1, 1);
            inventoryDump = inventoryDump + "]";
            Debug.Log(inventoryDump);

            Debug.Log("**********Inventory Dump Finished**********");
        }
    }
}