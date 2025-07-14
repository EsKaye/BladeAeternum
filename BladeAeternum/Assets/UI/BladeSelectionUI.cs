using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BladeAeternum.UI
{
    /// <summary>
    /// UI for displaying and equipping unlocked blades from BladeInventory.
    /// </summary>
    public class BladeSelectionUI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Characters.BladeInventory bladeInventory;
        [SerializeField] private Transform bladeListParent;
        [SerializeField] private GameObject bladeButtonPrefab;
        [SerializeField] private Text equippedBladeText;
        #endregion

        #region State
        private List<GameObject> bladeButtons = new List<GameObject>();
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            RefreshBladeList();
        }
        #endregion

        #region Public Methods
        public void RefreshBladeList()
        {
            foreach (var btn in bladeButtons)
                Destroy(btn);
            bladeButtons.Clear();
            var blades = bladeInventory.GetUnlockedBlades();
            foreach (var blade in blades)
            {
                var btnObj = Instantiate(bladeButtonPrefab, bladeListParent);
                var btnText = btnObj.GetComponentInChildren<Text>();
                btnText.text = blade.bladeName;
                var bladeData = blade;
                btnObj.GetComponent<Button>().onClick.AddListener(() => EquipBlade(bladeData));
                bladeButtons.Add(btnObj);
            }
            UpdateEquippedBladeText();
        }

        public void EquipBlade(Combat.BladeData blade)
        {
            bladeInventory.EquipBlade(blade);
            UpdateEquippedBladeText();
        }
        #endregion

        #region Private Methods
        private void UpdateEquippedBladeText()
        {
            var current = bladeInventory.GetCurrentBlade();
            equippedBladeText.text = current != null ? $"Equipped: {current.bladeName}" : "No Blade Equipped";
        }
        #endregion
    }
} 