using CalRemix.Content.Items.Lore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.TheInsacredTexts
{
    public class LoreUIState : UIState
    {
        private static readonly SoundStyle PageFlip = new("CalRemix/Assets/Sounds/PageFlip");
        public static int inputCooldown = 0;
        public static int page = 0;
        public override void Update(GameTime gameTime)
        {
            bool shouldShow = !Main.gameMenu;
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            if (Main.LocalPlayer is null)
                return;
            if (Main.LocalPlayer.dead || !Main.LocalPlayer.TryGetModPlayer(out TheInsacredTextsPlayer p) || !p.reading)
                return;
            if (Main.keyState.IsKeyDown(Keys.Escape))
            {
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<TheInsacredTextsPlayer>().reading = false;
                return;
            }
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (inputCooldown > 0)
                inputCooldown--;
            if (Main.keyState.IsKeyDown(Keys.Right) && page < LoreDeath.Page.Length - 1 & inputCooldown <= 0)
            {
                SoundEngine.PlaySound(PageFlip, Main.LocalPlayer.Center);
                page++;
                inputCooldown = 12;
            }
            else if (Main.keyState.IsKeyDown(Keys.Left) && page > 0 & inputCooldown <= 0)
            {
                SoundEngine.PlaySound(PageFlip, Main.LocalPlayer.Center);
                page--;
                inputCooldown = 12;
            }
            DrawPage(spriteBatch);
        }

        private static void DrawPage(SpriteBatch spriteBatch)
        {
            Texture2D texture = (page % 2 == 0) ? ModContent.Request<Texture2D>("CalRemix/UI/TheInsacredTexts/Page1").Value : ModContent.Request<Texture2D>("CalRemix/UI/TheInsacredTexts/Page2").Value;
            spriteBatch.Draw(texture, new Vector2((Main.screenWidth / 2) - texture.Width / 2, (Main.screenHeight / 2) - texture.Height / 2), Color.White);
            Vector2 topCenter = new(Main.screenWidth / 2, (Main.screenHeight / 2) - texture.Height / 2);
            Color color = new(197, 179, 174);
            string title = "Rubrum Beginning";
            string subtitle = "Story of Calamity";
            if (page == 0)
            {
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, title, (int)topCenter.X - (FontAssets.MouseText.Value.MeasureString(title).X / 2), (int)topCenter.Y + (texture.Height / 2) - 80, Color.DarkRed, color, Vector2.Zero, 1.2f);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, subtitle, (int)topCenter.X - (FontAssets.MouseText.Value.MeasureString(subtitle).X / 2), (int)topCenter.Y + (texture.Height / 2) - 40, Color.DarkRed, color, Vector2.Zero);
                Texture2D logo = ModContent.Request<Texture2D>("CalRemix/UI/TheInsacredTexts/Logo").Value;
                spriteBatch.Draw(logo, new Vector2((Main.screenWidth / 2) - logo.Width / 2, (Main.screenHeight / 2) - (logo.Height / 2) + 60), Color.White);
            }
            else
            {
                string chapter = string.Empty;
                switch (page)
                {
                    case 1:
                        chapter = "Chapter 1: Rebirth of a Multiverse";
                        break;
                    case 5:
                        chapter = "Chapter 2: Spreading Entropy";
                        break;
                    case 9:
                        chapter = "Chapter 3: Igniting the Jungle";
                        break;
                    case 15:
                        chapter = "Chapter 4: Blood within Metal";
                        break;
                    case 18:
                        chapter = "Chapter 5: The Rise of Four Heroes";
                        break;
                    case 24:
                        chapter = "Chapter 6: Fading Victory";
                        break;
                    case 28:
                        chapter = "Chapter 7: Brothers in Ice";
                        break;
                    case 31:
                        chapter = "Chapter 8: Darkness to Light";
                        break;
                    case 35:
                        chapter = "Chapter 9: The Birth of a Hero";
                        break;
                }
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, chapter, (int)topCenter.X - (FontAssets.MouseText.Value.MeasureString(chapter).X / 2), (int)topCenter.Y + (FontAssets.MouseText.Value.MeasureString(chapter).Y / 2) + 10, Color.DarkRed, color, Vector2.Zero);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, LoreUI.PageText[page], (int)topCenter.X - (texture.Width / 2) + 56, (int)(topCenter.Y + FontAssets.MouseText.Value.MeasureString(title).Y / 2) + 40, Color.DarkRed, color, Vector2.Zero, 0.85f);
                /*
                string[] text = LoreDeath.Chapter[page].Split(' ');
                string line = string.Empty;
                float height = (int)(topCenter.Y + FontAssets.MouseText.Value.MeasureString(title).Y / 2) + 20;
                foreach (string s in text)
                {
                    if (line.Length < 100)
                        line += s + " ";
                    else
                    {
                        Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, line, (int)topCenter.X - (texture.Width / 2) + 56, height, Color.DarkRed, Color.LightSalmon, Vector2.Zero, 0.8f);
                        height += (FontAssets.MouseText.Value.MeasureString(title).Y / 2) + 5;
                        line = string.Empty;
                    }
                }
                */

            }
        }
        private static Rectangle Mouse()
        {
            return new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class LoreUI : ModSystem
    {
        private UserInterface LoreUserInterface;
        internal LoreUIState LoreState;
        internal static string[] PageText;
        public override void Load()
        {
            PageText = new string[LoreDeath.Page.Length];
            LoreState = new();
            LoreUserInterface = new();
            LoreUserInterface.SetState(LoreState);
        }
        public override void PostSetupContent()
        {
            for (int i = 0; i < LoreDeath.Page.Length; i++)
            {
                string[] text = LoreDeath.Page[i].Split(' ');
                string line = string.Empty;
                string final = string.Empty;
                foreach (string s in text)
                {
                    if (line.Length < 50)
                        line += s + " ";
                    else
                    {
                        line += s + "\n";
                        final += line;
                        line = string.Empty;
                    }
                }
                final += line;
                PageText[i] = final;
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            LoreUserInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.LocalPlayer.GetModPlayer<TheInsacredTextsPlayer>().reading)
            {
                int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (layerIndex != -1)
                {
                    layers.Insert(layerIndex, new LegacyGameInterfaceLayer("CalRemix: InsacredTexts",
                        delegate
                        {
                            LoreUserInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI));
                }
            }
        }
    }
    public static class LoreDeath
    {
        public static readonly string[] Page =
        {
            // Chapter 1 1-4
            string.Empty,
            "Long after the destruction of the original Calamimultiverse the most powerful entity in all of the world, Goozma, The Immortal Sludge of Creation created the universe along with a race of Light Gods, Dark Gods, and Neutral Gods after finding the endless void of existence too empty for their own taste. The Light Gods were obsessed with order, hierarchy, creation, and balance. The Dark Gods, on the other hand, were obsessed with chaos, anarchy, destruction, and war. Despite their differences, they always did their own thing until the Neutral Gods started fawning over the Light Gods, making the Dark Gods jealous. ",
            "The Dark Gods finally got tired of the Light Gods’ beauty and attention, like how their Godly Creator got tired of its emptiness before as it reminded the Dark Gods of empty attention, and decided to declare war against them. The Light Gods, in their ever-present desire for order, smoothly declined the idea of conflict, for they required no such troubles in their perfect world forever preserved by their orderly light. The Dark Gods were furious, and so to become infinitely more powerful for the upcoming war, to be fought for centuries, perhaps eons, the Dark Gods fused into a greater being, with Noxus becoming the main consciousness by forcefully inserting his dark, entropic powers into the conscious hive mind of every god before the fusion, with them being both the first and most powerful. ",
            "Noxus, even more powerful than its creator, slew Goozma, releasing its all mighty Godly Essence to the entire spacetime, manifested in what would be eventually referred to as Auric Souls, higher Vessels of Goozma, The Immortal Sludge of Creation’s power, that relentlessly search for suitable hosts through space and time by creating rips in spacetime and traveling between the Auric Eras. After such success, Noxus, not knowing what consequences the death of Goozma, Higher Slime Deity's would have, waged war against all of the Light Gods to wipe out their kind from the universe and forever submerge this Calamimultiverse into complete and total entropic chaos. ",
            "It was a close battle between Noxus and the Light Gods, with Noxus killing each Light God one by one while also taking serious damage due to the Pre-Mordial Light Auric Soul residing in every Light God at the moment, in the very first Auric Era, while both Noxus and the Light Gods did not have knowledge of it. Amidst the chaos, a group of Light Gods fled to a planet called Ferraria, so two of the Dark Gods, Cthulhu and Steve, decided to split from Noxus and chase them while the rest of the remaining and weakened Light Gods and Noxus fought. Shortly after, the Light Gods one by one fled to Ferraria to form The Pre-Mordial Light, Xeroc, a being that would hopefully end the Dark Gods as a last resort. Xeroc slapped both Cthulhu and Steve in their rears, launching them into the moon.",
            // Chapter 2 5-8
            "Noxus, after eradicating the rest of the Light Gods, could not fight Xeroc, the Pre-Mordial Light, in its current state, due to Xeroc’s Auric Soul becoming exponentially stronger every second, so Noxus started its goal of corrupting and gaining power across the universe, which resulted in the twisted, wretched birth of the sickening Astral Infection in its corrupted wake. Unlike the Light Gods, who prefer not to act on much in the universe, for order comes to it naturally this way, the Dark Gods chose to stir chaos across the universe, squashing anything with the potential to become powerful enough to threaten them. ",
            "After gaining enough power due to unknowingly absorbing the Dark Nebula Auric Soul, which traveled from Goozma’s Death to this moment to make Noxus its vessel and turn him into The Dark Pre-Mordial, Noxus. Finally gathering enough raw strength for it to destroy Xeroc, The Pre-Mordial Light, Noxus was contacted by Cthulhu and Moon Lord for help, but they went to Ferraria to instead punish them for their failure by slaying their now frail and weak bodies after their separation with the Great Dark Noxus. Cthulhu was the first to fall, with Noxus firing a destructive laser that completely demolished it, with only his flesh bathed in ichor remaining. This eventually formed into the Crimson over time. ",
            "Moon Lord was defeated soon after, with his bottom half sliced off and rotted in sickeningly green cursed fire over time, forming the Corruption. What was left of him was weakened and sealed inside of Ferraria's moon, forever to be known as the Celestial Lunar Prison. Now setting its eyes toward Xeroc, Great Beacon of Light, Noxus completely obliterated Xeroc in a raging barrage of all powerful Godly Dark Nebula Energy. With nothing left but its Pre-Mordial Light essence left behind, Noxus infused it into the lands of Ferraria as a means to prevent The Light’s influence from spreading throughout the Calamimultiverse. ",
            "Noxus, believing Ferraria to no longer be of great significance, left to continue corrupting and devouring other planets with his Dark Nebula. Unbeknownst to Noxus, Xeroc’s will of creation and Pre-Mordial Light allowed sentient life to be born in the land. Xeroc’s lifeforce will periodically expand throughout the air in an attempt to form again, with the Blood Moon happening as a result of it. Some of Ferraria’s inhabitants found out about this, and getting discomfort from it, they made houses under the ground to avoid what is believed to be Xeroc's Bloodlight gaze. However, the moon never truly turned to blood, and it was the mild insanity that made it seem like it was.",
            // Chapter 3 9-14
            "22 years before the Great Calamity, there was a family that lived in the Jungles of Ferraria. In this family were Ignalius, his younger sister Talphia, his older sister Aedia, his younger brother Ryphos, his father who he was named after Ignaus, and his mother Astrael. Ignaus was the leader of the Light God’s cult and always managed to get into all and every sort of trouble with the Great Elder Jungle Leaders and his own family. Astrael, on the other hand, always taught Ignalius to follow the way of peace and only wielded the bow Astral Daefeat while hunting and for emergencies. Ignalius also had a girlfriend named Flores, the daughter of one of the Great Elder Jungle Leaders. Flores was a friend to Ignalius’ family and gave Ignalius a rare dragon egg as a gift to show her love for him. ",
            "One day, Ignaus asked Flores for a favor to just casually go and grab the Terminus so he could study its great concealed Godly Power, so Flores gave him the Terminus and left, not knowing his true motive. Ignaus set up a ritual to reform Xeroc using the Terminus so that he could show off to his followers. Ignaus ended up accomplishing this, but Xeroc, the Mad, Destroyed, Pre-Mordial, still unable to properly assemble itself into the Great Pre-Mordial Light, showed up in a hideous, gruesome, and incomprehensible eldritch form that drove Ignaus, along with anyone else that witnessed it, temporarily raving mad. After finding out, Flores pleaded with the Great Elder Jungle Leaders of the jungle to spare their lives, but they decided they had enough of Ignaus and the trouble he caused, along with the rest of his family. ",
            "They gathered the three, took them to the pits of the Vile Crags in the Underworld, and threw them into the scathing hot lava alongside some of their possessions including the dragon egg Flores gave Ignalius. As they burned in the lava, the heat of it caused the dragon egg to hatch— and thus, Jungle Dragon was born. The Jungle Dragon grabbed Ignalius and carried him out of the lava on his back. Ignalius’s family did not survive the incident and Flores’ own family was executed shortly after for helping and defending Ignalius’s family, causing her to flee away from the Jungle. Ignalius swore revenge on the leaders of the jungle for killing them.",
            "Soon after Ignalius had mostly recovered, he went to the Great Elder Jungle Leaders of the jungle. Ignalius confronted them inside the Great Elder Jungle town hall and, in pure rage, slammed his butt down with the force of a thousand Gods, smashing it so hard that it killed everyone in the room, destroying the whole Great Jungle City completely and pulverizing the Great Elder Jungle Leaders, breaking his butt and creating such a loud slapping noise that was heard for miles. The Great Elder Jungle Leaders were now dead, but for Ignalius, this wasn’t enough to satisfy him—he changed his name to Yharim, the Jungle Tyrant and decided to take his anger and hatred out on the rest of the world, as well. ",
            "Yharim began to rise to power. Yharon, his loyal pet and friend, has the ability to revive, being a Phoenix-dragon. However, he is soul-linked to Yharim, meaning that if Yharim dies, Yharon will not revive. He had Yharon burn down a large section of the jungle to invoke fear into the people, although he used more than just brute force. He was a charismatic person, capable of invigorating any soldier to fight for him. Eventually, Yharim’s Tyranny took over the jungle, claimed the Lihzahrds’ Jungle Temple as his own, made himself an army, and began his iron-fisted rule over the world.",
            "The beginning of Yharim’s rule was unimaginably harsh, far worse than any of the 76 leaders of the Great Theocratic Religious Jungle Republic Of The Jungle who came before him. His first action upon taking the throne was to enforce control upon much of the surrounding kingdoms, cities, and capitals. He succeeded in it all. Anyone who resisted further had their people massacred, or captured to serve under inhumane conditions, where they often collapsed, or ended their own lives to escape a worse fate.",
            // Chapter 4 15-17
            "On a planet destroyed by Noxus, now turned into The Dark Nebula, far away from Ferraria, there was a robot named Draedon. Draedon was once an organic, intelligent being named Samuel Graydron. All life on the planet ceased after Noxus attacked, except for Samuel who was still clinging onto dear life thanks to one of his many creations, the Metalgear. After a few days of agony, assisted only by his own creations, he had built a device capable of replicating the complexities of his mind. After uploading his consciousness, Draedon was born. Draedon constructed a spaceship and explored the vastness of space for a new home, and was eventually drawn to Ferraria thanks to its beautiful and enticing leader, Yharim. ",
            "Draedon would create anything Yharim required, with near-infinite funding and test subjects, as well as the added advantage of being able to work on some of his own projects. An invisible deal was also made that Draedon must meet Yharim 3 times a week to kiss. At some point, he also made a weapon of immense power: his personal sword, the Miraculous Miracle Blade. He also made the original Auric Tesla armor, infused with a dormant power yet to be explained by any, which was made for Yharim so he could use his full power potential without breaking his butt again if that ever did have to happen. For Draedon’s first experiments, he decided to make three mechanical beings to do some tasks for him. ",
            "After one of Yharim’s agents stole schematics from the Mechanic, Draedon began his work. First was The Twins, a bot made for security and in general keeping an eye on things, infusing the Hallowed Metal with Souls of Sight, removing the need for any cameras. The second was The Destroyer, who was made to mine out huge chunks of the world for iron, gold, and other useful minerals, infusing the Hallowed Metal with Souls of Might, making these plates absurdly more sturdy than they could ever normally be. The third was Skeletron Prime, a bot made for war and destruction, infusing the Hallowed Metal with Souls of Fright, generating a fear aura that would scare away anyone who dares fight this machine. Yharim, alongside Yharon and Draedon, continued his conquest.",
            // Chapter 5 18-23
            "After learning of Yharim, the Jungle Tyrant’s atrocities against the kingdoms surrounding the Jungle, the other leaders of the Titanic Lands of Ferraria formed a coalition against Yharim. Four individuals led the armies of the world to counter the Tyrant. Braelor was born in a distant village far on the outskirts of the Royal Family’s kingdom. As a child, it was clear that he was blessed with superhuman strength, for at a young age he was already completing tasks that many grown men would have found impressive. Statis was a dark shadow ninja raised in a troubled world similar to Braelor. Statis’ parents raised him to follow in their footsteps, to learn of the arts that his ancestors had passed down for so many generations, art born from void and sentinel. ",
            "Daedalus, the younger brother of Permafrost, was a genius in nearly every field of science and art. He constructed weapons for battle, designed towering marvels, and peered ever further into the miracle of life. He was also an excellent strategist in battle, as proven in his efforts to protect the kingdom which was his patron. Silva, the most powerful of the four, was one of immense wisdom and strength. Having been born from the conceptualization of flora, and becoming the first Dryad and the Elemental of Nature itself, she is connected to all the plant life in the land of Ferraria, and her health and vitality were linked to the well-being of its entire population, from the sprawling forests to each blade of grass, making her nigh-immortal. ",
            "The four were able to seamlessly work together to fight against Yharim’s army. Braelor and Statis have both fought together against the corrupt rulers before Yharim, so warding off Yharim’s auric minions was no challenge. Daedalus was able to hack Draedon’s trio of mechs to turn against him and cause havoc among Yharim’s ranks, forcing them to send the mechs far away from them, to slumber in their glitched madness. Seeing Daedalus’s skill, Silva combined their abilities and created Wulfrum, an alloy of both plant and metal, infused with the joy of natural life. Due to its abundance, they created an army of Wulfrum machines to use for recon and battle that while weak, managed to recon the entire lands of Ferraria, leading them to have a great strategic advantage.",
            "The four fought valiantly, and inevitably, the body count of Yharim’s army rose exponentially. At first, it was but a gradual flow of violently mutilated corpses, but this increased further until Yharim was given no other options to dispose of the bodies. While the allied coalition buried their people, Yharim carelessly tossed away his own, so Yharim turned to what is now the Acidic, Toxic and Radiated Sulphur Ocean. The once crystal clear waters turned dark and toxic with the blood and entrails of many, choking the life out of its waters forever. This process destroyed the delicate and precious ecosystem. Only the hardiest and most desperate of the creatures in its bay clung onto their last hopes, and soon had adapted to the toxic environment with sudden aggressive changes to their unique biology. ",
            "As time moved forward, the world coalition appeared to be winning. Unfortunately, Yharim gained new tools for battle that will turn the tide for the worst, the arrival of Great Witch Calitas Jane and the Gigantic Gluttonous Devourer of Gods. Deep within the land, in a volcano protected from everything else on the Titanic Lands of Ferraria, there lived a family of Underdark mages whose magical powers rivaled those of demigods, one of whom was Calitas Jane, with her real name being Calamitas. After her family was killed by a rival faction, she used her proficiency in the darkest of magics to trap her family’s killers in the underworld to suffer for the rest eternity. Not long after, Calitas Jane heard a voice in her head, telling her to seek the Jungle Temple. This was Yharim, the Jungle Tyrant, who had seen Calitas Jane’s great power. She eventually found the Temple and joined Yharim’s ranks as the Calamity Witch. ",
            "Unbeknownst to them, the reason for Calitas Jane’s proficiency at magic was the Third Auric Soul to infuse itself on a host. Traveling through time and space  all the way from Goozma’s death, the Brimdeath Auric Soul fused itself into Calitas Jane after she found her dead family, being the reason for her sudden burst in power. At some point, Yharim somehow found out about the Moon Lord, and him being sealed inside of the Lunar Prison. He knew he couldn’t unseal the Moon Lord alone, so he sought out Amidias, the 22nd King of the Sea Kingdom. Amidias, seeing through Yharim’s nefarious and vile intentions and his bad evil auric aura, refused to help the Tyrant, and Yharim grew increasingly furious at Amidias and his Kingdom. ",
            // Chapter 6 24-27
            "Over time, one would think Yharim’s anger at the underworld’s lava for burning him and his family would fade but it grew more intense until he could bear it no longer. He came up with a plan to bring both regions into a war so they can kill each other while he watches in sadistic pleasure. In order to achieve this, he sent Calitas Jane on her first mission to spread brimstone flames and death disguised as an envoy of the underworld’s capital, the Crags. Amidias declared war against the underworld, and the Brimstone Elemental retaliated. At the end, both kingdoms were ravaged. The sea kingdom was evaporated by the underworld army while the capital of hell was extinguished and ruined by powerful typhoons. Amidias and the Brimstone Elemental were both exhausted and siphoned of most of their powers, so they both retreated into shells to one day rise again.",
            "With this, Calitas Jane completed her first mission, but began to have doubts in Yharim and even tried to point out some flaws in his cruel and tyrannic auric rule. However, Yharim kicked her right out of the throne room, and she grew very frustrated. She was also assigned a 2nd task just before the creation of her clone so she could be distracted from it: dealing with Silva, the Nature Elemental Dryad. Silva, however, could not be killed, for as long as plant life exists, she cannot die. Calitas Jane instead burned her soul into a million pieces and threw her body into the sulfurous sea to burn and dissolve forever, and over time, she sank into the abyss. However, her body grew into new plant life in even the deepest parts of the abyss, hence the presence of Planty Mush and Tenebris. Her soul was instead transformed into a race of tiny, lesser Dryads that fled and hid from the Ferrarians. ",
            "While Calitas Jane was carrying out her second mission, a third party intervened. One day, Yharim got a report from one of his men that an enormous cosmic serpent had devoured his elite platoon of his finest soldiers. Naturally, this interested him greatly, as very few beings other than the 4 leaders of the rebellion against him, could manage anything similar. Yharim sought out this man-eating serpent, and in his searches, found it feasting on its latest prey. It introduced itself as The Devourer of Gods. The Devourer of Gods, who consumed Astrum Deus, the Star Spawned Wyrm. Yharim made an agreement with the Devourer that he would feed him as long as it worked under him. The Devourer, putting its ego, as massive as himself, if not more, aside, took the offer, and joined Yharim’s ranks. Shortly after, Yharim requested the Devourer to capture the leaders of the rebellion and “do as he pleases”. ",
            "During an unnaturally quiet night, Braelor, Statis, and Daedalus wondered where Silva went and discussed battle plans to take on Yharim, the Jungle Tyrant, himself. Behind Braelor and Statis, a portal opened up and the Devourer snatched them, causing Daedalus to panic and flee. Inside his own pocket dimension, The Distortion, given to him by The Dark Nebula, Noxus, who also created the Devourer, the Devourer consumed them and gained their strengths and powers. After hearing of this, Yharim commissioned Draedon an armor of Cosmilite, a material found inside the Devourer’s Dimension as a reward for killing them. Throughout the war, Draedon had been conducting his own experiments to replicate the Wulfrum alloy Daedalus created, and then one day Draedon perfected it.",
            // Chapter 7 28-30
            "Despite the capture of Silva and the deaths of Braelor and Statis, the war raged on. Daedalus withdrew quietly back to his home, a large city in the coldest region of the world. He collected all battle plans and creations and took them to the snow-covered mountains, where his brother. Permafrost protected the city with an artificial blizzard and resided in a frozen castle. Arriving back home caused rumors of the Archmage and his vast Ancient Archive of Ancient Magic of The Ancients full forbidden knowledge in the castle of ice spread, drawing earnest scholars and wicked men alike to him. Permafrost’s perception of others sharpened until he could read their intentions with the slightest glance; in his decades of seclusion and the thousands that approached him, only a select few possessed an integrity that assured the Archmage his teachings would not be misused. ",
            "Draedon soon learned that Daedalus escaped to his home, so he himself led the Tyrant’s forces against the city, forces that far outnumbered the armies the kingdom had withstood before. Alongside the Tyrant’s forces are creations of his own, the dreaded Exo-Mechanical Exo-Beasts, built only for the sole purpose of Exo-Destruction and Exo-Mayhem. Leading the attack was XP-00 Hypnos, the strongest and most intelligent Exo-Mechanical Exo-Beast. Daedalus and Permafrost were not able to halt the attack. In the city which surrounded the castle, carnage raged on and many lives were lost. It would be one of the worst atrocities in recorded history of the Titanic Lands of Ferraria. They knew it had been a hopeless battle from the beginning, but such a slaughter… even they could not ignore such an atrocity. ",
            "Daedalus urged Permafrost to preserve the castle’s library containing knowledge while he fended off against the invaders. Permafrost sealed himself inside the ice castle and transformed it into Cryogen with old norse runic magic only he was even aware existed, which then sank deep underground into the snow. As the walls of the city finally fell, and its people were massacred, Daedalus looked death in the eye, and triggered the explosives he set within most of his creations. The blast was of such power that it incinerated Daedalus, embedding Cryogenic shrapnel far into the earth. Most of Yharim’s army was destroyed, along with the city itself. The Exo-Mechanical Exo-Beasts were severely damaged while XP-00 Hypnos remained intact and retreated into the Exosphere, a verboten, thunderous paradise devoid of organic life. Permafrost remains trapped inside the ice, waiting to be freed by a pure auric soul with no wretched intentions.",
            // Chapter 8 31-34
            "During this time, the war began to fade as the strength and numbers of both the rebellion’s and the Jungle Tyrant’s of the jungle forces faltered. At this point, Yharim had decided to try and make his own creations, and he found that he can use his abilities to clone living beings. The first thing he tried to do was to clone Yharon. This, however, failed miserably, creating the Electric Bumblefolly. Yharim also asked Draedon to make a clone of Calitas Jane in secret; even though this was weaker than her due to the lack of the Brimdeath Auric Soul, it was instead made to contain her power if needed. However, through looking into the mind of one of Yharim’s soldiers thanks to her auric powers, Calitas Jane found out about the clone of her and its purpose. ",
            "She, at this point, had enough of the Jungle Tyrant Yharim and attempted to rebel. She summoned The Lihzahrd Golem inside the temple and constructed Ravager, a beast of undead flesh and stone animated by her brimdeath fury. While Yharim’s soldiers were distracted, she stole important documents from the throne room, and escaped. After the Golem had been killed and the Ravager subdued, Yharim cursed Calitas Jane— to be compelled to return to him, with this curse intensifying over time. Calitas Jane met with a mysterious hooded figure and gave them the important documents she stole. In return, the figure recognized the familiar energy of the curse and removed it, allowing Calitas Jane to be free and escape the Tyrant’s grasp.",
            "As a result of the war, Xeroc, now reborn, has had enough and decided to intervene. Xeroc created two deities to carry out their will. Providence, who Xeroc infused with the Fourth Auric Soul: the Pure Flame Auric Soul, and Zeratros, who Xeroc infused with the Fifth Auric Soul: the Pure Light Auric Soul desired to turn the world into ash. One day, Yharim noticed this mix of flame and light. For one of the first times, he truly panicked as they went through the world, for if they were left for too long to gather enough power, they would create a flame powerful enough to evaporate the largest bodies of water, char the plants, and make the entire world devoid of life. Yharim gathered his army and went to war with Providence and Zeratros in an attempt to defeat them. However, Providence and Zeratros had an army as well: thousands of Guardians made from the souls of Providence’s most devout worshipers and thousands of light dragons and monks willing to serve Zeratros until their demise. ",
            "In the end, the battle ended with the deities winning, though barely. Zeratros had to take the form of a harmless creature and flee. The Profaned Goddess lost most of her power and her thousands of guardians became merely handfuls of them, and a great number of Yharim’s forces were killed in the conflict. That week a googolplexian souls were lost to both sides. With Calitas Jane gone and Draedon back on his own planet, Yharim sent the Devourer of Gods and his 3 sentinels after Providence so that she could be stopped for good. However, Providence was able to hide herself away in the sun, though if her holy artifact is drawn out, she will come and kill the one who dares lay their hands upon it. Devourer of Gods and his 3 sentinels continue their search for her, leaving the Tyrant by himself.",
            // Chapter 9 35-37
            "After ruling over Ferraria for some time, Yharim had become bored. So in his boredom, he renamed Ferraria to Terraria using the power of the Terminus. As a consequence, Xeroc once again turned to Yharim, but this time Xeroc was able to attain a form capable of combat. Yharim challenged them to a duel. Xeroc acknowledged the Tyrant's power, but showed Yharim how weak he was compared to them by uttering the simple words: “It’s Xerocin’ Time”. The fight was over before it even began, and Yharim faced defeat before he, or his opponent, even lifted a finger. He returned home, depressed at how absurdly outmatched he was to a weakened Light God. The land grew quiet, for the war finally ended and the Tyrant’s fury was satisfied enough. However, Yharim, in his quest for power to bring down Xeroc, learnt about the auric souls that Calitas Jane and Providence held, and set out to get one of his own. ",
            "Shortly before the Great Calamity, his wishes would be met, the Sixth Auric Soul, the Royal Dragon Rebirth Auric Soul made a host for itself on Yharon, who, thanks to his soul-link with Yharim, gave him half of the auric soul, both becoming infinitely more powerful, surpassing Calitas Jane’s and Draedon’s Exo-Creations power. One day, the hooded figure that Calitas Jane met contacted both Xeroc and Noxus on two separate occasions in secret. The figure is then revealed to be Flores, Yharim’s old girlfriend. After summoning them, sapping a bit of their energy, and combining the energies of both gods, she created a being with the ability to live forever and carve its own path in the universe. ",
            "Shortly after, Flores fell victim of Draedon’s virulent nanomachine plague he had left in the planet, as she was a species of anthropomorphic bee, turning into Virilli and becoming the holder of the Seventh Auric Soul, the Green Plague Auric Soul, maintaining part of her sanity thanks to the power of the soul, and hiding herself in the jungle to await until her creation awoke could finally take Yharim down. Unfortunately, her creation initially formed  as a statue, so she left thinking that it may never awaken merely holding a small hope by the time she was infected. In the middle of an open field years later, the statue came to life and the Terrarian spawned into existence, hosting the very last Auric Soul: the Godly Essence Auric Soul, beginning thus the Great Calamity Auric Era.",
        };
    }
}
