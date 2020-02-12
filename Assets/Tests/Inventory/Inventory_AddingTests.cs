using System.Collections;
using System.Collections.Generic;
using com.runtime.GameJamBois.BGJ20201.Items;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class Inventory_AddingTests : InventoryBasetest {

        [UnityTest]
        public IEnumerator itemAddsToDictionaryTest(){
            addNewItemToInventory(ITEM_ONE);

            // now read the dictionary to make sure it's actually added in
            var dict = inventory.GetItemCountStorage();
            Assert.IsTrue(dict.ContainsKey(ITEM_ONE),
                "Inventory does not add a new entry into the dictionary when a new item is added.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator itemAddsToObjectArrayTest() {
            bool containsAddedItem = false;
            Item addedItem = addNewItemToInventory(ITEM_ONE);

            // now look through this arraylist for the item you just added
            ArrayList arrayList = inventory.GetOrderedStorage();
            foreach(Item item in arrayList) {
                if (item == addedItem) {
                    containsAddedItem = true;
                }
            }

            Assert.IsTrue(containsAddedItem, "Inventory does not add the new item to the orderedArrayList.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator addedItem_DoesItemExistMethodTest() {
            Item item = addNewItemToInventory(ITEM_ONE);

            Assert.IsTrue(inventory.DoesItemExist(item),
                "Inventory does not show that an added item is there.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator noItemAdded_DoesItemExistMethodTest() {
            // this item is actually added to the inventory
            addNewItemToInventory(ITEM_ONE);

            // this item is not, but it does exist in the scene
            Item itemNotInInventory = getNewItem(ITEM_TWO);

            Assert.IsFalse(inventory.DoesItemExist(itemNotInInventory),
                "Inventory shows an item that was never added to the inventory, is actually there.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator totalItemCountTest() {
            int totalItemsToAdd = 10;
            Item item = addNewItemToInventory(ITEM_ONE);
            for (int i = 0; i < totalItemsToAdd-1; i++) {
                addNewItemToInventory(ITEM_ONE);
            }

            Assert.AreEqual(totalItemsToAdd, inventory.GetItemCount(item),
                "Total items in the inventory does not match how many were added.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator addXTest() {
            int totalItemsToAdd = Random.Range(0, 20);
            Item itemToAdd = getNewItem(ITEM_ONE);
            inventory.Add(itemToAdd, totalItemsToAdd);

            Assert.AreEqual(totalItemsToAdd, inventory.GetItemCount(itemToAdd),
                "Adding in a variable amount of items does not actually add that many items.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator twoTypesAddedMethodTest() {
            // TODO: this could be more robust by adding in x randomly named items
            addNewItemToInventory(ITEM_ONE);
            addNewItemToInventory(ITEM_TWO);

            // make sure the total different types returned is correct
            Assert.AreEqual(2, inventory.GetUniqueItemCount(),
                "Adding 2 different types of items is not accurately reported by the inventory.");
            yield return null;
        }

        [UnityTest]
        public IEnumerator itemOrderTest() {
            Item itemOne = addNewItemToInventory(ITEM_ONE);
            Item itemTwo = addNewItemToInventory(ITEM_TWO);

            // this checks the order at which items are added, is the order that they appear in the inventory
            Assert.AreEqual(itemOne, inventory.GetItemAt(0),
                "Item that was added first does not show up first in the inventory.");
            Assert.AreEqual(itemTwo, inventory.GetItemAt(1),
                "Item that was added second does not show up as second in the inventory.");
            yield return null;
        }
    }
}
