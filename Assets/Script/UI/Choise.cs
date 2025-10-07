using Nananami.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nananami
{
    class Choice : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_choise_text;
        [SerializeField] private Button m_button;
        [SerializeField] private LevelUpUIController m_level_up_ui_controller;
        public int index { get; set; }
        public void SetText(string text)
        {
            gameObject.SetActive(true);
            m_choise_text.text = text;
        }

        public void SetButtonState(bool enabled)
        {
            m_button.interactable = enabled;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }

        public void OnClicked()
        {
            m_level_up_ui_controller.OnChoosed(index);
        }
    }
}