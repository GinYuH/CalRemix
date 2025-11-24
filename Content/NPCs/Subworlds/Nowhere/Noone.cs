using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod;
using Terraria.Audio;

namespace CalRemix.Content.NPCs.Subworlds.Nowhere
{
    public class Noone : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 27;
            NPC.lifeMax = 10000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = ScornEater.HitSound with { Pitch = -2 };
            NPC.DeathSound = ScornEater.DeathSound with { Pitch = 2 };
            SpawnModBiomes = new int[1] { ModContent.GetInstance<NowhereBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            if (NPC.ai[1] == 0)
            {
                Timer++;
                if ((Timer % 150 == 0) || Math.Abs(NPC.velocity.X) < 1 || Math.Abs(NPC.velocity.Y) < 1)
                {
                    NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1, 2);
                    if (Main.rand.NextBool(3) && NPC.velocity.Y < 0)
                    {
                        NPC.velocity.Y *= -1;
                    }
                }
                if (Main.player[NPC.target].Distance(NPC.Center) < 100)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 120)
                    {
                        Enrage();
                    }
                    else
                    {
                        NPC.ai[1] = 0;
                    }
                }
            }
            else
            {
                NPC.velocity = NPC.SafeDirectionTo(Main.player[NPC.target].Center) * 2;
            }
            if (NPC.justHit && NPC.ai[1] == 0)
            {
                Enrage();
            }
        }

        public void Enrage(bool sound = true)
        {
            if (sound)
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/GenericJumpscare") with { Pitch = 3.6f, Volume = 3 });
            NPC.ai[1] = 1;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type ==  NPC.type && n.ai[1] == 0)
                {
                    n.ModNPC<Noone>().Enrage(false);
                }
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D face = ModContent.Request<Texture2D>(Texture + "Face").Value;

            float shakeMult = MathHelper.Lerp(1, 10, Utils.GetLerpValue(NPC.lifeMax, 0, NPC.life, true));
            float shakeMultFace = MathHelper.Lerp(2, 20, Utils.GetLerpValue(NPC.lifeMax, 0, NPC.life, true));

            spriteBatch.Draw(tex, NPC.Center - screenPos + Main.rand.NextVector2Circular(shakeMult, shakeMult), NPC.frame, Color.White, NPC.rotation, tex.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(face, NPC.Center - screenPos + Main.rand.NextVector2Circular(shakeMultFace, shakeMultFace), NPC.frame, Color.White, NPC.rotation, tex.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
