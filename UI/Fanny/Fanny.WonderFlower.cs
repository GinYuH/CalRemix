using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.Social.Base;
using Terraria.ModLoader.IO;
using System.IO;
using CalRemix.Projectiles.Weapons;
using System;
using CalamityMod;
using static System.Net.Mime.MediaTypeNames;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReLogic.Graphics;
using ReLogic.Content;
using System.Linq;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Systems;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadWonderFlowerPortraits()
        {
            FannyPortrait.LoadPortrait("TalkingFlower", 11, 5);
        }

        public static readonly SoundStyle OnwardAndUpwardSound = new("CalRemix/Sounds/Fanny/OnwardAndUpward");
        public static readonly SoundStyle WonderFannyVoice = new("CalRemix/Sounds/Fanny/WonderFannyTalk");

        public static void LoadWonderFlowerMessages()
        {
            FannyMessage wonderWings = new FannyMessage("Wonder_Wings", "Onward and upward!",
                "TalkingFlower", (FannySceneMetrics m) => Main.LocalPlayer.wingTimeMax > 0, 15, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: true, maxWidth: 500)
                .SpokenByAnotherFanny(FannyUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
            fannyMessages.Add(wonderWings);
        }
    }
}