using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class OcramDustShadowflame : ModDust
    {
        public override bool Update(Dust dust)
        {
            /*
            ActiveDust->Velocity.X *= 0.94f;
            ActiveDust->Velocity.Y *= 0.94f;
            ActiveDust->Scale += 0.002f;
            if (ActiveDust->NoLight)
            {
                DustScale *= 0.1f;
                ActiveDust->Scale -= 0.06f;
                if (ActiveDust->Scale < 1f)
                {
                    ActiveDust->Scale -= 0.06f;
                }
                if (View != null)
                {
                    if (View.Player.IsWet)
                    {
                        ActiveDust->Position.X += View.Player.velocity.X * 0.5f;
                        ActiveDust->Position.Y += View.Player.velocity.Y * 0.5f;
                    }
                    else
                    {
                        ActiveDust->Position.X += View.Player.velocity.X;
                        ActiveDust->Position.Y += View.Player.velocity.Y;
                    }
                }
            }
            if (DustScale > 1f)
            {
                DustScale = 1f;
            }
            Lighting.AddLight((int)ActiveDust->Position.X >> 4, (int)ActiveDust->Position.Y >> 4, new Vector3(DustScale * 0.6f, DustScale * 0.2f, DustScale));
            */
            return true;
        }
    }
}
