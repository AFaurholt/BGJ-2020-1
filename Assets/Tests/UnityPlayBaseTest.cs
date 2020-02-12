using System;
using System.Collections;
using System.Collections.Generic;
using com.runtime.GameJamBois.BGJ20201.Items;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests{
    public abstract class UnityPlayBaseTest {

        protected const string ITEM_ONE = "TestItemOne";
        protected const string ITEM_TWO = "TestItemTwo";

        /// <summary>
        /// Objects created by tests that you wish to remove!
        /// </summary>
        private ArrayList testGameObjectsToDestroy = new ArrayList();

        /// <summary>
        /// Creates a new GameObject,
        /// and adds it to an internal list that tracks game objects to destroy.
        /// </summary>
        /// <returns></returns>
        protected GameObject addGOAndStoreInDestroyList() {
            GameObject newObj = new GameObject();
            testGameObjectsToDestroy.Add(newObj);
            return newObj;
        }

        /// <summary>
        /// Same as above, but allows for one, or multiple, component types to be added.
        /// </summary>
        /// <returns></returns>
        protected GameObject addGOAndStoreInDestroyList(params Type[] typesToAdd) {
            GameObject obj = addGOAndStoreInDestroyList();
            foreach (Type type in typesToAdd) {
                obj.AddComponent(type);
            }

            return obj;
        }

        protected Item getNewItem(string name) {
            GameObject go = addGOAndStoreInDestroyList(typeof(Item));
            Item item = go.GetComponent<Item>();
            item.construct(name);
            return item;
        }

        protected void throwNotReadyYet() {
            Assert.Inconclusive("Test not ready to be run yet!");
        }

        /// <summary>
        /// Always will remove any GO's added to this list.
        /// Useful for when you don't want any GO's to be leftover in a scene
        /// after a test has run.
        /// </summary>
        [TearDown]
        public void removeAllNewGameObjects() {
            foreach (GameObject obj in testGameObjectsToDestroy) {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }
}
