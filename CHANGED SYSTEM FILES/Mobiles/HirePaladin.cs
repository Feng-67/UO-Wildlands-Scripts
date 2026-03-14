/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic; // Essential for the 'List' to work
using Server.Items;

namespace Server.Mobiles 
{
    public class HirePaladin : BaseHire 
    {
        [Constructable] 
        public HirePaladin()
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool()) 
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else 
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            Title = "the paladin";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            switch( Utility.Random(5) )
            {
                case 1: AddItem(new Bascinet()); break;
                case 2: AddItem(new CloseHelm()); break;
                case 3: AddItem(new NorseHelm()); break;
                case 4: AddItem(new Helmet()); break;
            }

            SetStr(600, 800); 
            SetDex(200, 200);
            SetInt(200, 300);

            SetDamage(18, 28);

            SetSkill(SkillName.Swords, 120, 120);
            SetSkill(SkillName.Anatomy, 120, 120);
            SetSkill(SkillName.Healing, 120, 120);
            SetSkill(SkillName.Tactics, 120, 120);
            SetSkill(SkillName.Parry, 120, 120);
            SetSkill(SkillName.Chivalry, 120, 120);

            Fame = 100;
            Karma = 250;
            this.ControlSlots = 4;
            this.SetHits(600);

            AddItem(new Shoes(Utility.RandomNeutralHue()));
            AddItem(new Shirt());
            AddItem(new VikingSword());
            AddItem(new MetalKiteShield());
 
            AddItem(new PlateChest());
            AddItem(new PlateLegs());
            AddItem(new PlateArms());
            AddItem(new LeatherGorget());
            
            PackItem(new Bandage(100));

            
            PackGold(0, 0);
        }


public override int PhysicalResistance { get { return 70; } }
public override int FireResistance     { get { return 70; } }
public override int ColdResistance     { get { return 70; } }
public override int PoisonResistance   { get { return 70; } }
public override int EnergyResistance   { get { return 70; } }

        public HirePaladin(Serial serial) : base(serial) { }

        public override bool ClickTitle { get { return false; } }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c); 

            if (this.IsBonded && c != null)
            {
                // Create a copy of the items in the corpse to avoid collection errors
                List<Item> items = new List<Item>(c.Items);
                foreach (Item item in items)
                {
                    this.EquipItem(item); 
                }
                // Delete the corpse after a tiny delay so the items are moved safely
                Timer.DelayCall(TimeSpan.FromSeconds(0.1), new TimerCallback(c.Delete));
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            // Subtitle matching the Fighter's style
            list.Add("<BASEFONT COLOR=#C0C0C0>Legendary Swordsman</BASEFONT>");

            if (this.IsBonded)
            {
                list.Add("<BASEFONT COLOR=#C0C0C0>[Bonded]</BASEFONT>");
            }
        }

        public override void Serialize(GenericWriter writer) 
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader) 
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}