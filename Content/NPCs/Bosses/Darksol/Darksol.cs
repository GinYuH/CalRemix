using CalamityMod;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Darksol;

public class Darksol : ModNPC
{
    public override string Texture => "CalRemix/Core/Graphics/Metaballs/BasicCircle";

    private float Time = 0;
    private float AttackTime = 0;
    private float BreakAmount = 0f;
    private Player Target = null;

    Vector2 orbiterBaseAngles = Vector2.Zero;

    private enum DarksolAIState
    {
        TestAI,
        //Phase 1
        SpawnAnimation,
        DarkSpears,
        DarkEnergyTorrent,
        GiantDarkEnergyOrb,
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
        NPC.width = 64;
        NPC.height = 64;
        NPC.scale = 4f;
        NPC.damage = 20;
        NPC.lifeMax = 55535;
        NPC.LifeMaxNERB(55535, 55535, 55535); //Goozma is a Calamity boss that has 55,535 HP and can use the most powerful attacks out of any boss in the mod.
        NPC.DR_NERD(0.9f, 0.9f, 0.9f, 0.9f); //Don't be fooled by his low HP as he takes reduced damage from all weapons.
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.dontTakeDamage = true;
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
        Time++;

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
            case DarksolAIState.DarkSpears:
                if(AttackTime < 90)
                {
                    if (AttackTime % 5 == 0)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -12 * Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)), ModContent.ProjectileType<HolySpear>(), 100, 1f, ai0: -2);
                }
                else if (AttackTime > 120 && AttackTime < 600)
                {
                    if (AttackTime % 10 == 0)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.NextFloat(-800f, 800f), -600f), new Vector2(3, 3), ModContent.ProjectileType<HolySpear>(), 100, 1f);

                    if (AttackTime % 30 == 0)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Target.Center) * 8f, ModContent.ProjectileType<HolySpear>(), 100, 1f);
                }
                else if(AttackTime >= 600)
                {
                    PickAttack();
                    return;
                }
                break;
            case DarksolAIState.DarkEnergyTorrent:
                break;
        }

        AttackTime++;
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
        AIState = DarksolAIState.TestAI;
        AttackTime = 0;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D bossTexture = TextureAssets.Npc[Type].Value;
        Vector2 origin = bossTexture.Size() * 0.5f;
        Vector2 stretchScale = Vector2.One + new Vector2((float)Math.Cos(Time / 15f) / 12f, (float)Math.Cos(Time / 15f) / -12f);
        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.Black, 0f, origin, stretchScale * (NPC.scale - 0.15f), SpriteEffects.None, 0);

        var ozmaShader = GameShaders.Misc[$"{Mod.Name}:Ozma"];

        BreakAmount = 0f;

        Vector2 worldPos = Vector2.Transform(NPC.Center - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix) / Main.ScreenSize.ToVector2();
        ozmaShader.Shader.Parameters["time"]?.SetValue(Time);
        ozmaShader.Shader.Parameters["dissolveRatio"]?.SetValue(BreakAmount);
        ozmaShader.Shader.Parameters["inversionRatio"]?.SetValue((float)Math.Sin(Time / 60f) / 2f + 0.5f);
        ozmaShader.Shader.Parameters["worldPosition"]?.SetValue(NPC.Center);
        ozmaShader.Shader.Parameters["scale"]?.SetValue(new Vector2(bossTexture.Width, bossTexture.Height) * NPC.scale);
        ozmaShader.Shader.Parameters["rotation"]?.SetValue(NPC.rotation);

        spriteBatch.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/MeltyNoise").Value;
        spriteBatch.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

        spriteBatch.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
        spriteBatch.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

        spriteBatch.GraphicsDevice.Textures[3] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Perlin").Value;
        spriteBatch.GraphicsDevice.SamplerStates[3] = SamplerState.LinearWrap;

        // Apply the shader and draw
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

        var Orbiters = GenerateRotatedRing(18, Quaternion.CreateFromYawPitchRoll(-Time / 100f, 0, Time / 100f));

        List <Vector3> orbitersBehind = Orbiters.Where(v => v.Z < 0.5f).ToList();
        orbitersBehind.Sort((a, b) => a.Z.CompareTo(b.Z));

        List<Vector3> orbitersAhead = Orbiters.Where(v => v.Z >= 0.5f).ToList();
        orbitersAhead.Sort((a, b) => a.Z.CompareTo(b.Z));

        foreach (Vector3 v in orbitersBehind)
        {
            Vector2 posOffset = new Vector2(v.X, v.Y) * 200f;
            float scaleMult = 1 + (v.Z / 2f);
            spriteBatch.Draw(bossTexture, NPC.Center + posOffset - Main.screenPosition, null, Color.White.MultiplyRGB(Color.Lerp(Color.DarkGray, Color.White, v.Z / 2f + 0.5f)), 0f, origin, NPC.scale / 5f * scaleMult, SpriteEffects.None, 0);
        }

        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.Red, 0f, origin, stretchScale * NPC.scale, SpriteEffects.None, 0);

        foreach (Vector3 v in orbitersAhead)
        {
            Vector2 posOffset = new Vector2(v.X, v.Y) * 200f;
            float scaleMult = 1 + (v.Z / 2f);
            spriteBatch.Draw(bossTexture, NPC.Center + posOffset - Main.screenPosition, null, Color.White.MultiplyRGB(Color.Lerp(Color.DarkGray, Color.White, v.Z / 2f + 0.5f)), 0f, origin, NPC.scale / 5f * scaleMult, SpriteEffects.None, 0);
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

            // Start with the local point in the XZ plane
            Vector3 localPoint = new Vector3(x, y, z);

            // Apply the desired rotation to the point
            // The multiplication syntax depends on how your Quaternion struct operator is defined.
            // Assuming the common syntax: result = rotation * vector
            Vector3 rotatedPoint = Vector3.Transform(localPoint, rotation);

            points.Add(rotatedPoint);
        }

        return points;
    }
}
