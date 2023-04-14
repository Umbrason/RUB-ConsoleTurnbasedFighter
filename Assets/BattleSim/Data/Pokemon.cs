using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[Serializable]
public class Pokemon
{
    public string Species
    {
        get
        {
            var rawText = details.Split(',')[0].Trim();
            return new Regex(@"[^A-Za-z0-9']+").Replace(rawText, "-");
        }
    }
    public string Level => details.Split(',')[1].Replace("L", "").Trim();
    public string Gender => details.Split(',').Length > 2 ? details.Split(',')[2].Trim() : "";


    public int HealthPercent
    {
        get
        {
            var health = condition.Split(" ")[0];
            if (health.Contains("/"))
            {
                var parts = health.Split("/");
                var nominator = parts[0];
                var denominator = parts[1];
                return UnityEngine.Mathf.RoundToInt(int.Parse(nominator) / (float)int.Parse(denominator) * 100);
            }
            return 0;
        }
    }

    public string Status => condition.Contains(" ") ? condition.Split(" ")[1] : "";

    public string Name => ident.Split(':', 2)[1].Trim();
    public string Owner => ident.Split(':', 2)[0].Trim();

    #region JSON data
    public string ident;
    public string details;
    public string condition;
    public bool active;
    public Stats stats;
    public List<string> moves;
    public string baseAbility;
    public string item;
    public string pokeball;
    public string ability;
    #endregion
}

public class Stats
{
    public int atk;
    public int def;
    public int spa;
    public int spd;
    public int spe;
}