using CalamityMod;
using CalamityMod.BiomeManagers;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.UI;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace CalRemix.Content.NPCs.Minibosses.Echinoderion
{
    public class Echinoderion : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 70;
            NPC.width = 32;
            NPC.height = 110;
            NPC.defense = 8;
            NPC.lifeMax = 400;
            NPC.knockBackResist = 0.9f;
            NPC.value = Item.buyPrice(gold: 12, silver: 10);
            NPC.noGravity = false;
            NPC.HitSound = CalamityMod.NPCs.NormalNPCs.Rimehound.HitSound with { Pitch = 1 };
            NPC.DeathSound = SoundID.NPCDeath27 with { Pitch = 1 };
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = true;
            SpawnModBiomes = [ModContent.GetInstance<SulphurousSeaBiome>().Type];
        }

        public override void AI()
        {
            CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedUnicornAI(NPC, Mod);
            NPC.spriteDirection = -NPC.velocity.X.DirectionalSign();
            if (NPC.velocity.X != 0 && NPC.velocity.Y >= 0)
                NPC.Calamity().newAI[0] += 0.05f * NPC.velocity.X;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !spawnInfo.Player.Calamity().ZoneSulphur)
            {
                return 0f;
            }
            return 0.035f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_Head").Value;
            Texture2D neckFront = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_NeckFront").Value;
            Texture2D neckBack = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_NeckBack").Value;
            Texture2D legFront = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_LegFront").Value;
            Texture2D legBack = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_LegBack").Value;
            Texture2D tail = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_Tail").Value;
            Texture2D frill = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Echinoderion/Echinoderion_Frill").Value;

            spriteBatch.Draw(legBack, NPC.Center - screenPos + new Vector2(NPC.spriteDirection * -16, 12), null, drawColor, NPC.rotation - MathF.Sin(NPC.Calamity().newAI[0]) * 0.3f, GetOrigin(new Vector2(22, 10), legBack.Width), NPC.scale, NPC.FlippedEffects(true), 0f);
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, drawColor, NPC.rotation, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(true), 0f);
            spriteBatch.Draw(legFront, NPC.Center - screenPos + new Vector2(NPC.spriteDirection * 20, 6), null, drawColor, NPC.rotation + MathF.Sin(NPC.Calamity().newAI[0]) * 0.3f, GetOrigin(new Vector2(10, 6), legFront.Width), NPC.scale, NPC.FlippedEffects(true), 0f);
            Vector2 neckPos = new Vector2(NPC.spriteDirection * -54, 4);
            spriteBatch.Draw(neckBack, NPC.Center - screenPos + neckPos, null, drawColor, NPC.rotation, GetOrigin(new Vector2(22, 16), neckBack.Width), NPC.scale, NPC.FlippedEffects(true), 0f);
            Vector2 neck2Pos = neckPos + new Vector2(NPC.spriteDirection * -10, -2);
            spriteBatch.Draw(neckFront, NPC.Center - screenPos + neck2Pos, null, drawColor, NPC.rotation, GetOrigin(new Vector2(22, 20), neckFront.Width), NPC.scale, NPC.FlippedEffects(true), 0f);
            Vector2 headPos = neck2Pos + new Vector2(NPC.spriteDirection * -38, -12);
            spriteBatch.Draw(head, NPC.Center - screenPos + headPos, null, drawColor, NPC.rotation, GetOrigin(new Vector2(46, 44), head.Width), NPC.scale, NPC.FlippedEffects(true), 0f);

            Vector2 frillPosition = new Vector2(NPC.spriteDirection * -30, -16);

            Vector2 tentacleBase = new Vector2(NPC.spriteDirection * -60, -10);

            Main.spriteBatch.EnterShaderRegion();
            GameShaders.Misc["CalamityMod:TeslaTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ZapTrail"));
            for (int j = 0; j < 4; j++)
            {
                List<Vector2> frille = new List<Vector2>();
                float ptAmt = 10;
                for (int i = 0; i < ptAmt; i++)
                {
                    float flip = (NPC.spriteDirection == -1 ? MathHelper.Pi : 0);
                    float spread = MathHelper.Lerp(MathHelper.ToRadians(-95), MathHelper.ToRadians(-10), j / 3f);
                    Vector2 start = new Vector2(1, 16).RotatedBy(spread);
                    start.X *= NPC.spriteDirection;
                    Vector2 end = (new Vector2(1, 16).RotatedBy(spread).RotatedBy(MathF.Sin(3 * Main.GlobalTimeWrappedHourly + i + j) * 0.1f) * 5).RotatedBy(-MathF.Abs(NPC.velocity.X) * 0.1f);
                    end.X *= NPC.spriteDirection;
                    frille.Add(NPC.Center + tentacleBase + Vector2.Lerp(start, end, i / (float)(ptAmt - 1)));
                }
                PrimitiveRenderer.RenderTrail(frille.ToArray(), new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f) => 3), new PrimitiveSettings.VertexColorFunction((float f) => new Color(100, 175, 140) * 0.8f), shader: GameShaders.Misc["CalamityMod:TeslaTrail"]));
            }
            Main.spriteBatch.ExitShaderRegion();

            spriteBatch.Draw(frill, NPC.Center - screenPos + frillPosition, null, Color.White * 0.9f, NPC.rotation, GetOrigin(new Vector2(12, 16), frill.Width), NPC.scale, NPC.FlippedEffects(true), 0f);

            Rectangle testFrame = tail.Frame(1, 1, 0, 0);

            Matrix rotation = Matrix.CreateRotationY(MathF.Sin(NPC.Calamity().newAI[0] * 0.4f) * 1f + (NPC.spriteDirection == -1 ? MathHelper.Pi : 0));
            Matrix translation = Matrix.CreateTranslation(new Vector3(NPC.Center.X - screenPos.X + 36 * NPC.spriteDirection, NPC.Center.Y - screenPos.Y + 10, 0));
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -220, 220);
            Matrix renderMatrix = rotation * translation * view * projection;
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["CalRemix:NormalDraw"].GetShader().Shader;

            short[] indices = [0, 1, 2, 1, 3, 2];
            Vector3 center = new Vector3(40, 0, 0);

            Vector3 topLeft = new Vector3(center.X - (testFrame.Width / 2), center.Y - (testFrame.Height / 2), 0);
            Vector3 topRight = new Vector3(center.X + (testFrame.Width / 2), center.Y - (testFrame.Height / 2), 0);
            Vector3 botLeft = new Vector3(center.X - (testFrame.Width / 2), center.Y + (testFrame.Height / 2), 0);
            Vector3 botRight = new Vector3(center.X + (testFrame.Width / 2), center.Y + (testFrame.Height / 2), 0);

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new(topLeft, NPC.GetAlpha(drawColor), new Vector2(0, 0));
            vertices[1] = new(topRight, NPC.GetAlpha(drawColor), new Vector2(1, 0));
            vertices[2] = new(botLeft, NPC.GetAlpha(drawColor), new Vector2(0, 1));
            vertices[3] = new(botRight, NPC.GetAlpha(drawColor), new Vector2(1, 1));

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Parameters["textureResolution"].SetValue(tail.Size());
                effect.Parameters["sampleTexture"].SetValue(tail);
                effect.Parameters["frame"].SetValue(new Vector4(testFrame.X, testFrame.Y, testFrame.Width, testFrame.Height));
                effect.Parameters["uWorldViewProjection"].SetValue(renderMatrix);
                effect.Parameters["opacity"].SetValue(1);
                pass.Apply();

                Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
            }

            return false;
        }

        public Vector2 GetOrigin(Vector2 baseVector, int width)
        {
            if (NPC.spriteDirection == 1)
            {
                return new Vector2(width - baseVector.X, baseVector.Y);
            }
            return baseVector;
        }
    }
}
