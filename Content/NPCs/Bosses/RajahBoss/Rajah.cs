using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.RajahItems;
using CalRemix.Content.Items.RajahItems.Supreme;
using CalRemix.Content.NPCs.Bosses.RajahBoss.Supreme;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    [AutoloadBossHead]
    public class Rajah : ModNPC
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/RajahBoss/Rajah";
        public int damage = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Rabbit");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 130;
            NPC.height = 220;
            NPC.aiStyle = -1;
            NPC.damage = 130;
            NPC.defense = 90;
            NPC.lifeMax = 65000;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 1000f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound");// Mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/Rajah");
            NPC.value = Item.sellPrice(0, 1, 10, 0);
            NPC.boss = true;
            NPC.netAlways = true;
            Music = MusicLoader.GetMusicSlot(CalRemix.instance, "CalRemix/Assets/Music/Bosses/RajahTheme");// Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/RajahTheme");
        }

        public bool isSupreme = false;
        public float[] internalAI = new float[6];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(internalAI[4]);
                writer.Write(isSupreme);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadSingle(); //SpaceOctopus AI stuff
                internalAI[1] = reader.ReadSingle(); //Is Flying
                internalAI[2] = reader.ReadSingle(); //Is Jumping
                internalAI[3] = reader.ReadSingle(); //Minion/Rocket Timer
                internalAI[4] = reader.ReadSingle(); //JumpFlyControl and Vertical dash
                isSupreme = reader.ReadBoolean();
            }
        }

        private Texture2D RajahTex;
        private Texture2D Glow;
        private Texture2D SupremeGlow;
        private Texture2D SupremeEyes;
        private Texture2D ArmTex;
        public int WeaponFrame = 0;
        public Vector2 MovePoint;
        public bool SelectPoint = false;

        /*
         * npc.ai[0] = Jump Timer
         * npc.ai[1] = Ground Minion Alternation
         * npc.ai[2] = Weapon Change timer
         * npc.ai[3] = Weapon type
         */

        public int roarTimer = 0;
        public int roarTimerMax = 240;
        public bool Roaring => roarTimer > 0;

        public void Roar(int timer)
        {
            roarTimer = timer;
            SoundEngine.PlaySound(new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound"), NPC.Center);
        }

        public Vector2 WeaponPos;
        public Vector2 StaffPos;

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (isSupreme)
            {
                modifiers.FinalDamage.Scale(0.7f);
            }
        }

        public float ProjSpeed()
        {
            if (NPC.life < (NPC.lifeMax * .85f)) //The lower the health, the more damage is done
            {
                return isSupreme ? 12f : 10f;
            }
            if (NPC.life < (NPC.lifeMax * .7f))
            {
                return isSupreme ? 13f : 11f;
            }
            if (NPC.life < (NPC.lifeMax * .65f))
            {
                return isSupreme ? 14f : 12f;
            }
            if (NPC.life < (NPC.lifeMax * .4f))
            {
                return isSupreme ? 15f : 13f;
            }
            if (NPC.life < (NPC.lifeMax * .25f))
            {
                return isSupreme ? 16f : 14f;
            }
            if (NPC.life < (NPC.lifeMax * .1f))
            {
                return isSupreme ? 16f : 15f;
            }
            return isSupreme ? 11f : 9f;
        }

        private bool SayLine = false;
        private bool DefenseLine = false;

        public override void AI()
        {
            if (Main.expertMode)
            {
                damage = NPC.damage / 4;
            }
            else
            {
                damage = NPC.damage / 2;
            }
            CalRemixNPC.Rajah = NPC.whoAmI;
            WeaponPos = new Vector2(NPC.Center.X + (NPC.direction == 1 ? -78 : 78), NPC.Center.Y - 9);
            StaffPos = new Vector2(NPC.Center.X + (NPC.direction == 1 ? 78 : -78), NPC.Center.Y - 9);
            if (Roaring) roarTimer--;

            if (Main.netMode != 1 && NPC.type == ModContent.NPCType<SupremeRajah>() && isSupreme == false)
            {
                isSupreme = true;
                NPC.netUpdate = true;
            }

            if (isSupreme)
            {
                if (NPC.ai[3] != 0 && !DefenseLine && !RemixDowned.downedRajahsRevenge && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    DefenseLine = true;
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Chat", Color.MediumPurple);

                }
                if (NPC.life <= NPC.lifeMax / 7 && !SayLine && Main.netMode != 1)
                {
                    SayLine = true;
                    string Name;

                    int bunnyKills = NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                    if (bunnyKills >= 100 && !RemixDowned.downedRajahsRevenge)
                    {
                        Name = "MUDERER";
                    }
                    else
                    {
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            Name = "Terrarians";
                        }
                        else if (!RemixDowned.downedRajahsRevenge)
                        {
                            Name = "Terrarian";
                        }
                        else
                        {
                            Name = Main.LocalPlayer.name;
                        }
                    }
                    if (Main.netMode != 1)
                    {
                        if (Main.netMode == 0)
                        {
                            Main.NewText(Language.GetTextValue("Mods.CalRemix.Dialog.Rajah.5", Name.ToUpper()), new Color(107, 137, 179));
                        }
                        else if (Main.netMode == 2 || Main.netMode == 1)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.CalRemix.Dialog.Rajah.5", Name.ToUpper()), new Color(107, 137, 179));
                        }
                    }
                    Music = MusicLoader.GetMusicSlot(CalRemix.instance, "CalRemix/Assets/Music/Bosses/LastStand");
                }
            }

            Player player = Main.player[NPC.target];
            if (NPC.target >= 0 && Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
                if (Main.player[NPC.target].dead)
                {
                    if (isSupreme)
                    {
                        if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.Rajah.6", new Color(107, 137, 179));
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<SupremeRajahBookIt>(), damage, 0, Main.myPlayer);
                        }
                    }
                    else
                    {
                        if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.Rajah.2", new Color(107, 137, 179));
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<RajahBookIt>(), damage, 0, Main.myPlayer);
                        }
                    }
                    NPC.active = false;
                    NPC.noTileCollide = true;
                    NPC.netUpdate = true;
                    return;
                }
            }

            if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > 10000)
            {
                NPC.TargetClosest(true);
                if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > 10000)
                {
                    if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.Rajah.3", new Color(107, 137, 179));
                    if (Main.netMode != 1)
                    {
                        if (isSupreme)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<SupremeRajahBookIt>(), damage, 0, Main.myPlayer); //Originally 100 damage
                        }
                        else
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<RajahBookIt>(), damage, 0, Main.myPlayer);
                        }
                    }
                    NPC.active = false;
                    NPC.noTileCollide = true;
                    NPC.netUpdate = true;
                    return;
                }
            }


            if (player.Center.X < NPC.Center.X)
            {
                NPC.direction = 1;
            }
            else
            {
                NPC.direction = -1;
            }

            if (internalAI[4] == 0)
            {
                if(player.Center.Y + player.height / 2 < NPC.Center.Y + NPC.height / 2 - 30f || Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > 2000 || isDashing)
                {
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    internalAI[4] = 2f;
                    NPC.ai[0] = 0;
                    return;
                }
                else
                {
                    NPC.noTileCollide = true;
                    NPC.noGravity = false;
                    isDashing = false;
                    JumpAI();
                }
            }
            else if(internalAI[4] == 1f)
            {
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                isDashing = false;
                if (player.Center.Y + player.height / 2 <= NPC.Center.Y + NPC.height / 2 + 20f) 
                {
                    if(NPC.collideY && NPC.velocity.Y > 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                        for (int num622 = (int)NPC.position.X - 20; num622 < (int)NPC.position.X + NPC.width + 40; num622 += 20)
                        {
                            for (int num623 = 0; num623 < 4; num623++)
                            {
                                int num624 = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, 31, 0f, 0f, 100);
                                Main.dust[num624].velocity *= 0.2f;
                            }
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), num622 - 20, NPC.position.Y + NPC.height - 8f, 0, 0, ModContent.ProjectileType<RajahStomp>(), damage, 6, Main.myPlayer, 0, 0);
                            int num625 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(num622 - 20, NPC.position.Y + NPC.height - 8f), default, Main.rand.Next(61, 64), 1f);
                            Main.gore[num625].velocity *= 0.4f;
                        }
                    }
                    NPC.noTileCollide = false;
                    NPC.velocity.X *= .2f;
                    NPC.velocity.Y = -2f;
                    internalAI[4] = 0f;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                    return;
                }
                if(Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > 1000)
                {
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    internalAI[4] = 2f;
                    NPC.ai[0] = 0;
                }
            }
            else if(internalAI[4] == 2f)
            {
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                FlyAI();
                if(Math.Abs(NPC.Center.X - player.Center.X) < 50f && player.position.Y > NPC.Center.Y + NPC.height / 2)
                {
                    internalAI[4] = 3f;
                    NPC.netUpdate = true;
                }
            }
            else if(internalAI[4] == 3f)
            {
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                isDashing = true;
                if(player.velocity.X == 0)
                {
                    NPC.velocity = (player.Center - NPC.Center) * .06f;
                }
                else
                {
                    NPC.velocity = (player.Center + new Vector2(100f * (player.velocity.X > 0? 1 : -1), 0) - NPC.Center) * .06f;
                }
                NPC.velocity = Vector2.Normalize(NPC.velocity) * 26f;
                if(NPC.velocity.X > 10f) NPC.velocity.X = 10f;
                internalAI[0] = 0f;
                internalAI[4] = 1f;
            }
            else if(internalAI[4] == 4f)
            {
                NPC.noTileCollide = true;
                NPC.noGravity = false;
                isDashing = false;
                if (player.Center.Y + player.height / 2 <= NPC.Center.Y + NPC.height / 2 + 20f) 
                {
                    internalAI[0] = 0f;
                    internalAI[4] = 1f;
                }
            }

            if (NPC.target <= 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }

            if (Main.netMode != 1)
            {
                NPC.ai[2]++;
                internalAI[3]++;
            }
            if (NPC.ai[2] >= 500)
            {
                internalAI[3] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                NPC.netUpdate = true;
            }
            else if (NPC.ai[3] == 0 && NPC.ai[2] >= ChangeRate())
            {
                if (Main.rand.Next(5) == 0)
                {
                    Roar(roarTimerMax);
                }
                if (Main.netMode != 1)
                {
                    internalAI[3] = 0;
                    NPC.ai[2] = 0;
                    if (ModLoader.HasMod("ThoriumMod") && Main.rand.Next(7) == 0)
                    {
                        NPC.ai[3] = 7;
                    }
                    else
                    {
                        if (isSupreme)
                        {
                            NPC.ai[3] = Main.rand.Next(7);
                        }
                        else
                        {
                            NPC.ai[3] = Main.rand.Next(4);
                        }
                    }
                }
                NPC.netUpdate = true;
            }

            if (Main.netMode != 1)
            {
                if (NPC.ai[3] == 0) //Minion Phase
                {
                    if (internalAI[3] >= 80)
                    {
                        internalAI[3] = 0;
                        if (internalAI[1] == 0)
                        {
                            if (NPC.CountNPCS(ModContent.NPCType<RabbitcopterSoldier>()) + CalamityUtils.CountProjectiles(ModContent.ProjectileType<BunnySummon1>()) < 5)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 200, (int)NPC.Center.X + 200), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 200, (int)NPC.Center.X + 200), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 200, (int)NPC.Center.X + 200), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                            }
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            if (NPC.ai[1] > 2)
                            {
                                NPC.ai[1] = 0;
                            }
                            if (NPC.ai[1] == 0)
                            {
                                if (NPC.CountNPCS(ModContent.NPCType<RabbitcopterSoldier>()) + CalamityUtils.CountProjectiles(ModContent.ProjectileType<BunnySummon1>()) < 5)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                }
                            }
                            else if (NPC.ai[1] == 1)
                            {
                                if (NPC.CountNPCS(ModContent.NPCType<BunnyBrawler>()) + CalamityUtils.CountProjectiles(ModContent.ProjectileType<BunnySummon2>()) < 5)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon2>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon2>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                }
                            }
                            else if (NPC.ai[1] == 2)
                            {
                                if (NPC.CountNPCS(ModContent.NPCType<BunnyBattler>()) + CalamityUtils.CountProjectiles(ModContent.ProjectileType<BunnySummon3>()) < 8)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));

                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));

                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));

                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)NPC.Center.X - 500, (int)NPC.Center.X + 500), Main.rand.Next((int)NPC.Center.Y - 200, (int)NPC.Center.Y - 50));
                                }
                            }
                            NPC.ai[1] += 1;
                            NPC.netUpdate = true;
                        }
                    }
                }
                else if (NPC.ai[3] == 1) //Bunzooka
                {
                    if (internalAI[3] > 40)
                    {
                        internalAI[3] = 0;
                        int Rocket = isSupreme ? ModContent.ProjectileType<RajahRocketEXR>() : ModContent.ProjectileType<RajahRocket>();
                        Vector2 dir = Vector2.Normalize(player.Center - WeaponPos);
                        dir *= ProjSpeed();
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, Rocket, damage, 5, Main.myPlayer);
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[3] == 2) //Royal Scepter
                {
                    int carrots = isSupreme ? 5 : 3;
                    int carrotType = isSupreme ? Mod.Find<ModProjectile>("CarrotEXR").Type : Mod.Find<ModProjectile>("CarrotHostile").Type;
                    float spread = 45f * 0.0174f * .5f;
                    Vector2 dir = Vector2.Normalize(player.Center - WeaponPos);
                    dir *= ProjSpeed() + (isSupreme? 3 : 1);
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / carrots * 2;
                    if (internalAI[3] > 40)
                    {
                        internalAI[3] = 0;
                        for (int i = 0; i < carrots; i++)
                        {
                            double offsetAngle = startAngle + deltaAngle * (i - (int)(carrots * .5f));
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), WeaponPos.X, WeaponPos.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), carrotType, damage, 5, Main.myPlayer, 0);
                        }
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[3] == 3) //Javelin
                {
                    int Javelin = isSupreme ? ModContent.ProjectileType<BaneTEXR>() : ModContent.ProjectileType<BaneR>();
                    if (internalAI[3] == (isSupreme ? 40 : 60))
                    {
                        float time = (player.Center - WeaponPos).Length() / ProjSpeed();
                        Vector2 dir = Vector2.Normalize(player.Center + (isSupreme? player.velocity * time : Vector2.Zero) - WeaponPos);
                        dir *= ProjSpeed();
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, Javelin, damage, 5, Main.myPlayer);
                    }
                    if (internalAI[3] > (isSupreme ? 60 : 90))
                    {
                        internalAI[3] = 0;
                    }
                    NPC.netUpdate = true;
                }
                else if (NPC.ai[3] == 4) //Excalihare
                {
                    if (internalAI[3] > 20)
                    {
                        internalAI[3] = 0;
                        Vector2 dir = Vector2.Normalize(player.Center - WeaponPos);
                        dir *= ProjSpeed() + 3f;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, ModContent.ProjectileType<ExcalihareR>(), damage, 5, Main.myPlayer);
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[3] == 5) //Fluffy Fury
                {
                    int Arrows = Main.rand.Next(2, 4);
                    float spread = 45f * 0.0174f * .3f;
                    float time = (player.Center - WeaponPos).Length() / ProjSpeed();
                    Vector2 dir = Vector2.Normalize(player.Center + (isSupreme? player.velocity * time : Vector2.Zero) - WeaponPos);
                    dir *= ProjSpeed() + (isSupreme? 3 : 1);
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / (Arrows * 2);
                    float delay = isSupreme? 15 : 50;
                    if (internalAI[3] > delay)
                    {
                        internalAI[3] = 0;
                        for (int i = 0; i < Arrows; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), WeaponPos.X, WeaponPos.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), Mod.Find<ModProjectile>("CarrowR").Type, damage, 5, Main.myPlayer);
                        }
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[3] == 6) //Rabbits Wrath
                {
                    if (internalAI[3] > 5)
                    {
                        internalAI[3] = 0;
                        Vector2 vector12 = new Vector2(player.Center.X, player.Center.Y);
                        float num75 = 14f;
                        for (int num120 = 0; num120 < 3; num120++)
                        {
                            Vector2 vector2 = player.Center + new Vector2(-(float)Main.rand.Next(0, 401) * player.direction, -600f);
                            vector2.Y -= 120 * num120;
                            Vector2 vector13 = vector12 - vector2;
                            if (vector13.Y < 0f)
                            {
                                vector13.Y *= -1f;
                            }
                            if (vector13.Y < 20f)
                            {
                                vector13.Y = 20f;
                            }
                            vector13.Normalize();
                            vector13 *= num75;
                            float num82 = vector13.X;
                            float num83 = vector13.Y;
                            float speedX5 = num82;
                            float speedY6 = num83 + Main.rand.Next(-40, 41) * 0.02f;
                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector2.X, vector2.Y, speedX5, speedY6, ModContent.ProjectileType<CarrotEXR>(), damage, 6, Main.myPlayer, 0, 0);
                            Main.projectile[p].tileCollide = false;
                        }
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[3] == 7) //Carrot Farmer
                {
                    if (!CalamityUtils.AnyProjectiles(ModContent.ProjectileType<CarrotFarmerR>()))
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<CarrotFarmerR>(), damage, 3f, Main.myPlayer, NPC.whoAmI);
                        NPC.netUpdate = true;
                    }
                }
            }

            if (Main.expertMode)
            {
                if (NPC.life < (NPC.lifeMax * .85f)) //The lower the health, the more damage is done
                {
                    NPC.damage = (int)(NPC.defDamage * 1.1f);
                }
                if (NPC.life < (NPC.lifeMax * .7f))
                {
                    NPC.damage = (int)(NPC.defDamage * 1.3f);
                }
                if (NPC.life < (NPC.lifeMax * .65f))
                {
                    NPC.damage = (int)(NPC.defDamage * 1.5f);
                }
                if (NPC.life < (NPC.lifeMax * .4f))
                {
                    NPC.damage = (int)(NPC.defDamage * 1.7f);
                }
                if (NPC.life < (NPC.lifeMax * .25f))
                {
                    NPC.damage = (int)(NPC.defDamage * 1.9f);
                }
                if (NPC.life < (NPC.lifeMax / 7))
                {
                    NPC.damage = (int)(NPC.defDamage * 2.2f);
                }
            }
            else
            {
                if (NPC.life == NPC.lifeMax / 7)
                {
                    NPC.damage = (int)(NPC.defDamage * 1.5f);
                }
            }

            NPC.rotation = 0;
        }

        public string WeaponTexture()
        {
            if (NPC.ai[3] == 1) //Bunzooka
            {
                return "NPCs/Bosses/Rajah/RajahArmsB";
            }
            else if (NPC.ai[3] == 2) //Scepter
            {
                return "NPCs/Bosses/Rajah/RajahArmsR";
            }
            else if (NPC.ai[3] == 3 && internalAI[3] <= (isSupreme ? 40 : 60)) //Javelin
            {
                return "NPCs/Bosses/Rajah/RajahArmsS";
            }
            else if (NPC.ai[3] == 4) //Excalihare
            {
                return "NPCs/Bosses/Rajah/Supreme/Excalihare";
            }
            else if (NPC.ai[3] == 5) //Fluffy Fury
            {
                return "NPCs/Bosses/Rajah/Supreme/FluffyFury";
            }
            else if (NPC.ai[3] == 6) //Rabbits Wrath
            {
                return "NPCs/Bosses/Rajah/Supreme/RabbitsWrath";
            }
            else
            {
                return "BlankTex";
            }
        }

        public void JumpAI()
        {
            internalAI[1] = 1;
            if (NPC.ai[0] == 0f)
            {
                NPC.noTileCollide = false;
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.8f;
                    internalAI[2] += 1f;
                    if (internalAI[2] > 0f)
                    {
                        if (NPC.life < (NPC.lifeMax * .85f)) //The lower the health, the more frequent the jumps
                        {
                            internalAI[2] += 2;
                        }
                        if (NPC.life < (NPC.lifeMax * .7f))
                        {
                            internalAI[2] += 2;
                        }
                        if (NPC.life < (NPC.lifeMax * .65f))
                        {
                            internalAI[2] += 2;
                        }
                        if (NPC.life < (NPC.lifeMax * .4f))
                        {
                            internalAI[2] += 2;
                        }
                        if (NPC.life < (NPC.lifeMax * .25f))
                        {
                            internalAI[2] += 2;
                        }
                        if (NPC.life < (NPC.lifeMax * .1f))
                        {
                            internalAI[2] += 2;
                        }
                    }
                    if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) > 800f)
                    {
                        internalAI[2] = -1f;
                    }
                    if (internalAI[2] >= 250f)
                    {
                        internalAI[2] = -20f;
                    }
                    else if (internalAI[2] == -1f)
                    {
                        NPC.TargetClosest(true);
                        float longth = Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X);
                        NPC.velocity.X = (6 + longth * .01f) * NPC.direction;
                        NPC.velocity.Y = -12.1f;
                        NPC.ai[0] = 1f;
                        internalAI[2] = 0f;
                        NPC.netUpdate = true;
                    }
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                    NPC.ai[0] = 0f;
                    for (int num622 = (int)NPC.position.X - 20; num622 < (int)NPC.position.X + NPC.width + 40; num622 += 20)
                    {
                        for (int num623 = 0; num623 < 4; num623++)
                        {
                            int num624 = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, 31, 0f, 0f, 100);
                            Main.dust[num624].velocity *= 0.2f;
                        }
                        int num625 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(num622 - 20, NPC.position.Y + NPC.height - 8f), default, Main.rand.Next(61, 64), 1f);
                        Main.gore[num625].velocity *= 0.4f;
                    }
                }
                else
                {
                    NPC.TargetClosest(true);
                    if (NPC.position.X < Main.player[NPC.target].position.X && NPC.position.X + NPC.width > Main.player[NPC.target].position.X + Main.player[NPC.target].width)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.9f;
                        NPC.velocity.Y = NPC.velocity.Y + 0.4f;
                    }
                    else
                    {
                        
                        float num626 = 3f;
                        float longth = Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X);
                        num626 = 3f + longth * .056f;
                        
                        if (Main.player[NPC.target].velocity.X != 0)
                        {
                            num626 += Math.Abs(Main.player[NPC.target].velocity.X);
                        }

                        if (NPC.direction < 0)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.2f;
                        }
                        else if (NPC.direction > 0)
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.2f;
                        }

                        if (NPC.velocity.X < -num626)
                        {
                            NPC.velocity.X = -num626;
                        }
                        if (NPC.velocity.X > num626)
                        {
                            NPC.velocity.X = num626;
                        }
                    }
                }

                Player player = Main.player[NPC.target];
                if(player.Center.Y + player.height / 2 <= NPC.Center.Y + NPC.height / 2 + 20f && NPC.velocity.Y > 0)
                {
                    internalAI[4] = 4f;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                    return;
                }
                else if(Math.Abs(NPC.Center.X - player.Center.X) < 50f && player.position.Y > NPC.Center.Y + NPC.height / 2)
                {
                    internalAI[4] = 3f;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                    return;
                }
            }
        }

        bool isDashing = false;
        public void FlyAI()
        {
            float speed = 14f;
            if (isSupreme)
            {
                if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > 1000)
                {
                    speed = 50f;
                    isDashing = true;
                }
                else
                {
                    speed = 20f;
                    isDashing = false;
                }
            }
            else if (NPC.life < (NPC.lifeMax * .85f)) //The lower the health, the more damage is done
            {
                speed = 15f;
            }
            else if (NPC.life < (NPC.lifeMax * .7f))
            {
                speed = 16f;
            }
            else if (NPC.life < (NPC.lifeMax * .65f))
            {
                speed = 17f;
            }
            else if (NPC.life < (NPC.lifeMax * .4f))
            {
                speed = 18f;
            }
            else if (NPC.life < (NPC.lifeMax * .25f))
            {
                speed = 19f;
            }
            else if (NPC.life < (NPC.lifeMax * .1f))
            {
                speed = 20f;
            }
            AISpaceOctopus(NPC, Main.player[NPC.target].Center, .35f, speed, 300);
            internalAI[1] = 0;
        }

        public static void AISpaceOctopus(NPC npc, Vector2 targetCenter = default(Vector2), float moveSpeed = 0.15f, float velMax = 5f, float hoverDistance = 250f)
		{
            float pos = 200f;
            if(Main.player[npc.target].velocity.X == 0)
            {
                pos = 0;
            }
            else
            {
                pos = (Main.player[npc.target].velocity.X > 0? 1f: -1f) * 200f;
            }
			Vector2 wantedVelocity = targetCenter - npc.Center + new Vector2(pos, -hoverDistance);
			float dist = (float)Math.Sqrt(wantedVelocity.X * wantedVelocity.X + wantedVelocity.Y * wantedVelocity.Y);
			if (dist < 20f)
			{
				wantedVelocity = npc.velocity;
			}
			else if (dist < 40f)
			{
				wantedVelocity.Normalize();
				wantedVelocity *= velMax * 0.35f;
			}
			else if (dist < 80f)
			{
				wantedVelocity.Normalize();
				wantedVelocity *= velMax * 0.65f;
			}
			else
			{
				wantedVelocity.Normalize();
				wantedVelocity *= velMax;
			}
			if (npc.velocity.X < wantedVelocity.X)
			{
				npc.velocity.X = npc.velocity.X + moveSpeed;
				if (npc.velocity.X < 0f && wantedVelocity.X > 0f)
				{
					npc.velocity.X = npc.velocity.X + moveSpeed;
				}
			}
			else if (npc.velocity.X > wantedVelocity.X)
			{
				npc.velocity.X = npc.velocity.X - moveSpeed;
				if (npc.velocity.X > 0f && wantedVelocity.X < 0f)
				{
					npc.velocity.X = npc.velocity.X - moveSpeed;
				}
			}
			if (npc.velocity.Y < wantedVelocity.Y)
			{
				npc.velocity.Y = npc.velocity.Y + moveSpeed;
				if (npc.velocity.Y < 0f && wantedVelocity.Y > 0f)
				{
					npc.velocity.Y = npc.velocity.Y + moveSpeed;
				}
			}
			else if (npc.velocity.Y > wantedVelocity.Y)
			{
				npc.velocity.Y = npc.velocity.Y - moveSpeed;
				if (npc.velocity.Y > 0f && wantedVelocity.Y < 0f)
				{
					npc.velocity.Y = npc.velocity.Y - moveSpeed;
				}
			}
        }

        public int ChangeRate()
        {
            if (NPC.type == ModContent.NPCType<SupremeRajah>())
            {
                return 120;
            }
            return 240;
        }

        public override void FindFrame(int frameHeight)
        {
            if (internalAI[1] == 0)
            {
                WeaponFrame = frameHeight * 5;
                if (NPC.frameCounter++ > 3)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                    if (NPC.frame.Y > frameHeight * 7)
                    {
                        NPC.frame.Y = 0;
                    }
                }
            }
            else
            {
                WeaponFrame = NPC.frame.Y;
                if (NPC.ai[0] == 0f)
                {
                    if (internalAI[2] < -17f)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = 0;
                    }
                    else if (internalAI[2] < -14f)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight;
                    }
                    else if (internalAI[2] < -11f)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 2;
                    }
                    else if (internalAI[2] < -8f)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 3;
                    }
                    else if (internalAI[2] < -5f)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 4;
                    }
                    else if (internalAI[2] < -2f)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        if (NPC.frameCounter++ > 7.5f)
                        {
                            NPC.frameCounter = 0;
                            NPC.frame.Y += frameHeight;
                            if (NPC.frame.Y > frameHeight * 2)
                            {
                                NPC.frame.Y = 0;
                            }
                        }
                    }
                }
                else if (NPC.ai[0] == 1f)
                {
                    if (NPC.velocity.Y != 0f)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        NPC.frameCounter++;
                        if  (NPC.frame.Y > 3)
                        {
                            if (NPC.frameCounter > 0)
                            {
                                NPC.frameCounter = 0;
                                NPC.frame.Y = frameHeight * 6;
                            }
                            else if (NPC.frameCounter > 4)
                            {
                                NPC.frameCounter = 0;
                                NPC.frame.Y = frameHeight * 7;
                            }
                            else if (NPC.frameCounter > 8)
                            {
                                NPC.frameCounter = 0;
                                NPC.frame.Y = 0;
                            }
                        }
                        else
                        {
                            if (NPC.frameCounter > 7.5f)
                            {
                                NPC.frameCounter = 0;
                                NPC.frame.Y += frameHeight;
                                if (NPC.frame.Y > frameHeight * 2)
                                {
                                    NPC.frame.Y = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void OnKill()
        {
            if (isSupreme)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("Gores/SupremeRajahHelmet1").Type, 1f);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("Gores/SupremeRajahHelmet2").Type, 1f);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("Gores/SupremeRajahHelmet3").Type, 1f);
                if (!RemixDowned.downedRajahsRevenge)
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<SupremeRajahDefeat>());
                    Main.npc[n].Center = NPC.Center;
                }
                else
                {
                    string Name;
                    if (Main.netMode != 0)
                    {
                        Name = "Terrarians";
                    }
                    else
                    {
                        Name = Main.LocalPlayer.name;
                    }
                    if (Main.netMode != 1)
                    {
                        if (Main.netMode == 0)
                        {
                            Main.NewText(Language.GetTextValue("Mods.CalRemix.Dialog.Rajah.7", Name), new Color(107, 137, 179));
                        }
                        else if (Main.netMode == 2 || Main.netMode == 1)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.CalRemix.Dialog.Rajah.7", Name), new Color(107, 137, 179));
                        }
                    }

                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<SupremeRajahLeave>(), 100, 0, Main.myPlayer);
                    Main.projectile[p].Center = NPC.Center;
                }
            }
            else
            {
                int bunnyKills = NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                if (bunnyKills >= 100)
                {
                    if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.Rajah.4", new Color(107, 137, 179));
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<RajahBookIt>(), 100, 0, Main.myPlayer);
            }
            RemixDowned.downedRajah = true;
            NPC.value = 0f;
            NPC.boss = false;
        }

        public override void BossLoot(ref int potionType)
        {
            if (isSupreme)
            {
                potionType = ModContent.ItemType<SupremeHealingPotion>();
                return;
            }
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Master
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ItemID.BunnyStatue);

            //Expert
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<RajahBag>()));

            //Normal
            LeadingConditionRule normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
            npcLoot.Add(normalOnly);
            {
                normalOnly.Add(ModContent.ItemType<RajahMask>(), 7);

                normalOnly.Add(DropHelper.PerPlayer(ModContent.ItemType<RajahPelt>(), 1, 10, 26));

                int[] weapons = new int[]
                {
                    ModContent.ItemType<BaneOfTheBunny>(),
                    ModContent.ItemType<Bunzooka>(),
                    ModContent.ItemType<RoyalScepter>(),
                    ModContent.ItemType<Punisher>(),
                    ModContent.ItemType<RabbitcopterEars>(),
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, weapons));
            }

            //Always
            npcLoot.Add(ModContent.ItemType<RajahTrophy>(), 10);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);  //boss life scale in expertmode
            NPC.damage = (int)(NPC.damage * .6f);
        }

        public void RajahTexture()
        {
            string IsRoaring = Roaring ? "Roar" : "";
            string Supreme = isSupreme ? "Supreme/Supreme" : "";
            if (internalAI[1] == 0)
            {
                RajahTex = ModContent.Request<Texture2D>("NPCs/Bosses/Rajah/" + Supreme + "Rajah" + IsRoaring + "_Fly").Value;
                Glow = ModContent.Request<Texture2D>("Glowmasks/Rajah" + IsRoaring + "_Fly_Glow").Value;
                SupremeGlow = ModContent.Request<Texture2D>("Glowmasks/SupremeRajah" + IsRoaring + "_Fly_Glow").Value;
                SupremeEyes = ModContent.Request<Texture2D>("Glowmasks/SupremeRajah" + IsRoaring + "_Fly_Eyes").Value;
            }
            else
            {
                RajahTex = ModContent.Request<Texture2D>("NPCs/Bosses/Rajah/" + Supreme + "Rajah" + IsRoaring).Value;
                Glow = ModContent.Request<Texture2D>("Glowmasks/Rajah" + IsRoaring + "_Glow").Value;
                SupremeGlow = ModContent.Request<Texture2D>("Glowmasks/SupremeRajah" + IsRoaring + "_Glow").Value;
                SupremeEyes = ModContent.Request<Texture2D>("Glowmasks/SupremeRajah" + IsRoaring + "_Eyes").Value;
            }
        }
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            bool RageMode = !isSupreme && NPC.life < NPC.lifeMax / 7;
            bool SupremeRageMode = isSupreme && NPC.life < NPC.lifeMax / 7;
            RajahTexture();
            if (isSupreme && isDashing)
            {
                BaseDrawing.DrawAfterimage(spriteBatch, RajahTex, 0, NPC, 1f, 1f, 10, false, 0f, 0f, Main.DiscoColor);
            }
            if (RageMode)
            {
                Color RageColor = CalamityUtils.MulticolorLerp(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Firebrick, drawColor, Color.Firebrick);
                BaseDrawing.DrawAura(spriteBatch, RajahTex, 0, NPC.position, NPC.width, NPC.height, auraPercent, 1f, 1f, 0f, NPC.direction, 8, NPC.frame, 0f, -5f, RageColor);
            }
            else if (SupremeRageMode)
            {
                BaseDrawing.DrawAura(spriteBatch, RajahTex, 0, NPC.position, NPC.width, NPC.height, auraPercent, 1f, 1f, 0f, NPC.direction, 8, NPC.frame, 0f, -5f, Main.DiscoColor);
            }
            if (NPC.ai[3] != 0 && NPC.ai[3] < 6) //If holding a weapon
            {
                ArmTex = ModContent.Request<Texture2D>(WeaponTexture()).Value;
                Rectangle WeaponRectangle = new Rectangle(0, WeaponFrame, 300, 220);
                BaseDrawing.DrawTexture(spriteBatch, ArmTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, WeaponRectangle, drawColor, true);
            }
            BaseDrawing.DrawTexture(spriteBatch, RajahTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, NPC.frame, drawColor, true);
            if (NPC.ai[3] == 6) //If Rabbits Wrath
            {
                ArmTex = ModContent.Request<Texture2D>("NPCs/Bosses/Rajah/Supreme/RabbitsWrath").Value;
                Rectangle WeaponRectangle = new Rectangle(0, WeaponFrame, 300, 220);
                BaseDrawing.DrawTexture(spriteBatch, ArmTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, WeaponRectangle, drawColor, true);
            }
            if (RageMode)
            {
                int shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
                BaseDrawing.DrawTexture(spriteBatch, Glow, shader, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, NPC.frame, Color.White, true);
            }
            if (SupremeRageMode)
            {
                BaseDrawing.DrawTexture(spriteBatch, Glow, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, NPC.frame, Main.DiscoColor, true);
                BaseDrawing.DrawAura(spriteBatch, Glow, 0, NPC.position, NPC.width, NPC.height, auraPercent, 1f, 1f, 0f, NPC.direction, 8, NPC.frame, 0f, -5f, Main.DiscoColor);
                BaseDrawing.DrawTexture(spriteBatch, SupremeGlow, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, NPC.frame, Main.DiscoColor, true);
                BaseDrawing.DrawAura(spriteBatch, SupremeGlow, 0, NPC.position, NPC.width, NPC.height, auraPercent, 1f, 1f, 0f, NPC.direction, 8, NPC.frame, 0f, -5f, Main.DiscoColor);
                return false;
            }
            else if (isSupreme)
            {
                BaseDrawing.DrawTexture(spriteBatch, SupremeEyes, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 8, NPC.frame, Main.DiscoColor, true);
            }
            return false;
        }

        public override string BossHeadTexture => "CalRemix/Content/NPCs/Bosses/RajahBoss/Rajah_Head_Boss";

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 30f;
            if (moveSpeed == 0f || NPC.Center == point) return;
            float velMultiplier = 1f;
            Vector2 dist = point - NPC.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            NPC.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            NPC.velocity *= moveSpeed;
            NPC.velocity *= velMultiplier;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if(internalAI[4] == 4f || internalAI[4] == 2f || internalAI[4] == 1f)
            {
                target.wingTime = 0;
                target.velocity.Y = 1f;
            }
        }
    }

    [AutoloadBossHead]
    public class SupremeRajah : Rajah
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/RajahBoss/Supreme/SupremeRajah";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Rabbit; Champion of the Innocent");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 150;
            NPC.defense = 0;
            NPC.lifeMax = 1200000;
            NPC.life = 1200000;
            isSupreme = true;
            NPC.value = Item.sellPrice(3, 0, 0, 0);
            Music = MusicLoader.GetMusicSlot(CalRemix.instance, "CalRemix/Assets/Music/Bosses/RajahTheme");// Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/RajahTheme");
        }
        public override string BossHeadTexture => "CalRemix/Content/NPCs/Bosses/RajahBoss/SupremeRajah_Head_Boss";

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Master
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ItemID.BunnyStatue);

            //Expert
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<RajahCache>()));

            //Normal
            LeadingConditionRule normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
            npcLoot.Add(normalOnly);
            {
                normalOnly.Add(ModContent.ItemType<RajahMask>(), 7);

                normalOnly.Add(DropHelper.PerPlayer(ModContent.ItemType<ChampionPlate>(), 1, 15, 31));

                int[] weapons = new int[]
                {
                    ModContent.ItemType<Excalihare>(),
                    ModContent.ItemType<FluffyFury>(),
                    ModContent.ItemType<RabbitsWrath>(),
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, weapons));
            }

            //Always
            npcLoot.Add(ModContent.ItemType<RajahTrophy>(), 10);
        }
    }

    //FUCK YOU FUCK YOU FUCK YOU
    public class BaseDrawing
    {
        public static void DrawAura(object sb, Texture2D texture, int shader, Entity codable, float auraPercent, float distanceScalar = 1f, float offsetX = 0f, float offsetY = 0f, Color? overrideColor = null)
        {
            int frameCount = ((codable is NPC) ? Main.npcFrameCount[((NPC)codable).type] : Main.projFrames[((Projectile)codable).type]);
            Rectangle frame = ((codable is NPC) ? ((NPC)codable).frame : Utils.Frame(texture, 1, frameCount, 0, ((Projectile)codable).frame));
            float scale = ((codable is NPC) ? ((NPC)codable).scale : ((Projectile)codable).scale);
            float rotation = ((codable is NPC) ? ((NPC)codable).rotation : ((Projectile)codable).rotation);
            int spriteDirection = ((codable is NPC) ? ((NPC)codable).spriteDirection : ((Projectile)codable).spriteDirection);
            float offsetY2 = ((codable is NPC) ? ((NPC)codable).gfxOffY : 0f);
            DrawAura(sb, texture, shader, codable.position + new Vector2(0f, offsetY2), codable.width, codable.height, auraPercent, distanceScalar, scale, rotation, spriteDirection, frameCount, frame, offsetX, offsetY, overrideColor);
        }

        public static void DrawAura(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, float auraPercent, float distanceScalar = 1f, float scale = 1f, float rotation = 0f, int direction = 0, int framecount = 1, Rectangle frame = default(Rectangle), float offsetX = 0f, float offsetY = 0f, Color? overrideColor = null)
        {
            Color lightColor = (overrideColor.HasValue ? overrideColor.Value : Lighting.GetColor((position + new Vector2((float)width * 0.5f, (float)height * 0.5f)).ToTileCoordinates()));
            float percentHalf = auraPercent * 5f * distanceScalar;
            float percentLight = MathHelper.Lerp(0.8f, 0.2f, auraPercent);
            lightColor.R = (byte)((float)(int)lightColor.R * percentLight);
            lightColor.G = (byte)((float)(int)lightColor.G * percentLight);
            lightColor.B = (byte)((float)(int)lightColor.B * percentLight);
            lightColor.A = (byte)((float)(int)lightColor.A * percentLight);
            Vector2 position2 = position;
            for (int i = 0; i < 4; i++)
            {
                float offX = offsetX;
                float offY = offsetY;
                switch (i)
                {
                    case 0:
                        offX += percentHalf;
                        break;
                    case 1:
                        offX -= percentHalf;
                        break;
                    case 2:
                        offY += percentHalf;
                        break;
                    case 3:
                        offY -= percentHalf;
                        break;
                }
                position2 = new Vector2(position.X + offX, position.Y + offY);
                DrawTexture(sb, texture, shader, position2, width, height, scale, rotation, direction, framecount, frame, lightColor);
            }
        }

        public static void DrawAfterimage(object sb, Texture2D texture, int shader, Entity codable, float distanceScalar = 1f, float sizeScalar = 1f, int imageCount = 7, bool useOldPos = true, float offsetX = 0f, float offsetY = 0f, Color? overrideColor = null, Rectangle? overrideFrame = null, int overrideFrameCount = 0)
        {
            int frameCount = ((overrideFrameCount > 0) ? overrideFrameCount : ((codable is NPC) ? Main.npcFrameCount[((NPC)codable).type] : Main.projFrames[((Projectile)codable).type]));
            Rectangle frame = (overrideFrame.HasValue ? overrideFrame.Value : ((codable is NPC) ? ((NPC)codable).frame : Utils.Frame(texture, 1, frameCount, 0, ((Projectile)codable).frame)));
            float scale = ((codable is NPC) ? ((NPC)codable).scale : ((Projectile)codable).scale);
            float rotation = ((codable is NPC) ? ((NPC)codable).rotation : ((Projectile)codable).rotation);
            int spriteDirection = ((codable is NPC) ? ((NPC)codable).spriteDirection : ((Projectile)codable).spriteDirection);
            Vector2[] velocities = new Vector2[1] { codable.velocity };
            if (useOldPos)
            {
                velocities = ((codable is NPC) ? ((NPC)codable).oldPos : ((Projectile)codable).oldPos);
            }
            float offsetY2 = ((codable is NPC) ? ((NPC)codable).gfxOffY : 0f);
            DrawAfterimage(sb, texture, shader, codable.position + new Vector2(0f, offsetY2), codable.width, codable.height, velocities, scale, rotation, spriteDirection, frameCount, frame, distanceScalar, sizeScalar, imageCount, useOldPos, offsetX, offsetY, overrideColor);
        }

        public static void DrawAfterimage(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, Vector2[] oldPoints, float scale = 1f, float rotation = 0f, int direction = 0, int framecount = 1, Rectangle frame = default(Rectangle), float distanceScalar = 1f, float sizeScalar = 1f, int imageCount = 7, bool useOldPos = true, float offsetX = 0f, float offsetY = 0f, Color? overrideColor = null)
        {
            new Vector2(texture.Width / 2, texture.Height / framecount / 2);
            Color lightColor = (overrideColor.HasValue ? overrideColor.Value : Lighting.GetColor((position + new Vector2((float)width * 0.5f, (float)height * 0.5f)).ToTileCoordinates()));
            Vector2 velAddon = default(Vector2);
            Vector2 originalpos = position;
            Vector2 offset = new Vector2(offsetX, offsetY);
            for (int i = 1; i <= imageCount; i++)
            {
                scale *= sizeScalar;
                Color newLightColor = lightColor;
                newLightColor.R = (byte)(newLightColor.R * (imageCount + 3 - i) / (imageCount + 9));
                newLightColor.G = (byte)(newLightColor.G * (imageCount + 3 - i) / (imageCount + 9));
                newLightColor.B = (byte)(newLightColor.B * (imageCount + 3 - i) / (imageCount + 9));
                newLightColor.A = (byte)(newLightColor.A * (imageCount + 3 - i) / (imageCount + 9));
                if (useOldPos)
                {
                    position = Vector2.Lerp(originalpos, (i - 1 >= oldPoints.Length) ? oldPoints[oldPoints.Length - 1] : oldPoints[i - 1], distanceScalar);
                    DrawTexture(sb, texture, shader, position + offset, width, height, scale, rotation, direction, framecount, frame, newLightColor);
                }
                else
                {
                    Vector2 velocity = ((i - 1 >= oldPoints.Length) ? oldPoints[oldPoints.Length - 1] : oldPoints[i - 1]);
                    velAddon += velocity * distanceScalar;
                    DrawTexture(sb, texture, shader, position + offset - velAddon, width, height, scale, rotation, direction, framecount, frame, newLightColor);
                }
            }
        }

        public static void DrawTexture(object sb, Texture2D texture, int shader, Entity codable, Color? overrideColor = null, bool drawCentered = false, Vector2 overrideOrigin = default(Vector2))
        {
            DrawTexture(sb, texture, shader, codable, 1, overrideColor, drawCentered, overrideOrigin);
        }

        public static void DrawTexture(object sb, Texture2D texture, int shader, Entity codable, int framecountX, Color? overrideColor = null, bool drawCentered = false, Vector2 overrideOrigin = default(Vector2))
        {
            Color lightColor = (overrideColor.HasValue ? overrideColor.Value : ((codable is Item) ? ((Item)codable).GetAlpha(Lighting.GetColor(codable.Center.ToTileCoordinates())) : ((codable is NPC n) ? n.GetAlpha(Lighting.GetColor(n.position.ToTileCoordinates())) : ((codable is Projectile) ? ((Projectile)codable).GetAlpha(Lighting.GetColor(codable.Center.ToTileCoordinates())) : Lighting.GetColor(codable.Center.ToTileCoordinates())))));
            int frameCount = ((codable is Item) ? 1 : ((codable is NPC) ? Main.npcFrameCount[((NPC)codable).type] : Main.projFrames[((Projectile)codable).type]));
            Rectangle frame = ((codable is NPC) ? ((NPC)codable).frame : Utils.Frame(texture, 1, frameCount, 0, ((Projectile)codable).frame));
            float scale = ((codable is Item) ? ((Item)codable).scale : ((codable is NPC) ? ((NPC)codable).scale : ((Projectile)codable).scale));
            float rotation = ((codable is Item) ? 0f : ((codable is NPC) ? ((NPC)codable).rotation : ((Projectile)codable).rotation));
            int spriteDirection = ((codable is Item) ? 1 : ((codable is NPC) ? ((NPC)codable).spriteDirection : ((Projectile)codable).spriteDirection));
            float offsetY = ((codable is NPC) ? ((NPC)codable).gfxOffY : 0f);
            DrawTexture(sb, texture, shader, codable.position + new Vector2(0f, offsetY), codable.width, codable.height, scale, rotation, spriteDirection, frameCount, framecountX, frame, lightColor, drawCentered, overrideOrigin);
        }

        public static void DrawTexture(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, float scale, float rotation, int direction, int framecount, Rectangle frame, Color? overrideColor = null, bool drawCentered = false, Vector2 overrideOrigin = default(Vector2))
        {
            DrawTexture(sb, texture, shader, position, width, height, scale, rotation, direction, framecount, 1, frame, overrideColor, drawCentered, overrideOrigin);
        }

        public static void DrawTexture(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, float scale, float rotation, int direction, int framecount, int framecountX, Rectangle frame, Color? overrideColor = null, bool drawCentered = false, Vector2 overrideOrigin = default(Vector2))
        {
            Vector2 origin = ((overrideOrigin != default(Vector2)) ? overrideOrigin : new Vector2(frame.Width / framecountX / 2, texture.Height / framecount / 2));
            Color lightColor = (overrideColor.HasValue ? overrideColor.Value : Lighting.GetColor((position + new Vector2((float)width * 0.5f, (float)height * 0.5f)).ToTileCoordinates()));
            if (sb is List<DrawData>)
            {
                DrawData dd = default(DrawData);
                dd = new DrawData(texture, GetDrawPosition(position, origin, width, height, texture.Width, texture.Height, frame, framecount, framecountX, scale, drawCentered), (Rectangle?)frame, lightColor, rotation, origin, scale, (direction == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                dd.shader = shader;
                ((List<DrawData>)sb).Add(dd);
            }
            else if (sb is SpriteBatch)
            {
                bool num = shader > 0;
                if (num)
                {
                    ((SpriteBatch)sb).End();
                    ((SpriteBatch)sb).Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    GameShaders.Armor.ApplySecondary(shader, (Entity)(object)Main.player[Main.myPlayer], (DrawData?)null);
                }
                ((SpriteBatch)sb).Draw(texture, GetDrawPosition(position, origin, width, height, texture.Width, texture.Height, frame, framecount, framecountX, scale, drawCentered), frame, lightColor, rotation, origin, scale, (direction == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                if (num)
                {
                    ((SpriteBatch)sb).End();
                    ((SpriteBatch)sb).Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                }
            }
        }

        public static Vector2 GetDrawPosition(Vector2 position, Vector2 origin, int width, int height, int texWidth, int texHeight, Rectangle frame, int framecount, float scale, bool drawCentered = false)
        {
            return GetDrawPosition(position, origin, width, height, texWidth, texHeight, frame, framecount, 1, scale, drawCentered);
        }
        public static Vector2 GetDrawPosition(Vector2 position, Vector2 origin, int width, int height, int texWidth, int texHeight, Rectangle frame, int framecount, int framecountX, float scale, bool drawCentered = false)
        {
            Vector2 screenPos = new Vector2((int)Main.screenPosition.X, (int)Main.screenPosition.Y);
            if (drawCentered)
            {
                Vector2 texHalf = new Vector2(texWidth / framecountX / 2, texHeight / framecount / 2);
                return position + new Vector2(width / 2, height / 2) - (texHalf * scale) + (origin * scale) - screenPos;
            }
            return position - screenPos + new Vector2(width / 2, height) - (new Vector2(texWidth / framecountX / 2, texHeight / framecount) * scale) + (origin * scale) + new Vector2(0f, 5f);
        }

        public static Rectangle GetAdvancedFrame(int currentFrame, int frameOffsetX, int frameWidth, int frameHeight, int pixelSpaceX = 0, int pixelSpaceY = 2)
        {
            int column = (currentFrame / frameOffsetX);
            currentFrame -= (column * frameOffsetX);
            pixelSpaceY *= currentFrame;
            int startX = (frameOffsetX == 0 ? 0 : column * (frameWidth + pixelSpaceX));
            int startY = (frameHeight * currentFrame) + pixelSpaceY;
            return new Rectangle(startX, startY, frameWidth, frameHeight);
        }

        public static Rectangle GetFrame(int currentFrame, int frameWidth, int frameHeight, int pixelSpaceX = 0, int pixelSpaceY = 2)
        {
            pixelSpaceY *= currentFrame;
            int startY = (frameHeight * currentFrame) + pixelSpaceY;
            return new Rectangle(0, startY, frameWidth - pixelSpaceX, frameHeight);
        }
    }
}
