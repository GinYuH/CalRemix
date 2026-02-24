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
    public class OcramDeathLaser : OldLaserAbstract
    {
        private const float LIGHT_LEVEL = 0.75f;
        public override Vector3 laserLight => new Vector3(LIGHT_LEVEL, LIGHT_LEVEL * 0.5f, 0f);

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            //Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 3;
            //Projectile.light = 0.75f; dont use this
            Projectile.alpha = 255;
            //Projectile.MaxUpdates = 2;
            Projectile.extraUpdates = 2; // do not be fooled! maxupdates in old terraria vers is directly extraupdates
            Projectile.scale = 1.8f;
            Projectile.timeLeft = 1200;
            Projectile.DamageType = DamageClass.Magic; // why?
        }
    }
}
