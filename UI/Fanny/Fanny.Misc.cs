﻿using CalamityMod;
using CalamityMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        //Loads fanny messages that aren't associated with anything else in particular
        public static void LoadGeneralFannyMessages()
        {
            fannyMessages.Add(new FannyMessage("LowHP", "It looks like you're low on health. If your health reaches 0, you'll die. To combat this, don't let your health reach 0!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 1200, onlyPlayOnce: false).SetHoverTextOverride("Thanks Fanny! I'll heal."));

            fannyMessages.Add(new FannyMessage("Invisible", "Where did you go?",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.invis || Main.LocalPlayer.shroomiteStealth || Main.LocalPlayer.vortexStealthActive || (Main.LocalPlayer.Calamity().rogueStealth >= Main.LocalPlayer.Calamity().rogueStealthMax && Main.LocalPlayer.Calamity().rogueStealthMax > 0), onlyPlayOnce: true).SetHoverTextOverride("I'm still here Fanny!"));
            
            fannyMessages.Add(new FannyMessage("DarkArea", "Fun fact. The human head can still be conscious after decapitation for the average of 20 seconds.",
                "Nuhuh", (FannySceneMetrics scene) => DarkArea()));
            
            fannyMessages.Add(new FannyMessage("ConstantDeath", "Is that someone behind you...?",
                "Sob", (FannySceneMetrics scene) => DontStarveDarknessDamageDealer.darknessTimer >= 300 && !Main.LocalPlayer.DeadOrGhost));
            
            fannyMessages.Add(new FannyMessage("Cursed", "Looks like you've been cursed! If you spam Left Click, you'll be able to use items again sooner!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.cursed));
            
            fannyMessages.Add(new FannyMessage("EvilMinions", "Oh, joy, another player reveling in their summoned minions like they've won the pixelated lottery. Just remember, those minions are as loyal as your Wi-Fi signal during a storm—here one minute, gone the next. Enjoy your fleeting companionship, I guess.",
                "EvilIdle", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.numMinions >= 10).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("EvilTerraBlade", "Oh, congratulations, you managed to get a Terra Blade. I'm sure you're feeling all proud and accomplished now. But hey, don't strain yourself patting your own back too hard. It's just a sword, after all. Now, go on, swing it around like the hero you think you are.",
                "EvilIdle", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.HasItem(ItemID.TerraBlade)).SpokenByEvilFanny());
        }
        private static bool DarkArea()
        {
            Vector3 vector = Lighting.GetColor((int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16).ToVector3();
            return vector.Length() >= 0.15f;
        }
    }
}
