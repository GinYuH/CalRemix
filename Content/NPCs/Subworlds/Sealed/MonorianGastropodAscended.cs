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
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Hostile;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Sounds;
using CalamityMod.Particles;
using CalRemix.UI;
using System.Linq;
using System.IO;
using CalamityMod.NPCs.NormalNPCs;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    [AutoloadBossHead]
    public class MonorianGastropodAscended : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public ref float SlimeTimer => ref NPC.Calamity().newAI[1];

        public Vector2 Anchor
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[3]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[3] = value.Y;
            }
        }

        public List<List<VerletSimulatedSegment>> Tentacles = new();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            if (Main.dedServ)
                return;
            HelperMessage.New("Gastropod",
                "The slug lives? And on top of that it seems invincible? Well that's inconvenient! Maybe try waiting things out and seeing if something happens?",
                "FannyIdle",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
            HelperMessage.New("Gastropod2",
                "That slime right there! Try leading the gastropod into it and its projectiles, he seems sludgephobic!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Juggular>() && n.ai[0] > 0));
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = NPC.height = 40;
            NPC.lifeMax = 60;
            NPC.damage = 200;
            NPC.defense = 26;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 8);
            NPC.boss = true;
            NPC.chaseable = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(Anchor);
            writer.Write(SlimeTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Anchor = reader.ReadVector2();
            SlimeTimer = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Tentacles.Count <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    List<VerletSimulatedSegment> tentacle = new();
                    Tentacles.Add(CalRemixHelper.CreateVerletChain(ref tentacle, 20, NPC.Center, [0]));
                }
            }
            if (Main.netMode != NetmodeID.Server && Tentacles != null)
                for (int i = 0; i < Tentacles.Count; i++)
                {
                    List<VerletSimulatedSegment> Segments = Tentacles[i];
                    Segments[0].oldPosition = Segments[0].position;
                    Segments[0].position = NPC.Center + Vector2.UnitX * MathHelper.Lerp(-20, 20, (i + 1) / (float)Tentacles.Count);

                    Tentacles[i] = VerletSimulatedSegment.SimpleSimulation(Segments, 3, loops: 5, gravity: 0.3f);

                    NPC.netUpdate = true;
                    NPC.netSpam = 0;
                }
            NPC.TargetClosest();
            if (!Target.active)
            {
                NPC.active = false;
                int jug = NPC.FindFirstNPC(ModContent.NPCType<Juggular>());
                if (jug != -1)
                {
                    Main.npc[jug].active = false;
                }
                return;
            }
            switch (State)
            {
                case 0:
                    {
                        NPC.chaseable = true;
                        CalamityUtils.SmoothMovement(NPC, 20, Main.player[NPC.target].Center - NPC.Center, 7, 0.1f, true);

                        if (SlimeTimer == 2)
                        {
                            Timer = 0;
                            State = 2;
                            NPC.netUpdate = true;
                        }
                        else if (Timer == (SlimeTimer > 2 ? 120 : 60))
                        {
                            Timer = 0;
                            State = 1;
                            NPC.netUpdate = true;
                        }
                    }
                    break;
                case 1:
                    {
                        NPC.velocity *= 0.9f;

                        if (Timer < 90 && Timer % 5 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { PitchVariance = 1 }, NPC.Center);
                            GeneralParticleHandler.SpawnParticle(new PlasmaExplosion(NPC.Center - Vector2.UnitY * 500, Vector2.Zero, Color.Cyan, Vector2.One, Main.rand.NextFloat(), 0.05f, 0.1f, 20));
                            int projCount = 8;
                            for (int i = 0; i < projCount; i++)
                            {
                                GeneralParticleHandler.SpawnParticle(new PointParticle(NPC.Center - Vector2.UnitY * 500, Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.Pi, MathHelper.Pi, i / (float)projCount)) * 30, false, 25, 1f, Color.Teal));
                                //Dust dust = Dust.NewDustPerfect(NPC.Center - Vector2.UnitY * 500, DustID.Vortex, Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.Pi, MathHelper.Pi, i / (float)projCount)) * 30, Scale: 4f);
                                //dust.noGravity = true;
                            }
                        }

                        if (Timer == 90)
                        {
                            SoundEngine.PlaySound(CommonCalamitySounds.LightningSound, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int projCount = 8;
                                for (int i = 0; i < projCount; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - Vector2.UnitY * 500, Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.Pi, MathHelper.Pi, i / (float)projCount)) * 10, ProjectileID.VortexLightning, CalRemixHelper.ProjectileDamage(120, 200), 1f, Main.myPlayer, Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.Pi, MathHelper.Pi, i / (float)projCount)).ToRotation(), Main.rand.Next(80));
                                }
                            }
                        }

                        if (Timer >= 150)
                        {
                            State = 0;
                            Timer = 0;
                            SlimeTimer++;
                            NPC.netUpdate = true;
                        }
                    }
                    break;
                case 2:
                    {
                        SlimeTimer = 22;
                        NPC.velocity *= 0.97f;
                        if (Timer == 120)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y - 1000, ModContent.NPCType<Juggular>());
                            }
                        }

                        if (Timer > 180)
                        {
                            State = 0;
                            Timer = 0;
                            NPC.netUpdate = true;
                        }
                    }
                    break;
            }
            Timer++;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            if (Main.netMode != NetmodeID.Server && Tentacles != null)
                for (int g = 0; g < Tentacles.Count; g++)
                {
                    List<VerletSimulatedSegment> Segments = Tentacles[g];
                    for (int i = 0; i < Segments.Count; i++)
                    {
                        if (i == 0)
                            continue;
                        VerletSimulatedSegment seg = Segments[i];
                        Vector2 truePos = State == 1 ? seg.position + Main.rand.NextVector2Circular(4, 4) : seg.position;
                        float dist = i > 0 ? Vector2.Distance(seg.position, Segments[i - 1].position) : 0;
                        if (dist <= 2)
                            dist = 2;
                        dist += 2;
                        float rot = 0f;
                        if (i > 0)
                            rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation();
                        else
                            rot = NPC.rotation;
                        float scalee = (1 - (i / Segments.Count)) * 0.8f;
                        Color c = i < 18 ? Color.Black : Color.Pink;
                        Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, truePos - Main.screenPosition, new Rectangle(0, 0, (int)dist, 2), c, rot, TextureAssets.BlackTile.Size() / 2, scalee, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    }
                }
            float animSpeed = State == 1 ? 50 : 7;
            Vector2 scale = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * animSpeed) * 0.022f, MathF.Cos(Main.GlobalTimeWrappedHourly * animSpeed) * 0.022f);
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale * Vector2.One + scale, 0, 0f);
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Gastrogel>(), 1, 35, 60);
            npcLoot.Add(ModContent.ItemType<MonorianGemShards>(), 1, 12, 20);
            npcLoot.Add(ModContent.ItemType<GastropodEye>());
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<Juggustar>())
            {
                return true;
            }
            return false;
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
            return attacker.type == ModContent.NPCType<Juggular>() && attacker.velocity != Vector2.Zero;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.SetMaxDamage(1);
            modifiers.DisableCrit();
            GeneralParticleHandler.SpawnParticle(new DetailedExplosion(NPC.Center, Vector2.Zero, Color.Orange * 0.8f, Vector2.One, Main.rand.NextFloat(), 0.01f, 0.4f, 20));
        }

        public override void OnKill()
        {
            int jug = NPC.FindFirstNPC(ModContent.NPCType<Juggular>());
            if (jug != -1)
            {
                Main.npc[jug].StrikeInstantKill();
            }
            RemixDowned.downedGastropod = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen = 0;
            NPC.friendlyRegen = 0;
            damage = 0;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = 60;
        }
    }
}
