using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HireMage : BaseHire
    {
        [Constructable]
        public HireMage() : base(AIType.AI_Mage)
        {
            ControlSlots = 4;
            SpeechHue = Utility.RandomDyedHue();
            Hue       = Utility.RandomSkinHue();

            Title      = "the mage";
            HairItemID = Race.RandomHair(Female);
            HairHue    = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(200, 500);
            SetDex(200, 250);
            SetInt(600, 800);

            SetHits(600);
            SetDamage(14, 22);

            SetSkill(SkillName.Magery,       120, 120);
            SetSkill(SkillName.EvalInt,      120, 120);
            SetSkill(SkillName.Meditation,   120, 120);
            SetSkill(SkillName.MagicResist,  120, 120);
            SetSkill(SkillName.Wrestling,    120, 120);
            SetSkill(SkillName.Inscribe,     120, 120);
            SetSkill(SkillName.ItemID,       120, 120);
            SetSkill(SkillName.Tactics,      120, 120);

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

            AddItem(new Robe(Utility.RandomNondyedHue()));
            AddItem(new WizardsHat());
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            AddItem(new Spellbook { Content = ulong.MaxValue });

            PackGold(0, 0);
        }

        public HireMage(Serial serial) : base(serial) { }

        public override bool ClickTitle { get { return false; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#C0C0C0>Legendary Mage</BASEFONT>");
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
