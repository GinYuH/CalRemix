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

        private bool isMouthOpen = false;
        private bool spinHead = false;
        private bool manualMouthOpen = false;

        public enum AttackTypes
        {
            MoveTowardsCursor = -1,
            None = 0,
            SpinAroundPlayer = 1,
            HoverOverPlayerAndBeEvil = 2
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
                case (int)AttackTypes.MoveTowardsCursor:
                    NPC.Center = Vector2.Lerp(NPC.Center, Main.MouseWorld, 0.1f);

                    break;
                case (int)AttackTypes.None:
                    
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

                    if (Timer > 50 && Main.rand.NextBool(12))
                    {
                        //TODO: DUST WHEN SPAWNING PROJS
                        //TODO: GIVE PROJS GLOWMASKS N SHIT
                        
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
                    //TODO: SPLIT INTO TWO ATTACKS
                    int startChasing = 200;
                    
                    if (Timer >= startChasing && Timer % 15 == 0)
                    {
                        LeftArm.ai[0] = LeftArm.Center.X + Main.rand.Next(-400, 400);
                        LeftArm.ai[1] = LeftArm.Center.Y + Main.rand.Next(-400, 400);
                        RightArm.ai[0] = LeftArm.Center.X + Main.rand.Next(-400, 400);
                        RightArm.ai[1] = LeftArm.Center.Y + Main.rand.Next(-400, 400);

                        NPC.rotation += Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
                    }

                    if (Timer == startChasing)
                    {
                        //roat
                    }

                    // chase player, else give time for arms to swing around
                    if (Timer >= startChasing)
                    {
                        manualMouthOpen = true;
                        NPC.velocity += NPC.DirectionTo(Target.Center);
                        NPC.velocity = NPC.velocity.ClampMagnitude(-22, 22);
                        Main.NewText(NPC.velocity);

                        // shortened laugh time. redundant but who cares 
                        if (Timer % 10 == 0)
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
                    }
                    else
                    {
                        NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(Target.Center.X, Target.Center.Y - 250), 0.1f);
                    }

                    if (Timer >= 4000)
                    {
                        NPC.velocity = Vector2.Zero;
                        Timer = 0;
                        manualMouthOpen = false;
                        Mode = (int)AttackTypes.SpinAroundPlayer;
                    }
                        
                    break;
            }

            #region Dumb Stupid Mouth Stuff Fuck You
            // sorry i just felt like being mean
            if (manualMouthOpen == false && Timer % 30 == 0)
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
