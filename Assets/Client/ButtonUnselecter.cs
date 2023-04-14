using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUnselecter : MonoBehaviour
{
    private Button cached_button;
    private Button Button => cached_button ??= GetComponent<Button>();
    void Start() => Button.onClick.AddListener(OnPressed);
    void OnPressed()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
