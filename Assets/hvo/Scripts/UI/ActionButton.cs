using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class ActionButton: MonoBehaviour{
    [SerializeField] private Image m_IconImage;
    [SerializeField] private Button m_Button;
    void OnDestroy()
    {
        m_Button.onClick.RemoveAllListeners();
    }

    public void Init(Sprite icon, UnityAction action){
        m_IconImage.sprite = icon;
        m_Button.onClick.AddListener(action);

    }
}