using CalamityMod;
using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.TVs;

public class CassetteID : ModSystem
{
    public static readonly List<(string path, string name)> CassettePaths = [];

    public static string[] PathsForCassettes =>
    [
        "CalRemix/Assets/Videos/PortedHim.mp4",
        "CalRemix/Assets/Videos/Hubris.mp4"
    ];
}

[Autoload(false)]
public class Cassette : ModItem, ILocalizedModType
{
    public override string LocalizationCategory => "Items.Cassettes";

    public override string Texture => "CalRemix/Content/Items/TVs/Cassette";

    public int CassetteType;

    protected override bool CloneNewInstances => true;
    public override string Name => CassetteID.CassettePaths[CassetteType].name + "Cassette";

    public Cassette(int i)
    {
        string name = CassetteID.PathsForCassettes[i];
        while (name.Contains('/'))
            name = name.Remove(0, name.IndexOf('/') + 1);

        name = name.Remove(name.IndexOf('.'));

        CassetteType = CassetteID.CassettePaths.Count;
        CassetteID.CassettePaths.Add((CassetteID.PathsForCassettes[i], name));
    }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 20;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.value = Item.buyPrice(1, 0, 0, 0);
        Item.rare = ItemRarityID.Green;
    }

    public override bool? UseItem(Player player)
    {
        if(player.whoAmI == Main.myPlayer)
        {
            Point mouseTile = Main.MouseWorld.ToTileCoordinates();
            MediaPlayerEntity playerEntity = CalamityUtils.FindTileEntity<MediaPlayerEntity>(mouseTile.X, mouseTile.Y, 2, 1);

            if (playerEntity == null)
                return false;

            playerEntity.player.ClearVideoQueue();

            playerEntity.CurrentContentPath = CassetteID.CassettePaths[CassetteType].path;
            playerEntity.StoredItem = Type;
            return true;
        }
        return false;
    }
}
