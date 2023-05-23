using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.World;
using CalamityMod.Projectiles.Boss;
using CalRemix.Projectiles;

namespace CalRemix.NPCs.Bosses
{
    internal class DerellectPlug : ModNPC
    {
        public bool initialized = false;

        public int offx;
        public int offy;

        Vector2 destiny;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derellect Plug");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.LifeMaxNERB(10000);
            NPC.damage = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 40;
            NPC.height = 40;
            NPC.dontTakeDamage = false;
            NPC.defense = 22;
        }

        public override void AI()
    {

    }


        public override void OnKill()
        {
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
        }
    }
}