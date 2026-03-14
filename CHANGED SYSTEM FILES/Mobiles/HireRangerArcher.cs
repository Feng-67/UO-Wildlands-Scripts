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
    public class HireRangerArcher : BaseHire 
    {
        [Constructable] 
        public HireRangerArcher()
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

            Title = "the archer";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(600, 800); 
            SetDex(200, 200);
            SetInt(200, 300);

            SetDamage(18, 28);

            SetSkill(SkillName.Archery, 120, 120);
            SetSkill(SkillName.Tactics, 120, 120);
            SetSkill(SkillName.Anatomy, 120, 120);
            SetSkill(SkillName.Healing, 120, 120);
            SetSkill(SkillName.MagicResist, 120, 120);
            SetSkill(SkillName.Chivalry, 120, 120);

            Fame = 100;
            Karma = 100;
            this.ControlSlots = 4;
            this.SetHits(600);

            // Equipment
            AddItem(new Shirt());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Bow());
            AddItem(new BaseQuiver()); // Standard quiver

            // Random Armor (Leather or Studded)
            switch(Utility.Random(2))
            {
                case 0:
                    AddItem(new LeatherChest()); AddItem(new LeatherArms()); AddItem(new LeatherGloves()); AddItem(new LeatherLegs()); AddItem(new LeatherGorget()); break;
                case 1:
                    AddItem(new StuddedChest()); AddItem(new StuddedArms()); AddItem(new StuddedGloves()); AddItem(new StuddedLegs()); AddItem(new StuddedGorget()); break;
            }

            
            PackGold(0, 0);
        }

public override int PhysicalResistance { get { return 70; } }
public override int FireResistance     { get { return 70; } }
public override int ColdResistance     { get { return 70; } }
public override int PoisonResistance   { get { return 70; } }
public override int EnergyResistance   { get { return 70; } }

        public HireRangerArcher(Serial serial) : base(serial) { }

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

            list.Add("<BASEFONT COLOR=#C0C0C0>Legendary Archer</BASEFONT>");

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