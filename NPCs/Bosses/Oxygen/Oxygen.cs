using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Particles;
using CalRemix.Projectiles.Hostile;
using CalRemix.Items.Placeables;
using CalamityMod.Events;
using CalRemix.Biomes;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Materials;
using System;
using CalamityMod.Projectiles.Enemy;
using Newtonsoft.Json.Serialization;
using CalamityMod.Items.Placeables;
using System.Net.Http.Headers;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Tiles.Furniture.Monoliths;
using System.Collections.Generic;
using Terraria.Utilities;
using CalRemix.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace CalRemix.NPCs.Bosses.Oxygen
{
    //[AutoloadBossHead]
    public class Oxygen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/IonogenHit", 3);
        public static readonly SoundStyle ExplosionSound = new("CalRemix/Sounds/HydrogenExplode");

        public enum PhaseType
        {
            Sealed = 0,
            Idle = 1,
            MissileLaunch = 2,
            Mines = 3,
            Death = 4
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oxygen");
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 100;
            NPC.width = 82;
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
            NPC.DeathSound = null;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/AtomicReinforcement");
            }
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SulphurousSeaBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            //NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<HydrogenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            NPC.TargetClosest();
            float lifeRatio = NPC.life / NPC.lifeMax;
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            if (Target == null || Target.dead)
            {
                NPC.velocity.Y += 1;
                NPC.Calamity().newAI[3]++;
                if (NPC.Calamity().newAI[3] > 240)
                {
                    NPC.active = false;
                }
                return;
            }
            NPC.Calamity().newAI[3] = 0;
            switch (Phase)
            {
                case (int)PhaseType.Sealed:
                    {
                        break;
                    }
                case (int)PhaseType.Idle:
                    {
                        break;
                    }
                case (int)PhaseType.MissileLaunch:
                    {
                        break;
                    }
                case (int)PhaseType.Mines:
                    {
                        break;
                    }
                case (int)PhaseType.Death:
                    {
                        break;
                    }

            }
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("This machination of Yharim's artillery proved to be a challenge to fuse both magic and science cohesively. While Ivy's soul comfortably sits outside the construct, Hydrogen's power resonates with her mana; down to its devastating explosion.")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0 && Phase != (int)PhaseType.Death)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(HitSound, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<SeaPrism>(), 1, 4, 8);
        }
        public override void OnKill()
        {
            RemixDowned.downedHydrogen = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedHydrogen = true;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }

        public override bool CheckDead()
        {
            NPC.life = 1;
            NPC.Calamity().newAI[0] = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;

            NPC.netUpdate = true;

            // Prevent netUpdate from being blocked by the spam counter.
            if (NPC.netSpam >= 10)
                NPC.netSpam = 9;
            return false;
        }

        public override bool CheckActive()
        {
            return NPC.Calamity().newAI[0] != 1;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (Phase == (int)PhaseType.Sealed || Phase == (int)PhaseType.Death)
                return false;
            return null;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if ((Phase == (int)PhaseType.Sealed && !ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[projectile.type]) || Phase == (int)PhaseType.Death)
                return false;
            return null;
        }
    }
}
