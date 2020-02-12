using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class Inventory_ListenerTests : InventoryBasetest {

        private int someValueToIncrement = 0;

        [UnityTest]
        public IEnumerator methodToBeCalledOnInventoryUpdateTest() {
            // add a very simple method to this, to listen for inventory updates
            inventory.OnInventoryChange += simpleMethod;

            // modify the inventory to force an expected listener call!
            addNewItemToInventory(ITEM_ONE);

            // simulates 1 Update frame to be called, thus, the added method should be called
            yield return null;

            Assert.AreEqual(1, someValueToIncrement, "Listener methods are not called when the inventory is updated.");
        }

        private void simpleMethod() {
            someValueToIncrement = someValueToIncrement + 1;
        }
    }
}
