
using System.Globalization;
using UnityEngine;

public class PokemonDisplay : MonoBehaviour
{
    private Pokemon m_pokemon;
    public Pokemon pokemon
    {
        set
        {
            m_pokemon = value ?? m_pokemon;
            Sprite.sprite = GetSprite(value);
            NameText.text = $"{value.Name}{value.Gender.Replace("F", " <color=#ff80df>♀</color>").Replace("M", " <color=#0000ff>♂</color>")} Lv.{value.Level}";
            AbilityText.text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ability);
            HPPercentage = value.HealthPercent;
            Status = value.Status;
            Item = value.item;
        }
        get => m_pokemon;
    }
    public string Item { set { ItemText.text = value; } }

    public Move[] Moves
    {
        set
        {
            var minIndex = Mathf.Min(MoveTexts.Length, value.Length);
            for (int i = 0; i < minIndex; i++)
                MoveTexts[i].text = $"{GetMoveText(value[i], ((int)MoveTexts[i].alignment & (int)TMPro.HorizontalAlignmentOptions.Right) != 0)}";
        }
    }

    private string m_statusText;
    public string Status
    {
        set
        {
            m_statusText = GetStatusText(value);
            var isAnchoredRight = ((int)HPText.alignment & (int)TMPro.HorizontalAlignmentOptions.Right) != 0;
            HPText.text = isAnchoredRight ? $"{m_statusText} {m_HPBarText}" : $"{m_HPBarText} {m_statusText}".Trim();
        }
    }

    private string m_HPBarText;
    public int HPPercentage
    {
        set
        {
            var greenText = new string('/', value);
            var grayText = new string('/', 100 - value);
            var isAnchoredRight = ((int)HPText.alignment & (int)TMPro.HorizontalAlignmentOptions.Right) != 0;
            m_HPBarText = $"<mspace=.095em><color=green>{greenText}<color=#333333>{grayText}</mspace>";
            HPText.text = isAnchoredRight ? $"{m_statusText} {m_HPBarText}" : $"{m_HPBarText} {m_statusText}".Trim();
        }
    }

    public static Sprite GetSprite(Pokemon pokemon)
    {
        return Resources.Load<Sprite>(pokemon.Gender == "F" ? "female/" + pokemon.Species : pokemon.Species) ?? Resources.Load<Sprite>(pokemon.Species);
    }

    private string GetMoveText(Move move, bool isAnchoredRight)
    {
        var ppText = $"{move.pp.ToString().PadLeft(2, '0')}/{move.maxpp.ToString().PadLeft(2, '0')}";
        return isAnchoredRight ? $"{move.move} {ppText}" : $"{ppText} {move.move}";
    }

    private string GetStatusText(string status)
    {
        return status switch
        {
            "slp" => "<color=#b7e8e6>slp</color>",
            "par" => "<color=#edeb55>par</color>",
            "brn" => "<color=#ed5c3b>brn</color>",
            "psn" => "<color=#c71ea2>psn</color>",
            "tox" => "<color=#8c036e>tox</color>",
            _ => status
        };
    }

    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private TMPro.TMP_Text NameText;
    [SerializeField] private TMPro.TMP_Text AbilityText;
    [SerializeField] private TMPro.TMP_Text ItemText;
    [SerializeField] private TMPro.TMP_Text HPText;
    [SerializeField] private TMPro.TMP_Text[] MoveTexts;
}
