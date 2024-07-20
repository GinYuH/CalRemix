using Terraria.ModLoader;
using CalamityMod;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Accessories;
using CalRemix.Projectiles.Hostile;
using CalRemix.NPCs.Bosses.Wulfwyrm;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using static Terraria.ModLoader.ModContent;
using CalRemix.Tiles.PlaguedJungle;
using System;
using Terraria.ID;
using CalRemix.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.NPCs.Bosses.Oxygen;
using Terraria.Audio;
using CalRemix.World;

namespace CalRemix.Projectiles
{
    public class CalRemixProjectile : GlobalProjectile
    {
        public bool nihilicArrow = false;
        public bool rogueclone = false;
        public bool tvoproj = false;
        public bool uniproj = false;
        public bool hyperCharged = false;
        public int eye = 0;
        public int bladetimer = 0;
        public bool splitExplosive = false;
        NPC exc;
        public override bool InstancePerEntity => true;

        public override void SetStaticDefaults()
        {
            Main.projFrames[ProjectileType<MutatedTruffleMinion>()] = 1;
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bladetimer--;
            if (modPlayer.brimPortal && nihilicArrow && !projectile.minion && !projectile.sentry && projectile.velocity != Vector2.Zero)
                CalamityUtils.HomeInOnNPC(projectile, false, 2500, 10f, 1);
            if (modPlayer.tvo && tvoproj)
            {
                if (projectile.type == ProjectileType<RainbowComet>())
                {
                    CalamityUtils.HomeInOnNPC(projectile, true, 1200, 20, 1);
                }
            }
            if (projectile.type == ProjectileID.PureSpray)
            {
                PlagueToPureConvert((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
            }
            if (projectile.type == ProjectileID.CorruptSpray || projectile.type == ProjectileID.CrimsonSpray || projectile.type == ProjectileID.HallowSpray || projectile.type == ProjectileType<AstralSpray>() || projectile.type == ProjectileID.MushroomSpray)
            {
                PlagueToNeutralConvert((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
            }
            if (ModLoader.TryGetMod("NoxusBoss", out Mod nox))
            {
                int noxType = nox.Find<ModProjectile>("NoxusSprayerGas").Type;
                int i = (int)(projectile.position.X + projectile.width / 2) / 16;
                int j = (int)(projectile.position.Y + projectile.height / 2) / 16;
                if (projectile.type == noxType)
                {
                    for (int k = i - 4; k <= i + 4; k++)
                    {
                        for (int l = j - 4; l <= j + 4; l++)
                        {
                            if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(4 * 4 + 4 * 4))
                            {
                                int type = Main.tile[k, l].TileType;
                                if (type == TileType<MeldGunkPlaced>())
                                {
                                    Main.tile[k, l].TileType = TileID.Stone;
                                    WorldGen.SquareTileFrame(k, l, true);
                                }
                            }
                        }
                    }
                }
            }
            if (projectile.type == ProjectileType<MurasamaSlash>())
            {
                projectile.scale = 4f;
                projectile.width = ContentSamples.ProjectilesByType[ProjectileType<MurasamaSlash>()].width * 4;
                projectile.height = ContentSamples.ProjectilesByType[ProjectileType<MurasamaSlash>()].height * 4;
            }
            if (projectile.type == ProjectileType<MutatedTruffleMinion>())
            {
                projectile.frame = 0;

            }

            if (CalRemixWorld.oxydayTime > 0 && projectile.Center.Y < Main.worldSurface * 16.0 && Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16] != null && Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16].WallType == 0 && (projectile.velocity.X > 0f && Main.windSpeedCurrent < 0f || projectile.velocity.X < 0f && Main.windSpeedCurrent > 0f || Math.Abs(projectile.velocity.X) < Math.Abs(Main.windSpeedCurrent * Main.windPhysicsStrength) * 180f) && Math.Abs(projectile.velocity.X) < 16f)
            {
                projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
                MathHelper.Clamp(projectile.velocity.X, -222f, 222f);
            }
            if (CalRemixWorld.oxydayTime > 0)
            {
                if (ProjectileID.Sets.IsAGolfBall[projectile.type] && projectile.position.Y < 656 && !NPC.AnyNPCs(NPCType<Oxygen>()))
                {
                    SoundEngine.PlaySound(SoundID.Shatter with { Volume = 1 });
                    projectile.Kill();
                    NPC.NewNPC(projectile.GetSource_FromThis(), (int)projectile.position.X, 656, NPCType<Oxygen>());
                }
            }
            if (projectile.minion || projectile.sentry || projectile.hostile || !projectile.friendly || projectile.damage <= 0)
                return;
            if (modPlayer.pearl)
                CalamityUtils.HomeInOnNPC(projectile, false, 320, projectile.velocity.Length(), 1);
            eye++;
            if (modPlayer.astralEye && eye % 120 == 0 && eye > 0 && projectile.type != ProjectileType<HomingAstralFireball>())
                Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, projectile.velocity * 0.75f, ProjectileType<HomingAstralFireball>(), 10, 0, projectile.owner);
        }

        public static void PlagueToPureConvert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        if (type == TileType<PlaguedGrass>())
                        {
                            Main.tile[k, l].TileType = TileID.JungleGrass;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedMud>())
                        {
                            Main.tile[k, l].TileType = TileID.Mud;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<Sporezol>())
                        {
                            Main.tile[k, l].TileType = TileID.Copper;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedStone>())
                        {
                            Main.tile[k, l].TileType = TileID.Stone;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedMudWall>() || wall == WallType<PlaguedMudWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.MudUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedHive>())
                        {
                            Main.tile[k, l].TileType = TileID.Hive;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedSilt>())
                        {
                            Main.tile[k, l].TileType = TileID.Silt;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedSand>())
                        {
                            Main.tile[k, l].TileType = TileID.Sand;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedPipe>())
                        {
                            Main.tile[k, l].TileType = TileID.RichMahogany;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedPipeWall>())
                        {
                            Main.tile[k, l].WallType = WallID.RichMaogany;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedHiveWall>())
                        {
                            Main.tile[k, l].WallType = WallID.HiveUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedStone>())
                        {
                            Main.tile[k, l].TileType = TileID.Stone;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedVineWall>() || wall == WallType<PlaguedVineWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.GrassUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedStoneWall>() || wall == WallType<PlaguedStoneWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.Stone;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedClay>())
                        {
                            Main.tile[k, l].TileType = TileID.ClayBlock;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                    }
                }
            }
        }


        public static void PlagueToNeutralConvert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        if (type == TileType<PlaguedMud>())
                        {
                            Main.tile[k, l].TileType = TileID.Mud;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<Sporezol>())
                        {
                            Main.tile[k, l].TileType = TileID.Copper;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedMudWall>() || wall == WallType<PlaguedMudWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.MudUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedHive>())
                        {
                            Main.tile[k, l].TileType = TileID.Hive;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedSilt>())
                        {
                            Main.tile[k, l].TileType = TileID.Silt;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == WallType<PlaguedHiveWall>())
                        {
                            Main.tile[k, l].WallType = WallID.HiveUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == TileType<PlaguedClay>())
                        {
                            Main.tile[k, l].TileType = TileID.ClayBlock;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            var source = projectile.GetSource_FromThis();
            if (modPlayer.tvo && CalamityUtils.CountProjectiles(ProjectileType<PlagueSeeker>()) > 3 && projectile.type == ProjectileType<PlagueSeeker>())
            {
                projectile.active = false;
            }
            if (modPlayer.arcanumHands && projectile.type != ProjectileType<ArmofAgony>() && CalamityUtils.CountProjectiles(ProjectileType<ArmofAgony>()) < 8)
            {
                target.AddBuff(BuffType<BrimstoneFlames>(), 180);
                int apparatusDamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(260);
                int proj = Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ProjectileType<ArmofAgony>(), apparatusDamage, 4f, projectile.owner);
                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].originalDamage = apparatusDamage;
                }
                Main.projectile[proj].DamageType = DamageClass.Summon;
            }
            if (modPlayer.tvo && projectile.type != ProjectileType<JewelSpike>())
            {
                target.AddBuff(BuffType<BrimstoneFlames>(), 180);
                int apparatusDamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(1060);
                if (CalamityUtils.CountProjectiles(ProjectileType<JewelSpike>()) < 3)
                {
                    int proj = Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ProjectileType<JewelSpike>(), apparatusDamage, 4f, projectile.owner);
                    if (proj.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].originalDamage = apparatusDamage;
                    }
                    Main.projectile[proj].DamageType = DamageClass.Summon;
                    Main.projectile[proj].GetGlobalProjectile<CalRemixProjectile>().tvoproj = true;
                }
            }
            if (modPlayer.roguebox && projectile.Calamity().stealthStrike && player.ownedProjectileCounts[ProjectileType<DarksunTornado>()] <= 1)
            {
                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ProjectileType<DarksunTornado>(), 20000, 0, Main.LocalPlayer.whoAmI);
                if (p.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[p].originalDamage = 20000;
                }
            }
            if (modPlayer.tvo && projectile.type != ProjectileType<DarksunTornado>() && projectile.type != ProjectileType<NanoFlare>())
            {
                int dam = (int)(projectile.damage * 0.2f);
                if (CalamityUtils.CountProjectiles(ProjectileType<DarksunTornado>()) < 3)
                {
                    int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ProjectileType<DarksunTornado>(), dam, 0, projectile.owner);
                    if (p.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[p].originalDamage = dam;
                    }
                }
                if (CalamityUtils.CountProjectiles(ProjectileType<DarksunTornado>()) < 2)
                    CalamityUtils.ProjectileRain(projectile.GetSource_FromAI(), target.Center, 300, 20, -500, -800, 10, ProjectileType<NanoFlare>(), dam, 0, projectile.owner);
            }
            if (modPlayer.godfather && projectile.type == ProjectileType<CosmicBlast>())
            {
                Main.player[projectile.owner].Heal(5);
            }
            if (modPlayer.crystalconflict && projectile.type == ProjectileType<CryonicShield>())
            {
                target.AddBuff(BuffType<GodSlayerInferno>(), 60);
                if (modPlayer.tvo)
                {
                    target.AddBuff(BuffType<GlacialState>(), 60);
                }
            }
            if (modPlayer.wormMeal && projectile.DamageType == DamageClass.Summon)
            {
                if (Main.rand.NextBool(20))
                {
                    target.AddBuff(BuffID.Confused, 480);
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            CalRemixPlayer p = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (p.hydrogenSoul)
            {
                if (ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[projectile.type] && projectile.type != ProjectileID.Grenade && projectile.type != ProjectileID.StickyGrenade && projectile.type != ProjectileID.PartyGirlGrenade && projectile.type != ProjectileID.Beenade)
                {
                    modifiers.FinalDamage *= 50;
                }
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            CalRemixPlayer pe = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (pe.hydrogenSoul)
            {
                if (!splitExplosive && ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[projectile.type])
                {
                    int p = Projectile.NewProjectile(projectile.GetSource_Death(), projectile.Center, Vector2.UnitX * 4, projectile.type, projectile.damage / 2, projectile.knockBack, projectile.owner);
                    Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().splitExplosive = true;
                    int q = Projectile.NewProjectile(projectile.GetSource_Death(), projectile.Center, Vector2.UnitX * -4, projectile.type, projectile.damage / 2, projectile.knockBack, projectile.owner);
                    Main.projectile[q].GetGlobalProjectile<CalRemixProjectile>().splitExplosive = true;

                }
            }
        }

        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            if (modPlayer.crystalconflict)
            {
                if (projectile.type == ProjectileType<CryonicShield>())
                    return Color.Magenta;
                if (uniproj && (projectile.type == ProjectileType<GalileosPlanet>()
                    || projectile.type == ProjectileType<CosmicBlast>()
                    || projectile.type == ProjectileType<EndoIceShard>()))
                    return Color.HotPink;
                return null;
            }
            if (modPlayer.tvo && tvoproj)
            {
                if (projectile.type == ProjectileType<JewelSpike>())
                {
                    return Color.Tan;
                }
                return null;
            }
            if (modPlayer.tvo && projectile.type == ProjectileType<EclipseMirrorBurst>())
            {
                projectile.damage = 1000000;
                return Color.LightBlue;
            }
            return null;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (NPC.AnyNPCs(NPCType<WulfwyrmHead>()))
            {
                NPC exc = Main.npc[CalRemixGlobalNPC.wulfyrm];
                if (projectile.type == ProjectileType<ExcavatorShot>() && exc.ModNPC<WulfwyrmHead>().DeathCharge) // not even gonna bother iterating through npcs since literally no other entity uses this projectile
                {
                    hyperCharged = true;
                }
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (hyperCharged)
                target.AddBuff(BuffType<GlacialState>(), 50);
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileType<MutatedTruffleMinion>())
            {
                Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
                MutatedTruffleMinion m = projectile.ModProjectile<MutatedTruffleMinion>();
                Vector2 drawPosition = projectile.Center - Main.screenPosition;
                Rectangle frame = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
                Vector2 origin = frame.Size() * 0.5f;
                float drawRotation = projectile.rotation + (projectile.spriteDirection == -1 && m.State != 0 ? MathHelper.Pi : 0f);
                SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                if (CalamityConfig.Instance.Afterimages && (projectile.ai[0] == 1 || projectile.ai[0] == 3))
                {
                    for (int i = 0; i < projectile.oldPos.Length; i++)
                    {
                        Color afterimageDrawColor = Color.Green with { A = 25 } * projectile.Opacity * (1f - i / (float)projectile.oldPos.Length);
                        Vector2 afterimageDrawPosition = projectile.oldPos[i] + projectile.Size * 0.5f - Main.screenPosition;
                        Main.EntitySpriteDraw(texture, afterimageDrawPosition, frame, afterimageDrawColor, drawRotation, origin, projectile.scale, effects, 0);
                    }
                }

                Main.EntitySpriteDraw(texture, drawPosition, frame, projectile.GetAlpha(lightColor), drawRotation, origin, projectile.scale, effects, 0);
                return false;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
    }
}