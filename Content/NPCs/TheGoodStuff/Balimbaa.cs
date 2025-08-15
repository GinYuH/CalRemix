using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using CalRemix.UI;
using System.Linq;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Potions;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Core.World;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Melee;
using Terraria.WorldBuilding;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Materials;
using System;
using Terraria.GameContent.Events;
using System.Collections.Generic;
using CalRemix.Content.Items.Placeables.Banners;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class Balimbaa : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 200;
            NPC.width = 100;
            NPC.height = 80;
            NPC.defense = 16;
            NPC.lifeMax = 400;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(gold: 1);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath63;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToElectricity = true;
            Banner = Type;
            BannerItem = ModContent.ItemType<BalimbaaBanner>();
        }

        public override void AI()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = false;
            if (this.NPC.ai[0] == 0f)
            {
                NPC.TargetClosest();
                if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    this.NPC.ai[0] = 1f;
                    return;
                }
                Vector2 vector195 = Main.player[NPC.target].Center - NPC.Center;
                vector195.Y -= Main.player[NPC.target].height / 4;
                float num488 = vector195.Length();
                Vector2 center30 = NPC.Center;
                center30.X = Main.player[NPC.target].Center.X;
                Vector2 vector196 = center30 - NPC.Center;
                if (vector196.Length() > 8f && Collision.CanHit(NPC.Center, 1, 1, center30, 1, 1))
                {
                    this.NPC.ai[0] = 3f;
                    this.NPC.ai[1] = center30.X;
                    this.NPC.ai[2] = center30.Y;
                    Vector2 center31 = NPC.Center;
                    center31.Y = Main.player[NPC.target].Center.Y;
                    if (vector196.Length() > 8f && Collision.CanHit(NPC.Center, 1, 1, center31, 1, 1) && Collision.CanHit(center31, 1, 1, Main.player[NPC.target].position, 1, 1))
                    {
                        this.NPC.ai[0] = 3f;
                        this.NPC.ai[1] = center31.X;
                        this.NPC.ai[2] = center31.Y;
                    }
                }
                else
                {
                    center30 = NPC.Center;
                    center30.Y = Main.player[NPC.target].Center.Y;
                    if ((center30 - NPC.Center).Length() > 8f && Collision.CanHit(NPC.Center, 1, 1, center30, 1, 1))
                    {
                        this.NPC.ai[0] = 3f;
                        this.NPC.ai[1] = center30.X;
                        this.NPC.ai[2] = center30.Y;
                    }
                }
                if (this.NPC.ai[0] == 0f)
                {
                    NPC.localAI[0] = 0f;
                    vector195.Normalize();
                    vector195 *= 0.5f;
                    NPC.velocity += vector195;
                    this.NPC.ai[0] = 4f;
                    this.NPC.ai[1] = 0f;
                }
            }
            else if (this.NPC.ai[0] == 1f)
            {
                Vector2 vector197 = Main.player[NPC.target].Center - NPC.Center;
                float num489 = vector197.Length();
                float num490 = 2f;
                num490 += num489 / 200f;
                int num492 = 50;
                vector197.Normalize();
                vector197 *= num490;
                NPC.velocity = (NPC.velocity * (num492 - 1) + vector197) / num492;
                if (!Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    this.NPC.ai[0] = 0f;
                    this.NPC.ai[1] = 0f;
                }
            }
            else if (this.NPC.ai[0] == 2f)
            {
                NPC.noTileCollide = true;
                Vector2 vector198 = Main.player[NPC.target].Center - NPC.Center;
                float num493 = vector198.Length();
                float num494 = 2f;
                int num495 = 4;
                vector198.Normalize();
                vector198 *= num494;
                NPC.velocity = (NPC.velocity * (num495 - 1) + vector198) / num495;
                if (num493 < 600f && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    this.NPC.ai[0] = 0f;
                }
            }
            else if (this.NPC.ai[0] == 3f)
            {
                Vector2 vector199 = new Vector2(this.NPC.ai[1], this.NPC.ai[2]);
                Vector2 vector200 = vector199 - NPC.Center;
                float num496 = vector200.Length();
                float num497 = 1f;
                float num498 = 3f;
                vector200.Normalize();
                vector200 *= num497;
                NPC.velocity = (NPC.velocity * (num498 - 1f) + vector200) / num498;
                if (NPC.collideX || NPC.collideY)
                {
                    this.NPC.ai[0] = 4f;
                    this.NPC.ai[1] = 0f;
                }
                if (num496 < num497 || num496 > 800f || Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    this.NPC.ai[0] = 0f;
                }
            }
            else
            {
                if (this.NPC.ai[0] != 4f)
                {
                    return;
                }
                if (NPC.collideX)
                {
                    NPC.velocity.X *= -0.8f;
                }
                if (NPC.collideY)
                {
                    NPC.velocity.Y *= -0.8f;
                }
                Vector2 vector202;
                if (NPC.velocity.X == 0f && NPC.velocity.Y == 0f)
                {
                    vector202 = Main.player[NPC.target].Center - NPC.Center;
                    vector202.Y -= Main.player[NPC.target].height / 4;
                    vector202.Normalize();
                    NPC.velocity = vector202 * 0.1f;
                }
                float num499 = 1.5f;
                float num500 = 20f;
                vector202 = NPC.velocity;
                vector202.Normalize();
                vector202 *= num499;
                NPC.velocity = (NPC.velocity * (num500 - 1f) + vector202) / num500;
                this.NPC.ai[1] += 1f;
                if (this.NPC.ai[1] > 180f)
                {
                    this.NPC.ai[0] = 0f;
                    this.NPC.ai[1] = 0f;
                }
                if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                {
                    this.NPC.ai[0] = 0f;
                }
                NPC.localAI[0] += 1f;
                if (!(NPC.localAI[0] >= 5f) || Collision.SolidCollision(NPC.position - new Vector2(10f, 10f), NPC.width + 20, NPC.height + 20))
                {
                    return;
                }
                NPC.localAI[0] = 0f;
                Vector2 center32 = NPC.Center;
                center32.X = Main.player[NPC.target].Center.X;
                if (Collision.CanHit(NPC.Center, 1, 1, center32, 1, 1) && Collision.CanHit(NPC.Center, 1, 1, center32, 1, 1) && Collision.CanHit(Main.player[NPC.target].Center, 1, 1, center32, 1, 1))
                {
                    this.NPC.ai[0] = 3f;
                    this.NPC.ai[1] = center32.X;
                    this.NPC.ai[2] = center32.Y;
                    return;
                }
                center32 = NPC.Center;
                center32.Y = Main.player[NPC.target].Center.Y;
                if (Collision.CanHit(NPC.Center, 1, 1, center32, 1, 1) && Collision.CanHit(Main.player[NPC.target].Center, 1, 1, center32, 1, 1))
                {
                    this.NPC.ai[0] = 3f;
                    this.NPC.ai[1] = center32.X;
                    this.NPC.ai[2] = center32.Y;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter % 8 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Party,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
                return 0f;
            else if (BirthdayParty.PartyIsUp && Main._shouldUseWindyDayMusic && Main.hardMode)
                return SpawnCondition.OverworldDay.Chance * 0.2f;
            else
                return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.ConfettiGun, 1, 3, 8);
        }

        public override void OnKill()
        {
            int blimps = Main.rand.Next(2, 5);
            for (int i = 0; i < blimps; i++)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X + Main.rand.Next(-20, 20), (int)NPC.Center.Y + Main.rand.Next(-20, 20), ModContent.NPCType<Blimpaa>());
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Balimbaa1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Balimbaa2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Balimbaa3").Type, NPC.scale);
                    for (int num502 = 0; num502 < 36; num502++)
                    {
                        int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 16f), NPC.width, NPC.height - 16, ConfettiDust(), 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int num226 = 36;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.Normalize(NPC.velocity) * new Vector2((float)NPC.width / 2f, (float)NPC.height) * 0.75f;
                        vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * MathHelper.TwoPi / (float)num226), default) + NPC.Center;
                        Vector2 vector7 = vector6 - NPC.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, ConfettiDust(), vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].noLight = true;
                        Main.dust[num228].velocity = vector7;
                    }
                }
            }
        }

        public static int ConfettiDust()
        {
            int[] dusts = new int[] { DustID.Confetti, DustID.Confetti_Blue, DustID.Confetti_Green, DustID.Confetti_Pink, DustID.Confetti_Yellow };
            return Utils.SelectRandom(Main.rand, dusts);
        }
    }
}
