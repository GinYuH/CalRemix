using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.DataStructures;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class GigaSilk : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public List<VerletSimulatedSegment>[] segments = [new(), new()];

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.velocity.Y < 14 && Projectile.ai[0] > 20)
            {
                Projectile.velocity.Y += 0.5f;
            }

            for (int i = 0; i < segments.Length; i++)
            {
                CalRemixHelper.CreateVerletChain(ref segments[i], Main.rand.Next(4, 16), Projectile.Center, new() { 0 });

                if (segments[i].Count > 0)
                {
                    List<VerletSimulatedSegment> segs = segments[i];
                    segs[0].oldPosition = segs[0].position;
                    segs[0].position = Projectile.Center;
                    VerletSimulatedSegment.SimpleSimulation(segs, 10 * i);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Webbed, 120);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                List<VerletSimulatedSegment> segs = segments[i];
                List<Vector2> vs = new();
                for (int j = 0; j < segs.Count; j++)
                {
                    vs.Add(segs[j].position);
                }
                PrimitiveRenderer.RenderTrail(vs, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => 2), new PrimitiveSettings.VertexColorFunction((float f, Vector2 v) => Lighting.GetColor((v + Main.screenPosition).ToTileCoordinates()).MultiplyRGB(Color.White))));
            }
            return false;
        }
    }
}