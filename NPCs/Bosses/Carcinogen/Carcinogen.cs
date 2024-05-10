using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Items;
using System.Linq;
using CalRemix.UI;
using System.Collections;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;

namespace CalRemix.NPCs.Bosses.Carcinogen
{
    public class Carcinogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public enum PhaseType
        {
            Idle = 0
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carcinogen");
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 200;
            NPC.width = 86;
            NPC.height = 88;
            NPC.defense = 15;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(40000, 48000, 300000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            Main.LocalPlayer.AddBuff(BuffID.Blackout, 22);
            Main.LocalPlayer.wingTime = 0;
            NPC.TargetClosest();
            if (Target == null || Target.dead)
            {
                NPC.velocity.Y += 1;
                return;
            }
            switch (Phase)
            {
                case (int)PhaseType.Idle:
                    {
                        int tpDistX = 1000;
                        int tpDistY = 500;
                        NPC.ai[1]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 4;
                        if (NPC.ai[1] == 180)
                        {
                            teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                        }
                        if (NPC.ai[1] > 180)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Dirt);
                                Main.dust[d].noGravity = true;
                            }
                        }
                        if (NPC.ai[1] > 240)
                        {
                            DustExplosion();
                            NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                            DustExplosion();
                            NPC.ai[1] = 0;
                        }
                    }
                    break;
            }
            
            base.AI();
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
        new FlavorTextBestiaryInfoElement("When Yharim realized that the Archmage had defected, Calamitas was ordered to hunt down and kill her former mentor. After a tearful confrontation, she decided to instead seal him away, and fake his death. The spellwork present in this living seal is inspired, and a tragic homage to every lesson Permafrost taught his student.")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
