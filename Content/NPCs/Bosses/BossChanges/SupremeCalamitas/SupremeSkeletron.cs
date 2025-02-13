using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.NPCs.SupremeCalamitas;
using System;
using CalamityMod.Projectiles.Boss;
using System.Diagnostics;
using Terraria;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using CalamityMod.Items.Placeables.Furniture;
using CalRemix.Content.Projectiles.Accessories;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod;
using Terraria.Audio;

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
        private float mouthOpenRate = origMouthOpenRate;

        public enum AttackTypes
        {
            AttachToCursor = -3,
            LerpTowardsCursor = -2,
            None = -1,
            Spawn = 0,
            SpinAroundPlayer = 1,
            HoverOverPlayerAndBeEvil = 2,
            ChasePlayer = 3
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eucharist Damsel");
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
            NPC.ai[2] = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SupremeSkeletronHand>(), 0, 0, 0, 0, NPC.whoAmI, Target.whoAmI);
            NPC.ai[3] = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SupremeSkeletronHand>(), 0, 0, 0, 1, NPC.whoAmI, Target.whoAmI);


        }
        public override void AI()
        {
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
                    
                    if (Timer >= 300)
                    {
                        Timer = 0;
                        Mode = (int)AttackTypes.SpinAroundPlayer;
                    }
                    break;
                case (int)AttackTypes.SpinAroundPlayer:
                    spinHead = true;

                    float IdealPositionX = Target.Center.X - (int)(Math.Cos(Timer * 0.2f) * 350);
                    float IdealPositionY = Target.Center.Y - (int)(Math.Sin(Timer * 0.2f) * 350);
                    Vector2 idealPosition = new Vector2(IdealPositionX, IdealPositionY);
                    NPC.Center = Vector2.Lerp(NPC.Center, idealPosition, 0.1f);
                    //Dust.NewDustPerfect(idealPosition, DustID.CrimsonSpray, Vector2.Zero);

                    //TODO: disable contact damage, if timer > 50 reenable contact damage

                    if (Timer > 50 && Main.rand.NextBool(12))
                    {
                        //TODO: DUST WHEN SPAWNING PROJS
                        //TODO: GIVE PROJS GLOWMASKS N SHIT

                        SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = -1, Volume = 2f }, NPC.position);
                        int sickle1 = Projectile.NewProjectile(NPC.GetSource_FromThis(), LeftArm.Center, LeftArm.Center.DirectionTo(Target.Center) * -15, ModContent.ProjectileType<SupremeSickle>(), 200, 0, -1, LeftArm.Center.DirectionTo(Target.Center).X, LeftArm.Center.DirectionTo(Target.Center).Y);
                        int sickle2 = Projectile.NewProjectile(NPC.GetSource_FromThis(), RightArm.Center, RightArm.Center.DirectionTo(Target.Center) * -15, ModContent.ProjectileType<SupremeSickle>(), 200, 0, -1, RightArm.Center.DirectionTo(Target.Center).X, RightArm.Center.DirectionTo(Target.Center).Y);
                    }

                    if (Timer >= 500)
                    {
                        spinHead = false;

                        Timer = 0;
                        Mode = 2;
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
                        mouthOpenRate = 1000000;
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
                        Mode = (int)AttackTypes.SpinAroundPlayer;
                    }
                    break;
            }

            #region Dumb Stupid Mouth Stuff Fuck You
            // sorry i just felt like being mean
            if (Timer % mouthOpenRate == 0)
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

        //public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        //{
            //return base.CanHitPlayer(target, ref cooldownSlot);
        //}

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
