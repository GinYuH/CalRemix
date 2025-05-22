using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Projectiles.Boss;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.NPCs.Eclipse;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class CrimsonKaijuHand : ModProjectile
    {
        public ref float NPCIndex => ref Projectile.ai[0];

        public ref float PlayerIndex => ref Projectile.ai[1];

        public ref float AttackTime => ref Projectile.localAI[0];

        public ref float MaxAttackTime => ref Projectile.localAI[1];

        public List<VerletSimulatedSegment> Segments;
        int segmentCount = 20;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 22222;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
            foreach (var v in Segments)
            {
                writer.Write(v.position.X);
                writer.Write(v.position.Y);
                writer.Write(v.locked);
                writer.Write(v.oldPosition.X);
                writer.Write(v.oldPosition.Y);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            Projectile.localAI[2] = reader.ReadSingle();
            List<VerletSimulatedSegment> segs = new List<VerletSimulatedSegment>();
            for (int i = 0; i < 10; i++)
            {
                VerletSimulatedSegment v = new VerletSimulatedSegment(new Vector2(reader.ReadSingle(), reader.ReadSingle()), reader.ReadBoolean());
                v.oldPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                segs.Add(v);
            }
            Segments = segs;
        }


        public override void AI()
        {
            Projectile.timeLeft = 22;
            NPC n = Main.npc[(int)NPCIndex];
            Player p = Main.player[(int)PlayerIndex];
            if (n == null || !n.active || n.type != ModContent.NPCType<CrimsonKaiju>() || n.life <= 0)
            {
                Projectile.Kill();
                return;
            }

            if (Segments == null)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(Projectile.Center);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                Segments[Segments.Count - 1].locked = true;
            }
            else
            {
                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = Projectile.Center;

                Segments[^1].oldPosition = Segments[^1].position;
                Segments[^1].position = n.Center + new Vector2(40 * n.direction, 0);

                Segments = VerletSimulatedSegment.SimpleSimulation(Segments, MathHelper.Lerp(8, 16, n.Distance(Projectile.Center) / 1200f), loops: segmentCount, gravity: 0.2f);

                Projectile.netUpdate = true;
                Projectile.netSpam = 0;
            }


            bool validPlayer = true;
            if (p == null || !p.active || p.dead)
            {
                Projectile.velocity = Projectile.SafeDirectionTo(n.Center) * 10;
                if (Projectile.Hitbox.Intersects(n.Hitbox))
                {
                    Projectile.Kill();
                    return;
                }
            }
            int hitPlayerTime = 30; // how long it takes to hit the player
            if (validPlayer)
            {
                Vector2 playerOff = Vector2.Zero;
                Projectile.localAI[0]++;
                float comp = Utils.GetLerpValue(0, hitPlayerTime, AttackTime, true);
                int dist = (int)Projectile.localAI[2];
                hitPlayerTime = 60;
                Vector2 origin = n.Center + new Vector2(40 * n.direction, 0);
                if (AttackTime <= hitPlayerTime)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.position = Vector2.Lerp(origin, origin + origin.DirectionTo(p.Center) * dist, comp);
                }
                if (AttackTime > hitPlayerTime)
                {
                    comp = Utils.GetLerpValue(hitPlayerTime, hitPlayerTime + 20, AttackTime, true);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.position = Vector2.Lerp(origin + origin.DirectionTo(p.Center) * dist, origin, comp);
                    if (Projectile.Distance(origin) < 40)
                    {
                        Projectile.Kill();
                        return;
                    }
                }
                if (p.Hitbox.Intersects(Projectile.Hitbox))
                {
                    MaxAttackTime = -22;
                }
                if (MaxAttackTime == -22)
                {
                    p.position = Projectile.position - playerOff;
                    p.velocity = Projectile.velocity;
                }                
            }
            Projectile.rotation = n.SafeDirectionTo(p.Center).ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Color color = lightColor;
            NPC kaiju = Main.npc[(int)NPCIndex];
            if (kaiju == null || !kaiju.active || kaiju.type != ModContent.NPCType<CrimsonKaiju>())
            {
                return false;
            }
            if (Segments != null)
            {
                for (int i = 0; i < Segments.Count; i++)
                {
                    VerletSimulatedSegment seg = Segments[i];
                    float rot;
                    float dist;
                    if (i > 0)
                    {
                        rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                        dist = seg.position.Distance(Segments[i - 1].position);
                    }
                    else
                    {
                        rot = Projectile.rotation;
                        dist = seg.position.Distance(Segments[i + 1].position);
                    }
                    if (i > 0)
                        Main.EntitySpriteDraw(tex, seg.position - Main.screenPosition, new Rectangle(0, 22, 26, 24), Projectile.GetAlpha(Lighting.GetColor(new Point((int)seg.position.X / 16, (int)seg.position.Y / 16))), rot, new Vector2(13, 12), new Vector2(1, dist / 24), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    else
                        Main.EntitySpriteDraw(tex, seg.position - Main.screenPosition, new Rectangle(0, 0, 26, 20), color, rot, new Vector2(13, 10), Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(Projectile.whoAmI);
        }
    }
}
