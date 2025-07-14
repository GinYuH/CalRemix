using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Tiles.Ores;
using CalamityMod.CalPlayer;
using CalRemix.Core.Subworlds;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class OmegaFist : ModNPC
    {
        public NPC Papa => Main.npc[PapaIndex];
        public int PapaIndex => (int)NPC.ai[0] - 1;
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.LifeMaxNERB(25000, 50000, 150000);
            NPC.damage = 280;
            NPC.defense = 12;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.HitSound = AuricOre.MineSound;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.DR_NERD(0.05f);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            if (PapaIndex == -1)
            {
                NPC.active = false;
                return;
            }
            if (!Papa.active || Papa.type != ModContent.NPCType<SkeletronOmega>())
            {
                NPC.active = false;
                return;
            }
        }

        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(ModContent.NPCType<SkeletronOmega>());
        }
    }
}