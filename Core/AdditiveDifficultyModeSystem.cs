using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CalamityMod.Items.Armor.Demonshade;
using CalamityMod.World;
using CalRemix.Content.DifficultyModes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Core
{
    public class AdditiveDifficultyModeSystem : ModSystem
    {
        internal static bool _hasCheckedItOutYet = false; //Simple variable to add a cool effect to the mode selector 

        public static List<AdditiveDifficultyMode> Difficulties = new List<AdditiveDifficultyMode>(); //Difficulty modes ordered by ascending difficulty
        public static List<AdditiveDifficultyMode[]> DifficultyTiers; //Difficulty modes grouped together by difficulty
        public static int MostAlternateDifficulties; //The most alternate difficulties at any tier that exists. Used to know the widest space to take in the ui

        public override void Load()
        {
            MostAlternateDifficulties = 1;
            //Initialize base mod difficulties
            Difficulties = new List<AdditiveDifficultyMode>() { new NoDifficulty(), new TitanicDifficulty(), new DeathDifficulty() };

            CalculateDifficultyData();
        }

        public override void Unload()
        {
            Difficulties = null;
        }

        public static AdditiveDifficultyMode GetCurrentDifficulty
        {
            get
            {
                AdditiveDifficultyMode mode = Difficulties[0];

                for (int i = 1; i < Difficulties.Count; i++)
                {
                    if (Difficulties[i].Enabled)
                        mode = Difficulties[i];
                }

                return mode;
            }
        }

        public static void CalculateDifficultyData()
        {
            MostAlternateDifficulties = 1;
            Difficulties = Difficulties.OrderBy(d => d.DifficultyScale).ToList();

            //Difficulties are arranged in "tiers". This is done so that multiple mods can add their own alternate difficulties sharing a tier with the base ones
            DifficultyTiers = new List<AdditiveDifficultyMode[]>();
            float currentTier = -1;
            int tierIndex = -1;

            for (int i = 0; i < Difficulties.Count; i++)
            {
                //if we are at a new tier, create a new list of difficulties at that tier.
                if (currentTier != Difficulties[i].DifficultyScale)
                {
                    DifficultyTiers.Add(new AdditiveDifficultyMode[] { Difficulties[i] });
                    currentTier = Difficulties[i].DifficultyScale;
                    tierIndex++;
                }

                //if the tier already exists, just add it to the list of other difficulties at that tier.
                else
                {
                    //ugly
                    DifficultyTiers[tierIndex] = DifficultyTiers[tierIndex].Append(Difficulties[i]).ToArray();
                    MostAlternateDifficulties = Math.Max(DifficultyTiers[tierIndex].Length, MostAlternateDifficulties);
                }

                Difficulties[i]._difficultyTier = tierIndex;
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            //Apparently this is ran after worldgen so it cant always be set to true
            tag["hasCheckedOutTheCoolDifficultyUI"] = _hasCheckedItOutYet;
        }

        public override void OnWorldLoad()
        {
            _hasCheckedItOutYet = false;
        }

        public override void OnWorldUnload()
        {
            _hasCheckedItOutYet = false;
        }

        public override void PostWorldGen()
        {
            _hasCheckedItOutYet = false;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            _hasCheckedItOutYet = tag.GetBool("hasCheckedOutTheCoolDifficultyUI");

            //No need to check it out if rev is already on (Such as in old worlds)
            if (CalamityWorld.revenge)
                _hasCheckedItOutYet = true;
        }
    }

    public abstract class AdditiveDifficultyMode
    {
        public abstract bool Enabled
        {
            get; set;
        }

        public abstract Asset<Texture2D> Texture { get; }
        public virtual LocalizedText ExpandedDescription => LocalizedText.Empty;

        public float DifficultyScale;
        public LocalizedText Name;
        public LocalizedText ShortDescription;

        public string ActivationTextKey;
        public string DeactivationTextKey;

        public SoundStyle ActivationSound;

        public Color ChatTextColor;

        internal int _difficultyTier;

        /// <summary>
        /// Used to know which difficulties to toggle on when selecting a particular difficulty.
        /// </summary>
        public virtual bool RequiresDifficulty(AdditiveDifficultyMode mode) => false;
    }

    public class NoDifficulty : AdditiveDifficultyMode
    {
        public override bool Enabled
        {
            get => true;
            set { }
        }

        private Asset<Texture2D> _texture;
        public override Asset<Texture2D> Texture
        {
            get
            {
                if (_texture == null)
                    _texture = ModContent.Request<Texture2D>("CalRemix/UI/ModeIndicatorAdditive/ModeIndicator_None");

                return _texture;
            }
        }

        public NoDifficulty()
        {
            DifficultyScale = 0;
            Name = CalRemixHelper.LocalText("UI.DifficultyModes.NoDifficulty.Name");
            ShortDescription = CalRemixHelper.LocalText("UI.DifficultyModes.NoDifficulty.Info");

            ActivationTextKey = string.Empty;
            DeactivationTextKey = string.Empty;

            ActivationSound = SoundID.MenuTick with { Volume = 1f };

            ChatTextColor = Color.White;
        }
    }

    public class TitanicDifficulty : AdditiveDifficultyMode
    {
        public override bool Enabled
        {
            get => DifficultyModeWorld.titanicMode;
            set => DifficultyModeWorld.titanicMode = value;
        }

        private Asset<Texture2D> _texture;
        public override Asset<Texture2D> Texture
        {
            get
            {
                if (_texture == null)
                    _texture = ModContent.Request<Texture2D>("CalRemix/UI/ModeIndicatorAdditive/ModeIndicator_Titanic");

                return _texture;
            }
        }

        public override LocalizedText ExpandedDescription
        {
            get
            {
                return CalRemixHelper.LocalText("UI.DifficultyModes.TitanicDifficulty.ExtraInfo");
            }
        }

        public TitanicDifficulty()
        {
            DifficultyScale = 0.25f;
            Name = CalRemixHelper.LocalText("UI.DifficultyModes.TitanicDifficulty.Name");
            ShortDescription = CalRemixHelper.LocalText("UI.DifficultyModes.TitanicDifficulty.Info");

            ActivationTextKey = "Mods.CalamityMod.UI.RevengeanceActivate";
            DeactivationTextKey = "Mods.CalamityMod.UI.RevengeanceDeactivate";

            ActivationSound = SoundID.Item119;

            ChatTextColor = Color.Crimson;
        }
    }

    public class DeathDifficulty : AdditiveDifficultyMode
    {
        public override bool Enabled
        {
            get => CalamityWorld.death;
            set => CalamityWorld.death = value;
        }

        private Asset<Texture2D> _texture;
        public override Asset<Texture2D> Texture
        {
            get
            {
                if (_texture == null)
                    _texture = ModContent.Request<Texture2D>("CalamityMod/UI/ModeIndicator/ModeIndicator_Death");

                return _texture;
            }
        }

        public override LocalizedText ExpandedDescription => CalamityUtils.GetText("UI.DeathExpandedInfo");

        public DeathDifficulty()
        {
            DifficultyScale = 0.5f;
            Name = CalamityUtils.GetText("UI.Death");
            ShortDescription = CalamityUtils.GetText("UI.DeathShortInfo");

            ActivationTextKey = "Mods.CalamityMod.UI.DeathActivate";
            DeactivationTextKey = "Mods.CalamityMod.UI.DeathDeactivate";

            ActivationSound = DemonshadeHelm.ActivationSound;

            ChatTextColor = Color.MediumOrchid;
        }
    }
}