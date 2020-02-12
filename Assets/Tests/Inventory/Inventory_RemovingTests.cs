using System.Collections;
using System.Collections.Generic;
using com.runtime.GameJamBois.BGJ20201.Items;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class Inventory_RemovingTests : InventoryBasetest {

        private Item addAndRemoveItem(string itemName) {
            Item item = addNewItemToInventory(itemName);
            inventory.Remove(item);
            return item;
        }

        [UnityTest]
        public IEnumerator removeAllItemsDictionaryTest() {
            addAndRemoveItem(ITEM_ONE);

            var dict = inventory.GetItemCountStorage();

            Assert.IsFalse(dict.ContainsKey(ITEM_ONE),
                "Key in the dictionary within the inventory is not wiped when all of that item are removed.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator removeAllItemsOrderedArrayTest() {
            Item item = addAndRemoveItem(ITEM_ONE);

            ArrayList array = inventory.GetOrderedStorage();

            Assert.IsFalse(array.Contains(item),
                "Entry in the ordered array within the inventory is not removed when all of that item are removed.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator simpleRemoveDecrementTest() {
            Item item = getNewItem(ITEM_ONE);
            int count = 10;
            inventory.Add(item, count);

            inventory.Remove(item);

            Assert.AreEqual(count - 1, inventory.GetItemCount(item),
                "Item is not decremented from the inventory when removing it.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator removeXTest() {
            Item item = getNewItem(ITEM_ONE);
            int count = 10, remove = 5;
            inventory.Add(item, count);

            inventory.Remove(item, remove);

            Assert.AreEqual(count - remove, inventory.GetItemCount(item),
                "When removing a variable amount of items, they are not actually removed from the inventory.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator tryToRemoveAnItemYouDontHaveTest() {
            throwNotReadyYet();
            yield return null;
        }

        [UnityTest]
        public IEnumerator removeXWhenYouHaveLessThanXTest() {
            throwNotReadyYet();
            yield return null;
        }
    }
}
