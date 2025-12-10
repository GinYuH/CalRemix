using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.DataStructures;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ozma;

public class Ozma : ModNPC
{
    float Time = 0;
    float AttackTime = 0;
    int TurnCounter = 0;
    Player Target = null;
    internal static bool ShouldDoomsdayHeal => false;

    private enum OzmaAttack
    {
        None = 0,

        //Odd Attacks
        FlareStar, 
        //Wiki Description: Deals damage based on the characters' level. Can miss.
        //My Idea: Fires plasma balls at the player that ater being fired combine into eachother, creating a massive explosion once all have been absorbed.

        Meteor,
        //Wiki Description: Deals random damage to the entire party with the possibility of dealing 9999 to all of them.
        //Fabsolian Description: A massive meteor impacts the ground causing a swarm of small meteors to spread out from the impact zone.  All hits will do a random amount of damage ranging from 10% to 100% of your HP.

        Doomsday,
        //Wiki Description: Deals Shadow damage to all targets, including Ozma. (Addendum: UNLESS, the prerequisite side=quest isnt done. In which case it heals him (alongside him just being unable to be hurt)
        //My Idea: Would cause explosions all around the player that can damage both the player and Ozma.

        Flare,
        //Wiki Description: Deals non-elemental damage to one target.
        //My Idea: Aims an explosion at a target, which explodes shortly after, unleashing a radial spread of fireballs.

        Holy,
        //Wiki Description: Deals Holy damage to one target. Ozma will not use the ability if the target absorbs it.
        //My Idea: Light beams come down from above, inflicting a debuff that deals rapid damage whilst within them.

        Death,
        //Wiki Description: Defeats one party member if it hits.
        //My Idea: Summons Reaper Phantoms around player's that slash at the target with their scythe soon after being summoned. ONE SHOTS :D

        AbsorbMP,
        //Wiki Description: Used if Ozma is low on MP. Drains MP from the party.
        //My Idea: Ozma attempts to get close to a player, if in range, Ozma will begin draining them of their Mana, and if their mana reaches zero during this, they die.

        //Even Attacks
        Curse,
        //Wiki Description: Deals non-elemental damage to all party members and inflicts Confuse, Poison, Slow, Mini, and Darkness.
        //Fabsolian Description: A miasma of green energy spreads around the arena.  If you touch it you will be inflicted with: Cursed, Horror, Plague, Blind, and Marked.

        Mini,
        //Wiki Description: Inflicts Mini status on all targets.
        //Fabsolian Description: If you do too much damage to him in one hit it will be reduced to 0 and he will immediately counterattack with a move that reduces all of your stats by 50%.

        LV4_Holy,
        //Wiki Description: Deals Holy damage to all party members whose levels are a multiple of 4. Ozma will only use Lv4 Holy if the party has a member susceptible to it.
        //My Idea: Light beams come down from above, inflicting a debuff that deals rapid damage whilst within them.

        LV5_Death,
        //Wiki Description: Defeats all party members whose levels are a multiple of 5. Ozma will only use Lv5 Death if the party has a member susceptible to it; if no party member is of a suitable level, Ozma will not waste its turn.
        //My Idea: Summons Reaper Phantoms around player's that slash at the target with their scythe soon after being summoned. ONE SHOTS :D

        Curaga,
        //Wiki Description: Used as a counterattack; there is a higher chance of countering with Curaga when Ozma's health is low.
        //Fabsolian Description: When under 20,000 HP he has a chance to heal himself for 6,000 - 9,999 HP with his Cure move, the chances of him using it become higher the lower his HP is.
        //                       Even if he uses this attack he still has a 50% chance of doing a regular attack immediately after it.

        Esuna,
        //Wiki Description: Used if Ozma is afflicted with a status ailment.
        //My Idea: Clears all Debuffs Ozma has been inflicted with (and perhaps gives immunities to them for the rest of the fight?) and unleashes a bullet hell of projectiles based on the Elements of all debuffs it had been inflicted with.

    }

    private static OzmaAttack[] OddAttacks => [
        //OzmaAttack.FlareStar, 
        //OzmaAttack.Meteor, 
        OzmaAttack.Doomsday, 
        //OzmaAttack.Flare, 
        //OzmaAttack.Holy, 
        //OzmaAttack.Death, 
        //OzmaAttack.AbsorbMP
    ];

    private static OzmaAttack[] EvenAttacks => [
        OzmaAttack.Curse,
        //OzmaAttack.Mini,
        //OzmaAttack.LV4_Holy,
        //OzmaAttack.LV5_Death,
        OzmaAttack.Curaga,
        //OzmaAttack.Esuna,
    ];

    OzmaAttack CurrentAttack = OzmaAttack.None;

    public override void SetDefaults()
    {
        NPC.width = 256;
        NPC.height = 256;
        NPC.scale = 2f;
        NPC.damage = 124;
        NPC.LifeMaxNERB(55535, 5553500, 55535);
        if (Main.expertMode)
            NPC.lifeMax /= 2;
        NPC.DR_NERD(0.95f, 0.95f, 0.95f, 0.95f);
        NPC.defense = 30;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.npcSlots = 10f;
        NPC.netAlways = true;
        NPC.hide = true;

        //Music = -1;
        //SceneEffectPriority = SceneEffectPriority.None;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (NPC.lifeMax != 55535)
            NPC.lifeMax = 55535;
        NPC.life = NPC.lifeMax;
        PickTarget();
        NPC.damage = 0;
    }

    public override void AI()
    {
        Time++;

        switch (CurrentAttack)
        {
            case OzmaAttack.None:
                if (AttackTime == 0)
                    PickTarget();

                NPC.velocity = ((Target.Center - (Vector2.UnitY * 96)) - NPC.Center) / 30f;
                NPC.rotation *= 0.9f;
                NPC.rotation += NPC.velocity.X / 188f;

                if (AttackTime >= 240)
                {
                    PickAttack();
                    return;
                }
                break;
            case OzmaAttack.Doomsday:
                NPC.velocity = ((Target.Center - (Vector2.UnitY * 96)) - NPC.Center) / 30f;
                NPC.rotation *= 0.9f;
                NPC.rotation += NPC.velocity.X / 188f;

                if (AttackTime >= 600)
                {
                    PickAttack();
                    return;
                }
                if (AttackTime % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    foreach(Player p in Main.ActivePlayers)
                    {
                        Vector2 goalPos = p.Center + (p.velocity * 45) + Main.rand.NextVector2Circular(500, 500);
                        float curveStrength = AttackTime % 20 == 0 ? Main.rand.NextFloat(0.1f, 0.5f) : Main.rand.NextFloat(-0.5f, -0.1f);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + Main.rand.NextVector2Circular(NPC.width, NPC.height), Vector2.Zero, ModContent.ProjectileType<DoomsdayOrb>(), 50, 1f, -1, goalPos.X, goalPos.Y, curveStrength);
                    }
                }
                break;
            case OzmaAttack.Curse:
                NPC.velocity = ((Target.Center - (Vector2.UnitY * 180)) - NPC.Center) / 30f;
                NPC.rotation *= 0.9f;
                NPC.rotation += NPC.velocity.X / 188f;

                if (AttackTime >= 700)
                {
                    PickAttack();
                    return;
                }
                if (AttackTime <= 580 && AttackTime % 15 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-MathHelper.Pi / 3f, MathHelper.Pi / 3f)) * 12f;
                    velocity += NPC.velocity * 2f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2(Main.rand.NextFloat(-128, 128), -180), velocity, ModContent.ProjectileType<Miasma>(), 10, 1f);
                }
                break;
            case OzmaAttack.Curaga:
                NPC.velocity *= 0.9f;
                NPC.rotation *= 0.925f;

                if (AttackTime >= 180)
                {
                    PickAttack();
                    return;
                }
                else if (AttackTime == 60)
                {
                    int healAmt = Main.rand.Next(6000, 10000);
                    if (healAmt + NPC.life > NPC.lifeMax)
                        healAmt = NPC.lifeMax - NPC.life;
                    NPC.life += healAmt;
                    CombatText.NewText(NPC.Hitbox, CombatText.HealLife, healAmt, true);
                    NPC.netUpdate = true;
                }
                break;


        }

        AttackTime++;
    }

    private void PickAttack()
    {
        if (CurrentAttack != OzmaAttack.None)
            CurrentAttack = OzmaAttack.None;
        else 
        {
            if (TurnCounter % 2 == 0)
                CurrentAttack = EvenAttacks[Main.rand.Next(EvenAttacks.Length)];
            else
                CurrentAttack = OddAttacks[Main.rand.Next(OddAttacks.Length)];
            TurnCounter++;
        }
        AttackTime = 0;
    }

    private void PickTarget()
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

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D bossTexture = TextureAssets.Npc[Type].Value;
        Vector2 origin = bossTexture.Size() * 0.5f;
        Vector2 stretchScale = Vector2.One;// + new Vector2((float)Math.Cos(Time / 15f) / 12f, (float)Math.Cos(Time / 15f) / -12f);

        var ozmaShader = GameShaders.Misc[$"{Mod.Name}:Ozma"];

        ozmaShader.Shader.Parameters["uTime"]?.SetValue(Time);
        ozmaShader.Shader.Parameters["uRotSpeed"]?.SetValue(0.01f);
        ozmaShader.Shader.Parameters["uSwirlStrength"]?.SetValue(3f);
        ozmaShader.Shader.Parameters["uSaturation"]?.SetValue(1.5f);
        ozmaShader.Shader.Parameters["uPixelSize"]?.SetValue(0);
        ozmaShader.Shader.Parameters["uOutlineColor"]?.SetValue(Color.DarkBlue.ToVector3());        
        ozmaShader.Shader.Parameters["uRotation"]?.SetValue(new Vector3(-Time / 100f, 0, NPC.rotation)); //yaw, pitch, roll

        spriteBatch.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ozma/OzmaLight").Value;
        spriteBatch.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

        spriteBatch.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ozma/OzmaDark").Value;
        spriteBatch.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

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

        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.Transparent, 0f, origin, stretchScale * NPC.scale, SpriteEffects.None, 0);
        
        return false;
    }

    public override void DrawBehind(int index)
    {
        Main.instance.DrawCacheNPCsMoonMoon.Add(index);
    }
}

public class DoomsdayOrb : ModProjectile
{
    public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 360;
        Projectile.scale = 0.75f;
        Projectile.timeLeft = TravelTime + ExplodeDelay + 24;
        Projectile.penetrate = -1;
        Projectile.hostile = true;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }

    int TravelTime => 60;
    int ExplodeDelay => 0;
    int Time = 0;
    int defDamage = 0;
    List<Vector2> travelPoints = [];
    BezierCurve TravelCurve = null;
    float ExplosionScale = 1f;

    public override void OnSpawn(IEntitySource source)
    {
        ExplosionScale = Projectile.scale;

        Projectile.scale = 1f;

        Vector2 goalPos = new(Projectile.ai[0], Projectile.ai[1]);

        Vector2 direction = goalPos - Projectile.Center;
        float curveIntensity = Projectile.ai[2];
        Vector2 perpindicular = direction.RotatedBy(MathHelper.PiOver2);

        Vector2 controlPoint1 = Projectile.Center + (direction * 0.25f) + (perpindicular * curveIntensity);
        Vector2 controlPoint2 = Projectile.Center + (direction * 0.75f) + (perpindicular * curveIntensity);

        TravelCurve = new (Projectile.Center, controlPoint1, controlPoint2, goalPos);
        travelPoints = TravelCurve.GetPoints(TravelTime);

        defDamage = Projectile.damage;
        Projectile.damage = 0;
    }

    public override void AI()
    {
        if (Time < TravelTime)
            Projectile.Center = TravelCurve.Evaluate(CalamityUtils.CircInEasing(Time / (float)(TravelTime - 1), 1));

        if(Time == TravelTime + ExplodeDelay)
        {
            DetailedExplosion explosion = new(Projectile.Center, Vector2.Zero, Color.Magenta, Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), 0.1f, ExplosionScale, 24);
            GeneralParticleHandler.SpawnParticle(explosion);
            Projectile.damage = defDamage;
            Projectile.velocity = Vector2.Zero;
            Projectile.Opacity = 0f;
        }

        Time++;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Ozma.ShouldDoomsdayHeal)
        {
            target.life += damageDone * 2;
            CombatText hurtText = Main.combatText.First(t => t.text == damageDone.ToString() && (t.color == CombatText.DamagedHostile || t.color == CombatText.DamagedHostileCrit));
            if (hurtText != null)
                hurtText.color = CombatText.HealLife;
        }
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        target.AddBuff(BuffID.ShadowFlame, 180);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D shadowball = TextureAssets.Projectile[ProjectileID.CultistBossFireBallClone].Value;
        Rectangle frame = shadowball.Frame(1, 4, 0, (Time / 5) % 4);
        Main.EntitySpriteDraw(shadowball, Projectile.Center - Main.screenPosition, frame, lightColor * Projectile.Opacity, Projectile.rotation, frame.Size() / 2f, Projectile.scale, 0);

        Texture2D telegraph = ModContent.Request<Texture2D>("CalamityMod/Particles/SmallBloomRing").Value;
        Vector2 goalPos = new(Projectile.ai[0], Projectile.ai[1]);
        Main.EntitySpriteDraw(telegraph, goalPos - Main.screenPosition, null, Color.Red * Projectile.Opacity, Projectile.rotation, telegraph.Size() / 2f, Projectile.scale * 3, 0);

        return false;
    }
}

public class Miasma : ModProjectile
{
    public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 64;
        Projectile.scale = 1f;
        Projectile.timeLeft = 600;
        Projectile.penetrate = -1;
        Projectile.hostile = true;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }

    int Time = 0;

    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0; i < 32; i++)
        {
            MiasmaGas gas = new(Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * Main.rand.NextFloat(0.2f, 3f), Color.Lime, Color.DarkGreen, 2f, 0.5f, 60, Main.rand.NextFloat(-0.05f, 0.05f));
            GeneralParticleHandler.SpawnParticle(gas);
        }
    }

    public override void AI()
    {
        MiasmaGas gas = new(Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(1f, 7f), Color.Lime, Color.DarkGreen, 2f, 0.75f, 60, Main.rand.NextFloat(-0.05f, 0.05f));
        GeneralParticleHandler.SpawnParticle(gas);

        if (Time % 8 == 0)
        {
            float randRadius = 80;
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(randRadius, randRadius), DustID.GreenFairy, Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(1f, 4f));
        }

        Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.25f);

        if (Projectile.velocity.Y < 5f)
            Projectile.velocity.Y += 0.2f;
        else
        {
            Projectile.velocity.Y = 5f;
            Projectile.velocity.X *= 0.9f;
        }
        Projectile.position.X += (float)Math.Sin(Time / 10f) * (0.025f * Time);

        Time++;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        //Cursed, Horror, Plague, Blind, and Marked
        target.AddBuff(BuffID.Horrified, 300);
        target.AddBuff(BuffID.Darkness, 300);
        target.AddBuff(BuffID.CursedInferno, 300);
        target.AddBuff(BuffID.Poisoned, 300);
        target.AddBuff(BuffID.OgreSpit, 300);
        target.AddBuff(BuffID.Stinky, 300);
        target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        target.AddBuff(ModContent.BuffType<Plague>(), 300);
        target.AddBuff(ModContent.BuffType<AbsorberAffliction>(), 300);
        target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 300);
    }
}

public class MiasmaGas : Particle
{
    public override string Texture => "CalamityMod/Particles/MediumMist";
    public override int FrameVariants => 3;
    public override bool UseAdditiveBlend => true;

    private int UpTime;
    private float Opacity;
    private Color ColorFire;
    private Color ColorFade;
    private float Spin;
    private float GoalScale;

    public MiasmaGas(Vector2 position, Vector2 velocity, Color colorFire, Color colorFade, float scale, float opacity, int upTime, float rotationSpeed = 0f)
    {
        Position = position;
        Velocity = velocity;
        ColorFire = colorFire;
        ColorFade = colorFade;
        GoalScale = scale;
        Scale = 0f;
        Opacity = opacity;
        UpTime = upTime;
        Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        Spin = rotationSpeed;
        Variant = Main.rand.Next(3);
        Time = 0;
    }

    public override void Update()
    {
        Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f);
        Velocity *= 0.95f;

        if (Time <= 20)
        {
            Scale = MathHelper.Lerp(0f, GoalScale, CalamityUtils.CircOutEasing(Time / 20f, 1));
        }
        else
        {
            Scale += 0.01f;
        }

        if (Collision.IsWorldPointSolid(Position + Velocity, true))
            Velocity = Velocity.RotatedBy(Main.rand.NextBool() ? MathHelper.PiOver2 : -MathHelper.PiOver2);
        else if (Collision.IsWorldPointSolid(Position, true))
            Velocity = Vector2.Zero;

        if (Time > UpTime)
            Kill();

        float TimeRatio = CalamityUtils.SineInEasing(MathHelper.Clamp(Time / (float)UpTime, 0f, 1f), 1);
        Color = Color.Lerp(ColorFire, ColorFade, TimeRatio) * Opacity * (1 - TimeRatio);
    }
}