using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class DreadonFriendly : ModNPC
    {
        public override string Texture => "CalRemix/Content/NPCs/Subworlds/Sealed/Dreadon";
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];


        public Vector2 TargetLocation
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public bool IsFlying
        { 
            get => NPC.Calamity().newAI[3] == 1;
            set => NPC.Calamity().newAI[3] = value.ToInt();
        }

        public bool IsGroovin
        {
            get => NPC.Calamity().newAI[0] == 1;
            set => NPC.Calamity().newAI[0] = value.ToInt();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.lifeMax = 20000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.dontTakeDamageFromHostiles = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SealedUndergroundBiome>().Type };
        }
        public override void AI()
        {
            if (Main.LocalPlayer.controlUseItem)
            {
                //State = 1;
            }
            if (State == 1)
            {
                NPC.boss = true;
                IsGroovin = false;
                Timer++;
                if (Timer == 1)
                {
                    NPC.velocity.Y = -6;
                    NPC.noTileCollide = true;
                }
                else if (Timer == 70)
                {
                    NPC.noGravity = true;
                    IsFlying = true;
                    TargetLocation = NPC.Center - Vector2.UnitY * 300;
                }
                else if (Timer == 180)
                {
                    TargetLocation = Target.Center - Vector2.UnitY * 300;
                }
                else if (Timer == 290)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)SealedSubworldData.tentPos.X, (int)SealedSubworldData.tentPos.Y - 600, ModContent.NPCType<SkeletronOmega>());
                }
                else if (Timer > 300 && !NPC.AnyNPCs(ModContent.NPCType<SkeletronOmega>()))
                {
                    State = 2;
                    Timer = 0;
                }
            }
            else if (State == 2)
            {
                Timer++;
                if (Timer == 1)
                {
                    TargetLocation = SealedSubworldData.tentPos - Vector2.UnitY * 200;
                }
                else if (Timer == 120)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)SealedSubworldData.TentLeft - 300, (int)SealedSubworldData.tentPos.Y, ModContent.NPCType<Godraycaster>());
                }
                else if (Timer == 600)
                {
                    int oldEye = NPC.FindFirstNPC(ModContent.NPCType<Godraycaster>());
                    if (oldEye != -1)
                    {
                        int newEye = NPC.NewNPC(NPC.GetSource_FromThis(), (int)SealedSubworldData.TentLeft - 300, (int)Main.npc[oldEye].Center.Y, ModContent.NPCType<Godraycaster>());
                        Main.npc[newEye].ModNPC<Godraycaster>().IsSecondEye = true;
                        Main.npc[newEye].localAI[3] = oldEye + 1;
                        Main.npc[newEye].netUpdate = true;
                        Main.npc[oldEye].localAI[3] = newEye + 1;
                        Main.npc[oldEye].netUpdate = true;
                    }
                }
                else if (Timer > 126 && !NPC.AnyNPCs(ModContent.NPCType<Godraycaster>()))
                {
                    State = 3;
                    Timer = 0;
                }
            }
            else if (State == 3)
            {
                Timer++;
                if (Timer == 1)
                {
                    TargetLocation = SealedSubworldData.tentPos - Vector2.UnitY * 50;
                }
                else if (Timer == 120)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)SealedSubworldData.TentLeft - 300, (int)SealedSubworldData.tentPos.Y + 100, ModContent.NPCType<ObliteratorHead>());
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
                }
                else if (Timer > 620 && !NPC.AnyNPCs(ModContent.NPCType<ObliteratorHead>()))
                {
                    State = 0;
                    Timer = 0;
                    IsFlying = false;
                    NPC.boss = false;
                    NPC.noTileCollide = false;
                    NPC.noGravity = false;
                    NPC.velocity.X = 0;
                    NPC.Calamity().ShouldCloseHPBar = true;
                    NPC.life = NPC.lifeMax;
                    NPC.dontTakeDamage = false;
                }
            }

            NPC.Calamity().ShouldCloseHPBar = true;

            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (NPC.frameCounter > 8 * 6)
                NPC.frameCounter = 0;


            if (IsFlying)
            {
                CalamityUtils.SmoothMovement(NPC, 10, TargetLocation - NPC.Center, 20, 0.8f, true);
            }

            if (NPC.life < (int)(NPC.lifeMax * 0.2f))
            {
                NPC.life = NPC.lifeMax;
                NPC.dontTakeDamage = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D head = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/DreadonHead").Value;
            Texture2D carpet = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/AuricCarpet").Value;

            if (IsFlying)
                spriteBatch.Draw(carpet, NPC.Bottom - screenPos - Vector2.UnitY * 8, carpet.Frame(1, 6, 0, (int)(Utils.Lerp(0, 5, Utils.GetLerpValue(0, 7 * 6, NPC.frameCounter, true)))), drawColor * Utils.GetLerpValue(50, 60, Timer, true), 0, new Vector2(carpet.Width / 2, 0), NPC.scale * Utils.GetLerpValue(50, 60, Timer, true), fx, 0f);

            int armLength = 26;
            int legLength = 24;
            Vector2 legOffset = new Vector2(4, 20);
            Vector2 armOffset = new Vector2(16, 10);

            float armRot = MathHelper.ToRadians(50);

            if (IsGroovin)
                armRot += MathF.Sin(Main.GlobalTimeWrappedHourly * 10) * 0.5f;

            DrawLimb(spriteBatch, legOffset - screenPos, legLength, 0);
            DrawLimb(spriteBatch, legOffset with { X = -legOffset.X} - screenPos, legLength, 0);
            DrawLimb(spriteBatch, armOffset - screenPos, armLength, -armRot, true);
            DrawLimb(spriteBatch, armOffset with { X = -armOffset.X} - screenPos, armLength, armRot, true);

            float bodyOffset = 0;
            float headOffset = 12;

            float headRot = 0;

            if (IsGroovin || NPC.IsABestiaryIconDummy)
            {
                headRot = MathF.Sin(Main.GlobalTimeWrappedHourly * 5) * 0.25f;
                headOffset += MathF.Sin(Main.GlobalTimeWrappedHourly * 10 + 1) * 2;
                bodyOffset = MathF.Sin(Main.GlobalTimeWrappedHourly * 10) * 2;
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + texture.Height / 14 + bodyOffset), texture.Frame(1, 7, 0, (int)Utils.Lerp(0, 6, Utils.GetLerpValue(0, 7 * 6, NPC.frameCounter, true))), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 7), NPC.scale, fx, 0f);
            spriteBatch.Draw(head, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY - texture.Height / 14 + headOffset), null, drawColor, headRot, new Vector2(head.Width / 2, head.Height), NPC.scale, fx, 0f);

           return false;
        }

        public void DrawLimb(SpriteBatch spriteBatch, Vector2 pos, float lenght, float rot, bool arm = false)
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.Center + pos, new Rectangle(0, 0, 4, (int)lenght), Color.Black, rot, new Vector2(2, 0), NPC.scale, 0, 0);

            if (arm)
            {
                for (int i = 0; i < 2; i++)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.Center + pos + Vector2.UnitY.RotatedBy(rot) * lenght * 0.6f, new Rectangle(0, 0, 4, 10), Color.Black, rot + MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / 1f), new Vector2(2, 0), NPC.scale, 0, 0);
                }
            }
            else
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.Center + pos + Vector2.UnitY.RotatedBy(rot) * lenght, new Rectangle(0, 0, 4, 10), Color.Black, rot - MathHelper.PiOver2 * pos.X.DirectionalSign(), new Vector2(2, 0), NPC.scale, 0, 0);
            }

        }

        public override bool CheckActive()
        {
            return false;
        }
        public override bool NeedSaving()
        {
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
