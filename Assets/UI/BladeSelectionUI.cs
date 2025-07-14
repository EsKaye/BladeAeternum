using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BladeAeternum.UI
{
    /// <summary>
    /// UI for displaying and equipping unlocked blades from BladeInventory. Now includes 3D preview and equip confirm.
    /// </summary>
    public class BladeSelectionUI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Characters.BladeInventory bladeInventory;
        [SerializeField] private Transform bladeListParent;
        [SerializeField] private GameObject bladeButtonPrefab;
        [SerializeField] private Text equippedBladeText;
        [SerializeField] private Transform bladePreviewParent;
        [SerializeField] private Text bladeStatsText;
        [SerializeField] private Button equipConfirmButton;
        #endregion

        #region State
        private List<GameObject> bladeButtons = new List<GameObject>();
        private GameObject currentPreview;
        private Combat.BladeData selectedBlade;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            RefreshBladeList();
            equipConfirmButton.onClick.AddListener(OnEquipConfirm);
        }
        private void OnDisable()
        {
            equipConfirmButton.onClick.RemoveListener(OnEquipConfirm);
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
                btnObj.GetComponent<Button>().onClick.AddListener(() => OnBladeSelected(bladeData));
                bladeButtons.Add(btnObj);
            }
            UpdateEquippedBladeText();
        }

        public void OnBladeSelected(Combat.BladeData blade)
        {
            selectedBlade = blade;
            UpdateBladePreview();
            UpdateBladeStats();
        }

        private void OnEquipConfirm()
        {
            if (selectedBlade != null)
            {
                bladeInventory.EquipBlade(selectedBlade);
                UpdateEquippedBladeText();
            }
        }
        #endregion

        #region Private Methods
        private void UpdateEquippedBladeText()
        {
            var current = bladeInventory.GetCurrentBlade();
            equippedBladeText.text = current != null ? $"Equipped: {current.bladeName}" : "No Blade Equipped";
        }
        private void UpdateBladePreview()
        {
            if (currentPreview != null)
                Destroy(currentPreview);
            if (selectedBlade != null && selectedBlade.bladeModel != null)
            {
                currentPreview = Instantiate(selectedBlade.bladeModel, bladePreviewParent);
                currentPreview.transform.localPosition = Vector3.zero;
                currentPreview.transform.localRotation = Quaternion.identity;
            }
        }
        private void UpdateBladeStats()
        {
            if (selectedBlade != null)
            {
                bladeStatsText.text = $"Damage: x{selectedBlade.damageMultiplier}\nUnlock: {selectedBlade.unlockRequirement}";
            }
            else
            {
                bladeStatsText.text = "";
            }
        }
        #endregion
    }
} 