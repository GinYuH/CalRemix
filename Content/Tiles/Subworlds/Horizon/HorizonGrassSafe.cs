using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalRemix.Content.NPCs.Bosses.SealedOne.OrbitingOrb;

namespace CalRemix.Content.Tiles.Subworlds.Horizon
{
    public class HorizonGrassSafe : HorizonGrass
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(255, 255, 255));
            HitSound = SoundID.Grass;
            DustType = DustID.WhiteTorch;
            Main.tileBlendAll[Type] = true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }
    }
}