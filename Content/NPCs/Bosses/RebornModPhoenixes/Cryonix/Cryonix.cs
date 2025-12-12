using CalamityMod;
using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Cryonix
{
    public class Cryonix : PhoenixAbstract
    {
        public override int damage => 30;
        public override int defense => 10;
        public override int health => 9000;
        public override int projType => ModContent.ProjectileType<CryoIcicle>();
        public override int dustType => DustID.Frost;
        public override int invulThreshold => 600;

        public override void ShootProjectiles()
        {
            Vector2 vector8 = targetPos;
            float num48 = 50f;
            int damage = 14;
            int type = projType;
            SoundEngine.PlaySound(SoundID.Item17, vector8);
            int total = 10;
            float randomRot = Main.rand.NextFloat(0, MathHelper.TwoPi);

            for (int i = 0; i < total; i++)
            {
                int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -num48), type, damage, 0f, Main.myPlayer);
                Main.projectile[proj].velocity = Main.projectile[proj].velocity.RotatedBy(((MathHelper.TwoPi / total) * i) + randomRot);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); crest
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); acc
            npcLoot.Add(ModContent.ItemType<FlashFreeze>(), 2 / 3);
        }
    }
}
