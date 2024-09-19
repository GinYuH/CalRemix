using CalRemix.Content.NPCs.Bosses.Hydrogen;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class Fridge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fridge");
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 106;
            Projectile.height = 138;
            Projectile.friendly = true;
            Projectile.timeLeft = 99999;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            int Hydrogen = NPC.FindFirstNPC(ModContent.NPCType<Hydrogen>());
            if (Projectile.velocity.Length() < 2)
            {
                Projectile.ai[0]++;
                if (Projectile.ai[0] > 60)
                {
                    if (Projectile.ai[1] == 0)
                    {
                        if (Projectile.ai[0] == 61)
                        {
                            SoundEngine.PlaySound(SoundID.DoorOpen);
                        }
                        if (Projectile.ai[0] == 90)
                        {
                            SoundEngine.PlaySound(SoundID.LucyTheAxeTalk);
                            CombatText.NewText(Projectile.getRect(), Color.White, "Quick! In if you want to live!", true);
                        }
                        if (Main.LocalPlayer.getRect().Intersects(Projectile.getRect()))
                        {
                            Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().fridge = true;
                            Projectile.ai[1] = 1;
                            SoundEngine.PlaySound(SoundID.DoorClosed);
                        }
                        Projectile.frame = 1;
                        if (Hydrogen <= -1 || Main.npc[Hydrogen].life == Main.npc[Hydrogen].lifeMax)
                        {
                            Projectile.ai[1] = 3;
                            Projectile.ai[2] = 2222;
                        }
                    }
                    else if (Projectile.ai[1] > 1)
                    {
                        Projectile.ai[2]++;
                        if (Projectile.ai[2] == 60)
                        {
                            SoundEngine.PlaySound(SoundID.LucyTheAxeTalk);
                            CombatText.NewText(Projectile.getRect(), Color.White, "You're welcome!", true);
                            Projectile.ai[1] = 3;
                            Projectile.timeLeft = 120;
                        }
                    }
                }
            }
            if (Projectile.ai[1] == 1)
            {
                Projectile.frame = 0;
                Main.LocalPlayer.noItems = true;
                Main.LocalPlayer.immune = true;
                //Main.LocalPlayer.invis = true;
                Main.LocalPlayer.position = Projectile.position + new Vector2(20, 20);
                Main.LocalPlayer.mount.Dismount(Main.LocalPlayer);
                if (Projectile.velocity.Length() < 2)
                {
                    if (Hydrogen <= -1 || Main.npc[Hydrogen].life == Main.npc[Hydrogen].lifeMax)
                    {
                        Projectile.ai[2]++;
                        if (Projectile.ai[2] > 90)
                        {
                            Projectile.frame = 1;
                            SoundEngine.PlaySound(SoundID.DoorOpen);
                            Projectile.ai[1] = 2;
                            Main.LocalPlayer.velocity = new Vector2(10, 0);
                            Projectile.ai[2] = 0;
                        }
                    }
                }
            }
            if (Projectile.ai[1] != 3)
            { 
                if (Projectile.velocity.Y < 12)
                    Projectile.velocity.Y += 1f;

                if (Projectile.position.Y > Main.LocalPlayer.position.Y - Projectile.height)
                {
                    Projectile.tileCollide = true;
                }
            }
            else if (Projectile.ai[2] > 120)
            {
                Projectile.velocity.Y -= 1f;
                Projectile.tileCollide = false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[1] != 3)
            overPlayers.Add(Projectile.whoAmI);
        }
    }
}