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
using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;

namespace CalRemix.Content.NPCs.Subworlds.Edis
{
    public class Lobotomite : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float Mode => ref NPC.ai[1];
        public ref float TimerVisual => ref NPC.localAI[0];

        private static int origMouthOpenRate = 30;
        
        private bool isMouthOpen = false;
        private bool spinHead = false;
        private float mouthOpenRate = origMouthOpenRate;
        private bool doEyeGlow = false;

        public enum AttackTypes
        {
            Idle = 0,
            Awake = 1,
            ChasePlayer = 2
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eucharist Damsel");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 38;
            NPC.height = 30;
            NPC.lifeMax = 5000;
            NPC.damage = 0;
            NPC.defense = 50;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrotherHit;
            NPC.DeathSound = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrotherDeath;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
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
                case (int)AttackTypes.Idle:
                    NPC.noTileCollide = false;
                    NPC.noGravity = false;
                    mouthOpenRate = -1;

                    if (NPC.Center.Distance(Target.Center) < 600)
                    {
                        Timer = 0;
                        NPC.velocity = Vector2.Zero;
                        Mode = (int)AttackTypes.Awake;
                        spinHead = true;
                    }

                    // this wouldnt normally work, but since its the first loop, it does
                    if (Timer == 0)
                    {
                        NPC.rotation = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
                    }
                    break;
                case (int)AttackTypes.Awake:
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;

                    if (Timer == 1)
                        NPC.velocity.Y -= 16f;

                    if (NPC.velocity.Y < 0)
                        NPC.velocity.Y *= 0.9f;
                    else if (NPC.velocity.Y > -0.01)
                        NPC.velocity.Y = 0;

                    if (NPC.velocity.Y >= -0.5f)
                    {
                        SoundEngine.PlaySound(SoundID.Roar with { Pitch = -0.5f }, NPC.position);
                        mouthOpenRate = -1;
                        isMouthOpen = true;
                        doEyeGlow = true;
                        TimerVisual = 100;
                    }

                    if (Timer >= 100)
                    {
                        Timer = 0;
                        mouthOpenRate = origMouthOpenRate;
                        Mode = (int)AttackTypes.ChasePlayer;
                    }
                    break;
                case (int)AttackTypes.ChasePlayer:
                    if (Timer == 1)
                        isMouthOpen = false;

                    // crick neck, flail hands around, and spawn some bones every interval
                    if (Timer % 15 == 0 || Timer == 1)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit2, NPC.Center);
                        //NPC.rotation += Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
                        if (Main.rand.NextBool())
                            NPC.rotation += Main.rand.NextFloat(-MathHelper.Pi, -0.5f);
                        else
                            NPC.rotation += Main.rand.NextFloat(0.5f, MathHelper.Pi);

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 vel = new Vector2(0, -Main.rand.NextFloat(7, 10));
                            vel = vel.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4));
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<SupremeBone>(), 200, 0, -1, NPC.target);
                        }
                    }

                    // chase player
                    mouthOpenRate = 10;
                    NPC.velocity += NPC.DirectionTo(Target.Center);
                    NPC.velocity = NPC.velocity.ClampMagnitude(-22, 22);

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

            #region Head Rotation Stuff
            if (spinHead)
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
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => true;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glowmask = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossChanges/SupremeCalamitas/SupremeSkeletronGlow").Value;

            int mouthOpen = isMouthOpen ? 1 : 0;
            float eyeOpacity = doEyeGlow ? Math.Clamp((float)Math.Sin(TimerVisual * 0.01f) + 0.5f, 0.5f, 1) : 0f;

            spriteBatch.Draw(texture, NPC.Center - screenPos, texture.Frame(1, 2, 0, mouthOpen), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 4), NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowmask, NPC.Center - screenPos, glowmask.Frame(1, 2, 0, mouthOpen), Color.White * eyeOpacity, NPC.rotation, new Vector2(glowmask.Width / 2, glowmask.Height / 4), NPC.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
