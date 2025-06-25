using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json.Linq;

using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace CalRemix;

public sealed class PersistentData : ILoadable
{
    private static readonly Preferences data = new(Path.Combine(Main.SavePath, "remix", "persist.json"));

    private static List<string> helpers = [];
    
    void ILoadable.Load(Mod mod)
    {
        data.Load();

        helpers = data.Get<JArray>(nameof(helpers), []).Select(x => x.ToString()).ToList();
    }

    void ILoadable.Unload()
    {
        Save();
    }

    private static void Save()
    {
        data.Put(nameof(helpers), helpers);
        
        data.Save();
    }

    public static void UnlockHelper(string helper)
    {
        if (helpers.Contains(helper))
        {
            return;
        }

        helpers.Add(helper);
        Save();
    }

    public static List<string> GetHelpers()
    {
        return helpers;
    }
}