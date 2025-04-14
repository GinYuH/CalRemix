using CalRemix.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Placeables;

public abstract class MinecraftStone : ModItem
{
    protected abstract int CreateTile { get; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ItemID.Sets.IsAMaterial[Type] = true;

        Item.ResearchUnlockCount = 100;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.CloneDefaults(ItemID.StoneBlock);

        Item.width      = Item.height = 24;
        Item.createTile = CreateTile;
    }
}

public sealed class Andesite : MinecraftStone
{
    protected override int CreateTile => ModContent.TileType<AndesitePlaced>();
}

public sealed class Diorite : MinecraftStone
{
    protected override int CreateTile => ModContent.TileType<DioritePlaced>();
}

public sealed class Granite : MinecraftStone
{
    protected override int CreateTile => ModContent.TileType<GranitePlaced>();
}