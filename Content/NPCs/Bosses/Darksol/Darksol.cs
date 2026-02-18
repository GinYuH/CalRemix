using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Darksol;

public class Darksol : ModNPC
{
    private float Time = 0;
    private float AttackTime = 0;
    private float AttackStartTime = 0;
    private float BreakAmount = 0f;
    private Player Target = null;

    internal enum DarksolAIState
    {
        TestAI,
        //Phase 1
        SpawnAnimation,
        DarkSpears,
        DarkEnergyTorrent,
        DarkMatterSquad,
        DarkEnergySpin,
        SneakyCure, //When under 20,000 HP he has a chance to heal himself for 6,000 - 9,999 HP with his Cure move, the chances of him using it become higher the lower his HP is. Even if he uses this attack he still has a 50% chance of doing a regular attack immediately after it.
        SpinDashes,
        DarkMeteor,
        DarkStormCall,
        FleeAnimation,
        //Phase 2
        Phase2Transition,
        TeleportDashes,
        LaserBarrage,
        DarkEnergyBurst,
        TheTwelfthAttack,
        FinalAttackInstaKiller, //When reduced to 0 HP he will unleash one final move that needs to be avoided in order to win.  If this move hits you even once you will die and he will despawn.
        DeathAnimation
    }
    DarksolAIState AIState = DarksolAIState.SpawnAnimation;

    internal static Dictionary<DarksolAIState, float> AttackWeights = [];

    internal Vector2 AttackVector = Vector2.Zero;
    internal bool AttackBool = false;
    internal int AttackSign = 1;
    internal float StoredTime = 0;
    internal float StoredRatio = 0f;

    private enum DarkOrbID
    {
        Standard = -1,
        ArcLeft,
        ArcRight,
        FluctuateSpeed,
        WiggleLeft,
        WiggleRight,
        FluctuateSpeedExtreme,
        SplitShotMix,
        SplitShot,
        SplitShotDistanceSpread,
        BackAndForth1Step,
        BackAndForth2Step,
        TurnAroundAtSomePoint,
        TurnAround,
        UpwardTurnAround
    }

    public override void SetDefaults()
    {
        NPC.width = 256;
        NPC.height = 256;
        NPC.damage = 200;
        NPC.LifeMaxNERB(5553500, 5553500, 5553500);
        NPC.DR_NERD(0, 0, 0, 0);
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.npcSlots = 10f;
        NPC.netAlways = true;
        //Music = -1;
        //SceneEffectPriority = SceneEffectPriority.None;
    }

    public override void OnSpawn(IEntitySource source)
    {
        GetTarget();

        NPC.Center = Target.Center - Vector2.UnitY * 600f;
    }

    public override void AI()
    {
        float HealthRatio = NPC.life / (float)NPC.lifeMax;
        Time++;

        if(HealthRatio < 0.75f && Time % 30 == 0)
        {
            Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.NextFloat(-800f, 800f), -600f), new Vector2(0, 6), ModContent.ProjectileType<DarkSpear>(), 100, 1f, ai0: 1, ai1: MathHelper.PiOver2);
        }

        switch (AIState)
        {
            case DarksolAIState.TestAI:
                NPC.velocity = (Main.MouseWorld - NPC.Center) / 30f;
                NPC.rotation *= 0.9f;
                NPC.rotation -= NPC.velocity.X / 128f;

                GetTarget();
                /*
                if (AttackTime % 60 == 0)
                {
                    Vector2 dir = NPC.DirectionTo(Target.Center);
                    float rot = dir.ToRotation();
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * 5f, ModContent.ProjectileType<DarkOrb>(), 100, 0f, ai0: (float)DarkOrbID.WiggleLeft, ai1: rot + MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * 5f, ModContent.ProjectileType<DarkOrb>(), 100, 0f, ai0: (float)DarkOrbID.WiggleRight, ai1: rot + MathHelper.PiOver2);
                }
                */
                break;
            case DarksolAIState.SpawnAnimation:
                if (AttackTime <= 90f)
                    NPC.Center = Target.Center - Vector2.Lerp(Vector2.UnitY * 600f, Vector2.UnitY * 256f, Time / 90f);
                else
                {
                    PickAttack();
                    return;
                }
                break;
            case DarksolAIState.DarkMatterSquad:
                float dist = 210;
                Vector2 fromTarget = (NPC.Center - Target.Center).SafeNormalize(Vector2.UnitX);
                Vector2 goalPos = Target.Center + (fromTarget * dist) - (Vector2.UnitY * 32);
                if (NPC.DistanceSQ(goalPos) <= 4096)
                    NPC.velocity *= 0.9f;
                else if (NPC.velocity.LengthSquared() < 144f)
                    NPC.velocity += NPC.DirectionTo(goalPos).SafeNormalize(Vector2.Zero) * 0.1f;
                else
                    NPC.velocity = NPC.DirectionTo(goalPos).SafeNormalize(Vector2.Zero) * 12f;

                if (AttackTime % 60 == 0)
                {
                    if (AttackTime >= 300)
                    {
                        NPC.velocity = Vector2.Zero;
                        PickAttack();
                        return;
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DarkMatter>());
                    }
                }

                break;
            case DarksolAIState.DarkSpears:
                if(AttackTime <= 90)
                {
                    if (AttackTime % 5 == 0)
                    {
                        float rot = MathHelper.Pi / 90f * AttackTime;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -10 * Vector2.UnitY.RotatedBy(rot - MathHelper.PiOver2), ModContent.ProjectileType<DarkSpear>(), 100, 1f, ai1: -(MathHelper.PiOver2 + MathHelper.PiOver4));
                    }
                }
                else if (AttackTime > 180 && AttackTime < 600)
                {
                    dist = 210;
                    fromTarget = (NPC.Center - Target.Center).SafeNormalize(Vector2.UnitX);
                    goalPos = Target.Center + (fromTarget * dist) - (Vector2.UnitY * 32);
                    if (NPC.DistanceSQ(goalPos) <= 4096)
                        NPC.velocity *= 0.9f;
                    else if (NPC.velocity.LengthSquared() < 144f)
                        NPC.velocity += NPC.DirectionTo(goalPos).SafeNormalize(Vector2.Zero) * 0.1f;
                    else
                        NPC.velocity = NPC.DirectionTo(goalPos).SafeNormalize(Vector2.Zero) * 12f;

                    if (AttackTime % 10 == 0)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.NextFloat(-800f, 800f), -600f), new Vector2(3, 3), ModContent.ProjectileType<DarkSpear>(), 100, 1f, ai0: 1, ai1: MathHelper.PiOver4);

                    if (AttackTime % 30 == 0)
                    {
                        Vector2 dir = NPC.DirectionTo(Target.Center);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * 16f, ModContent.ProjectileType<DarkSpear>(), 100, 1f, ai0: 1, ai1: dir.ToRotation());
                    }
                }
                else if(AttackTime >= 600)
                {
                    PickAttack();
                    return;
                }
                break;
            case DarksolAIState.DarkEnergyTorrent:
                NPC.velocity *= 0.6f;

                if(AttackTime >= 120)
                {
                    if(AttackTime % 30 == 0)
                    {
                        if (AttackTime >= 500)
                        {
                            PickAttack();
                            return;
                        }
                        else
                        {
                            int projCount = 20;

                            for (int i = 0; i < projCount; i++)
                            {
                                float startRot = MathHelper.TwoPi / projCount * i;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkEnergyBolt>(), 100, 1f, ai0: startRot, ai1: AttackTime % 60 == 0 ? -1 : 1);
                            }
                        }
                    }
                }
                break;
            case DarksolAIState.DarkEnergySpin:
                if (AttackTime < 360)
                {
                    if (AttackTime == 0)
                        AttackVector = NPC.DirectionFrom(Target.Center);
                    NPC.Center = Target.Center + AttackVector.RotatedBy(AttackTime / 30f) * 440;
                    if (AttackTime % 15 == 0)
                    {
                        Vector2 dir = NPC.DirectionTo(Target.Center);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * 16f, ModContent.ProjectileType<DarkSpear>(), 100, 1f, ai0: 1, ai1: dir.ToRotation());
                    }
                }
                if (AttackTime > 500)
                {
                    PickAttack();
                    return;
                }
                break;
            case DarksolAIState.SpinDashes:
                if (AttackTime == 0)
                {
                    AttackBool = false;
                    AttackSign = NPC.Center.X < Target.Center.X ? 1 : -1;
                }

                if (AttackTime <= 45)
                {
                    NPC.rotation += MathHelper.Lerp(0f, 0.1f, AttackTime / 45f);
                    if (AttackTime == 45)
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 72f;
                }
                else if (AttackTime < 120)
                {
                    NPC.velocity *= 0.925f;
                    if(AttackTime <= 90)
                        NPC.rotation += MathHelper.Lerp(0.1f, 0f, (AttackTime - 45) / 45f);
                    if (AttackTime >= 90 && AttackBool)
                    {
                        NPC.velocity = Vector2.Zero;
                        AttackTime = 0;
                        return;
                    }
                }
                else if (AttackTime == 120)
                {
                    float rotation = 240f;

                    Vector2 laserVelocity = Target.Center - NPC.Center;
                    laserVelocity.Normalize();

                    float beamDirection = -1f;
                    if (laserVelocity.X < 0f)
                        beamDirection = 1f;

                    laserVelocity = laserVelocity.RotatedBy(-beamDirection * MathHelper.PiOver2);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, laserVelocity, ModContent.ProjectileType<DarkBeam>(), 200, 0f, -1, beamDirection * MathHelper.TwoPi / rotation, 180);
                }
                else if (AttackTime <= 360)
                {
                    //Will move away from the player whilst firing the Moon Lord Laser because IM A BAD PERSON.
                    if (AttackTime < 300)
                        NPC.velocity = NPC.DirectionFrom(Target.Center) * 4f;
                    else
                        NPC.velocity *= 0.75f;

                    NPC.rotation = NPC.rotation.AngleTowards(0, 0.05f);
                    if (Math.Abs(NPC.rotation) < 0.001f)
                        NPC.rotation = 0f;

                    if (AttackTime == 360)
                    {
                        NPC.velocity = Vector2.Zero;
                        PickAttack();
                        return;
                    }
                }
                break;
            case DarksolAIState.SneakyCure:
                if (AttackTime <= 30)
                {
                    NPC.scale = 1 - (AttackTime / 30f);
                    if(AttackTime == 30)
                    {
                        NPC.Center = Target.Center + Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(400, 600);
                        AttackBool = false;
                        NPC.Opacity = 0f;
                        StoredRatio = HealthRatio;
                    }    
                }
                else if (AttackTime < 600 || AttackBool)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame);
                    NPC.Opacity = 0f;

                    if(HealthRatio <= StoredRatio - 0.01f)
                    {
                        AttackBool = true;
                        StoredTime = AttackTime + 1;
                    }

                    if (AttackBool)
                    {
                        float wrappedTime = AttackTime - StoredTime;
                        NPC.scale = (wrappedTime / 30f);
                        NPC.Opacity = 1f;
                        if (wrappedTime == 30)
                        {
                            NPC.velocity = Vector2.Zero;
                            PickAttack();
                            return;
                        }
                    }
                }
                else
                {
                    if (AttackTime == 600)
                    {
                        NPC.Opacity = 1f;
                        NPC.life += NPC.lifeMax / 10;
                        if (NPC.life > NPC.lifeMax)
                            NPC.life = NPC.lifeMax;
                    }

                    float wrappedTime = AttackTime - 600;
                    NPC.scale = (wrappedTime / 30f);
                    if (wrappedTime == 30)
                    {
                        NPC.velocity = Vector2.Zero;
                        PickAttack();
                        return;
                    }
                }
                break;
        }

        AttackTime++;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        if (AIState == DarksolAIState.SpinDashes || (AIState == DarksolAIState.SneakyCure && AttackTime < 600))
            AttackBool = true;
    }

    private void GetTarget()
    {
        if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            // Below comment by Fabsol in regards to BoC. Likely would have used same logic for Goozma.
            // Ignore tank players, target low HP players, Brain is smart
            CalamityTargetingParameters options = CalamityTargetingParameters.BossDefaults;
            options.aggroRatio = -1f;
            options.finishThemOff = true;
            CalamityUtils.CalamityTargeting(NPC, options);
        }

        Target = Main.player[NPC.target];
    }

    private void PickAttack()
    {
        //Need to figure out how to make Darksol Smart and ignore attacks the player is "Immune to"
        //Only example of an attack immunity is the Miasma since supposedly Auric would have provided immunity to its debuffs.
        if (AIState == DarksolAIState.DarkSpears)
            AIState = DarksolAIState.DarkEnergyTorrent;
        else if (AIState == DarksolAIState.DarkEnergyTorrent)
            AIState = DarksolAIState.DarkEnergySpin;
        else if (AIState == DarksolAIState.DarkEnergySpin)
            AIState = DarksolAIState.SpinDashes;
        else if (AIState == DarksolAIState.SpinDashes && NPC.CountNPCS(ModContent.NPCType<DarkMatter>()) <= 15) //SMART AI REALLY REALLY SMART STUFF HERE
            AIState = DarksolAIState.DarkMatterSquad;
        else if ((AIState == DarksolAIState.DarkMatterSquad || AIState == DarksolAIState.SpinDashes) && NPC.life < (NPC.lifeMax * 0.9f))
            AIState = DarksolAIState.SneakyCure;
        else
            AIState = DarksolAIState.DarkSpears;
        AttackTime = 0;
        AttackStartTime = Time + 1;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D bossTexture = TextureAssets.Npc[Type].Value;
        Vector2 origin = bossTexture.Size() * 0.5f;
        Vector2 stretchScale = Vector2.One + new Vector2((float)Math.Cos(Time / 15f) / 12f, (float)Math.Cos(Time / 15f) / -12f);
        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.Black, 0f, origin, stretchScale * MathHelper.Clamp((NPC.scale - 0.15f), 0f, 10f), SpriteEffects.None, 0);

        var ozmaShader = GameShaders.Misc[$"{Mod.Name}:Ozma"];

        BreakAmount = 0f;

        Vector2 worldPos = Vector2.Transform(NPC.Center - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix) / Main.ScreenSize.ToVector2();
        ozmaShader.Shader.Parameters["time"]?.SetValue(Time);
        ozmaShader.Shader.Parameters["dissolveRatio"]?.SetValue(BreakAmount);
        ozmaShader.Shader.Parameters["inversionRatio"]?.SetValue((float)Math.Sin(Time / 60f) / 2f + 0.5f);
        ozmaShader.Shader.Parameters["worldPosition"]?.SetValue(NPC.Center);
        ozmaShader.Shader.Parameters["scale"]?.SetValue(new Vector2(bossTexture.Width, bossTexture.Height) * NPC.scale);
        ozmaShader.Shader.Parameters["rotation"]?.SetValue(NPC.rotation);

        spriteBatch.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/HarshNoise").Value;
        spriteBatch.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

        spriteBatch.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
        spriteBatch.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

        spriteBatch.GraphicsDevice.Textures[3] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Perlin").Value;
        spriteBatch.GraphicsDevice.SamplerStates[3] = SamplerState.LinearWrap;

        spriteBatch.End();

        spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            ozmaShader.Shader,
            Main.GameViewMatrix.TransformationMatrix
        );

        var Orbiters = GenerateRotatedRing(24, Quaternion.CreateFromYawPitchRoll(-Time / 100f, 0, Time / 100f));

        List <Vector3> orbitersBehind = Orbiters.Where(v => v.Z < 0.5f).ToList();
        orbitersBehind.Sort((a, b) => a.Z.CompareTo(b.Z));

        List<Vector3> orbitersAhead = Orbiters.Where(v => v.Z >= 0.5f).ToList();
        orbitersAhead.Sort((a, b) => a.Z.CompareTo(b.Z));

        foreach (Vector3 v in orbitersBehind)
        {
            Vector2 posOffset = new Vector2(v.X, v.Y) * 160f * stretchScale;
            float scaleMult = 1 + (v.Z / 2f);
            spriteBatch.Draw(bossTexture, NPC.Center + posOffset - Main.screenPosition, null, Color.White.MultiplyRGB(Color.Lerp(Color.DarkGray, Color.White, v.Z / 2f + 0.5f)), 0f, origin, NPC.scale / 8f * scaleMult, SpriteEffects.None, 0);
        }

        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.White, 0f, origin, stretchScale * NPC.scale, SpriteEffects.None, 0);

        foreach (Vector3 v in orbitersAhead)
        {
            Vector2 posOffset = new Vector2(v.X, v.Y) * 160f * stretchScale;
            float scaleMult = 1 + (v.Z / 2f);
            spriteBatch.Draw(bossTexture, NPC.Center + posOffset - Main.screenPosition, null, Color.White.MultiplyRGB(Color.Lerp(Color.DarkGray, Color.White, v.Z / 2f + 0.5f)), 0f, origin, NPC.scale / 8f * scaleMult, SpriteEffects.None, 0);
        }

        spriteBatch.End();

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix
        );
        return false;
    }

    public static List<Vector3> GenerateRotatedRing(int numPoints, Quaternion rotation)
    {
        List<Vector3> points = new List<Vector3>();
        const float radius = 1f;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = 2f * (float)Math.PI * i / numPoints;

            float x = (float)Math.Cos(angle) * radius;
            float z = (float)Math.Sin(angle) * radius;
            float y = 0f;

            Vector3 localPoint = new Vector3(x, y, z);

            Vector3 rotatedPoint = Vector3.Transform(localPoint, rotation);

            points.Add(rotatedPoint);
        }

        return points;
    }
}

public class DarksolSystem : ModSystem
{
    public static int DarksolBoss => darksolIndex;
    private static int darksolIndex = -1;
    public override void PreUpdateNPCs()
    {
        darksolIndex = NPC.FindFirstNPC(ModContent.NPCType<Darksol>());
    }
}

public class DarkMatter : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = 64;
        NPC.height = 64;
        NPC.damage = 20;
        NPC.lifeMax = 120000;
        NPC.DR_NERD(0, 0, 0, 0);
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.netAlways = true;
    }

    float Time = 0;

    public override void AI()
    {
        Player target = Main.LocalPlayer;// Main.player[Main.npc[DarksolSystem.DarksolBoss].target];

        float targetDist = 360f;
        if (NPC.Distance(target.Center) > targetDist)
            NPC.velocity += NPC.DirectionTo(target.Center) * 0.1f;
        else
            NPC.velocity -= NPC.DirectionTo(target.Center) * 0.1f;

        if(Time > 60 && Time % 180 == 0)
        {
            Vector2 dir = NPC.DirectionTo(target.Center);
            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * 16f, ModContent.ProjectileType<DarkSpear>(), 100, 1f, ai0: 1, ai1: dir.ToRotation());
        }

        NPC.direction = target.Center.X > NPC.Center.X ? 1 : -1;

        Time++;
    }
}

public class DarkSpear : ModProjectile
{
    public override string Texture => "CalamityMod/Projectiles/Boss/HolySpear";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 12;
    }

    public override void SetDefaults()
    {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.penetrate = -1;
        Projectile.Opacity = 1f;
        Projectile.tileCollide = false;
        Projectile.timeLeft = Lifetime;
        Projectile.damage = 10;
        Projectile.scale = 1f;
        Projectile.hostile = true;
    }

    private bool DirectlyFromDarksol => Projectile.ai[0] == 0;
    private float AccelerationDirection => Projectile.ai[1];

    private Vector2 InitialVelocity = Vector2.Zero;
    private Vector2 AcceleratingVelocity = Vector2.Zero;
    private static float Acceleration => 0.175f;
    private static int Lifetime => 240;
    private float SpeedCap => 1200f;
    private bool HitSpeedCap = false;

    public override void OnSpawn(IEntitySource source)
    {
        InitialVelocity = Projectile.velocity;
        Projectile.rotation = Projectile.velocity.ToRotation();
    }

    public override void AI()
    {
        if (DirectlyFromDarksol)
        {
            int UpTime = Lifetime - Projectile.timeLeft;
            InitialVelocity *= 0.975f;
            if (UpTime > 60)
                AcceleratingVelocity += AccelerationDirection.ToRotationVector2() * Acceleration;
            Projectile.velocity = InitialVelocity + AcceleratingVelocity;
        }
        else
            Projectile.velocity += AccelerationDirection.ToRotationVector2() * Acceleration;

        if(!HitSpeedCap && Projectile.velocity.Length() > SpeedCap)
        {
            HitSpeedCap = true;
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * SpeedCap;
        }

        Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        CalamityUtils.DrawAfterimagesCentered(Projectile, 2, Color.Magenta);
        return false;
    }
}

public class DarkEnergyBolt : ModProjectile
{
    public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 6;
    }

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 16;
        Projectile.timeLeft = 360;
        Projectile.penetrate = -1;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }

    private float RandomColorOffset = 0;
    private Vector2 AttackCenter;
    private float Radius = 16;
    private ref float Rotation => ref Projectile.ai[0];
    private ref float RotationSign => ref Projectile.ai[1];

    public override void OnSpawn(IEntitySource source)
    {
        RandomColorOffset = Main.rand.NextFloat(100f);
        AttackCenter = Projectile.Center;
    }

    public override void AI()
    {
        Radius += 8f;
        Rotation += 0.01f * RotationSign;
        Projectile.Center = AttackCenter + Rotation.ToRotationVector2() * Radius;
    }

    private float WidthFunction(float completionRatio, Vector2 vertexPos) => MathHelper.Lerp(0f, 32f, MathF.Pow(completionRatio, 1f / 2.5f));

    private Color ColorFunction(float completionRatio, Vector2 vertexPos)
    {
        float offsetTime = Main.GlobalTimeWrappedHourly + RandomColorOffset;
        float fadeToEnd = MathHelper.Lerp(0.65f, 1f, (float)Math.Cos(-offsetTime * 3f) * 0.5f + 0.5f);
        float fadeOpacity = Utils.GetLerpValue(1f, 0.64f, completionRatio, true) * Projectile.Opacity;
        Color endColor = Color.Lerp(Color.Magenta, Color.Fuchsia, (float)Math.Sin(completionRatio * MathHelper.Pi * 1.6f - offsetTime * 4f) * 0.5f + 0.5f);
        return Color.Lerp(Color.Black, endColor, fadeToEnd) * fadeOpacity;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
        PrimitiveRenderer.RenderTrail(Projectile.oldPos, new PrimitiveSettings(WidthFunction, ColorFunction, (_,_) => Projectile.Size * 0.5f, pixelate: false, shader: GameShaders.Misc["CalamityMod:TrailStreak"]), 32);
        return false;
    }
}

//Its actually really canonical for Goozma to have a Moon Lord Laser since they're both dark gods guys.
public class DarkBeam : ModProjectile
{
    public override string Texture => "CalamityMod/Projectiles/Boss/ProvidenceHolyRayNight";
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
    }

    public override void SetDefaults()
    {
        Projectile.Calamity().DealsDefenseDamage = true;
        Projectile.width = 48;
        Projectile.height = 48;
        Projectile.hostile = true;
        Projectile.alpha = 255;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 210;
        CooldownSlot = ImmunityCooldownID.Bosses;
    }

    float ExtraWidth = 0f;
    float Time = 0;
    Vector2 beamDir = Vector2.Zero;

    public override void OnSpawn(IEntitySource source)
    {
        beamDir = Projectile.velocity;
        Projectile.velocity = Vector2.Zero;
        Projectile.timeLeft = (int)Projectile.ai[1];
    }

    public override void AI()
    {
        Projectile.Center = Main.npc[DarksolSystem.DarksolBoss].Center + Main.npc[DarksolSystem.DarksolBoss].velocity;

        if (Time <= 30)
            ExtraWidth = MathHelper.Lerp(0, 2, Time / 30f);
        if(Projectile.timeLeft <= 30)
            ExtraWidth = MathHelper.Lerp(0, 2, Projectile.timeLeft / 30f);

        float[] array3 = new float[3];
        Collision.LaserScan(Projectile.Center, Projectile.velocity, Projectile.width * Projectile.scale, 2400f, array3);
        float rayLength = 0f;
        for (int i = 0; i < array3.Length; i++)
        {
            rayLength += array3[i];
        }
        rayLength /= 3f;

        // Fire laser through walls at max length if target cannot be seen
        if (!Collision.CanHitLine(Main.npc[DarksolSystem.DarksolBoss].Center, 1, 1, Main.player[Main.npc[DarksolSystem.DarksolBoss].target].Center, 1, 1))
            rayLength = 2400f;

        Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], rayLength, 0.5f); // Length of laser, linear interpolation

        beamDir = beamDir.RotatedBy(Projectile.ai[0]);
        Projectile.rotation = beamDir.ToRotation() - MathHelper.PiOver2;

        Projectile.Center += beamDir * 96f;

        Time++;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (beamDir == Vector2.Zero)
            return false;

        Vector2 scale = new(Projectile.scale * ExtraWidth, Projectile.scale);

        Texture2D texture2D19 = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;
        Texture2D texture2D20 = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/ProvidenceHolyRayMidNight", AssetRequestMode.ImmediateLoad).Value;
        Texture2D texture2D21 = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/ProvidenceHolyRayEndNight", AssetRequestMode.ImmediateLoad).Value;

        float rayDrawLength = Projectile.localAI[1]; //length of laser
        Color baseColor = Color.Magenta;
        Vector2 vector = Projectile.Center - Main.screenPosition;
        Rectangle? sourceRectangle2 = null;
        Main.spriteBatch.Draw(texture2D19, vector, sourceRectangle2, baseColor, Projectile.rotation, texture2D19.Size() / 2f, scale, SpriteEffects.None, 0);
        rayDrawLength -= (texture2D19.Height / 2 + texture2D21.Height) * Projectile.scale;
        Vector2 projCenter = Projectile.Center;
        projCenter += beamDir * Projectile.scale * texture2D19.Height / 2f;

        if (rayDrawLength > 0f)
        {
            float raySegment = 0f;
            Rectangle drawRectangle = new Rectangle(0, 36 * (Projectile.timeLeft / 3 % 4), texture2D20.Width, 36);
            while (raySegment + 1f < rayDrawLength)
            {
                if (rayDrawLength - raySegment < drawRectangle.Height)
                    drawRectangle.Height = (int)(rayDrawLength - raySegment);

                Main.spriteBatch.Draw(texture2D20, projCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(drawRectangle), baseColor, Projectile.rotation, new Vector2(drawRectangle.Width / 2, 0f), scale, SpriteEffects.None, 0);
                raySegment += drawRectangle.Height * Projectile.scale;
                projCenter += beamDir * drawRectangle.Height * Projectile.scale;
                drawRectangle.Y += 36;

                if (drawRectangle.Y + drawRectangle.Height > texture2D20.Height)
                    drawRectangle.Y = 0;
            }
        }

        Vector2 vector2 = projCenter - Main.screenPosition;
        sourceRectangle2 = null;

        Main.spriteBatch.Draw(texture2D21, vector2, sourceRectangle2, baseColor, Projectile.rotation, texture2D21.Frame(1, 1, 0, 0).Top(), scale, SpriteEffects.None, 0);

        return false;
    }

    public override void CutTiles()
    {
        DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
        Vector2 unit = beamDir;
        Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Projectile.localAI[1], Projectile.width * Projectile.scale, DelegateMethods.CutTiles);
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        targetHitbox.Width = (int)((float)targetHitbox.Width * ExtraWidth);
        targetHitbox.Height = (int)((float)targetHitbox.Height * ExtraWidth);

        if (projHitbox.Intersects(targetHitbox))
            return true;

        float useless = 0f;
        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + beamDir * Projectile.localAI[1], 22f * Projectile.scale, ref useless))
            return true;

        return false;
    }
    public override bool CanHitPlayer(Player target) => Projectile.scale >= 0.5f;
}