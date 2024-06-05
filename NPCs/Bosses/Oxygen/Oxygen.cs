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
using CalRemix.Items.Materials;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using CalRemix.UI;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CalRemix.NPCs.Bosses.Oxygen
{
    [AutoloadBossHead]
    public class Oxygen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float DepthLevel => ref NPC.Calamity().newAI[0];
        public ref float MaxDepthLevel => ref NPC.Calamity().newAI[2];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/IonogenHit", 3);
        public static readonly SoundStyle ExplosionSound = new("CalRemix/Sounds/HydrogenExplode");

        public enum PhaseType
        {
            Idle = 0,
            Gusts = 1,
            Fling = 2,
            Bubbles = 3,
            Whirlpool = 4
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oxygen");
            Main.npcFrameCount[Type] = 4;
            if (Main.dedServ)
                return;
            FannyMessage f = new FannyMessage("Oxygen",
                "Oxygen is quite the fickle foe. Many have died foolishly trying to take a crack at it on the surface. However, that glass shell doesn't seem to be built for pressure. Try leading it to the bottom of the Abyss!",
                "Idle",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
            FannyManager.LoadFannyMessage(f);
            FannyMessage e = new FannyMessage("OxygenEvil",
                "Ok look, Fanny may be an imbecile, but if you're gonna take any words of his to heart it should be these. Leading it to the abyss is the only way you're defeating this idiotic ball.",
                "EvilIdle",
                FannyMessage.AlwaysShow).SpokenByEvilFanny().NeedsActivation(2f);
            f.AddEndEvent(e.ActivateMessage);
            FannyManager.LoadFannyMessage(e);
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 100;
            NPC.width = 82;
            NPC.height = 88;
            NPC.defense = 99999999;
            NPC.Calamity().DR = 0.99999f;
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
            NPC.DeathSound = CalamityMod.NPCs.Providence.Providence.HurtSound;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/AtomicReinforcement");
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
            writer.Write(NPC.Calamity().newAI[2]);
            writer.Write(NPC.Calamity().newAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[3] = reader.ReadSingle();
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<OxygenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
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
            NPC.Calamity().newAI[1]--;
            foreach (Player p in Main.player)
            {
                if (p == null)
                    continue;
                if (!p.active)
                    continue;
                if (p.dead)
                    continue;
                p.Calamity().infiniteFlight = true;
                if (p.Distance(NPC.Center) < 320)
                {
                    p.breath += (int)MathHelper.Clamp(p.breathMax / 120 * (1 + MaxDepthLevel), -0.001f, p.breathMax - p.breath);
                }
            }
            if (Target.Calamity().ZoneAbyssLayer4 || BossRushEvent.BossRushActive)
            {
                NPC.defense = 0;
                NPC.Calamity().DR = 0;
                if (MaxDepthLevel == 3)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.2f);
                    NPC.HitEffect(dmg: (int)(NPC.lifeMax * 0.2f));
                    Shard(18, true);
                }
                DepthLevel = 4;
                if (MaxDepthLevel < 4)
                MaxDepthLevel = 4;
            }
            else if (Target.Calamity().ZoneAbyssLayer3)
            {
                if (MaxDepthLevel == 2)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.15f);
                    NPC.HitEffect(dmg : (int)(NPC.lifeMax * 0.15f));
                    Shard(12, true);
                }
                DepthLevel = 3;
                if (MaxDepthLevel < 3)
                    MaxDepthLevel = 3;
            }
            else if (Target.Calamity().ZoneAbyssLayer2)
            {
                if (MaxDepthLevel == 1)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.1f);
                    NPC.HitEffect(dmg: (int)(NPC.lifeMax * 0.1f));
                    Shard(8, true);
                }
                DepthLevel = 2;
                if (MaxDepthLevel < 2)
                    MaxDepthLevel = 2;
            }
            else if (Target.Calamity().ZoneAbyssLayer1)
            {
                if (MaxDepthLevel == 0)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.05f);
                    NPC.HitEffect(dmg: (int)(NPC.lifeMax * 0.05f));
                    Shard(4, true);
                }
                DepthLevel = 1;
                if (MaxDepthLevel < 1)
                    MaxDepthLevel = 1;
            }
            else
            {
                DepthLevel = 0;
            }
            switch (Phase)
            {
                case (int)PhaseType.Idle:
                    {
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 5;
                        int phaseTime = 180;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > phaseTime)
                        {
                            Phase = DepthLevel > 1 && Main.rand.NextBool(5 - (int)DepthLevel) ? (int)PhaseType.Bubbles : (int)PhaseType.Gusts;
                            NPC.ai[1] = 0;
                        }
                        break;
                    }
                case (int)PhaseType.Gusts:
                    {
                        int spawnClouds = 90;
                        int phaseTime = spawnClouds + 240;
                        float speedMax = 2;
                        float acc = 0.2f;
                        NPC.ai[1]++;
                        if (NPC.ai[1] == spawnClouds)
                        {
                            int cloudAmt = 12;
                            int cloudSpacing = rev ? 480 : 400;
                            int cloudDist = 1200;
                            int cloudStart = 800 + Main.rand.Next(0, 64);
                            float cloudSpeed = death ? 16 : rev ? 12 : 10;
                            SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack, NPC.Center);
                            for (int i = 0; i < cloudAmt / 2; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(Target.Center.X + cloudDist, Target.Center.Y - cloudStart + i * cloudSpacing), new Vector2(-cloudSpeed, 0), ModContent.ProjectileType<OxygenCloud>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Main.rand.Next(0, TextureAssets.Cloud.Length - 1));
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(Target.Center.X - cloudDist, Target.Center.Y - cloudStart + i * cloudSpacing + cloudSpacing / 3), new Vector2(cloudSpeed, 0), ModContent.ProjectileType<OxygenCloud>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Main.rand.Next(0, TextureAssets.Cloud.Length - 1));
                            }
                        }
                        if (NPC.ai[1] > spawnClouds)
                        {
                            NPC.velocity.X *= 0.95f;
                            NPC.velocity.Y -= acc;
                            if (NPC.velocity.Y < -speedMax)
                                NPC.velocity.Y = -speedMax;
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            Phase = DepthLevel > 2 && Main.rand.NextBool(6 - (int)DepthLevel) ? (int)PhaseType.Bubbles : (int)PhaseType.Fling;
                            NPC.ai[1] = 0;
                        }
                        break;
                    }
                case (int)PhaseType.Fling:
                    {
                        NPC.ai[1]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 5;
                        int flingStrength = 30;
                        int flingRate = death ? 50 : rev ? 60 : expert ? 70 : 80;
                        int totalFlings = 3;
                        if (NPC.ai[1] % flingRate == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item43, NPC.Center);
                            Target.velocity += Main.rand.NextVector2Circular(flingStrength, flingStrength);
                            NPC.ai[2]++;
                            DustExplosion(Target.velocity.SafeNormalize(Vector2.Zero));
                        }
                        if (NPC.ai[2] >= totalFlings + (totalFlings - 1))
                        {
                            NPC.ai[2] = 0;
                            NPC.ai[1] = 0;
                            Phase = DepthLevel > 2 && Main.rand.NextBool(5 - (int)DepthLevel) ? (int)PhaseType.Whirlpool : (int)PhaseType.Idle;
                        }

                        break;
                    }
                case (int)PhaseType.Bubbles:
                    {
                        int spawnBubbles = 60;
                        int bubbleAmt = 64;
                        int bubbleRangeX = 1000;
                        int bubbleRangeY = 500;
                        int phaseTime = spawnBubbles + 240;
                        int dash = spawnBubbles + 120;
                        NPC.ai[1]++;
                        if (NPC.ai[1] == spawnBubbles)
                        {
                            SoundEngine.PlaySound(SoundID.NPCDeath45, Target.Center);
                            for (int i = 0; i < bubbleAmt; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.Next(-bubbleRangeX, bubbleRangeX), Main.rand.Next(-bubbleRangeY, bubbleRangeY)), Vector2.Zero, ModContent.ProjectileType<OxygenBubble>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer);
                            }
                        }
                        if (NPC.ai[1] < dash)
                        {
                            NPC.velocity *= 0.97f;
                            NPC.rotation += 0.6f;
                        }
                        else if (NPC.ai[1] == dash)
                        {
                            SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                            NPC.rotation = NPC.velocity.ToRotation();
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 20;
                            if (master)
                                NPC.velocity += Target.velocity;
                        }
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation();
                            NPC.velocity *= 0.99f;
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.rotation = 0;
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Fling;
                        }
                        break;
                    }
                case (int)PhaseType.Whirlpool:
                    {
                        NPC.velocity *= 0.97f;
                        int spawnTornado = 60;
                        int totalObjects = death ? 16 : rev ? 10 : 8;
                        int tornadoRate = rev ? 60 : expert ? 80 :90;
                        int totalTornados = death ? 10 : rev ? 8 : 5;
                        int phaseTime = tornadoRate * totalTornados + spawnTornado;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > spawnTornado && NPC.ai[1] % tornadoRate == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                            int dir = Main.rand.NextBool().ToInt();
                            for (int i = 0; i < totalObjects; i++)
                            {
                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OxygenDebris>(), (int)(NPC.damage * 0.25f), 0, Main.myPlayer, i + 1, totalObjects, Main.rand.NextFloat(0, 4f));
                                Main.projectile[p].localAI[0] = Main.rand.Next(1, 5);
                                Main.projectile[p].localAI[1] = dir;
                            }
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Gusts;
                        }
                        break;
                    }

            }
        }

        public void DustExplosion(Vector2 extraSpeed)
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18) * extraSpeed;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("In a magical blunder, this construct found itself a resident of the elemental family as an unwilling eye of her storm. While Tempest carried a plethora of cargo on Yharim's behalf, she slipped into her airstream which sparked a feedback loop outside her control."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
            });
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (DepthLevel < 4)
            {
                modifiers.SetMaxDamage(1);
                Shard();
                NPC.life += 1;
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (DepthLevel < 4)
            {
                modifiers.SetMaxDamage(1);
                Shard();
                NPC.life += 1;
            }
        }

        public void Shard(int amt = 10, bool ignoreCooldown = false)
        {
            if (NPC.Calamity().newAI[1] <= 0 || ignoreCooldown)
            {
                int shardSpeed = Main.rand.Next(7, 11);
                for (int i = 0; i < amt; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.2f, Pitch = 0.4f }, NPC.Center);
                    Vector2 square = new Vector2(Main.rand.Next((int)NPC.position.X, (int)NPC.position.X + NPC.width), Main.rand.Next((int)NPC.position.Y, (int)NPC.position.Y + NPC.height));
                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), square, new Vector2(Main.rand.Next(-shardSpeed, shardSpeed), Main.rand.Next(-shardSpeed, shardSpeed)), ModContent.ProjectileType<Oxshard>(), (int)(NPC.damage * 0.5f), 0f, Main.myPlayer, ai1: Main.rand.Next(1, 7));
                    Main.projectile[p].scale = Main.rand.NextFloat(1f, 2f);
                }
                NPC.Calamity().newAI[1] = 40;
            }
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundStyle hite = MaxDepthLevel == 4 ? SoundID.NPCHit3 : HitSound;
                SoundEngine.PlaySound(hite, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
                }
                for (int i = 0; i < 20; i++)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(NPC.width, NPC.height).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("OxygenShrap" + Main.rand.Next(1, 7)).Type);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(NPC.width, NPC.height).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("Oxygen" + Main.rand.Next(1, 7)).Type);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<EssenceofBabil>(), 1, 4, 8);
        }
        public override void OnKill()
        {
            RemixDowned.downedOxygen = true;
            CalRemixWorld.oxydayTime = 0;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedOxygen = true;
            CalRemixWorld.oxydayTime = 0;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawPos = NPC.Center - screenPos;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Texture2D plasmom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomRing").Value;
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
            float scaleFactor = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 0.075f;
            spriteBatch.Draw(plasmom, drawPos, null, NPC.GetAlpha(Color.Cyan * 0.78f), NPC.rotation, plasmom.Size() / 2, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bloom, drawPos, null, NPC.GetAlpha(Color.Cyan * 1f), NPC.rotation, bloom.Size() / 2, 0.6f + scaleFactor, SpriteEffects.None, 0f);
            spriteBatch.End();
            spriteBatch.Begin();
            if (MaxDepthLevel < 4)
            {
                spriteBatch.Draw(TextureAssets.Npc[Type].Value, drawPos, TextureAssets.Npc[Type].Frame(1, 4, 0, (int)MathHelper.Clamp(MaxDepthLevel, 0, 3)), NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(TextureAssets.Npc[Type].Value.Width / 2, TextureAssets.Npc[Type].Value.Height / 8), NPC.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
