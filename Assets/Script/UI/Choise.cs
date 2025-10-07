using TMPro;
using UnityEngine;

namespace Nananami
{
    class Choise : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_choise_text;
        [SerializeField] private int m_ui_index;
        public int index { get; set; }
        public void SetText(string text)
        {
            gameObject.SetActive(true);
            m_choise_text.text = text;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}