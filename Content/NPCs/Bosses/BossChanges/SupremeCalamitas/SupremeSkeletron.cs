using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;
using System.Linq;
using Terraria;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.NPCs.SupremeCalamitas;
using MonoMod.Cil;
using Terraria.Audio;

using SCal = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class SupremeSkeletron : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float Mode => ref NPC.ai[1];
        public ref NPC LeftArm => ref Main.npc[(int)NPC.ai[2]];
        public ref NPC RightArm => ref Main.npc[(int)NPC.ai[3]];
        public ref float TimerVisual => ref NPC.localAI[0];

        private static int origMouthOpenRate = 30;
        
        private bool isMouthOpen = false;
        private bool spinHead = false;
        private bool headContact = true;
        private float mouthOpenRate = origMouthOpenRate;
        private Vector2 idealPosForWandering = Vector2.Zero;

        public enum AttackTypes
        {
            AttachToCursor = -3,
            LerpTowardsCursor = -2,
            None = -1,
            Spawn = 0,
            SpinAroundPlayer = 1,
            HoverOverPlayerAndBeEvil = 2,
            ChasePlayer = 3,
            WanderAbovePlayer = 4
        }

        public override void Load()
        {
            base.Load();

            // this is where we are epic and make supreme calamitas care about
            // our awesome and better version of sepulcher
            IL.CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.DoHeartsSpawningCastAnimation += il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(x => x.MatchCall<NPC>(nameof(NPC.CountNPCS)));
                c.Remove();
                c.EmitDelegate(
                    (int type) => NPC.CountNPCS(type) + NPC.CountNPCS(ModContent.NPCType<SupremeSkeletron>())
                );

                c.GotoNext(x => x.MatchCall<NPC>(nameof(NPC.NewNPC)));
                c.Remove();
                c.EmitLdarg0();
                c.EmitDelegate(
                    (
                        IEntitySource source,
                        int           x,
                        int           y,
                        int           type,
                        int           start,
                        float         ai0,
                        float         ai1,
                        float         ai2,
                        float         ai3,
                        int           target,
                        SCal          self
                    ) =>
                    {
                        if (self.hasSummonedSepulcher2 && type == ModContent.NPCType<SepulcherHead>())
                        {
                            type = ModContent.NPCType<SupremeSkeletron>();
                        }
                        
                        return NPC.NewNPC(
                            source,
                            x,
                            y,
                            type,
                            0,
                            ai0,
                            ai1,
                            ai2,
                            ai3,
                            target
                        );
                    }
                );
            };

            // TODO: Doesn't work for some reason.  Unimportant.
            /*IL.CalamityMod.Projectiles.Summon.ProfanedCrystalMageFireballSplit.CanHitNPC += il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(MoveType.AfterLabel, x => x.MatchStloc(out _));

                c.EmitLdarg(1); // NPC target
                c.EmitDelegate(
                    (bool value, NPC target) => value || target.type == ModContent.NPCType<SupremeSkeletron>() || target.type == ModContent.NPCType<SupremeSkeletronHand>()
                );
            };*/

            var boss = BossRushEvent.Bosses.First(x => x.EntityID == ModContent.NPCType<SCal>());
            boss.HostileNPCsToNotDelete.Add(ModContent.NPCType<SupremeSkeletron>());
            boss.HostileNPCsToNotDelete.Add(ModContent.NPCType<SupremeSkeletronHand>());
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eucharist Damsel");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 68;
            NPC.height = 96;
            NPC.lifeMax = 500000;
            NPC.damage = 0;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
            NPC.HitSound = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrotherHit;
            NPC.DeathSound = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrotherDeath;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            // my arms.. i need my arms...
            // first is left, second is right
            NPC.ai[2] = CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), NPC.Center, ModContent.NPCType<SupremeSkeletronHand>(), 0, 0, 0, 0, NPC.whoAmI, Target.whoAmI).whoAmI;
            NPC.ai[3] = CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), NPC.Center, ModContent.NPCType<SupremeSkeletronHand>(), 0, 0, 0, 1, NPC.whoAmI, Target.whoAmI).whoAmI;


        }
        public override void AI()
        {
            int cala = NPC.FindFirstNPC(ModContent.NPCType<SCal>());
            if (cala > -1)
            {
                NPC calamitas = Main.npc[cala];
                float lifeRatio = (float)calamitas.life / (float)calamitas.lifeMax;
                if (lifeRatio <= 0.01f)
                {
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            // This is dangerous because Calamity only expects SepulcherHead to
            // set this.  Should be fine for now!
            CalamityGlobalNPC.SCalWorm = NPC.whoAmI;
            
            if (NPC.target < 0 || NPC.target == 255 || Target.dead || !Target.active || Vector2.Distance(Target.Center, NPC.Center) > 8000)
            {
                NPC.TargetClosest(faceTarget: false);
                NPC.netUpdate = true;
            }

            switch (Mode)
            {
                case (int)AttackTypes.AttachToCursor:
                    NPC.Center = Main.MouseWorld;
                    break;
                case (int)AttackTypes.LerpTowardsCursor:
                    NPC.Center = Vector2.Lerp(NPC.Center, Main.MouseWorld, 0.1f);
                    break;
                case (int)AttackTypes.None:

                    break;
                case (int)AttackTypes.Spawn:
                    NPC.velocity += NPC.DirectionTo(new Vector2(Target.Center.X, Target.Center.Y - 100));
                    NPC.velocity = NPC.velocity.ClampMagnitude(-22, 22);

                    if (Timer >= 50)
                    {
                        Timer = 0;
                        Mode = (int)AttackTypes.SpinAroundPlayer;
                    }
                    break;
                case (int)AttackTypes.SpinAroundPlayer:
                    spinHead = true;
                    headContact = false;

                    float IdealPositionX = Target.Center.X - (int)(Math.Cos(Timer * 0.2f) * 350);
                    float IdealPositionY = Target.Center.Y - (int)(Math.Sin(Timer * 0.2f) * 350);
                    Vector2 idealPosition = new Vector2(IdealPositionX, IdealPositionY);
                    // lerping like this means he never actually reaches his spot, which is kinda gross imo but w/e it works
                    NPC.Center = Vector2.Lerp(NPC.Center, idealPosition, 0.1f);
                    //Dust.NewDustPerfect(idealPosition, DustID.CrimsonSpray, Vector2.Zero);

                    if (Timer >= 50 && Main.rand.NextBool(12))
                    {
                        //TODO: DUST WHEN SPAWNING PROJS
                        //TODO: GIVE PROJS PROPER DESPAWN AT HIGH DISTANCES + DUST ON DESPAWN

                        SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = -1, Volume = 2f }, NPC.position);
                        int sickle1 = Projectile.NewProjectile(NPC.GetSource_FromThis(), LeftArm.Center, LeftArm.Center.DirectionTo(Target.Center) * -15, ModContent.ProjectileType<SupremeSickle>(), 200, 0, -1, LeftArm.Center.DirectionTo(Target.Center).X, LeftArm.Center.DirectionTo(Target.Center).Y);
                        int sickle2 = Projectile.NewProjectile(NPC.GetSource_FromThis(), RightArm.Center, RightArm.Center.DirectionTo(Target.Center) * -15, ModContent.ProjectileType<SupremeSickle>(), 200, 0, -1, RightArm.Center.DirectionTo(Target.Center).X, RightArm.Center.DirectionTo(Target.Center).Y);

                        headContact = true;
                    }

                    if (Timer >= 500)
                    {
                        spinHead = false;
                        headContact = true;

                        Timer = 0;
                        Mode = (int)AttackTypes.HoverOverPlayerAndBeEvil;
                    }
                    break;
                case (int)AttackTypes.HoverOverPlayerAndBeEvil:
                    if (Timer < 200)
                    {
                        NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(Target.Center.X, Target.Center.Y - 250), 0.1f);
                    }

                    if (Timer >= 200)
                    {
                        NPC.velocity = Vector2.Zero;
                        Timer = 0;
                        Mode = (int)AttackTypes.ChasePlayer;
                    }  
                    break;
                case (int)AttackTypes.ChasePlayer:
                    int startChasing = 75;

                    // little telegraph to tell the player IM GONAN GO EVIL...
                    if (Timer == 1)
                    {
                        SoundEngine.PlaySound(SoundID.Roar with { Pitch = -0.5f}, NPC.position);
                        mouthOpenRate = -1;
                        isMouthOpen = true;
                    }
                    else if (Timer == 60)
                        isMouthOpen = false;

                    // crick neck, flail hands around, and spawn some bones every interval
                    if (Timer >= startChasing && Timer % 15 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit2, NPC.Center);
                        //NPC.rotation += Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
                        if (Main.rand.NextBool())
                            NPC.rotation += Main.rand.NextFloat(-MathHelper.Pi, -0.5f);
                        else
                            NPC.rotation += Main.rand.NextFloat(0.5f, MathHelper.Pi);

                        LeftArm.ai[0] = LeftArm.Center.X + Main.rand.Next(-400, 400);
                        LeftArm.ai[1] = LeftArm.Center.Y + Main.rand.Next(-400, 400);
                        RightArm.ai[0] = LeftArm.Center.X + Main.rand.Next(-400, 400);
                        RightArm.ai[1] = LeftArm.Center.Y + Main.rand.Next(-400, 400);

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 vel = new Vector2(0, -Main.rand.NextFloat(7, 10));
                            vel = vel.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4));
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<SupremeBone>(), 200, 0, -1, NPC.target);
                        }
                    }

                    // chase player
                    if (Timer >= startChasing)
                    {
                        mouthOpenRate = 10;
                        NPC.velocity += NPC.DirectionTo(Target.Center);
                        NPC.velocity = NPC.velocity.ClampMagnitude(-22, 22);
                    }

                    if (Timer >= 700)
                    {
                        NPC.velocity = Vector2.Zero;
                        Timer = 0;
                        mouthOpenRate = origMouthOpenRate;
                        Mode = (int)AttackTypes.WanderAbovePlayer;
                    }
                    break;
                case (int)AttackTypes.WanderAbovePlayer:
                    headContact = false;
                    mouthOpenRate = -1;
                    if (Timer == 0 || Timer == 1)
                    {
                        isMouthOpen = false;
                    }
                    
                    // if starting or close to target, make up a new target
                    // having the failsafe be triggered by vector2.zero means he cant go to the top left of the world
                    // zzzzzzzzzzz
                    int boundingBoxVerticalOffset = 125;
                    int boundingBoxHeight = 400;
                    int boundingBoxWidth = 400;
                    int boundingBoxHalfWidth = boundingBoxWidth / 2;
                    float leftRightMovement = (float)Math.Sin(Timer / 50) * 250;
                    if (idealPosForWandering == Vector2.Zero || Timer % 25 == 0 || NPC.Distance(idealPosForWandering) < 100)
                    {
                        // making a "bounding box" of areas he can choose to go to
                        float lowerBound = Target.Center.Y - boundingBoxVerticalOffset;
                        float upperBound = Target.Center.Y - boundingBoxHeight - boundingBoxVerticalOffset;
                        float leftBound = Target.Center.X - boundingBoxHalfWidth + leftRightMovement;
                        float rightBound = Target.Center.X + boundingBoxHalfWidth + leftRightMovement;
                        Vector2 earthBound = new Vector2(Main.rand.NextFloat(leftBound, rightBound), Main.rand.NextFloat(lowerBound, upperBound));
                        idealPosForWandering = earthBound;
                    }
                    /*
                    // show bounding box in oppa dust style
                    float lowerBound2 = Target.Center.Y - boundingBoxVerticalOffset;
                    float upperBound2 = Target.Center.Y - boundingBoxHeight - boundingBoxVerticalOffset;
                    float leftBound2 = Target.Center.X - boundingBoxHalfWidth + leftRightMovement;
                    float rightBound2 = Target.Center.X + boundingBoxHalfWidth + leftRightMovement;
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound2, rightBound2), Main.rand.NextFloat(lowerBound2, upperBound2)), DustID.BlueFairy, Vector2.Zero);
                        Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound2, rightBound2), lowerBound2), DustID.BlueFairy, Vector2.Zero);
                        Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound2, rightBound2), upperBound2), DustID.BlueFairy, Vector2.Zero);
                        Dust.NewDustPerfect(new Vector2(leftBound2, Main.rand.NextFloat(lowerBound2, upperBound2)), DustID.BlueFairy, Vector2.Zero);
                        Dust.NewDustPerfect(new Vector2(rightBound2, Main.rand.NextFloat(lowerBound2, upperBound2)), DustID.BlueFairy, Vector2.Zero);
                    }
                    Dust.NewDustPerfect(idealPosForWandering, DustID.CrimsonSpray, Vector2.Zero);
                    */

                    NPC.velocity += NPC.DirectionTo(idealPosForWandering) * 1.25f;
                    NPC.velocity = NPC.velocity.ClampMagnitude(-15, 15);

                    // manually open mouth to align w firing of projectile
                    // this is important cuz we wanna switch to real timer, not clientside vanity timer
                    int skullFireRate = 12;
                    if (Timer > skullFireRate && Timer % skullFireRate == 0)
                    {
                        if (isMouthOpen)
                        {
                            isMouthOpen = false;
                        }
                        else
                        {
                            isMouthOpen = true;
                            SoundEngine.PlaySound(SoundID.Item73 with { MaxInstances = -1, Volume = 2f }, NPC.position);
                            Vector2 mouthPos = new Vector2(NPC.Center.X, NPC.Center.Y + 20);

                            // dust explosion upon firing skull
                            for (int i = 0; i < 24; i++)
                            {
                                Vector2 dustVelocity = new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-10, 10));
                                Dust.NewDustPerfect(mouthPos, ModContent.DustType<BrimstoneFireDustMatte>(), dustVelocity);
                            }
                            Vector2 velocity = NPC.DirectionTo(Target.Center) * 8;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), mouthPos, velocity, ModContent.ProjectileType<SupremeSkull>(), 200, 0, Main.myPlayer);
                        }
                    }

                    if (Timer >= 700)
                    {
                        headContact = true;
                        mouthOpenRate = origMouthOpenRate;
                        NPC.velocity = Vector2.Zero;
                        idealPosForWandering = Vector2.Zero;
                        
                        Timer = 0;
                        Mode = (int)AttackTypes.SpinAroundPlayer;
                    }
                    break;
            }

            #region Mouth Open And Close
            // if u set mouthOpenRate to -1... he never opens and/or closes his mouth!
            if (mouthOpenRate != -1 && TimerVisual % mouthOpenRate == 0)
            {
                if (isMouthOpen)
                {
                    isMouthOpen = false;
                }
                else
                {
                    isMouthOpen = true;
                }
            }
            #endregion

            #region Head Spin
            if (spinHead)
            {
                // functionally resets the spin level so u dont end up w a huge value
                if (NPC.rotation >= MathHelper.TwoPi)
                {
                    NPC.rotation -= MathHelper.TwoPi;
                }
                NPC.rotation += 0.5f;
            }
            else
            {
                // if left-leaning, reset rot towards right. if right leaning, reset rot towards left
                if (NPC.rotation >= MathHelper.Pi)
                {
                    NPC.rotation = MathHelper.Lerp(NPC.rotation, MathHelper.TwoPi, 0.1f);
                }
                else if (NPC.rotation >= 0)
                {
                    NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
                }
                // and for negatives
                else if (NPC.rotation <= -MathHelper.Pi)
                {
                    NPC.rotation = MathHelper.Lerp(NPC.rotation, MathHelper.TwoPi, 0.1f);
                }
                else if (NPC.rotation <= 0)
                {
                    NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
                }
                // note: if the head ends left-leaning, it wont be reset to zero. this could cause issues
            }
            #endregion

            Timer++;
            TimerVisual++;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => headContact;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glowmask = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossChanges/SupremeCalamitas/SupremeSkeletronGlow").Value;

            int mouthOpen = isMouthOpen ? 1 : 0;
            float eyeOpacity = Math.Clamp((float)Math.Sin(TimerVisual * 0.01f) + 0.5f, 0, 1);

            spriteBatch.Draw(texture, NPC.Center - screenPos, texture.Frame(1, 2, 0, mouthOpen), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 4), NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowmask, NPC.Center - screenPos, glowmask.Frame(1, 2, 0, mouthOpen), Color.White * eyeOpacity, NPC.rotation, new Vector2(glowmask.Width / 2, glowmask.Height / 4), NPC.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
