using CalamityMod;
using CalamityMod.Items.SummonItems;
using CalamityMod.TileEntities;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.Bosses.Aurelionium
{
    public class Aurelionium : ModNPC
    {
        // This is not a real boss. It exists to sit in the Boss Checklist and trick people into thinking that a Gilded Isle boss exists
        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.LifeMaxNERB(13500, 20100, 999992);
            NPC.defense = 10;
            NPC.damage = 25;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().canBreakPlayerDefense = true;
        }
        public override void AI()
        {
            NPC.StrikeInstantKill();
        }
        public override void OnKill()
        {
            RemixDowned.downedAurelionium = true;
            CalRemixWorld.UpdateWorldBool();
        }
    }
}
