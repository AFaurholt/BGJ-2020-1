using UnityEngine;
using UnityEngine.UI;

namespace com.runtime.GameJamBois.BGJ20201.Items {
    /// <summary>
    /// Stores things you'll need to know about an item in your inventory!
    /// </summary>
    public class Item : MonoBehaviour {
        [SerializeField]
        private string itemName;

        [SerializeField]
        private Sprite icon;

        public void construct(string name) {
            itemName = name;
        }

        public Sprite getIcon() {
            return icon;
        }

        public string getItemName() {
            return itemName;
        }

        /// <summary>
        /// Defines our own definition of 'Equals' for items.
        /// Item equivalency is based on their names.
        /// </summary>
        public override bool Equals(object other) {
            Item otherItem = (Item)other;
            return itemName.Equals(otherItem.getItemName());
        }
    }
}