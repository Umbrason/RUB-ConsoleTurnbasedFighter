using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PokemonBattle : MonoBehaviour
{
    private string format = "gen8randombattle";
    private BattleSimulationInstance battleSimulationInstance = null;

    [SerializeField] private PokemonDisplay P1Display;
    [SerializeField] private Image[] P1TeamImages;
    [SerializeField] private PokemonDisplay P2Display;
    [SerializeField] private Image[] P2TeamImages;

    public Pokemon ActivePokemonP1 => P1Display.pokemon;
    public Pokemon ActivePokemonP2 => P2Display.pokemon;


    void Awake()
    {
        battleSimulationInstance = new BattleSimulationInstance();
        battleSimulationInstance.Start(format, new Player("Umbrason"), new Player("Luxdottir"));
    }

    void Update()
    {
        while (battleSimulationInstance.P1PokemonUpdates.Count > 0)
            P1Display.pokemon = battleSimulationInstance.P1PokemonUpdates.Dequeue().Apply(P1Display.pokemon);
        while (battleSimulationInstance.P2PokemonUpdates.Count > 0)
            P2Display.pokemon = battleSimulationInstance.P2PokemonUpdates.Dequeue().Apply(P2Display.pokemon);
        while (battleSimulationInstance.P1ChoiceRequests.Count > 0)
        {
            var request = battleSimulationInstance.P1ChoiceRequests.Dequeue();
            UpdatePokemonDisplayFromChoiceRequest(request, P1Display, P1TeamImages);
            p1Request = request;
        }
        while (battleSimulationInstance.P2ChoiceRequests.Count > 0)
        {
            var request = battleSimulationInstance.P2ChoiceRequests.Dequeue();
            UpdatePokemonDisplayFromChoiceRequest(request, P2Display, P2TeamImages);
            p2Request = request;
        }
    }

    private void UpdatePokemonDisplayFromChoiceRequest(ChoiceRequest request, PokemonDisplay display, Image[] teamImages)
    {
        for (int i = 0; i < request.side.pokemon.Count; i++)
        {
            teamImages[i].color = request.side.pokemon[i].HealthPercent > 0 ? Color.white : new Color(.05f, .05f, .05f, 1f);
            teamImages[i].sprite = PokemonDisplay.GetSprite(request.side.pokemon[i]);
        }
        if (request.side.pokemon.Any(pkmn => pkmn.active) && request.active.Count > 0)
        {
            var active = request.side.pokemon.First(pkmn => pkmn.active);
            UpdatePokemonDisplay(display, request.active[0], active, display == P2Display);
        }
        else UnityEngine.Debug.Log("no active pokemon");
    }

    private void UpdatePokemonDisplay(PokemonDisplay display, ActivePKMNInfo info, Pokemon activePokemon, bool alignRight)
    {
        display.pokemon = activePokemon;
        display.Moves = info.moves.ToArray();
    }

    void OnDestroy()
    {
        battleSimulationInstance.Stop();
    }

    [System.NonSerialized] public ChoiceRequest p1Request = null;
    [System.NonSerialized] public ChoiceRequest p2Request = null;

    public void Switch(int playerID, int pokemonID)
    {
        var request = playerID == 1 ? p1Request : p2Request;
        if (request == null) return;
        var pokemon = request.side.pokemon[pokemonID - 1];
        battleSimulationInstance.Switch(playerID, pokemon);
        if (playerID == 1) p1Request = null;
        else p2Request = null;
    }

    public void MakeMove(int playerID, int moveID)
    {
        var request = playerID == 1 ? p1Request : p2Request;
        if (request == null) return;
        var move = request.active[0].moves[moveID - 1];
        battleSimulationInstance.MakeMove(playerID, move);
        if (playerID == 1) p1Request = null;
        else p2Request = null;
    }
}
