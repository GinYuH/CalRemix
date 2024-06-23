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
using CalRemix.Items.Materials;
using CalRemix.Items.Potions;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.World;

namespace CalRemix.NPCs
{
    public class GreenDemon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Demon");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("Green Demon",
                "Gee wilickers! A Green Demon! These guys pack a real punch, but they are absolutely TERRIFIED of radioactive toads! or was it frogs? salamanders? Well either way you better catch some!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 400;
            NPC.width = 112;
            NPC.height = 112;
            NPC.defense = 4;
            NPC.lifeMax = 20000;
            NPC.knockBackResist = 0.6f;
            NPC.value = Item.buyPrice(1);
            NPC.noGravity = false;
            NPC.HitSound = CalamityMod.NPCs.NormalNPCs.Rimehound.HitSound;
            NPC.DeathSound = SoundID.NPCDeath27;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToElectricity = true;
        }

        public override void AI()
        {
           /* NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            if (NPC.velocity.X > 0)
            {
                NPC.direction = -1;
            }
            else if (NPC.velocity.X < 0)
            {
                NPC.direction = 1;
            }*/
            if (NPC.Remix().GreenAI[1] == 0)
            {
                CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedHerplingAI(NPC, Mod);
                NPC.Remix().GreenAI[0]++;
                if (NPC.Remix().GreenAI[0] % 65 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    Vector2 dist = Main.player[NPC.target].Center - NPC.Center;
                    dist.Normalize();
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < Main.rand.Next(5, 9); i++)
                        {
                            Vector2 velocity = dist * 16;
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)(3 - 1)));
                            perturbedSpeed += new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
                            int type = Main.rand.NextBool(3) ? ModContent.ProjectileType<CalamityMod.Projectiles.Ranged.SevensStrikerGrape>() : ModContent.ProjectileType<CalamityMod.Projectiles.Ranged.SevensStrikerOrange>();
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, type, 50, 0f);
                            Main.projectile[p].hostile = true;
                            Main.projectile[p].friendly = false;
                            Main.projectile[p].DamageType = DamageClass.Generic;
                        }
                    }
                }
                if (NPC.life <= NPC.lifeMax * 0.5f && NPC.Remix().GreenAI[0] % 95 == 0)
                {
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int type = ModContent.ProjectileType<CalamityMod.Projectiles.Ranged.AMR2>();
                        int dmg = Main.expertMode ? 375 : 500;
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitY * 14, new Vector2(NPC.direction * 22, 0), type, dmg, 0f);
                        Main.projectile[p].hostile = true;
                        Main.projectile[p].friendly = false;
                        Main.projectile[p].DamageType = DamageClass.Generic;
                    }
                }
                if (NPC.Remix().GreenAI[0] >= 300)
                {
                    NPC.Remix().GreenAI[1] = 1;
                    NPC.Remix().GreenAI[0] = 0;
                }
            }
            if (NPC.Remix().GreenAI[1] == 1)
            {
                NPC.Remix().GreenAI[0]++;
                if (NPC.Remix().GreenAI[0] == 1)
                {
                    NPC.velocity.Y = -20;
                }
                if (NPC.velocity.Y == 0f)
                {
                    int xamt = 30;
                    for (int i = 0; i < xamt; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            Tile be = Main.tile[(int)(NPC.position.X / 16) + (i - xamt / 2), (int)(NPC.position.Y / 16) + j];
                            if (be.IsTileSolid())
                            Dust.NewDust(new Vector2((int)NPC.position.X + i * 16 - ((xamt / 2) * 16), (int)NPC.position.Y + j * 16), 16, 16, ModContent.DustType<CalamityMod.Dusts.AbsorberDust>(), Scale: 2f);
                        }
                    }
                    foreach (Player p in Main.player)
                    {
                        if (p == null)
                            continue;
                        if (!p.active)
                            continue;
                        if (p.Distance(NPC.Center) < xamt * 16 && p.CheckSolidGround())
                        {
                            p.velocity.Y -= 20;
                            p.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 1000, 0);
                        }
                    }
                    NPC.Remix().GreenAI[1] = 2;
                    NPC.Remix().GreenAI[0] = 0;
                    if (NPC.life <= NPC.lifeMax * 0.5f)
                    {
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int type = ModContent.ProjectileType<CalamityMod.Projectiles.Ranged.AMR2>();
                            int dmg = Main.expertMode ? 375 : 500;
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitY * 14, new Vector2(NPC.direction * 22, 0), type, dmg, 0f);
                            Main.projectile[p].hostile = true;
                            Main.projectile[p].friendly = false;
                            Main.projectile[p].DamageType = DamageClass.Generic;
                        }
                    }
                }
            }
            if (NPC.Remix().GreenAI[1] == 2)
            {
                NPC.Remix().GreenAI[0]++;
                if (NPC.Remix().GreenAI[0] >= 120)
                {
                    NPC.Remix().GreenAI[1] = 0;
                    NPC.Remix().GreenAI[0] = 0;
                }
                if (NPC.life <= NPC.lifeMax * 0.5f && NPC.Remix().GreenAI[0] % 42 == 0)
                {
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int type = ModContent.ProjectileType<CalamityMod.Projectiles.Ranged.AMR2>();
                        int dmg = Main.expertMode ? 375 : 500;
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitY * 14, new Vector2(NPC.direction * 22, 0), type, dmg, 0f);
                        Main.projectile[p].hostile = true;
                        Main.projectile[p].friendly = false;
                        Main.projectile[p].DamageType = DamageClass.Generic;
                    }
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement("Eat vegetables and fruits to become strong. LIKE ME!")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > 6.0)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.life > NPC.lifeMax * 0.5f)
            {
                if (NPC.frame.Y > frameHeight * 3)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                if (NPC.frame.Y < frameHeight * 4 || NPC.frame.Y > frameHeight * 7)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.eclipse || !DownedBossSystem.downedLeviathan || !CalRemixWorld.greenDemon)
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.4f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<GreenDemonHead>());
            npcLoot.Add(ModContent.ItemType<ZumboSauce>(), 10);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            NPC.life += (int)MathHelper.Lerp(hurtInfo.Damage, 0, NPC.lifeMax - NPC.life);
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<CalamityMod.Projectiles.Summon.EXPLODINGFROG>() 
                || projectile.type == ModContent.ProjectileType<CalamityMod.Projectiles.Summon.FrogGore1>()
                || projectile.type == ModContent.ProjectileType<CalamityMod.Projectiles.Summon.FrogGore2>()
                || projectile.type == ModContent.ProjectileType<CalamityMod.Projectiles.Summon.FrogGore3>()
                || projectile.type == ModContent.ProjectileType<CalamityMod.Projectiles.Summon.FrogGore4>()
                || projectile.type == ModContent.ProjectileType<CalamityMod.Projectiles.Summon.FrogGore5>()
                )
            {
                modifiers.SetInstantKill();
                if (Main.player[projectile.owner].HasItem(ModContent.ItemType<CausticCroakerStaff>()) && Main.rand.NextBool(10))
                {
                    Main.player[projectile.owner].ConsumeItem(ModContent.ItemType<CausticCroakerStaff>());
                }
            }
        }
    }
}
