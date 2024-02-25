using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System.IO;
using CalamityMod.World;
using System;
using CalRemix.Tiles;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Tiles.FurnitureMonolith;
using CalRemix.Items.Placeables;
using CalamityMod.Walls;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.Cryogen;
using CalRemix.UI;
using CalamityMod.Tiles.Ores;
using CalRemix.Backgrounds.Plague;
using CalRemix.Tiles.PlaguedJungle;
using CalRemix.Projectiles.TileTypeless;
using CalamityMod.Tiles.Plates;
using CalamityMod.NPCs;
using CalRemix.Projectiles.Weapons;
using CalRemix.Items.Weapons;
using CalRemix.NPCs;
using CalamityMod.Tiles;
using CalamityMod.Tiles.SunkenSea;
using System.Threading;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.NPCs.Abyss;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Fishing.SulphurCatches;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Tiles.DraedonStructures;
using Terraria.WorldBuilding;
using log4net.Repository.Hierarchy;
using log4net.Core;
using CalamityMod.Tiles.FurnitureVoid;
using CalamityMod.Items.SummonItems;
using CalRemix.Items;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.Items.TreasureBags;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.GreatSandShark;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.PrimordialWyrm;
using Terraria.UI;
using CalRemix.Retheme;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.DataStructures;
using CalamityMod.NPCs.AquaticScourge;
using Terraria.GameContent.Bestiary;
using CalamityMod.NPCs.HiveMind;
using System.Linq;
using System.Reflection;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader.Core;

namespace CalRemix
{
    public class CalRemixBuff : GlobalBuff
    {
        public override bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
        {
            return base.PreDraw(spriteBatch, type, buffIndex, ref drawParams);
        }
    }
}