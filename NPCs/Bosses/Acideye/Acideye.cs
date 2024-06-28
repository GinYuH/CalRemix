using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using System.IO;
using Microsoft.Xna.Framework;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.World;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Audio;
using CalamityMod.Sounds;
using CalRemix.Projectiles.Hostile;
using CalamityMod.Particles;
using CalRemix.Items.Weapons;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Items.Accessories;
using CalRemix.Items.Placeables.Relics;
using CalRemix.World;
using CalRemix.Items.Bags;

namespace CalRemix.NPCs.Bosses.Acideye
{
    [AutoloadBossHead]
    public class Acideye : ModNPC
    {
        private Player Target => Main.player[NPC.target];
        public ref float Phase => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float Attack => ref NPC.ai[2];
        public ref float Step => ref NPC.ai[3];
        public float Subphase = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Subphase);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Subphase = reader.ReadSingle();
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 9;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<PearlAura>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Irradiated>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<SulphuricPoisoning>()] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new()
            {
                Scale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            value.Frame = 1;
        }
        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.width = 110;
            NPC.height = 110;
            NPC.lifeMax = 4000;
            NPC.defense = 10;
            NPC.damage = 25;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 20);
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().canBreakPlayerDefense = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SulphurousSeaBiome>().Type };
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/Opticatalysis");
        }
        public override void AI()
        {
            NPC.TargetClosest();
            float life = (float)NPC.life / (float)NPC.lifeMax;
            bool p2 = CalamityWorld.death ? life <= 0.65f : (CalamityWorld.revenge ? life <= 0.60f : (Main.expertMode ? life <= 0.55f : life <= 0.5f));
            bool p3 = CalamityWorld.death ? life <= 0.5f : (CalamityWorld.revenge ? life <= 0.45f : (Main.expertMode ? life <= 0.4f : life <= 0.3f));
            if (!NPC.HasValidTarget)
            {
                NPC.velocity.Y--;
                return;
            }
            Timer++;
            switch (Phase)
            {
                // Phase 1
                case 0:
                    if (p2)
                    {
                        Phase = 1;
                        Timer = 0;
                        Attack = 0;
                        Step = 0;
                        NPC.netUpdate = true;
                    }
                    if (Attack == 0)
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation() - MathHelper.PiOver2;
                        switch (Subphase)
                        {
                            case 0:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center - Vector2.UnitY * 320f) * 7.5f / 1.5f) / 44f;
                                if (Timer >= 100f)
                                {
                                    if (Step < 4)
                                    {
                                        NPC eye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MutatedEye>());
                                        eye.velocity = NPC.DirectionTo(Target.Center) * 6f;
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                            case 1:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center - Vector2.UnitY * 320f) * 7.5f / 1.5f) / 44f;
                                if (Timer >= 60f)
                                {
                                    if (Step < 6)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item17, NPC.Center);
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidBomb>(), 15 / 2, 0, ai0: 6);
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                            case 2:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center) * 4.5f / 1.5f) / 44f;
                                if (Timer >= 420f)
                                {
                                    Attack = 1;
                                    Timer = 0;
                                    Step = 0;
                                    NPC.netUpdate = true;
                                }
                                break;
                        }
                    }
                    else if (Attack == 1)
                    {
                        if (Step <= 0)
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 12f;
                            Step = 1;
                        }
                        if (Timer >= 25f)
                        {
                            NPC.velocity.X *= 0.98f;
                            NPC.velocity.Y *= 0.98f;
                            if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                                NPC.velocity.X = 0f;
                            if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                                NPC.velocity.Y = 0f;
                        }
                        if (Timer < 35f)
                            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                        else
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.AngleTo(Target.Center) - MathHelper.PiOver2, 0.5f);
                        if (Timer >= 70f)
                        {
                            if (Step < 3f)
                            {
                                NPC.velocity = NPC.DirectionTo(Target.Center) * 12f;
                                Step++;
                                Timer = 0f;
                            }
                            else if (Step >= 3f)
                            {
                                SwitchSubphase();
                                Timer = 0;
                                Attack = 0;
                                Step = 0;
                                NPC.netUpdate = true;
                            }
                        }
                    }
                    break;
                // Transition 1-2
                case 1:
                    NPC.velocity *= 0.5f;
                    NPC.rotation += MathHelper.ToRadians(24);
                    if (Timer > 180)
                    {
                        if (Main.netMode != NetmodeID.Server)
                            Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("Eye").Type);
                        SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        Phase = 2;
                        Timer = 0;
                        NPC.netUpdate = true;
                    }
                    break;
                // Phase 2
                case 2:
                    if (p3)
                    {
                        Phase = 3;
                        Timer = 0;
                        Attack = 0;
                        Step = 0;
                        NPC.netUpdate = true;
                    }
                    if (Attack == 0)
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation() - MathHelper.PiOver2;
                        switch (Subphase)
                        {
                            case 0:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center - Vector2.UnitY * 240f) * 8f / 1.5f) / 44f;
                                if (Timer >= 60f)
                                {
                                    if (Step < 6)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item17, NPC.Center);
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidBomb>(), 15 / 2, 0, ai0: 6);
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                            case 1:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center - Vector2.UnitX * 240f *(Target.Center.X > NPC.Center.X ? 1 : -1)) * 7.5f / 1.5f) / 44f;
                                if (Timer >= 60f)
                                {
                                    if (Step < 6)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item17, NPC.Center);
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidBomb>(), 15 / 2, 0, ai0: 6);
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                            case 2:
                                NPC.velocity = (NPC.velocity * 63f + NPC.DirectionTo(Target.Center) * 5.5f / 1.5f) / 64f;
                                if (Timer >= 100f)
                                {
                                    if (Step < 3)
                                    {
                                        NPC eye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MutatedEye>());
                                        eye.velocity = NPC.DirectionTo(Target.Center) * 6f;
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                        }
                    }
                    else if (Attack == 1)
                    {
                        if (Step <= 0)
                        {
                            for (int a = 0; a < 4; a++)
                                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (-Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(45f)) * 6f, ModContent.ProjectileType<AcidShot>(), 30 / 2, 0);
                            SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 18f;
                            Step = 1;
                        }
                        if (Timer >= 25f)
                        {
                            NPC.velocity.X *= 0.96f;
                            NPC.velocity.Y *= 0.96f;
                            if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                                NPC.velocity.X = 0f;
                            if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                                NPC.velocity.Y = 0f;
                        }
                        if (Timer < 35f)
                            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                        else
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.AngleTo(Target.Center) - MathHelper.PiOver2, 0.5f);
                        if (Timer % 25 == 0 && Main.netMode != NetmodeID.Server)
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcidBomb>(), 15 / 2, 0, ai0: 2);
                        if (Timer >= 70f)
                        {
                            if (Step < 3f)
                            {
                                for (int a = 0; a < 4; a++)
                                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (-Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(45f)) * 6f, ModContent.ProjectileType<AcidShot>(), 30 / 2, 0);
                                SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
                                NPC.velocity = NPC.DirectionTo(Target.Center) * 18f;
                                Step++;
                                Timer = 0f;
                            }
                            else if (Step >= 3f)
                            {
                                SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                                if (Main.netMode != NetmodeID.Server)
                                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidTeeth>(), 30 / 2, 0);
                                SwitchSubphase();
                                Timer = 0;
                                Attack = 0;
                                Step = 0;
                                NPC.netUpdate = true;
                            }
                        }
                    }
                    break;
                // Transition 2-3
                case 3:
                    NPC.velocity *= 0.5f;
                    NPC.rotation += MathHelper.ToRadians(24);
                    if (Timer > 180)
                    {
                        Particle wave = new PulseRing(NPC.Center, Vector2.Zero, Color.GreenYellow, 0.2f, 1.2f, 20);
                        GeneralParticleHandler.SpawnParticle(wave);
                        SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
                        SoundEngine.PlaySound(CommonCalamitySounds.ExoPlasmaExplosionSound, NPC.Center);
                        Phase = 4;
                        Timer = 0;
                        NPC.netUpdate = true;
                    }
                    break;
                // Phase 3
                case 4:
                    if (Attack == 0)
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation() - MathHelper.PiOver2;
                        switch (Subphase)
                        {
                            case 0:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center - Vector2.UnitY * 240f) * 8f / 1.5f) / 44f;
                                if (Timer >= 45f)
                                {
                                    if (Step < 8)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item17, NPC.Center);
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidBomb>(), 15 / 2, 0, ai0: 6);
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                            case 1:
                                NPC.velocity = (NPC.velocity * 43f + NPC.DirectionTo(Target.Center - Vector2.UnitX * 240f * (Target.Center.X > NPC.Center.X ? 1 : -1)) * 7.5f / 1.5f) / 44f;
                                if (Timer >= 90f)
                                {
                                    if (Step < 3)
                                    {
                                        SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                                        if (Main.netMode != NetmodeID.Server)
                                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidTeeth>(), 30 / 2, 0);
                                        Step++;
                                    }
                                    else
                                    {
                                        SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                                        if (Main.netMode != NetmodeID.Server)
                                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidTeeth>(), 30 / 2, 0);
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                            case 2:
                                NPC.velocity = (NPC.velocity * 83f + NPC.DirectionTo(Target.Center) * 7.5f / 1.5f) / 84f;
                                if (Timer >= 60f)
                                {
                                    if (Step < 3)
                                    {
                                        NPC eye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MutatedEye>());
                                        eye.velocity = NPC.DirectionTo(Target.Center) * 6f;
                                        Step++;
                                    }
                                    else
                                    {
                                        Attack = 1;
                                        Step = 0;
                                        NPC.netUpdate = true;
                                    }
                                    Timer = 0;
                                }
                                break;
                        }
                    }
                    else if (Attack == 1)
                    {
                        if (Step <= 0)
                        {
                            for (int a = 0; a < 6; a++)
                                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (-Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(135f)) * 6f, ModContent.ProjectileType<AcidShot>(), 30 / 2, 0);
                            SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 18f;
                            Step = 1;
                        }
                        if (Timer >= 25f)
                        {
                            if (Step < 3)
                            {
                                NPC.velocity.X *= 0.96f;
                                NPC.velocity.Y *= 0.96f;
                                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                                    NPC.velocity.X = 0f;
                                if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                                    NPC.velocity.Y = 0f;
                            }
                            else
                            {
                                NPC.velocity.X *= 0.9f;
                                NPC.velocity.Y *= 0.9f;
                                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                                    NPC.velocity.X = 0f;
                                if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                                    NPC.velocity.Y = 0f;
                            }
                        }
                        if (Step > 3 && Timer % 10 == 0 && Main.netMode != NetmodeID.Server)
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcidBomb>(), 15 / 2, 0, ai0: 4);
                        if (Timer < 35f)
                            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                        else
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.AngleTo(Target.Center) - MathHelper.PiOver2, 0.5f);
                        if (Timer >= 35f && Step >= 4)
                        {
                            if (Step < 7)
                            {
                                SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
                                NPC.velocity = NPC.DirectionTo(Target.Center) * 24f;
                            }
                            else
                            {
                                SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                                if (Main.netMode != NetmodeID.Server)
                                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 6f, ModContent.ProjectileType<AcidTeeth>(), 30 / 2, 0);
                                SwitchSubphase();
                                Timer = 0;
                                Attack = 0;
                                Step = 0;
                                NPC.netUpdate = true;
                            }
                            Step++;
                            Timer = 0f;
                        }
                        else if (Timer >= 70f && Step < 4)
                        {
                            if (Step < 7)
                            {
                                if (Step == 3)
                                {
                                    SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
                                    NPC.velocity = NPC.DirectionTo(Target.Center) * 24f;
                                }
                                else
                                {
                                    for (int a = 0; a < 6; a++)
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (-Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(135f)) * 6f, ModContent.ProjectileType<AcidShot>(), 30 / 2, 0);
                                    SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
                                    NPC.velocity = NPC.DirectionTo(Target.Center) * 18f;
                                }
                            }
                            Step++;
                            Timer = 0f;
                        }
                    }
                    break;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Attack == 1 || Phase == 4)
                target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
            if (Attack == 1 && Phase == 4)
                target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 120);
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Attack == 1 && Timer < 35f)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulphurousSeaAcid, Scale: 1f + (Main.rand.NextFloat() / 2f));
                    dust.noGravity = true;
                    dust.velocity *= 5f;
                }
            }
            if (Phase == 3 && Timer == 180)
            {
                for (int k = 0; k < 22; k++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulphurousSeaAcid, Scale: 4f + (Main.rand.NextFloat() * 2f));
                    dust.noGravity = true;
                    dust.velocity = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                }
            }
            if (Phase == 1 || Phase == 3 || Phase == 4 && Attack != 1)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulphurousSeaAcid, Scale: 1f + (Main.rand.NextFloat() / 2f));
                    dust.noGravity = true;
                    dust.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                }
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
                return;
            for (int k = 0; k < 22; k++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulphurousSeaAcid, Scale: 4f + (Main.rand.NextFloat() * 2f));
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                if (Phase < 2)
                {
                    if (NPC.frame.Y > frameHeight * 1 || NPC.frame.Y < 0)
                        NPC.frame.Y = 0;
                    else
                        NPC.frame.Y += frameHeight;
                }
                else if (Phase < 4 && Phase >= 2)
                {
                    if (NPC.frame.Y > frameHeight * 4 || NPC.frame.Y < frameHeight * 3)
                        NPC.frame.Y = frameHeight * 3;
                    else
                        NPC.frame.Y += frameHeight;
                }
                else
                {
                    if (NPC.frame.Y > frameHeight * 7 || NPC.frame.Y < frameHeight * 6)
                        NPC.frame.Y = frameHeight * 6;
                    else
                        NPC.frame.Y += frameHeight;
                }
                NPC.frameCounter = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("A large eye mutated by the Sulphurous Sea. Defeating this may allow its prey to come out of hiding.")
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle rect = new((Attack == 1 && Phase > 1 || Phase == 4 && Attack == 0 && Subphase == 1) ? texture.Width / 2 : 0, NPC.frame.Y, texture.Width / 2, texture.Height / Main.npcFrameCount[Type]);
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), rect, (Phase == 4) ? new Color(255, 255, 255, 255) : drawColor, NPC.rotation, rect.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Phase < 4)
            {
                Texture2D texture = ModContent.Request<Texture2D>($"{nameof(CalRemix)}/NPCs/Bosses/Acideye/Acideye_Glow", AssetRequestMode.ImmediateLoad).Value;
                Rectangle rect = new((Attack == 1 && Phase > 1 || Phase == 4 && Attack == 0 && Subphase == 1) ? texture.Width / 2 : 0, NPC.frame.Y, texture.Width / 2, texture.Height / Main.npcFrameCount[Type]);
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), rect, new Color(255, 255, 255, 255), NPC.rotation, rect.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AcideyeBag>()));
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();
            int[] itemIDs =
            {
                ModContent.ItemType<PungentBomber>(),
                ModContent.ItemType<AcidBow>(),
                ModContent.ItemType<CausticClaw>(),
                ModContent.ItemType<CorrosiveEyeStaff>(),
                ModContent.ItemType<RazorTeeth>()
            };
            mainRule.Add(ModContent.ItemType<DeterioratingLens>());
            mainRule.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, itemIDs));
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<AcidsighterRelic>());
        }
        public override void OnKill()
        {
            RemixDowned.downedAcidsighter = true;
            CalRemixWorld.UpdateWorldBool();
        }
        private void SwitchSubphase()
        {
            Subphase++;
            if (Subphase > 2)
                Subphase = 0;
        }
    }
}
