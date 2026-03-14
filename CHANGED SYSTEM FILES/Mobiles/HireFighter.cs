/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic; // Added this to fix the List error
using Server.Items;

namespace Server.Mobiles 
{
    public class HireFighter : BaseHire 
    {
        [Constructable] 
        public HireFighter()
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

            Title = "the fighter";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(600, 800); 
            SetDex(200, 200);
            SetInt(200, 300);

            SetDamage(18, 28);

            SetSkill(SkillName.Tactics, 120, 120);
            SetSkill(SkillName.Swords, 120, 120);
            SetSkill(SkillName.Parry, 120, 120);
            SetSkill(SkillName.Bushido, 120, 120);
            SetSkill(SkillName.Chivalry, 120, 120);
            SetSkill(SkillName.Anatomy, 120, 120);

            Fame = 100;
            Karma = 100;
            this.ControlSlots = 4;
            this.SetHits(600);

            switch (Utility.Random(2)) 
            {
                case 0: AddItem(new Shoes(Utility.RandomNeutralHue())); break;
                case 1: AddItem(new Boots(Utility.RandomNeutralHue())); break;
            }
			
            AddItem(new Shirt());

            switch (Utility.Random(5)) 
            {
                case 0: AddItem(new Longsword()); break;
                case 1: AddItem(new Broadsword()); break;
                case 2: AddItem(new VikingSword()); break;
                case 3: AddItem(new BattleAxe()); break;
                case 4: AddItem(new TwoHandedAxe()); break;
            }

            if (FindItemOnLayer(Layer.TwoHanded) == null)
            {
                switch (Utility.Random(8))
                {
                    case 0: AddItem(new BronzeShield()); break;
                    case 1: AddItem(new HeaterShield()); break;
                    case 2: AddItem(new MetalKiteShield()); break;
                    case 3: AddItem(new MetalShield()); break;
                    case 4: AddItem(new WoodenKiteShield()); break;
                    case 5: AddItem(new WoodenShield()); break;
                    case 6: AddItem(new OrderShield()); break;
                    case 7: AddItem(new ChaosShield()); break;
                }
            }
		  
            switch(Utility.Random(5))
            {
                case 1: AddItem(new Bascinet()); break;
                case 2: AddItem(new CloseHelm()); break;
                case 3: AddItem(new NorseHelm()); break;
                case 4: AddItem(new Helmet()); break;
            }

            switch(Utility.Random(4))
            {
                case 0: // Leather
                    AddItem(new LeatherChest()); AddItem(new LeatherArms()); AddItem(new LeatherGloves()); AddItem(new LeatherGorget()); AddItem(new LeatherLegs()); break;
                case 1: // Studded Leather
                    AddItem(new StuddedChest()); AddItem(new StuddedArms()); AddItem(new StuddedGloves()); AddItem(new StuddedGorget()); AddItem(new StuddedLegs()); break;
                case 2: // Ringmail
                    AddItem(new RingmailChest()); AddItem(new RingmailArms()); AddItem(new RingmailGloves()); AddItem(new RingmailLegs()); break;
                case 3: // Chain
                    AddItem(new ChainChest()); AddItem(new ChainLegs()); break;
            }

            PackGold(0, 0);
        }

public override int PhysicalResistance { get { return 70; } }
public override int FireResistance     { get { return 70; } }
public override int ColdResistance     { get { return 70; } }
public override int PoisonResistance   { get { return 70; } }
public override int EnergyResistance   { get { return 70; } }

        public HireFighter(Serial serial) : base(serial) { }

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