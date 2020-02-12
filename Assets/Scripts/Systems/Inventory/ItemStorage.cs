using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.runtime.GameJamBois.BGJ20201.Items {
    /// <summary>
    /// A basic storage system for items. Stores one item -> count.
    /// Does not include stacking limits yet, and each type of item is stored
    /// at 1 slot (so you can't have 64 of one item in one index and 64 in another,
    /// you'd have 128 in 1 slot).
    /// </summary>
    public class ItemStorage : MonoBehaviour{
        // simple item name to count storage
        protected Dictionary<string, int> itemCountStorage = new Dictionary<string, int>();

        // tracks order of items in the storage, and keeps 1 of each 'Item'
        protected ArrayList itemOrderStorage = new ArrayList();

        // items to initialize the storage with
        [Tooltip("Want to start with items when the game starts? Add them here!")]
        [SerializeField]
        protected Item[] itemsToStartWith;

        // internal bool to know if the inventory has been updated
        protected static bool inventoryUpdated = false;

        // NOTE: if you're not careful you might overload this Start()
        // make sure to use base.Start() if you need to update your start
        protected void Start() {
            // if we have any items to start with, go ahead and add that here.
            ProcessStartingItems();
        }

        /******* READING/INSPECTING STORAGE *******/

        /// <summary>
        /// Reads this item in the storage
        /// </summary>
        public Item GetItemAt(int i) {
            return (Item)itemOrderStorage[i];
        }

        /// <summary>
        /// Literally returns you the next item in the desired direction.
        /// 
        /// This does not do any sort of logic checking other than that.
        /// </summary>
        public int GetNextItemIndex(int startIndex, bool leftOrRight) {
            return SafeIncrement(startIndex, leftOrRight);
        }

        /// <summary>
        /// Total number of unique item types in this storage.
        /// </summary>
        public int GetUniqueItemCount() {
            return itemCountStorage.Keys.Count;
        }

        /// <summary>
        /// Count of a certain item type.
        /// </summary>
        public int GetItemCount(Item item) {
            return itemCountStorage[item.getItemName()];
        }

        /// <summary>
        /// Safely increments the index throughout the inventory, and wraps
        /// back around should you hit the limits.
        /// </summary>
        private int SafeIncrement(int startIndex, bool leftToRight) {
            int index = startIndex;
            int change = 1;
            if (!leftToRight) {
                change = -1;
            }

            index += change;
            if (leftToRight) {
                if (index == itemOrderStorage.Count) {
                    index = 0;
                }
            } else {
                if (index == -1) {
                    index = itemOrderStorage.Count - 1;
                }
            }

            return index;
        }

        /******* ADDING TO STORAGE *******/

        /// <summary>
        /// Simple add method. Creates entry if none,
        /// otherwise increments the item that already exists.
        /// </summary>
        public void Add(Item item, int count) {
            // prevents trying to add 0 the first time!
            if (count == 0) {
                return;
            }

            // if no entry exists, add an entry now
            if (!DoesItemExist(item)) {
                itemCountStorage.Add(item.getItemName(), count);
                itemOrderStorage.Add(item);
            } else {
                itemCountStorage[item.getItemName()] += count;
            }

            inventoryUpdated = true;
            Debug.Log("Inventory: added [" + count + "] item(s) [" + item + "]");
        }

        /// <summary>
        /// If you just want to add 1 item, this method is what you want.
        /// </summary>
        public void Add(Item item) {
            Add(item, 1);
        }

        /******* REMOVING FROM STORAGE *******/

        /// <summary>
        /// Removes this item from the object storage if found.
        /// 
        /// If there's no more of that item, completely removes the item from the storage.
        /// </summary>
        public void Remove(Item item, int count) {
            try {
                itemCountStorage[item.getItemName()] -= count;

                // remove from the orderInventory if this is the last one!
                // also remove the key from the dictionary too
                if (itemCountStorage[item.getItemName()] == 0) {
                    RemoveItemCompletely(item);
                }
            } catch (KeyNotFoundException e) {
                Debug.Log("Inventory: no item exists for: " + item);
                return;
            }

            Debug.Log("Inventory: removed [" + count + "] item(s) [" + item + "]");
            inventoryUpdated = true;
        }

        /// <summary>
        /// Remove just 1 of this item.
        /// </summary>
        public void Remove(Item item) {
            Remove(item, 1);
        }

        /// <summary>
        /// Simple as can be, reads in items you want to start this
        /// inventory off with.
        /// 
        /// If no items were added we'll just skip this.
        /// </summary>
        public void ProcessStartingItems() {
            if (itemsToStartWith == null) {
                return;
            }

            foreach (Item item in itemsToStartWith) {
                Add(item);
            }
        }

        /// <summary>
        /// Is there an entry in the itemInventory for this?
        /// </summary>
        public bool DoesItemExist(Item item) {
            return itemCountStorage.ContainsKey(item.getItemName());
        }

        /// <summary>
        /// Removes any objects that include '(Clone)' in the name,
        /// in case you start adding in items this way.
        /// </summary>
        protected string stripOutClone(GameObject obj) {
            string name = obj.name;
            return name.Replace("(Clone)", "").Trim();
        }

        /// <summary>
        /// Fully removes this item from both storage locations,
        /// like wiping the entry.
        /// 
        /// Very destructive, hense it being private.
        /// </summary>
        private void RemoveItemCompletely(Item item) {
            int indexToRemove = -1;
            for (int i = 0; i < itemOrderStorage.Count; i++) {
                Item itemInArrayList = (Item)itemOrderStorage[i];
                if (itemInArrayList.Equals(item)) {
                    indexToRemove = i;
                    i = itemOrderStorage.Count;
                }
            }

            if (indexToRemove == -1) {
                Debug.Log("Trying to remove an item that does not exist in the ordered inventory array!");
            } else {
                itemOrderStorage.RemoveAt(indexToRemove);
                itemCountStorage.Remove(item.getItemName());
            }
        }
    }
}