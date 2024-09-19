using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Placeables.MusicBoxes
{
    public abstract class RemixMusicBox : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => (this != null);
    }
    public class AcidsighterMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Acidsighter, Type, TileType<Tiles.MusicBoxes.AcidsighterMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.AcidsighterMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ARMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.AcidRainTier2, Type, TileType<Tiles.MusicBoxes.ARMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ARMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class CryoSlimeMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.CryoSlime, Type, TileType<Tiles.MusicBoxes.CryoSlimeMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.CryoSlimeMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class DerellectMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Derellect, Type, TileType<Tiles.MusicBoxes.DerellectMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.DerellectMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class EolMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.EmpressofLightDay, Type, TileType<Tiles.MusicBoxes.EolMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.EolMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ExcavatorMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.WulfrumExcavator, Type, TileType<Tiles.MusicBoxes.ExcavatorMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExcavatorMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class HypnosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Hypnos, Type, TileType<Tiles.MusicBoxes.HypnosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.HypnosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class LaRugaMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.LaRuga, Type, TileType<Tiles.MusicBoxes.LaRugaMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.LaRugaMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PlaguedJungleMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.PlaguedJungle, Type, TileType<Tiles.MusicBoxes.PlaguedJungleMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PlaguedJungleMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PolyphemalusMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Polyphemalus, Type, TileType<Tiles.MusicBoxes.PolyphemalusMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PolyphemalusMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PolyphemalusGFBMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.PolyphemalusAlt, Type, TileType<Tiles.MusicBoxes.PolyphemalusGFBMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PolyphemalusGFBMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class TitleMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Menu, Type, TileType<Tiles.MusicBoxes.TitleMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.TitleMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
}
