using System.Collections;
using System.Collections.Generic;
using com.runtime.GameJamBois.BGJ20201.Items;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class InventoryBasetest : UnityPlayBaseTest {
        protected Inventory inventory;
        protected GameObject gameObjectWithInventory;

        /// <summary>
        /// Wrapper to add a new item to the inventory.
        /// </summary>
        protected Item addNewItemToInventory(string name) {
            Item item = getNewItem(name);
            inventory.Add(item);
            return item;
        }

        /// <summary>
        /// Create a GO with the inventory component already attached.
        /// </summary>
        [SetUp]
        public void addGOWithInventory() {
            gameObjectWithInventory = addGOAndStoreInDestroyList(typeof(Inventory));
            inventory = gameObjectWithInventory.GetComponent<Inventory>();
        }

        /// <summary>
        /// Always make this inventory object null after every test!
        /// </summary>
        [TearDown]
        public void wipeInventoryRef() {
            inventory = null;
            gameObjectWithInventory = null;
        }
    }
}
