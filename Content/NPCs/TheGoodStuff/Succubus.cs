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
using CalRemix.Content.Items.Placeables.Banners;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class Succubus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 30;
            NPC.width = 32;
            NPC.height = 36;
            NPC.defense = 3;
            NPC.lifeMax = 110;
            NPC.knockBackResist = 0.8f;
            NPC.value = Item.buyPrice(silver: 2);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit21;
            NPC.DeathSound = SoundID.NPCDeath24;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToCold = true;
            Banner = ModContent.NPCType<Succubus>();
            BannerItem = ModContent.ItemType<SuccubusBanner>();
        }

        public override void AI()
        {
            NPC.noGravity = true;
            if (NPC.collideX)
            {
                NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                {
                    NPC.velocity.Y = 1f;
                }
                if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                {
                    NPC.velocity.Y = -1f;
                }
            }
            NPC.TargetClosest();
            
            {
                if (NPC.direction == -1 && NPC.velocity.X > -4f)
                {
                    NPC.velocity.X -= 0.1f;
                    if (NPC.velocity.X > 4f)
                    {
                        NPC.velocity.X -= 0.1f;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X += 0.05f;
                    }
                    if (NPC.velocity.X < -4f)
                    {
                        NPC.velocity.X = -4f;
                    }
                }
                else if (NPC.direction == 1 && NPC.velocity.X < 4f)
                {
                    NPC.velocity.X += 0.1f;
                    if (NPC.velocity.X < -4f)
                    {
                        NPC.velocity.X += 0.1f;
                    }
                    else if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X -= 0.05f;
                    }
                    if (NPC.velocity.X > 4f)
                    {
                        NPC.velocity.X = 4f;
                    }
                }
                if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
                {
                    NPC.velocity.Y -= 0.04f;
                    if ((double)NPC.velocity.Y > 1.5)
                    {
                        NPC.velocity.Y -= 0.05f;
                    }
                    else if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y += 0.03f;
                    }
                    if ((double)NPC.velocity.Y < -1.5)
                    {
                        NPC.velocity.Y = -1.5f;
                    }
                }
                else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
                {
                    NPC.velocity.Y += 0.04f;
                    if ((double)NPC.velocity.Y < -1.5)
                    {
                        NPC.velocity.Y += 0.05f;
                    }
                    else if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y -= 0.03f;
                    }
                    if ((double)NPC.velocity.Y > 1.5)
                    {
                        NPC.velocity.Y = 1.5f;
                    }
                }
            }
            {
                if (NPC.wet)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.95f;
                    }
                    NPC.velocity.Y -= 0.5f;
                    if (NPC.velocity.Y < -4f)
                    {
                        NPC.velocity.Y = -4f;
                    }
                    NPC.TargetClosest();
                }
                if (NPC.type == NPCID.Hellbat)
                {
                    if (NPC.direction == -1 && NPC.velocity.X > -4f)
                    {
                        NPC.velocity.X -= 0.1f;
                        if (NPC.velocity.X > 4f)
                        {
                            NPC.velocity.X -= 0.07f;
                        }
                        else if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X += 0.03f;
                        }
                        if (NPC.velocity.X < -4f)
                        {
                            NPC.velocity.X = -4f;
                        }
                    }
                    else if (NPC.direction == 1 && NPC.velocity.X < 4f)
                    {
                        NPC.velocity.X += 0.1f;
                        if (NPC.velocity.X < -4f)
                        {
                            NPC.velocity.X += 0.07f;
                        }
                        else if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X -= 0.03f;
                        }
                        if (NPC.velocity.X > 4f)
                        {
                            NPC.velocity.X = 4f;
                        }
                    }
                    if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
                    {
                        NPC.velocity.Y -= 0.04f;
                        if ((double)NPC.velocity.Y > 1.5)
                        {
                            NPC.velocity.Y -= 0.03f;
                        }
                        else if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y += 0.02f;
                        }
                        if ((double)NPC.velocity.Y < -1.5)
                        {
                            NPC.velocity.Y = -1.5f;
                        }
                    }
                    else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
                    {
                        NPC.velocity.Y += 0.04f;
                        if ((double)NPC.velocity.Y < -1.5)
                        {
                            NPC.velocity.Y += 0.03f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y -= 0.02f;
                        }
                        if ((double)NPC.velocity.Y > 1.5)
                        {
                            NPC.velocity.Y = 1.5f;
                        }
                    }
                }
                else
                {
                    if (NPC.direction == -1 && NPC.velocity.X > -4f)
                    {
                        NPC.velocity.X -= 0.1f;
                        if (NPC.velocity.X > 4f)
                        {
                            NPC.velocity.X -= 0.1f;
                        }
                        else if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X += 0.05f;
                        }
                        if (NPC.velocity.X < -4f)
                        {
                            NPC.velocity.X = -4f;
                        }
                    }
                    else if (NPC.direction == 1 && NPC.velocity.X < 4f)
                    {
                        NPC.velocity.X += 0.1f;
                        if (NPC.velocity.X < -4f)
                        {
                            NPC.velocity.X += 0.1f;
                        }
                        else if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X -= 0.05f;
                        }
                        if (NPC.velocity.X > 4f)
                        {
                            NPC.velocity.X = 4f;
                        }
                    }
                    if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
                    {
                        NPC.velocity.Y -= 0.04f;
                        if ((double)NPC.velocity.Y > 1.5)
                        {
                            NPC.velocity.Y -= 0.05f;
                        }
                        else if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y += 0.03f;
                        }
                        if ((double)NPC.velocity.Y < -1.5)
                        {
                            NPC.velocity.Y = -1.5f;
                        }
                    }
                    else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
                    {
                        NPC.velocity.Y += 0.04f;
                        if ((double)NPC.velocity.Y < -1.5)
                        {
                            NPC.velocity.Y += 0.05f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y -= 0.03f;
                        }
                        if ((double)NPC.velocity.Y > 1.5)
                        {
                            NPC.velocity.Y = 1.5f;
                        }
                    }
                }
            }
            NPC.ai[1] += 1f;
            if (NPC.ai[1] > 200f)
            {
                if (!Main.player[NPC.target].wet && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    NPC.ai[1] = 0f;
                }
                float num732 = 0.2f;
                float num733 = 0.1f;
                float num734 = 4f;
                float num735 = 1.5f;
                if (NPC.type == NPCID.Harpy || NPC.type == NPCID.Demon || NPC.type == NPCID.VoodooDemon)
                {
                    num732 = 0.12f;
                    num733 = 0.07f;
                    num734 = 3f;
                    num735 = 1.25f;
                }
                if (NPC.ai[1] > 1000f)
                {
                    NPC.ai[1] = 0f;
                }
                NPC.ai[2] += 1f;
                if (NPC.ai[2] > 0f)
                {
                    if (NPC.velocity.Y < num735)
                    {
                        NPC.velocity.Y += num733;
                    }
                }
                else if (NPC.velocity.Y > 0f - num735)
                {
                    NPC.velocity.Y -= num733;
                }
                if (NPC.ai[2] < -150f || NPC.ai[2] > 150f)
                {
                    if (NPC.velocity.X < num734)
                    {
                        NPC.velocity.X += num732;
                    }
                }
                else if (NPC.velocity.X > 0f - num734)
                {
                    NPC.velocity.X -= num732;
                }
                if (NPC.ai[2] > 300f)
                {
                    NPC.ai[2] = -300f;
                }
            }
            int range = 200;
            if (Main.player[NPC.target].Distance(NPC.Center) <= range)
            {
                NPC.ai[3]++;
                Player p = Main.player[NPC.target];
                p.AddBuff(BuffID.OnFire, 60);
                if (NPC.ai[3] > 180)
                    p.AddBuff(BuffID.Confused, 60);
                if (NPC.ai[3] > 360)
                    p.AddBuff(BuffID.VortexDebuff, 60);
            }
            else if (NPC.ai[3] > 0)
            {
                NPC.ai[3]--;
            }
            NPC.spriteDirection = NPC.direction;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[0] % 60 == 0)
            {
                float dustAmt = 100;
                for (int i = 0; i < (int)dustAmt; i++)
                {
                    Vector2 pos = Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / dustAmt)) * range;
                    Dust d = Dust.NewDustPerfect(NPC.Center + pos, DustID.Shadowflame, Vector2.Zero);
                    d.noGravity = true;
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
            if (NPC.frame.Y > frameHeight)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.Succubus").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
                return 0f;
            else if (spawnInfo.Player.position.Y / 16 > GenVars.lavaLine)
                return SpawnCondition.Cavern.Chance * 0.025f;
            else
                return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DemonicBoneAsh>());
            npcLoot.Add(ModContent.ItemType<OldLordClaymore>(), 30);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Succubus1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Succubus2").Type, NPC.scale);
                }
            }
        }
    }

    public class Succubus2 : Succubus
    {
        public override string Texture => "CalRemix/Content/NPCs/TheGoodStuff/Succubus_Alt";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
                return 0f;
            else
                return SpawnCondition.Underworld.Chance * 0.025f;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Succubus1_Alt").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Succubus2_Alt").Type, NPC.scale);
                }
            }
        }
    }
}
