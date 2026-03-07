using UnityEngine;
using TMPro;

namespace BoardAgain.Battle
{
    public class TooltipManager : MonoBehaviour
    {
        public GameObject tooltipBox;
        public TextMeshProUGUI tooltipText;
        public Vector2 offset = new Vector2(0, 50);

        public void ShowTooltip(string text, Vector3 position)
        {
            tooltipBox.SetActive(true);
            tooltipText.text = text;
            tooltipBox.transform.position = position + (Vector3)offset;
        }

        public void HideTooltip()
        {
            tooltipBox.SetActive(false);
        }
    }
}