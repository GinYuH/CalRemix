using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using System.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.World;
using CalamityMod.BiomeManagers;
using CalamityMod.Projectiles.Boss;
using CalamityMod.NPCs.SupremeCalamitas;
using Terraria.DataStructures;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.NPCs
{
    [AutoloadBossHead]
    public class Clamitas : ModNPC
    {
        private static readonly SoundStyle SlamSound = new SoundStyle("CalamityMod/Sounds/Item/ClamImpact");
        private int hitAmount;
        private int attack, attack2 = -1;
        private bool hit;
        private bool hide;
        private Player Target => Main.player[NPC.target];
        public ref float Wait => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float Wait2 => ref NPC.ai[2];
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Slow] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<PearlAura>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<BrimstoneFlames>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<VulnerabilityHex>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<WeakBrimstoneFlames>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<RancorBurn>()] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new();
            nPCBestiaryDrawModifiers.Scale = 0.4f;
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            value.Frame = 1;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.width = 176;
            NPC.height = 176;
            NPC.lifeMax = Main.hardMode ? 30000 : 5000;
            NPC.defense = Main.hardMode ? 40 : 15;
            NPC.damage = Main.hardMode ? 120 : 70;
            if (CalamityWorld.death)
                NPC.damage = Main.hardMode ? 316 : 168;
            else if (CalamityWorld.revenge)
                NPC.damage = Main.hardMode ? 288 : 184;
            NPC.DR_NERD(0.35f);
            NPC.knockBackResist = 0f;
            NPC.value = Main.hardMode ? Item.buyPrice(gold: 20) : Item.buyPrice(gold: 5);
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.rarity = 4;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BrimstoneCragsBiome>().Type };
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hitAmount);
            writer.Write(attack);
            writer.Write(attack2);
            writer.Write(hit);
            writer.Write(hide);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hitAmount = reader.ReadInt32();
            attack = reader.ReadInt32();
            attack2 = reader.ReadInt32();
            hit = reader.ReadBoolean();
            hide = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (NPC.justHit && hitAmount < 5)
            {
                hitAmount++;
                hit = true;
            }
            NPC.chaseable = hit;
            if (hitAmount != 5)
                return;
            if (!hide)
                Lighting.AddLight(NPC.Center, (255 - NPC.alpha) / (255f * 5f), 0f, 0f);
            if (Main.netMode != NetmodeID.Server && !Main.player[NPC.target].dead && Main.player[NPC.target].active)
                player.AddBuff(ModContent.BuffType<Clamity>(), 2);

            if (Wait < 240f)
            {
                Wait += 1f;
                hide = false;
            }
            else if (attack == -1)
            {
                attack = Main.rand.Next(2);
                if (attack == 0)
                {
                    attack = Main.rand.Next(2);
                }
            }
            else if (attack == 0)
            {
                hide = true;
                NPC.defense = 9999;
                Timer += 1f;
                if (Timer <= 60f && Main.netMode != NetmodeID.Server)
                {
                    float vel = Target.Center.X > NPC.Center.X ? 1f : -1f;
                    for (int i = 0; i < 7; i++)
                    {
                        Vector2 pos = Target.Center + new Vector2(vel * 1280f, 480 - 160 * i);
                        Dusting(pos, pos + new Vector2(20, 20), 40, 40);
                    }
                }
                if (Timer == 60f && Main.netMode != NetmodeID.Server)
                {
                    float vel = Target.Center.X > NPC.Center.X ? 1f : -1f;
                    for (int i = 0; i < 7; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), Target.Center + new Vector2(vel * 1280f, 480 - 160 * i), new Vector2(-vel * 6f, 0), ModContent.ProjectileType<BrimstoneHellblast2>(), GetDamage(), 0, ai0: 1);
                        proj.timeLeft = 600;
                    }
                }
                if (Timer >= 120f)
                {
                    Wait = 0f;
                    Timer = 0f;
                    hide = false;
                    attack = -1;
                    NPC.defense = Main.hardMode ? 40 : 15;
                    NPC.netUpdate = true;
                }
            }
            else if (attack == 1)
            {
                switch (Timer)
                {
                    case 0:
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.TargetClosest();
                            Timer = 1f;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 1:
                        NPC.damage = 0;
                        NPC.chaseable = false;
                        NPC.dontTakeDamage = true;
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        NPC.alpha += Main.hardMode ? 8 : 5;
                        if (NPC.alpha >= 255)
                        {
                            NPC.alpha = 255;
                            NPC.position.X = player.Center.X - (NPC.width / 2);
                            NPC.position.Y = player.Center.Y - (NPC.height / 2) + player.gfxOffY - 200f;
                            NPC.position.X = NPC.position.X - 15f;
                            NPC.position.Y = NPC.position.Y - 100f;
                            Timer = 2f;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 2:
                        if (Main.rand.NextBool(2))
                            Dusting(NPC.position, NPC.Center, NPC.width, NPC.height);
                        NPC.alpha -= Main.hardMode ? 7 : 4;
                        if (NPC.alpha <= 0)
                        {
                            NPC.damage = Main.hardMode ? 120 : 70;
                            if (CalamityWorld.death)
                                NPC.damage = Main.hardMode ? 316 : 168;
                            else if (CalamityWorld.revenge)
                                NPC.damage = Main.hardMode ? 288 : 184;

                            NPC.chaseable = true;
                            NPC.dontTakeDamage = false;
                            NPC.alpha = 0;
                            Timer = 3f;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 3:
                        NPC.velocity.Y += 0.8f;
                        if (NPC.Center.Y > player.Center.Y - NPC.Center.Y + player.gfxOffY - 15f)
                        {
                            NPC.noTileCollide = false;
                            Timer = 4f;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 4:
                        if (NPC.velocity.Y == 0f)
                        {
                            Timer = 0f;
                            Wait = 0f;
                            NPC.netUpdate = true;
                            NPC.noGravity = false;
                            attack = -1;
                            SoundEngine.PlaySound(SlamSound, (Vector2?)NPC.position);
                            if (Main.netMode != NetmodeID.Server)
                            {
                                for (int i = (int)NPC.position.X - 30; i < (int)NPC.position.X + NPC.width + 60; i += 30)
                                {
                                    for (int j = 0; j < 5; j++)
                                    {
                                        Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X - 30f, NPC.position.Y + NPC.height), NPC.width + 30, 4, DustID.LifeDrain, 0f, 0f, 100, default, 1.5f);
                                        dust.velocity *= 0.2f;
                                    }

                                    Gore gore = Gore.NewGoreDirect(NPC.GetSource_FromAI(), new Vector2(i - 30, NPC.position.Y + NPC.height - 12f) * 0.4f, default, Main.rand.Next(61, 64));
                                    gore.velocity *= 0.4f;
                                    SoundEngine.PlaySound(SupremeCalamitas.BrimstoneShotSound, (Vector2?)NPC.position);
                                    for (int j = 0; j < GetDarts(); j++)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center).RotatedBy(MathHelper.ToRadians(360 / GetDarts() * j)) * 10f, ModContent.ProjectileType<BrimstoneBarrage>(), GetDamage(), 0, ai0: 1);
                                    }
                                }
                            }
                        }
                        NPC.velocity.Y += 0.8f;
                        break;
                }
            }
            if (!Main.hardMode)
                return;
            if (Wait2 < 180f)
            {
                Wait2 += 1f;
            }
            else
            {
                attack2 = Main.rand.Next(2);
                Wait2 = 0;
            }
            if (attack2 == 0)
            {
                SoundEngine.PlaySound(SupremeCalamitas.BrimstoneBigShotSound, (Vector2?)NPC.position);
                if (Main.netMode != NetmodeID.Server)
                {
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -1280f), new Vector2(0, 6), ModContent.ProjectileType<SCalBrimstoneGigablast>(), GetDamage(), 0, ai0: 1);
                    proj.timeLeft = 120;
                }
                attack2 = -1;
            }
            else if (attack2 == 1)
            {
                SoundEngine.PlaySound(SupremeCalamitas.BrimstoneShotSound, (Vector2?)NPC.position);
                if (Main.netMode != NetmodeID.Server)
                {
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(-6, 0), ModContent.ProjectileType<SCalBrimstoneFireblast>(), GetDamage(), 0, ai0: 1);
                    Projectile proj2 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(6, 0), ModContent.ProjectileType<SCalBrimstoneFireblast>(), GetDamage(), 0, ai0: 1);
                    proj.timeLeft = 120;
                    proj2.timeLeft = 120;
                }
                attack2 = -1;
            }
            NPC.localAI[0] += 0.5f;
            if (NPC.localAI[0] > 3)
            {
                NPC.localAI[0] = 0;
                NPC.localAI[1] = NPC.localAI[1] == 0 ? 1 : 0;
            }

        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
                return;
            for (int k = 0; k < 20; k++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 2f);
                dust.velocity *= 2f;
            }
        }
        public override bool CheckActive()
        {
            return Vector2.Distance(Target.Center, NPC.Center) > 5600f;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return hit;
            }
            return null;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Adaption at its finest. This clam was tranformed by the harsh air of Azafure after escaping the destruction of its home. Its meat is said to have the spiciest taste among all seafoods.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneCalamity && !NPC.AnyNPCs(ModContent.NPCType<Clamitas>()) && CalRemixWorld.clamitas)
            {
                return 0.01f;
            }
            return 0f;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = hitAmount >= 5 && attack != 0 ? 1 : 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle rect = new Rectangle(0, NPC.frame.Y * texture.Height / 2, texture.Width, texture.Height / 2);
            Vector2 origin = rect.Size() / 2f;
            Vector2 draw = NPC.Center - new Vector2(0,25) - screenPos + new Vector2(0f, NPC.gfxOffY);
            drawColor = Color.Lerp(drawColor, new Color(255, 0, 0, drawColor.A), Utils.GetLerpValue(0f, 255f, NPC.alpha, true));

            Texture2D glow = ModContent.Request<Texture2D>($"{nameof(CalRemix)}/NPCs/Clamitas_Fire", AssetRequestMode.ImmediateLoad).Value;
            Rectangle rectFire = new Rectangle(0, (int)NPC.localAI[1] * glow.Height / 2, glow.Width, glow.Height / 2);
            Vector2 origFire = rectFire.Size() / 2f;
            if (hitAmount >= 5 && attack != 0)
                spriteBatch.Draw(glow, draw, rectFire, new Color(255, 255, 255, 255), NPC.rotation, origFire, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, draw, rect, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>($"{nameof(CalRemix)}/NPCs/Clamitas_Glow", AssetRequestMode.ImmediateLoad).Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, glow.Width, glow.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = NPC.Center - new Vector2(0, 25) - screenPos + new Vector2(0f, NPC.gfxOffY);
            if (hitAmount >= 5 && attack != 0)
                spriteBatch.Draw(glow, draw, sourceRectangle, new Color(255, 255, 255, 255), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<BrimstoneSlag>(), 1, 25, 35);
            npcLoot.Add(ModContent.ItemType<BrimstoneSword>());
            npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<MolluskHusk>(), 1, 8, 15);
            npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<EyeofDesolation>());
            int[] itemIDs = new int[5]
            {
                ModContent.ItemType<Brimlash>(),
                ModContent.ItemType<BrimstoneFury>(),
                ModContent.ItemType<BurningSea>(),
                ModContent.ItemType<IgneousExaltation>(),
                ModContent.ItemType<Brimblade>()
            };
            LeadingConditionRule lcr = new(new Conditions.IsHardmode());
            lcr.OnSuccess(ItemDropRule.OneFromOptions(1, itemIDs));
            npcLoot.Add(lcr);
        }
        private static void Dusting(Vector2 position, Vector2 center, int w, int h)
        {
            Dust dust = Dust.NewDustDirect(position, w, h, DustID.LifeDrain, 0f, 0f, 200, default, 1.5f);
            dust.noGravity = true;
            dust.velocity *= 0.75f;
            Vector2 vector = new Vector2(Main.rand.Next(-200, 201), Main.rand.Next(-200, 201));
            vector.Normalize();
            vector *= Main.rand.Next(100, 200) * 0.04f;
            dust.velocity = vector;
            vector.Normalize();
            vector *= 34f;
            dust.position = center - vector;
        }
        private static int GetDamage()
        {

            int damage = 80;
            if (CalamityWorld.death)
                damage = 200;
            else if (CalamityWorld.revenge)
                damage = 160;
            else if (Main.expertMode)
                damage = 120;
            return damage;
        }
        private static int GetDarts()
        {
            int dartCount = 8;
            if (CalamityWorld.death)
                dartCount = 14;
            else if (CalamityWorld.revenge)
                dartCount = 12;
            else if (Main.expertMode)
                dartCount = 10;
            return dartCount;
        }
    }
}
