using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.Particles;
using CalamityMod.World;
using CalRemix.NPCs.Bosses.Hypnos;
using CalRemix.NPCs.Bosses.Losbaf;
using CalRemix.Retheme;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Items
{
    public class Keraunos : ModItem
    {
        private static readonly SoundStyle Use = AresTeslaCannon.TeslaOrbShootSound with { Pitch = 0.1f, PitchVariance = 0.2f };
        public override bool CanUseItem(Player player) => !ExoMechWorld.AnyExosOrDraedonActive && CalamityWorld.DraedonSummonCountdown <= 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Keraunos");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = Use;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Particle pulse = new PulseRing(player.Center, new Vector2(0, 0), Color.Cyan, 1f, 3f, 10);
                GeneralParticleHandler.SpawnParticle(pulse);
                Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, Vector2.UnitY * -22, ProjectileID.CultistBossLightningOrbArc, 0, 0, ai0: 4.7f);
                for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, (Vector2.UnitY * -22).RotatedByRandom(MathHelper.ToRadians(45f)), ProjectileID.CultistBossLightningOrbArc, 0, 0, ai0: 4.7f);
                Vector2 randomOffset = new(Main.screenWidth / 2 + (Main.rand.NextBool() ? -220 : 220), Main.screenHeight / 2 + (Main.rand.NextBool() ? -220 : 220));
                Vector2 randomPosition = new(player.Center.X + randomOffset.X, player.Center.Y + randomOffset.Y);
                Vector2 oppositePosition = new(player.Center.X - randomOffset.X, player.Center.Y - randomOffset.Y);
                bool randSwitchX = Main.rand.NextBool();
                bool randSwitchY = Main.rand.NextBool();

                NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)(player.Center.X), (int)(player.Center.Y + Main.screenHeight + 220), NPCType<Hypnos>());
                NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)(player.Center.X + Main.screenWidth - 220), (int)(player.Center.Y), NPCType<Artemis>());
                NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)(player.Center.X + Main.screenWidth + 220), (int)(player.Center.Y), NPCType<Apollo>());
                NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)(randSwitchX ? oppositePosition.X : randomPosition.X), (int)(randSwitchY ? oppositePosition.Y : randomPosition.Y), NPCType<ThanatosHead>());
                NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)(player.Center.X), (int)(player.Center.Y + Main.screenHeight - 220), NPCType<AresBody>());
                NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)(randSwitchX ? randomPosition.X : oppositePosition.X), (int)(randSwitchY ? randomPosition.Y : oppositePosition.Y), NPCType<Losbaf>());
                ExoMechWorld.ExoMayhem = true;
                CalRemixWorld.UpdateWorldBool();
                return true;
            }
            return false;
        }
    }
}
