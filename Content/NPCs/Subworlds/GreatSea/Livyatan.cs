using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis.Host.Mef;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Hostile;
using System.Collections.Generic;
using CalamityMod.InverseKinematics;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Livyatan : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public ref float CurrentPhase => ref NPC.ai[1];

        public ref float JawRotation => ref NPC.localAI[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 180;
            NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 56;
            NPC.lifeMax = 2000000;
            NPC.value = 1000000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            if (CurrentPhase == 0)
            {
            }
            else if (CurrentPhase == 1)
            {
            }
            else if (CurrentPhase == 2)
            {
            }
            handIK.Limbs[0].Rotation = GetIKRotationClamp((float)handIK.Limbs[0].Rotation, MathHelper.Pi, MathHelper.PiOver2);
            handIK.Update(NPC.Center + new Vector2(NPC.spriteDirection * 160, 40), Vector2.UnitY * 500);
            tailIK.Limbs[0].Rotation = GetIKRotationClamp((float)tailIK.Limbs[0].Rotation, MathHelper.ToRadians(270), MathHelper.ToRadians(120));
            tailIK.Update(NPC.Center + new Vector2(NPC.spriteDirection * -170, 0), NPC.Center + Vector2.UnitX * -NPC.spriteDirection * 500);
            NPC.spriteDirection = (Main.LocalPlayer.selectedItem == 0).ToDirectionInt();
            //JawRotation = NPC.Center.DirectionTo(Main.MouseWorld).ToRotation();


        }

        public float GetIKRotationClamp(float baseRotation, float min, float max)
        {
            float flipRot = NPC.spriteDirection == -1 ? MathHelper.Pi : 0;
            if (NPC.spriteDirection == -1)
            {
                return MathHelper.Clamp(baseRotation, NPC.spriteDirection * min + flipRot, NPC.spriteDirection * max + flipRot);
            }
            else
            {
                return MathHelper.Clamp(baseRotation, NPC.spriteDirection * max + flipRot, NPC.spriteDirection * min + flipRot);
            }
            return MathHelper.Clamp(baseRotation, NPC.spriteDirection * min + flipRot, NPC.spriteDirection * max + flipRot);
        }

        public LimbCollection handIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 96f, 142f);
        public Vector2 handDest = new Vector2();

        public LimbCollection tailIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 202f, 248f);
        public Vector2 tailDest = new Vector2();

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Head").Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "Mouth").Value;
            Texture2D arm = ModContent.Request<Texture2D>(Texture + "Arm").Value;
            Texture2D hand = ModContent.Request<Texture2D>(Texture + "Hand").Value;
            Texture2D pupil = ModContent.Request<Texture2D>(Texture + "Pupil").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "Eye").Value;
            Texture2D bodyLower = ModContent.Request<Texture2D>(Texture + "BodyLower").Value;
            Texture2D tail = ModContent.Request<Texture2D>(Texture + "Tail").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 headPos = new Vector2(180, -40);
            Vector2 adjustedHeadPos = new Vector2(180 * NPC.spriteDirection, -40);
            float headRotation = (NPC.Center + adjustedHeadPos).DirectionTo(Main.MouseWorld).ToRotation();
            headRotation *= NPC.spriteDirection;
            if (NPC.spriteDirection == -1)
            {
                headRotation += MathHelper.Pi;
            }
            adjustedHeadPos = adjustedHeadPos.RotatedBy(headRotation);

            DrawPiece(spriteBatch, jaw, screenPos, new Vector2(180, 50), new Vector2(334, 23), drawColor, rotationAnchor: JawRotation + headRotation);
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, fx, 0);
            DrawPiece(spriteBatch, head, screenPos, headPos, new Vector2(397, 54), drawColor, rotationOverride: headRotation);
            DrawPiece(spriteBatch, eye, screenPos, new Vector2(333, -3), new Vector2(10, 3), drawColor, rotationAnchor: headRotation);
            DrawPiece(spriteBatch, pupil, screenPos, new Vector2(333, -3), new Vector2(3, 3), drawColor, rotationAnchor: headRotation);

            for (int i = 0; i < handIK.Limbs.Length; i++)
            {
                Texture2D touse = (i == 0) ? arm : hand;
                Vector2 origin = (i == 0) ? new Vector2(30, 24) : new Vector2(35, 19);
                float offset = (i == 0) ? -MathHelper.PiOver4 * 3 : MathHelper.ToRadians(200);
                if (NPC.spriteDirection == -1)
                {
                    offset = -offset + MathHelper.Pi;
                }
                Limb limb = handIK.Limbs[i];
                spriteBatch.Draw(touse, limb.ConnectPoint - screenPos, null, NPC.GetAlpha(drawColor), (float)limb.Rotation + offset, GetPieceOrigin(touse.Width, origin), NPC.scale, fx, 0);
            }
            for (int i = 0; i < tailIK.Limbs.Length; i++)
            {
                Texture2D touse = (i == 0) ? bodyLower : tail;
                Vector2 origin = (i == 0) ? new Vector2(31, 52) : new Vector2(15, 43);
                float offset = (i == 0) ? MathHelper.ToRadians(180) : MathHelper.ToRadians(180);
                if (NPC.spriteDirection == -1)
                {
                    offset = -offset + MathHelper.Pi;
                }
                Limb limb = tailIK.Limbs[i];
                spriteBatch.Draw(touse, limb.ConnectPoint - screenPos, null, NPC.GetAlpha(drawColor), (float)limb.Rotation + offset, GetPieceOrigin(touse.Width, origin), NPC.scale, fx, 0);
            }
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, NPC.Center + adjustedHeadPos - screenPos, new Rectangle(0, 0, 22, 22), Color.Red, 0, Vector2.Zero, 1, 0);
            return false;
        }

        public void DrawPiece(SpriteBatch sb, Texture2D tex, Vector2 screenPos, Vector2 offset, Vector2 originOffset, Color drawColor, float rotationOverride = 0, float rotationAnchor = 0)
        {
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            sb.Draw(tex, GetPiecePosition(screenPos, offset, rotationOverride, rotationAnchor), null, NPC.GetAlpha(drawColor), NPC.rotation + NPC.spriteDirection * (rotationAnchor + rotationOverride), GetPieceOrigin(tex.Width, originOffset), NPC.scale, fx, 0);
        }

        public Vector2 GetPiecePosition(Vector2 screenPos, Vector2 offset, float rotationOverride = 0, float rotationAnchor = 0)
        {
            return NPC.Center - screenPos + NPC.scale * new Vector2(offset.X * NPC.spriteDirection, offset.Y).RotatedBy(NPC.rotation + rotationAnchor);
        }

        public Vector2 GetPieceOrigin(float baseWidth, Vector2 originalOffset)
        {
            return new Vector2(NPC.spriteDirection == 1 ? baseWidth - originalOffset.X : originalOffset.X, originalOffset.Y);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
