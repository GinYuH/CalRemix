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
using CalamityMod.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Animations;
using Terraria.GameContent;
using CalRemix.Content.Projectiles.Hostile;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class Blimpaa : ModNPC
    {
        public List<VerletSimulatedSegment> segments = new();

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 30;
            NPC.width = 60;
            NPC.height = 30;
            NPC.defense = 3;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(silver: 5);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath63;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToElectricity = true;
        }

        public override void AI()
        {
            if (NPC.ai[3] == 0)
            {
                NPC.ai[3] = Main.rand.Next(1, 8);
            }
            NPC.noGravity = true;
            NPC.TargetClosest();
            float num1347 = 4f;
            float num1348 = 0.25f;
            Vector2 vector314 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num1349 = Main.player[NPC.target].Center.X - vector314.X;
            float num1350 = Main.player[NPC.target].Center.Y - vector314.Y - 200f;
            float num1352 = (float)Math.Sqrt(num1349 * num1349 + num1350 * num1350);
            if (num1352 < 20f)
            {
                num1349 = NPC.velocity.X;
                num1350 = NPC.velocity.Y;
            }
            else
            {
                num1352 = num1347 / num1352;
                num1349 *= num1352;
                num1350 *= num1352;
            }
            if (NPC.velocity.X < num1349)
            {
                NPC.velocity.X += num1348;
                if (NPC.velocity.X < 0f && num1349 > 0f)
                {
                    NPC.velocity.X += num1348 * 2f;
                }
            }
            else if (NPC.velocity.X > num1349)
            {
                NPC.velocity.X -= num1348;
                if (NPC.velocity.X > 0f && num1349 < 0f)
                {
                    NPC.velocity.X -= num1348 * 2f;
                }
            }
            if (NPC.velocity.Y < num1350)
            {
                NPC.velocity.Y += num1348;
                if (NPC.velocity.Y < 0f && num1350 > 0f)
                {
                    NPC.velocity.Y += num1348 * 2f;
                }
            }
            else if (NPC.velocity.Y > num1350)
            {
                NPC.velocity.Y -= num1348;
                if (NPC.velocity.Y > 0f && num1350 < 0f)
                {
                    NPC.velocity.Y -= num1348 * 2f;
                }
            }
            if (NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && Main.netMode != 1)
            {
                this.NPC.ai[0] += 1f;
                if (NPC.ai[0] > 20f)
                {
                    this.NPC.ai[0] = 0f;
                    int num1353 = (int)(NPC.position.X + 10f + (float)Main.rand.Next(NPC.width - 20));
                    int num1354 = (int)(NPC.position.Y + (float)NPC.height + 4f);
                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), num1353, num1354, 0f, 5f, ModContent.ProjectileType<ConfettiEvil>(), CalRemixHelper.ProjectileDamage(10, 20), 0f, Main.myPlayer);
                    Main.projectile[p].DamageType = DamageClass.Generic;
                    Main.projectile[p].hostile = true;
                    Main.projectile[p].friendly = false;
                }
            }
            CalRemixHelper.CreateVerletChain(ref segments, 10, NPC.Bottom, [0]);

            segments[0].oldPosition = segments[^1].position;
            segments[0].position = NPC.Bottom;

            segments = VerletSimulatedSegment.SimpleSimulation(segments, 10, loops: 10, gravity: 0.9f);
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
            else if (BirthdayParty.PartyIsUp && Main._shouldUseWindyDayMusic)
                return SpawnCondition.OverworldDay.Chance * 0.2f;
            else
                return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.ConfettiGun, 1);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                VerletSimulatedSegment seg = segments[i];
                float rot = 0f;
                int dist = 4;
                if (i > 0)
                {
                    rot = seg.position.DirectionTo(segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                    dist = (int)seg.position.Distance(segments[i - 1].position) + 2;
                }
                else
                {
                    rot = NPC.rotation;
                }
                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, seg.position - Main.screenPosition, new Rectangle(0, 0, 2, dist), Color.White.MultiplyRGBA(Lighting.GetColor(seg.position.ToTileCoordinates())), rot, new Rectangle(0, 0, 4, dist).Size() / 2, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }

            string ext = NPC.ai[3] switch
            {
                2 => "_Green",
                3 => "_Magenta",
                4 => "_Purple",
                5 => "_Teal",
                6 => "_Yellow",
                7 => "_Blue",
                _ => ""
            };
            Texture2D tex = ModContent.Request<Texture2D>(Texture + ext).Value;
            Main.EntitySpriteDraw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 8), NPC.scale, 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    string ext = NPC.ai[3] switch
                    {
                        2 => "_Green",
                        3 => "_Magenta",
                        4 => "_Purple",
                        5 => "_Teal",
                        6 => "_Yellow",
                        7 => "_Blue",
                        _ => ""
                    };


                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Blimpaa1" + ext).Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Blimpaa2" + ext).Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Blimpaa3" + ext).Type, NPC.scale);
                    for (int num502 = 0; num502 < 36; num502++)
                    {
                        int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 16f), NPC.width, NPC.height - 16, Balimbaa.ConfettiDust(), 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int num226 = 36;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.Normalize(NPC.velocity) * new Vector2((float)NPC.width / 2f, (float)NPC.height) * 0.75f;
                        vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * MathHelper.TwoPi / (float)num226), default) + NPC.Center;
                        Vector2 vector7 = vector6 - NPC.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, Balimbaa.ConfettiDust(), vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].noLight = true;
                        Main.dust[num228].velocity = vector7;
                    }
                }
            }
        }
    }
}
