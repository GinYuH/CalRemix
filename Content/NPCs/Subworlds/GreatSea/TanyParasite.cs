using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.Animations;
using CalRemix.Core.World;
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class TanyParasite : ModNPC
    {
        public int BodyIndex => (int)NPC.ai[0] - 1;

        public ref float Timer => ref NPC.ai[1];

        public NPC Body => Main.npc[BodyIndex];

        List<VerletSimulatedSegment> Segments = new List<VerletSimulatedSegment>();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 70;
            NPC.width = 70;
            NPC.height = 40;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit45 with { Pitch = 0.4f };
            NPC.DeathSound = SoundID.NPCDeath32;
            NPC.GravityIgnoresLiquid = true;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
            NPC.waterMovementSpeed = 1f;
        }

        public override void AI()
        {
            Timer++;
            NPC.TargetClosest();
            NPC.timeLeft = 1000000;
            if (BodyIndex != -1)
            {
                if (!Body.active || Body.life <= 0 || Body.type != ModContent.NPCType<TanyHead>())
                {
                    NPC.active = false;
                    return;
                }
            }
            else
            {
                NPC.active = false;
                return;
            }

            int segCount = 20;
            Segments = CalRemixHelper.CreateVerletChain(ref Segments, segCount, Body.Center, new List<int>() { segCount - 1 });

            Segments[^1].oldPosition = Segments[^1].position;
            Segments[^1].position = Body.Center;

            Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 10, loops: 10, gravity: 0.1f);

            NPC.rotation = NPC.DirectionTo(Segments[^2].position).ToRotation() + MathHelper.PiOver2;


            NPC.netUpdate = true;
            NPC.netSpam = 0;

            NPC.Center = Segments[0].position;

            if (Timer % 90 == 0 && Timer > 30)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemQueenSlimeProjectileShoot with { Pitch = 1 }, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * 14, ModContent.ProjectileType<BrimstoneBarrage>(), NPC.damage, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (BodyIndex > -1)
            {
                for (int i = 0; i < Segments.Count; i++)
                {
                    VerletSimulatedSegment seg = Segments[i];
                    float rot = 0f;
                    int dist = 4;
                    if (i > 0)
                    {
                        rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                        dist = (int)seg.position.Distance(Segments[i - 1].position) + 2;
                    }
                    else
                    {
                        rot = NPC.rotation;
                    }
                    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, seg.position - Main.screenPosition, new Rectangle(0, 0, 4, dist), Color.Black, rot, new Rectangle(0, 0, 4, dist).Size() / 2, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                }

                Texture2D tex = TextureAssets.Npc[Type].Value;
                Texture2D eye = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/TanyParasiteEye").Value;
                SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 dir = NPC.IsABestiaryIconDummy ? Vector2.Zero : NPC.DirectionTo(Main.player[NPC.target].Center) * 4;
                spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
                spriteBatch.Draw(eye, NPC.Center - screenPos + dir, null, Color.White, 0, new Vector2(eye.Width / 2, eye.Height / 2), NPC.scale, 0, 0);

            }
            return false;
        }
    }
}
