using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles;

public abstract class MinecraftStonePlaced : ModTile
{
    protected abstract Color MapColor { get; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        TileID.Sets.CanBeClearedDuringOreRunner[Type]  = true;
        TileID.Sets.CanBeClearedDuringGeneration[Type] = true;

        Main.tileSolid[Type]      = true;
        Main.tileMergeDirt[Type]  = true;
        Main.tileBlockLight[Type] = true;

        Main.tileMerge[Type][TileID.Stone] = true;
        Main.tileMerge[TileID.Stone][Type] = true;

        var deepslate = ModContent.TileType<DeepslatePlaced>();
        if (Type != deepslate)
        {
            Main.tileMerge[Type][deepslate] = true;
            Main.tileMerge[deepslate][Type] = true; 
        }

        DustType = DustID.Stone;
        
        HitSound = SoundID.Tink;

        AddMapEntry(MapColor, CreateMapEntryName());
    }
}

public sealed class AndesitePlaced : MinecraftStonePlaced
{
    protected override Color MapColor => new(138, 138, 142);
}

public sealed class DioritePlaced : MinecraftStonePlaced
{
    protected override Color MapColor => new(190, 191, 193);
}

public sealed class GranitePlaced : MinecraftStonePlaced
{
    protected override Color MapColor => new(161, 107, 87);
}

public sealed class DeepslatePlaced : MinecraftStonePlaced
{
    protected override Color MapColor => new(61, 61, 67);

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        RegisterItemDrop(ItemID.StoneBlock);
    }
}