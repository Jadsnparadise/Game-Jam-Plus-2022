using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System.Ui
{
    public class TextMeshProController : MonoBehaviour
    {
        public static TextMeshProController Instance;
        [SerializeField] TMPro.TextMeshProUGUI text;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        public void SetManager(bool _activeSelf, GameObject _obj = null, string _text = "", Vector3 textOffset = new())
        {
            gameObject.SetActive(_activeSelf);
            if (_obj != null)
            {
                text.text = _text;
                gameObject.transform.position = _obj.transform.position + textOffset;
            }
        }
    }
}