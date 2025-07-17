using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.Particles;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Tiles.PlaguedJungle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.Sealed
{
    public class GastrogelBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(255, 87, 176));
            HitSound = SoundID.NPCHit1;
            DustType = DustID.PinkSlime;
        }
    }
    public class AstrogelBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(240, 55, 227));
            HitSound = SoundID.NPCHit1;
            DustType = DustID.ShadowbeamStaff;
        }
    }
    public class GildedHardlightBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileStone[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(246, 255, 186));
            HitSound = SoundID.Shatter with { Pitch = 1 };
            DustType = DustID.Enchanted_Gold;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 2;
            g = 2;
            b = 0;
        }
    }
    public class VoidInfusedTurnipFruitPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(36, 0, 36));
            HitSound = SoundID.NPCHit1;
            DustType = DustID.ShadowbeamStaff;
        }
    }
    public class GroundFleshBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(163, 34, 41));
            HitSound = SoundID.NPCHit1;
            DustType = DustID.Blood;
        }
    }
    public class CornSquarePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(255, 255, 0));
            HitSound = SoundID.GlommerBounce;
            DustType = DustID.YellowTorch;
        }
    }
}