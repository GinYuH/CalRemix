using CalRemix.UI.ElementalSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class OcramEyeLaser : OldLaserAbstract
    {
        private const float LIGHT_LEVEL = 0.75f;
        public override Vector3 laserLight => new Vector3(LIGHT_LEVEL * 0.7f, 0, LIGHT_LEVEL * 1f); // multiplying it by 1 is what the og does.

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            //Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 3;
            //Projectile.light = 0.75f;
            Projectile.alpha = 255;
            //Projectile.MaxUpdates = 2;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1.7f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
        }
    }
}
