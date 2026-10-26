using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HireFighter : BaseHire
    {
        [Constructable]
        public HireFighter()
        {
            ControlSlots = 4;
            SpeechHue = Utility.RandomDyedHue();
            Hue       = Utility.RandomSkinHue();

            Title        = "the fighter";
            HairItemID   = Race.RandomHair(Female);
            HairHue      = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(600, 700);
            SetDex(200, 250);
            SetInt(200, 300);

            SetHits(700);
            SetDamage(18, 28);

            SetSkill(SkillName.Tactics,  120, 120);
            SetSkill(SkillName.Swords,   120, 120);
            SetSkill(SkillName.Parry,    120, 120);
            SetSkill(SkillName.Bushido,  120, 120);
            SetSkill(SkillName.Chivalry, 120, 120);
            SetSkill(SkillName.Anatomy,  120, 120);

            SetResistance(ResistanceType.Physical, 70);
            SetResistance(ResistanceType.Fire,     70);
            SetResistance(ResistanceType.Cold,     70);
            SetResistance(ResistanceType.Poison,   70);
            SetResistance(ResistanceType.Energy,   70);

            Fame  = 10000;
            Karma = 10000;

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

            AddItem(new Shoes(Utility.RandomNeutralHue()));
            AddItem(new Shirt());

            BaseWeapon weapon;
            switch (Utility.Random(5))
            {
                case 0:  weapon = new Longsword();   break;
                case 1:  weapon = new Broadsword();  break;
                case 2:  weapon = new VikingSword(); break;
                case 3:  weapon = new BattleAxe();   break;
                default: weapon = new TwoHandedAxe(); break;
            }
            EquipItem(weapon);

            if (FindItemOnLayer(Layer.TwoHanded) == null)
            {
                BaseShield shield;
                switch (Utility.Random(8))
                {
                    case 0:  shield = new BronzeShield();    break;
                    case 1:  shield = new HeaterShield();    break;
                    case 2:  shield = new MetalKiteShield(); break;
                    case 3:  shield = new MetalShield();     break;
                    case 4:  shield = new WoodenKiteShield(); break;
                    case 5:  shield = new WoodenShield();    break;
                    case 6:  shield = new OrderShield();     break;
                    default: shield = new ChaosShield();     break;
                }
                EquipItem(shield);
            }

            BaseArmor helm;
            switch (Utility.Random(5))
            {
                case 1:  helm = new Bascinet();  break;
                case 2:  helm = new CloseHelm(); break;
                case 3:  helm = new NorseHelm(); break;
                case 4:  helm = new Helmet();    break;
                default: helm = null;            break;
            }
            if (helm != null) EquipItem(helm);

            switch (Utility.Random(4))
            {
                case 0:
                    EquipItem(new LeatherChest()); EquipItem(new LeatherArms());
                    EquipItem(new LeatherGloves()); EquipItem(new LeatherGorget());
                    EquipItem(new LeatherLegs());
                    break;
                case 1:
                    EquipItem(new StuddedChest()); EquipItem(new StuddedArms());
                    EquipItem(new StuddedGloves()); EquipItem(new StuddedGorget());
                    EquipItem(new StuddedLegs());
                    break;
                case 2:
                    EquipItem(new RingmailChest()); EquipItem(new RingmailArms());
                    EquipItem(new RingmailGloves()); EquipItem(new RingmailLegs());
                    break;
                case 3:
                    EquipItem(new ChainChest()); EquipItem(new ChainLegs());
                    break;
            }

            PackItem(new BookOfChivalry());

            TithingPoints = 10000;
            PackGold(0, 0);
        }

        public HireFighter(Serial serial) : base(serial) { }

        public override bool ClickTitle { get { return false; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#C0C0C0>Legendary Swordsman</BASEFONT>");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}