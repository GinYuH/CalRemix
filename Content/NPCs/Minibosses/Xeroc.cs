using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Graphics.Primitives;
using Terraria.Audio;
using CalRemix.Core.World;
using CalRemix.Content.Items.Materials;
using Terraria.DataStructures;
using Terraria.GameContent;
using CalamityMod.Projectiles.Boss;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Items.Fishing;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class Xeroc : ModNPC
    {

        public static SoundStyle ShotSound = new SoundStyle("CalRemix/Assets/Sounds/Xeroc/scream", 5) with { Pitch = -1 };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Avatar of Xeroc");
        }

        public override void SetDefaults()
        {
            NPC.width = 122;
            NPC.height = 182;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit8;
            NPC.DeathSound = BetterSoundID.ItemDeadlySphereVroom with {Pitch = 1};
            NPC.lifeMax = (int)(5000);
            NPC.defense = 14;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.damage = 40;
            NPC.aiStyle = -1;
        }

        public override void AI()
        {
            Vector2 vector = new Vector2(-150f, -250f);
            Vector2 vector10 = new Vector2(150f, -250f);
            Vector2 vector21 = new Vector2(0f, -350f);
            Vector2 vector29 = new Vector2(0f, -350f);
            Vector2 vector30 = new Vector2(-80f, -500f);
            float num = 0.5f;
            float num12 = 12f;
            float num23 = 40f;
            float num34 = 6400f;
            int num45 = 40;
            int num56 = 50;
            int num67 = 70;
            int num78 = 45;
            int num88 = 45;
            int num2 = 50;
            bool flag = NPC.AI_120_HallowBoss_IsInPhase2();
            bool flag5 = Main.expertMode;
            bool flag6 = flag && flag5;
            bool flag7 = NPC.ShouldEmpressBeEnraged();
            if (NPC.life == NPC.lifeMax && flag7 && !NPC.AI_120_HallowBoss_IsGenuinelyEnraged())
            {
                NPC.ai[3] += 2f;
            }
            bool flag8 = true;
            int num3 = 30;
            int num4 = 30;
            int num5 = 30;
            int num6 = 35;
            int num7 = 65;
            if (flag)
            {
                num56 = 60;
                num78 = 50;
                num88 = 50;
                num2 = 60;
                num67 = 65;
                num3 = 35;
                num4 = 35;
                num5 = 35;
                num6 = 40;
                num7 = 30;
            }
            num56 = NPC.GetAttackDamage_ForProjectiles(num56, num3);
            num78 = NPC.GetAttackDamage_ForProjectiles(num56, num3);
            num88 = NPC.GetAttackDamage_ForProjectiles(num56, num3);
            num2 = NPC.GetAttackDamage_ForProjectiles(num56, num3);
            num67 = NPC.GetAttackDamage_ForProjectiles(num56, num3);
            if (flag7)
            {
                num56 = 9999;
                num78 = 9999;
                num88 = 9999;
                num2 = 9999;
                num67 = 9999;
                flag5 = true;
            }
            float num8 = (flag5 ? 0.3f : 1f);
            bool flag9 = true;
            int num9 = 0;
            if (flag)
            {
                num9 += 15;
            }
            if (flag5)
            {
                num9 += 5;
            }
            switch ((int)NPC.ai[0])
            {
                case 0:
                    if (NPC.ai[1] == 0f)
                    {
                        NPC.velocity = new Vector2(0f, 5f);
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2(0f, -80f), Vector2.Zero, ProjectileID.HallowBossDeathAurora, 0, 0f, Main.myPlayer);
                        }
                    }
                    if (NPC.ai[1] == 10f)
                    {
                        SoundEngine.PlaySound(ShotSound, NPC.Center);
                    }
                    NPC.velocity *= 0.95f;
                    if (NPC.ai[1] > 10f && NPC.ai[1] < 150f)
                    {
                        int num64 = 2;
                        for (int m = 0; m < num64; m++)
                        {
                            float num65 = MathHelper.Lerp(1.3f, 0.7f, NPC.Opacity) * Utils.GetLerpValue(0f, 120f, NPC.ai[1], clamped: true);
                            Color newColor2 = Main.hslToRgb(NPC.ai[1] / 180f, 1f, 0.5f);
                            int num66 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0f, 0f, 0, newColor2);
                            Main.dust[num66].position = NPC.Center + Main.rand.NextVector2Circular((float)NPC.width * 3f, (float)NPC.height * 3f) + new Vector2(0f, -150f);
                            Main.dust[num66].velocity *= Main.rand.NextFloat() * 0.8f;
                            Main.dust[num66].noGravity = true;
                            Main.dust[num66].fadeIn = 0.6f + Main.rand.NextFloat() * 0.7f * num65;
                            Main.dust[num66].velocity += Vector2.UnitY * 3f;
                            Main.dust[num66].scale = 0.35f;
                            if (num66 != 6000)
                            {
                                Dust dust2 = Dust.CloneDust(num66);
                                dust2.scale /= 2f;
                                dust2.fadeIn *= 0.85f;
                                dust2.color = new Color(255, 255, 255, 255);
                            }
                        }
                    }
                    NPC.ai[1] += 1f;
                    flag8 = false;
                    flag9 = false;
                    NPC.Opacity = MathHelper.Clamp(NPC.ai[1] / 180f, 0f, 1f);
                    if (NPC.ai[1] >= 180f)
                    {
                        if (flag7 && !NPC.AI_120_HallowBoss_IsGenuinelyEnraged())
                        {
                            NPC.ai[3] += 2f;
                        }
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 0f;
                        NPC.netUpdate = true;
                        NPC.TargetClosest();
                    }
                    break;
                case 1:
                    {
                        float num28 = (flag ? 20f : 45f);
                        if (Main.getGoodWorld)
                        {
                            num28 /= 2f;
                        }
                        if (NPC.ai[1] <= 10f)
                        {
                            if (NPC.ai[1] == 0f)
                            {
                                NPC.TargetClosest();
                            }
                            NPCAimedTarget targetData6 = NPC.GetTargetData();
                            if (targetData6.Invalid)
                            {
                                NPC.ai[0] = 13f;
                                NPC.ai[1] = 0f;
                                NPC.ai[2] += 1f;
                                NPC.velocity /= 4f;
                                NPC.netUpdate = true;
                                break;
                            }
                            Vector2 center = targetData6.Center;
                            AI_120_HallowBoss_DashTo(center);
                            NPC.netUpdate = true;
                        }
                        if (NPC.velocity.Length() > 16f && NPC.ai[1] > 10f)
                        {
                            NPC.velocity /= 2f;
                        }
                        NPC.velocity *= 0.92f;
                        NPC.ai[1] += 1f;
                        if (!(NPC.ai[1] >= num28))
                        {
                            break;
                        }
                        int num29 = (int)NPC.ai[2];
                        int num30 = 2;
                        int num31 = 0;
                        if (!flag)
                        {
                            int num32 = num31++;
                            int num33 = num31++;
                            int num35 = num31++;
                            int num36 = num31++;
                            int num37 = num31++;
                            int num38 = num31++;
                            int num39 = num31++;
                            int num40 = num31++;
                            int num41 = num31++;
                            int num42 = num31++;
                            if (num29 % num31 == num32)
                            {
                                num30 = 2;
                            }
                            if (num29 % num31 == num33)
                            {
                                num30 = 8;
                            }
                            if (num29 % num31 == num35)
                            {
                                num30 = 6;
                            }
                            if (num29 % num31 == num36)
                            {
                                num30 = 8;
                            }
                            if (num29 % num31 == num37)
                            {
                                num30 = 5;
                            }
                            if (num29 % num31 == num38)
                            {
                                num30 = 2;
                            }
                            if (num29 % num31 == num39)
                            {
                                num30 = 8;
                            }
                            if (num29 % num31 == num40)
                            {
                                num30 = 4;
                            }
                            if (num29 % num31 == num41)
                            {
                                num30 = 8;
                            }
                            if (num29 % num31 == num42)
                            {
                                num30 = 5;
                            }
                            if ((float)NPC.life / (float)NPC.lifeMax <= 0.5f)
                            {
                                num30 = 10;
                            }
                        }
                        if (flag)
                        {
                            int num43 = num31++;
                            int num44 = num31++;
                            int num46 = num31++;
                            int num47 = -1;
                            if (flag5)
                            {
                                num47 = num31++;
                            }
                            int num48 = num31++;
                            int num49 = num31++;
                            int num50 = num31++;
                            int num51 = num31++;
                            int num52 = num31++;
                            int num53 = num31++;
                            if (num29 % num31 == num43)
                            {
                                num30 = 7;
                            }
                            if (num29 % num31 == num44)
                            {
                                num30 = 2;
                            }
                            if (num29 % num31 == num46)
                            {
                                num30 = 8;
                            }
                            if (num29 % num31 == num48)
                            {
                                num30 = 5;
                            }
                            if (num29 % num31 == num49)
                            {
                                num30 = 2;
                            }
                            if (num29 % num31 == num50)
                            {
                                num30 = 6;
                            }
                            if (num29 % num31 == num50)
                            {
                                num30 = 6;
                            }
                            if (num29 % num31 == num51)
                            {
                                num30 = 4;
                            }
                            if (num29 % num31 == num52)
                            {
                                num30 = 8;
                            }
                            if (num29 % num31 == num47)
                            {
                                num30 = 11;
                            }
                            if (num29 % num31 == num53)
                            {
                                num30 = 12;
                            }
                        }
                        NPC.TargetClosest();
                        NPCAimedTarget targetData7 = NPC.GetTargetData();
                        bool flag4 = false;
                        if (NPC.AI_120_HallowBoss_IsGenuinelyEnraged())
                        {
                            if (!Main.dayTime)
                            {
                                flag4 = true;
                            }
                            if (Main.dayTime && Main.time >= 53400.0)
                            {
                                flag4 = true;
                            }
                        }
                        if (targetData7.Invalid || NPC.Distance(targetData7.Center) > num34 || flag4)
                        {
                            num30 = 13;
                        }
                        if (num30 == 8 && targetData7.Center.X > NPC.Center.X)
                        {
                            num30 = 9;
                        }
                        if (flag5 && num30 != 5 && num30 != 12)
                        {
                            NPC.velocity = NPC.DirectionFrom(targetData7.Center).SafeNormalize(Vector2.Zero).RotatedBy((float)Math.PI / 2f * NPC.direction) * 20f;
                        }
                        NPC.ai[0] = num30;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] += 1f;
                        NPC.netUpdate = true;
                        break;
                    }
                case 2:
                    {
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                        }
                        float num89 = 90f - (float)num9;
                        Vector2 vector26 = new Vector2(-55f, -30f);
                        NPCAimedTarget targetData3 = NPC.GetTargetData();
                        Vector2 vector27 = (targetData3.Invalid ? NPC.Center : targetData3.Center);
                        if (NPC.Distance(vector27 + vector) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector27 + vector).SafeNormalize(Vector2.Zero) * num12, num);
                        }
                        if (NPC.ai[1] < 60f)
                        {
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + vector26, 1, Utils.GetLerpValue(0f, 60f, NPC.ai[1], clamped: true));
                        }
                        int num90 = 18;
                        if (flag5)
                        {
                            num90 = 12;
                        }
                        if ((int)NPC.ai[1] % num90 == 0 && NPC.ai[1] < 60f)
                        {
                            float ai3 = NPC.ai[1] / 60f;
                            Vector2 vector28 = new Vector2(0f, -6f).RotatedBy((float)Math.PI / 2f * NPC.direction);
                            if (flag6)
                            {
                                vector28 = new Vector2(0f, -10f).RotatedBy((float)Math.PI * 2f * Main.rand.NextFloat());
                            }
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + vector26, vector28, ModContent.ProjectileType<XerocBloodShot>(), num78, 0f, Main.myPlayer, NPC.target);
                            }
                            if (Main.netMode != 1)
                            {
                                int num91 = (int)(NPC.ai[1] / (float)num90);
                                for (int num92 = 0; num92 < 255; num92++)
                                {
                                    if (NPC.Boss_CanShootExtraAt(num92, num91 % 3, 3, 2400f))
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + vector26, vector28, ModContent.ProjectileType<XerocBloodShot>(), num78, 0f, Main.myPlayer, NPC.target);
                                    }
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 60f + num89)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 3:
                    {
                        NPC.ai[1] += 1f;
                        NPCAimedTarget targetData10 = NPC.GetTargetData();
                        Vector2 vector14 = (targetData10.Invalid ? NPC.Center : targetData10.Center);
                        if (NPC.Distance(vector14 + vector10) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector14 + vector10).SafeNormalize(Vector2.Zero) * num12, num);
                        }
                        if ((int)NPC.ai[1] % 180 == 0)
                        {
                            Vector2 vector15 = new Vector2(0f, -100f);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), targetData10.Center + vector15, Vector2.Zero, ProjectileID.HallowBossDeathAurora, num45, 0f, Main.myPlayer);
                        }
                        if (NPC.ai[1] >= 120f)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 4:
                    {
                        float num80 = 20 - num9;
                        new Vector2(0f, -100f);
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                        }
                        if (NPC.ai[1] >= 6f && NPC.ai[1] < 54f)
                        {
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + new Vector2(-55f, -20f), 2, Utils.GetLerpValue(0f, 100f, NPC.ai[1], clamped: true));
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + new Vector2(55f, -20f), 4, Utils.GetLerpValue(0f, 100f, NPC.ai[1], clamped: true));
                        }
                        NPCAimedTarget targetData2 = NPC.GetTargetData();
                        Vector2 vector20 = (targetData2.Invalid ? NPC.Center : targetData2.Center);
                        if (NPC.Distance(vector20 + vector21) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector20 + vector21).SafeNormalize(Vector2.Zero) * num12, num);
                        }
                        int num81 = 4;
                        if (flag5)
                        {
                            num81 = 5;
                        }
                        if ((int)NPC.ai[1] % 4 == 0 && NPC.ai[1] < 100f)
                        {
                            int num82 = 1;
                            for (int n = 0; n < num82; n++)
                            {
                                int num83 = (int)NPC.ai[1] / 4;
                                Vector2 vector22 = Vector2.UnitX.RotatedBy((float)Math.PI / (float)(num81 * 2) + (float)num83 * ((float)Math.PI / (float)num81) + 0f);
                                if (!flag5)
                                {
                                    vector22.X += ((vector22.X > 0f) ? 0.5f : (-0.5f));
                                }
                                vector22.Normalize();
                                float num84 = 300f;
                                if (flag5)
                                {
                                    num84 = 450f;
                                }
                                Vector2 center4 = targetData2.Center;
                                if (NPC.Distance(center4) > 2400f)
                                {
                                    continue;
                                }
                                if (Vector2.Dot(targetData2.Velocity.SafeNormalize(Vector2.UnitY), vector22) > 0f)
                                {
                                    vector22 *= -1f;
                                }
                                int num85 = 90;
                                Vector2 vector37 = center4 + targetData2.Velocity * num85;
                                Vector2 vector23 = center4 + vector22 * num84 - targetData2.Velocity * 30f;
                                if (vector23.Distance(center4) < num84)
                                {
                                    Vector2 vector24 = center4 - vector23;
                                    if (vector24 == Vector2.Zero)
                                    {
                                        vector24 = vector22;
                                    }
                                    vector23 = center4 - Vector2.Normalize(vector24) * num84;
                                }
                                Vector2 v3 = vector37 - vector23;
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vector23, vector23.DirectionFrom(NPC.Center) * 4, ProjectileType<XerocBloodShot>(), num56, 0f, Main.myPlayer, ai2: 1);
                                }
                                if (Main.netMode == 1)
                                {
                                    continue;
                                }
                                int num86 = (int)(NPC.ai[1] / 4f);
                                for (int num87 = 0; num87 < 255; num87++)
                                {
                                    if (!NPC.Boss_CanShootExtraAt(num87, num86 % 3, 3, 2400f))
                                    {
                                        continue;
                                    }
                                    Player player2 = Main.player[num87];
                                    center4 = player2.Center;
                                    if (Vector2.Dot(player2.velocity.SafeNormalize(Vector2.UnitY), vector22) > 0f)
                                    {
                                        vector22 *= -1f;
                                    }
                                    Vector2 vector38 = center4 + player2.velocity * num85;
                                    vector23 = center4 + vector22 * num84 - player2.velocity * 30f;
                                    if (vector23.Distance(center4) < num84)
                                    {
                                        Vector2 vector25 = center4 - vector23;
                                        if (vector25 == Vector2.Zero)
                                        {
                                            vector25 = vector22;
                                        }
                                        vector23 = center4 - Vector2.Normalize(vector25) * num84;
                                    }
                                    v3 = vector38 - vector23;
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vector23, Vector2.Zero, ModContent.ProjectileType<XerocBloodShot>(), num56, 0f, Main.myPlayer, ai2: 1);
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 100f + num80)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 5:
                    {
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                        }
                        float num60 = 30f;
                        num60 -= (float)num9;
                        Vector2 vector9 = new Vector2(55f, -30f);
                        Vector2 vector11 = NPC.Center + vector9;
                        if (NPC.ai[1] < 42f)
                        {
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + vector9, 3, Utils.GetLerpValue(0f, 42f, NPC.ai[1], clamped: true));
                        }
                        NPCAimedTarget targetData9 = NPC.GetTargetData();
                        Vector2 vector12 = (targetData9.Invalid ? NPC.Center : targetData9.Center);
                        if (NPC.Distance(vector12 + vector29) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector12 + vector29).SafeNormalize(Vector2.Zero) * num12, num);
                        }
                        if ((int)NPC.ai[1] % 42 == 0 && NPC.ai[1] < 42f)
                        {
                            float num61 = (float)Math.PI * 2f * Main.rand.NextFloat();
                            for (float num62 = 0f; num62 < 1f; num62 += 1f / 13f)
                            {
                                float num63 = num62;
                                Vector2 vector13 = Vector2.UnitY.RotatedBy((float)Math.PI / 2f + (float)Math.PI * 2f * num63 + num61);
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vector11 + vector13.RotatedBy(-1.5707963705062866) * 30f, vector13 * 8f, ProjectileType<XerocBloodShot>(), num88, 0f, Main.myPlayer, NPC.target);
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 42f + num60)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 6:
                    {
                        float num18 = 120 - num9;
                        Vector2 vector33 = new Vector2(0f, -100f);
                        Vector2 vector34 = NPC.Center + vector33;
                        NPCAimedTarget targetData4 = NPC.GetTargetData();
                        Vector2 vector2 = (targetData4.Invalid ? NPC.Center : targetData4.Center);
                        if (NPC.Distance(vector2 + vector30) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector2 + vector30).SafeNormalize(Vector2.Zero) * num12 * 0.3f, num * 0.7f);
                        }
                        if ((int)NPC.ai[1] % 60 == 0 && NPC.ai[1] < 180f)
                        {
                            int num19 = (int)NPC.ai[1] / 60;
                            int num20 = ((targetData4.Center.X > NPC.Center.X) ? 1 : 0);
                            float num21 = 6f;
                            if (flag5)
                            {
                                num21 = 8f;
                            }
                            float num22 = 1f / num21;
                            for (float num24 = 0f; num24 < 1f; num24 += num22)
                            {
                                float num25 = (num24 + num22 * 0.5f + (float)num19 * num22 * 0.5f) % 1f;
                                float ai = (float)Math.PI * 2f * (num25 + (float)num20);
                                // sun dance
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vector34, Vector2.Zero, 923, num2, 0f, Main.myPlayer, ai, NPC.whoAmI);
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 180f + num18)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 7:
                    {
                        float num68 = 20f;
                        float num69 = 60f;
                        float num70 = num69 * 4f;
                        if (flag5)
                        {
                            num68 = 40f;
                            num69 = 40f;
                            num70 = num69 * 6f;
                        }
                        num68 -= (float)num9;
                        NPCAimedTarget targetData11 = NPC.GetTargetData();
                        Vector2 vector16 = (targetData11.Invalid ? NPC.Center : targetData11.Center);
                        if (NPC.Distance(vector16 + vector29) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector16 + vector29).SafeNormalize(Vector2.Zero) * num12 * 0.4f, num);
                        }
                        if ((float)(int)NPC.ai[1] % num69 == 0f && NPC.ai[1] < num70)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                            Main.rand.NextFloat();
                            int num71 = (int)NPC.ai[1] / (int)num69;
                            float num72 = 13f;
                            float num73 = 150f;
                            float num74 = num72 * num73;
                            Vector2 center3 = targetData11.Center;
                            if (NPC.Distance(center3) <= 3200f)
                            {
                                Vector2 vector17 = Vector2.Zero;
                                Vector2 vector18 = Vector2.UnitY;
                                float num75 = 0.4f;
                                float num76 = 1.4f;
                                float num77 = 1f;
                                if (flag5)
                                {
                                    num72 += 5f;
                                    num73 += 50f;
                                    num77 *= 1f;
                                    num74 *= 0.5f;
                                }
                                switch (num71)
                                {
                                    case 0:
                                        center3 += new Vector2((0f - num74) / 2f, 0f) * num77;
                                        vector17 = new Vector2(0f, num74);
                                        vector18 = Vector2.UnitX;
                                        break;
                                    case 1:
                                        center3 += new Vector2(num74 / 2f, num73 / 2f) * num77;
                                        vector17 = new Vector2(0f, num74);
                                        vector18 = -Vector2.UnitX;
                                        break;
                                    case 2:
                                        center3 += new Vector2(0f - num74, 0f - num74) * num75 * num77;
                                        vector17 = new Vector2(num74 * num76, 0f);
                                        vector18 = new Vector2(1f, 1f);
                                        break;
                                    case 3:
                                        center3 += new Vector2(num74 * num75 + num73 / 2f, (0f - num74) * num75) * num77;
                                        vector17 = new Vector2((0f - num74) * num76, 0f);
                                        vector18 = new Vector2(-1f, 1f);
                                        break;
                                    case 4:
                                        center3 += new Vector2(0f - num74, num74) * num75 * num77;
                                        vector17 = new Vector2(num74 * num76, 0f);
                                        vector18 = center3.DirectionTo(targetData11.Center);
                                        break;
                                    case 5:
                                        center3 += new Vector2(num74 * num75 + num73 / 2f, num74 * num75) * num77;
                                        vector17 = new Vector2((0f - num74) * num76, 0f);
                                        vector18 = center3.DirectionTo(targetData11.Center);
                                        break;
                                }
                                for (float num79 = 0f; num79 <= 1f; num79 += 1f / num72)
                                {
                                    Vector2 origin = center3 + vector17 * (num79 - 0.5f);
                                    Vector2 v2 = vector18;
                                    if (flag5)
                                    {
                                        Vector2 vector19 = targetData11.Velocity * 20f * num79;
                                        Vector2 value2 = origin.DirectionTo(targetData11.Center + vector19);
                                        v2 = Vector2.Lerp(vector18, value2, 0.75f).SafeNormalize(Vector2.UnitY);
                                    }
                                    float ai2 = num79;
                                    if (Main.netMode != 1)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), origin, origin.DirectionTo(Main.player[NPC.target].Center), ProjectileType<XerocBloodShot>(), num67, 0f, Main.myPlayer, ai2: 1);
                                    }
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= num70 + num68)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 8:
                case 9:
                    {
                        float num26 = 20 - num9;
                        Vector2 vector3 = new Vector2(0f, -100f);
                        _ = NPC.Center + vector3;
                        flag9 = !(NPC.ai[1] >= 6f) || !(NPC.ai[1] <= 40f);
                        int num27 = ((NPC.ai[0] != 8f) ? 1 : (-1));
                        AI_120_HallowBoss_DoMagicEffect(NPC.Center, 5, Utils.GetLerpValue(40f, 90f, NPC.ai[1], clamped: true));
                        if (NPC.ai[1] <= 40f)
                        {
                            if (NPC.ai[1] == 20f)
                            {
                                SoundEngine.PlaySound(ShotSound, NPC.Center);
                            }
                            NPCAimedTarget targetData5 = NPC.GetTargetData();
                            Vector2 destination = (targetData5.Invalid ? NPC.Center : targetData5.Center) + new Vector2(num27 * -550, 0f);
                            NPC.SimpleFlyMovement(NPC.DirectionTo(destination).SafeNormalize(Vector2.Zero) * num12, num * 2f);
                            if (NPC.ai[1] == 40f)
                            {
                                NPC.velocity *= 0.3f;
                            }
                        }
                        else if (NPC.ai[1] <= 90f)
                        {
                            NPC.velocity = Vector2.Lerp(value2: new Vector2(num27 * 50, 0f), value1: NPC.velocity, amount: 0.05f);
                            if (NPC.ai[1] == 90f)
                            {
                                NPC.velocity *= 0.7f;
                            }
                            num8 *= 1.5f;
                        }
                        else
                        {
                            NPC.velocity *= 0.92f;
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 90f + num26)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 10:
                    {
                        float num93 = 20 - num9;
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                        }
                        flag9 = !(NPC.ai[1] >= 30f) || !(NPC.ai[1] <= 170f);
                        NPC.velocity *= 0.95f;
                        if (NPC.ai[1] == 90f)
                        {
                            if (NPC.ai[3] == 0f)
                            {
                                NPC.ai[3] = 1f;
                            }
                            if (NPC.ai[3] == 2f)
                            {
                                NPC.ai[3] = 3f;
                            }
                            NPC.Center = NPC.GetTargetData().Center + new Vector2(0f, -250f);
                            NPC.netUpdate = true;
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 180f + num93)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.ai[2] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 11:
                    {
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                        }
                        float num54 = 20 - num9;
                        new Vector2(0f, -100f);
                        if (NPC.ai[1] >= 6f && NPC.ai[1] < 54f)
                        {
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + new Vector2(-55f, -20f), 2, Utils.GetLerpValue(0f, 100f, NPC.ai[1], clamped: true));
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + new Vector2(55f, -20f), 4, Utils.GetLerpValue(0f, 100f, NPC.ai[1], clamped: true));
                        }
                        NPCAimedTarget targetData8 = NPC.GetTargetData();
                        Vector2 vector4 = (targetData8.Invalid ? NPC.Center : targetData8.Center);
                        if (NPC.Distance(vector4 + vector21) > num23)
                        {
                            NPC.SimpleFlyMovement(NPC.DirectionTo(vector4 + vector21).SafeNormalize(Vector2.Zero) * num12, num);
                        }
                        if ((int)NPC.ai[1] % 3 == 0 && NPC.ai[1] < 100f)
                        {
                            int num55 = 1;
                            for (int k = 0; k < num55; k++)
                            {
                                Vector2 vector5 = -targetData8.Velocity;
                                vector5.SafeNormalize(-Vector2.UnitY);
                                float num57 = 100f;
                                Vector2 center2 = targetData8.Center;
                                if (NPC.Distance(center2) > 2400f)
                                {
                                    continue;
                                }
                                int num58 = 90;
                                Vector2 vector35 = center2 + targetData8.Velocity * num58;
                                Vector2 vector6 = center2 + vector5 * num57;
                                if (vector6.Distance(center2) < num57)
                                {
                                    Vector2 vector7 = center2 - vector6;
                                    if (vector7 == Vector2.Zero)
                                    {
                                        vector7 = vector5;
                                    }
                                    vector6 = center2 - Vector2.Normalize(vector7) * num57;
                                }
                                Vector2 v = vector35 - vector6;
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vector6, vector6.DirectionTo(Main.player[NPC.target].Center), ProjectileType<XerocBloodShot>(), num56, 0f, Main.myPlayer, ai2: 1);
                                }
                                if (Main.netMode == 1)
                                {
                                    continue;
                                }
                                int num59 = (int)(NPC.ai[1] / 3f);
                                for (int l = 0; l < 255; l++)
                                {
                                    if (!NPC.Boss_CanShootExtraAt(l, num59 % 3, 3, 2400f))
                                    {
                                        continue;
                                    }
                                    Player player = Main.player[l];
                                    vector5 = -player.velocity;
                                    vector5.SafeNormalize(-Vector2.UnitY);
                                    num57 = 100f;
                                    center2 = player.Center;
                                    num58 = 90;
                                    Vector2 vector36 = center2 + player.velocity * num58;
                                    vector6 = center2 + vector5 * num57;
                                    if (vector6.Distance(center2) < num57)
                                    {
                                        Vector2 vector8 = center2 - vector6;
                                        if (vector8 == Vector2.Zero)
                                        {
                                            vector8 = vector5;
                                        }
                                        vector6 = center2 - Vector2.Normalize(vector8) * num57;
                                    }
                                    v = vector36 - vector6;
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vector6, vector6.DirectionTo(Main.player[NPC.target].Center), ProjectileType<XerocBloodShot>(), num56, 0f, Main.myPlayer, ai2: 1);
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 100f + num54)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 12:
                    {
                        float num14 = 90f - (float)num9;
                        Vector2 vector31 = new Vector2(-55f, -30f);
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                            NPC.velocity = new Vector2(0f, -12f);
                        }
                        NPC.velocity *= 0.95f;
                        bool flag3 = NPC.ai[1] < 60f && NPC.ai[1] >= 10f;
                        if (flag3)
                        {
                            AI_120_HallowBoss_DoMagicEffect(NPC.Center + vector31, 1, Utils.GetLerpValue(0f, 60f, NPC.ai[1], clamped: true));
                        }
                        int num15 = 6;
                        if (flag5)
                        {
                            num15 = 4;
                        }
                        float num16 = (NPC.ai[1] - 10f) / 50f;
                        if ((int)NPC.ai[1] % num15 == 0 && flag3)
                        {
                            _ = NPC.ai[1] / 60f;
                            Vector2 vector32 = (vector32 = new Vector2(0f, -10f).RotatedBy((float)Math.PI * 2f * num16));
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + vector31, vector32, ProjectileType<XerocBloodShot>(), num78, 0f, Main.myPlayer, NPC.target);
                            }
                            if (Main.netMode != 1)
                            {
                                int num17 = (int)(NPC.ai[1] % (float)num15);
                                for (int j = 0; j < 255; j++)
                                {
                                    if (NPC.Boss_CanShootExtraAt(j, num17 % 3, 3, 2400f))
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + vector31, vector32, ProjectileType<XerocBloodShot>(), num78, 0f, Main.myPlayer, NPC.target);
                                    }
                                }
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (NPC.ai[1] >= 60f + num14)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        break;
                    }
                case 13:
                    {
                        new Vector2(-55f, -30f);
                        if (NPC.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(ShotSound, NPC.Center);
                            NPC.velocity = new Vector2(0f, -7f);
                        }
                        NPC.velocity *= 0.95f;
                        NPC.TargetClosest();
                        NPCAimedTarget targetData = NPC.GetTargetData();
                        flag8 = false;
                        bool flag10 = false;
                        bool flag11 = false;
                        if (!flag10)
                        {
                            if (NPC.AI_120_HallowBoss_IsGenuinelyEnraged())
                            {
                                if (!Main.dayTime)
                                {
                                    flag11 = true;
                                }
                                if (Main.dayTime && Main.time >= 53400.0)
                                {
                                    flag11 = true;
                                }
                            }
                            flag10 = flag10 || flag11;
                        }
                        if (!flag10)
                        {
                            bool flag12 = targetData.Invalid || NPC.Distance(targetData.Center) > num34;
                            flag10 = flag10 || flag12;
                        }
                        NPC.alpha = Utils.Clamp(NPC.alpha + NPC.direction * 5, 0, 255);
                        bool flag2 = NPC.alpha == 0 || NPC.alpha == 255;
                        int num10 = 5;
                        for (int i = 0; i < num10; i++)
                        {
                            float num11 = MathHelper.Lerp(1.3f, 0.7f, NPC.Opacity);
                            Color newColor = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
                            int num13 = Dust.NewDust(NPC.position - NPC.Size * 0.5f, NPC.width * 2, NPC.height * 2, DustID.Blood, 0f, 0f, 0, newColor);
                            Main.dust[num13].position = NPC.Center + Main.rand.NextVector2Circular(NPC.width, NPC.height);
                            Main.dust[num13].velocity *= Main.rand.NextFloat() * 0.8f;
                            Main.dust[num13].noGravity = true;
                            Main.dust[num13].scale = 0.9f + Main.rand.NextFloat() * 1.2f;
                            Main.dust[num13].fadeIn = 0.4f + Main.rand.NextFloat() * 1.2f * num11;
                            Main.dust[num13].velocity += Vector2.UnitY * -2f;
                            Main.dust[num13].scale = 0.35f;
                            if (num13 != 6000)
                            {
                                Dust dust = Dust.CloneDust(num13);
                                dust.scale /= 2f;
                                dust.fadeIn *= 0.85f;
                                dust.color = new Color(255, 255, 255, 255);
                            }
                        }
                        NPC.ai[1] += 1f;
                        if (!(NPC.ai[1] >= 20f && flag2))
                        {
                            break;
                        }
                        if (NPC.alpha == 255)
                        {
                            NPC.active = false;
                            if (Main.netMode != 1)
                            {
                                NetMessage.SendData(23, -1, -1, null, NPC.whoAmI);
                            }
                            return;
                        }
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 0f;
                        NPC.netUpdate = true;
                        break;
                    }
            }
            NPC.dontTakeDamage = !flag9;
            NPC.damage = NPC.GetAttackDamage_ScaledByStrength((float)NPC.defDamage * num8);
            if (flag7)
            {
                NPC.damage = 9999;
            }
            if (flag)
            {
                NPC.defense = (int)((float)NPC.defDefense * 1.2f);
            }
            else
            {
                NPC.defense = NPC.defDefense;
            }
            if ((NPC.localAI[0] += 1f) >= 44f)
            {
                NPC.localAI[0] = 0f;
            }
            if (flag8)
            {
                NPC.alpha = Utils.Clamp(NPC.alpha - 5, 0, 255);
            }
            Lighting.AddLight(NPC.Center, Vector3.One * NPC.Opacity);
        }

        private void AI_120_HallowBoss_DoMagicEffect(Vector2 spot, int effectType, float progress)
        {
            float num = 4f;
            float num2 = 1f;
            float fadeIn = 0f;
            float num3 = 0.5f;
            int num4 = 2;
            int num5 = 267;
            switch (effectType)
            {
                case 1:
                    num2 = 0.5f;
                    fadeIn = 2f;
                    num3 = 0f;
                    break;
                case 2:
                case 4:
                    num = 50f;
                    num2 = 0.5f;
                    fadeIn = 0f;
                    num3 = 0f;
                    num4 = 4;
                    break;
                case 3:
                    num = 30f;
                    num2 = 0.1f;
                    fadeIn = 2.5f;
                    num3 = 0f;
                    break;
                case 5:
                    if (progress == 0f)
                    {
                        num4 = 0;
                    }
                    else
                    {
                        num4 = 5;
                        num5 = Main.rand.Next(86, 92);
                    }
                    if (progress >= 1f)
                    {
                        num4 = 0;
                    }
                    break;
            }
            for (int i = 0; i < num4; i++)
            {
                Dust dust = Dust.NewDustPerfect(spot, DustID.TheDestroyer, Main.rand.NextVector2CircularEdge(num, num) * (Main.rand.NextFloat() * (1f - num3) + num3), 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), (Main.rand.NextFloat() * 2f + 2f) * num2);
                dust.fadeIn = fadeIn;
                dust.noGravity = true;
                switch (effectType)
                {
                    case 2:
                    case 4:
                        {
                            dust.velocity *= 0.005f;
                            dust.scale = 3f * Utils.GetLerpValue(0.7f, 0f, progress, clamped: true) * Utils.GetLerpValue(0f, 0.3f, progress, clamped: true);
                            dust.velocity = ((float)Math.PI * 2f * ((float)i / 4f) + (float)Math.PI / 4f).ToRotationVector2() * 8f * Utils.GetLerpValue(1f, 0f, progress, clamped: true);
                            dust.velocity += NPC.velocity * 0.3f;
                            float num6 = 0f;
                            if (effectType == 4)
                            {
                                num6 = 0.5f;
                            }
                            dust.color = Main.hslToRgb(((float)i / 5f + num6 + progress * 0.5f) % 1f, 1f, 0.5f);
                            dust.color.A /= 2;
                            dust.alpha = 127;
                            break;
                        }
                    case 5:
                        if (progress == 0f)
                        {
                            dust.customData = this;
                            dust.scale = 1.5f;
                            dust.fadeIn = 0f;
                            dust.velocity = new Vector2(0f, -1f) + Main.rand.NextVector2Circular(1f, 1f);
                            dust.color = new Color(255, 255, 255, 80) * 0.3f;
                        }
                        else
                        {
                            dust.color = Main.hslToRgb(progress * 2f % 1f, 1f, 0.5f);
                            dust.alpha = 0;
                            dust.scale = 1f;
                            dust.fadeIn = 1.3f;
                            dust.velocity *= 3f;
                            dust.velocity.X *= 0.1f;
                            dust.velocity += NPC.velocity * 1f;
                        }
                        break;
                }
            }
        }

        private void AI_120_HallowBoss_DashTo(Vector2 targetPosition)
        {
            NPC.DirectionTo(targetPosition);
            targetPosition += new Vector2(0f, -300f);
            if (NPC.Distance(targetPosition) > 200f)
            {
                targetPosition -= NPC.DirectionTo(targetPosition) * 100f;
            }
            Vector2 vector = targetPosition - NPC.Center;
            float lerpValue = Utils.GetLerpValue(100f, 600f, vector.Length(), clamped: true);
            float num = vector.Length();
            if (num > 18f)
            {
                num = 18f;
            }
            NPC.velocity = Vector2.Lerp(vector.SafeNormalize(Vector2.Zero) * num, vector / 6f, lerpValue);
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value),
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.bloodMoon || !Main.hardMode)
                return 0f;
            if (NPC.CountNPCS(Type) > 1)
                return 0f;

            return SpawnCondition.OverworldNightMonster.Chance * 0.0022f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<GrandioseGland>(), 1, 4, 8);
            npcLoot.Add(ItemType<Gorecodile>(), 1, 8, 19);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + NPC.gfxOffY * Vector2.UnitY, NPC.frame, drawColor, NPC.rotation, new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, TextureAssets.Npc[NPC.type].Height() / 2), NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}