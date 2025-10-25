using System;
using System.Security.Policy;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Carcinogen
{
    public class SealedSky : CustomSky
    {
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.IsActive<SealedSubworld>();
            }
        }

        public static Color ChooseSealedColor(Player p)
        {
            if (Filters.Scene["CalRemix:VoidColors"].IsActive())
                return Color.Black;
            if (NPC.AnyNPCs(ModContent.NPCType<Disilphia>()))
                return Color.Brown;
            if (p.InModBiome<UnsealedSeaBiome>())
            {
                return Color.Gray;
            }
            else if (p.InModBiome<DarnwoodSwampBiome>())
            {
                return Color.DarkOliveGreen;
            }
            else if (p.InModBiome<VoidForestBiome>())
            {
                return Color.White with { A = 0 };
            }
            if (p.InModBiome<TurnipBiome>())
            {
                return Color.MediumPurple;
            }
            else if (p.InModBiome<VolcanicFieldBiome>())
            {
                return Color.OrangeRed;
            }
            else if (p.InModBiome<CarnelianForestBiome>())
            {
                return Color.Red;
            }
            else if (p.InModBiome<BadlandsBiome>())
            {
                return Color.MediumPurple;
            }
            else if (p.InModBiome<BarrensBiome>())
            {
                return Color.Gray;
            }
            else if (p.InModBiome<SealedFieldsBiome>())
            {
                return Color.Purple;
            }
            else
                return Color.Purple;
        }

        public static Color current = new();

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                Deactivate(Array.Empty<object>());
                return;
            }

            BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity + 0.01f, 0f, 1f);

            Opacity = BackgroundIntensity;

            current = Color.Lerp(current, ChooseSealedColor(Main.LocalPlayer), 0.01f);
            if (Main.LocalPlayer.InModBiome<BarrensBiome>() && !Main.LocalPlayer.InModBiome<VolcanicFieldBiome>() && !Main.LocalPlayer.InModBiome<BadlandsBiome>())
            {
                current = Color.Gray;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth < 0)
            {
                bool disilAlive = NPC.AnyNPCs(ModContent.NPCType<Disilphia>());
                float mult = disilAlive ? 1.5f : 0.6f;
                Color finalColor = current * mult;
                float faceOpacity = 0;
                Color faceColor = Color.White;
                Color brown = new Color(136, 0, 21);
                bool shake = false;
                if (disilAlive)
                {
                    NPC n = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Disilphia>())];
                    if (n.ModNPC is Disilphia disil)
                    {
                        if (disil.CurrentPhase == Disilphia.PhaseType.Ultimate)
                        {
                            float timer = disil.Timer;

                            if (timer < Disilphia.ULTIMA_BlackEnd)
                            {
                                finalColor = Color.Lerp(finalColor, Color.Black, Utils.GetLerpValue(0, Disilphia.ULTIMA_BlackEnd, timer, true));
                            }
                            else if (timer < Disilphia.ULTIMA_Face)
                            {
                                finalColor = Color.Black;
                            }
                            else if (timer >= Disilphia.ULTIMA_Face && timer <= Disilphia.ULTIMA_FaceDuration)
                            {
                                finalColor = Color.Black;
                                faceOpacity = MathHelper.Lerp(0.2f, 0, Utils.GetLerpValue(Disilphia.ULTIMA_Face, Disilphia.ULTIMA_FaceDuration, timer, true));
                            }
                            else if (timer < Disilphia.ULTIMA_Attack)
                            {
                                finalColor = Color.Black;
                            }
                            else if (timer >= Disilphia.ULTIMA_Attack && timer <= Disilphia.ULTIMA_FadeOutBlackGate)
                            {
                                faceColor = Color.Black;
                                faceOpacity = MathHelper.Lerp(0, 1, Utils.GetLerpValue(Disilphia.ULTIMA_Attack, Disilphia.ULTIMA_Attack + 10, timer, true));
                                finalColor = Color.Lerp(Color.Black, brown, Utils.GetLerpValue(Disilphia.ULTIMA_Attack, Disilphia.ULTIMA_Attack + 10, timer, true));
                                shake = true;
                            }
                            else if (timer > Disilphia.ULTIMA_FadeOutBlackGate && timer < Disilphia.ULTIMA_FadeOutBlackDuration)
                            {
                                faceColor = Color.Black;
                                faceOpacity = MathHelper.Lerp(1, 0, Utils.GetLerpValue(Disilphia.ULTIMA_FadeOutBlackGate, Disilphia.ULTIMA_FadeOutBlackDuration, timer, true));
                                finalColor = Color.Lerp(brown, Color.Black, Utils.GetLerpValue(Disilphia.ULTIMA_FadeOutBlackGate, Disilphia.ULTIMA_FadeOutBlackDuration, timer, true));
                                shake = true;
                            }
                            else if (timer <= Disilphia.ULTIMA_FinalFade)
                            {
                                finalColor = Color.Black;
                            }
                            else if (timer > Disilphia.ULTIMA_FinalFade && timer < Disilphia.ULTIMA_FinalFadeDuration)
                            {
                                finalColor = Color.Lerp(Color.Black, finalColor, Utils.GetLerpValue(Disilphia.ULTIMA_FinalFade, Disilphia.ULTIMA_FinalFadeDuration, timer, true));
                            }
                        }
                    }
                }
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y), finalColor);
                if (faceOpacity > 0)
                {
                    Texture2D face = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/DisilphiaFace").Value;
                    Vector2 offset = Vector2.Zero;
                    if (shake)
                    {
                        offset += Main.rand.NextVector2Circular(48, 48);
                    }
                    spriteBatch.Draw(face, new Vector2(Main.ScreenSize.X / 2f, Main.ScreenSize.Y / 3f) + offset, null, faceColor * faceOpacity, 0, face.Size() / 2, 1, 0, 0);
                }
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
