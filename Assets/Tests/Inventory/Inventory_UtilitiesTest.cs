using System;
using System.Collections;
using System.Collections.Generic;
using com.runtime.GameJamBois.BGJ20201.Items;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class Inventory_UtilitiesTests : InventoryBasetest {

        [UnityTest]
        public IEnumerator itemsToStartWithTest() {
            ArrayList items = new ArrayList();

            Item itemOne = getNewItem(ITEM_ONE);
            Item itemTwo = getNewItem(ITEM_TWO);

            items.Add(itemOne);
            items.Add(itemTwo);

            inventory.SetArrayForItemsToStartWith((Item[])items.ToArray(typeof(Item)));

            // this simulates the 'Start' call
            inventory.ProcessStartingItems();

            Assert.IsTrue(inventory.DoesItemExist(itemOne) && inventory.DoesItemExist(itemTwo),
                "Items added to the 'Items to Start With' array are not added to the inventory at the start.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator noItemsToStartWithTest() {
            // this simulates the 'Start' call
            inventory.ProcessStartingItems();

            // NOTE: when we have saving of inventories, this test may not pass.
            Assert.IsTrue(inventory.GetUniqueItemCount() == 0,
                "No items were provided to the 'Items to Start With' array, and yet there are items that started in the inventory.");

            yield return null;
        }
    }
}
