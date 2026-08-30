using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
    [Flipable(0xA797, 0xA79F)]
    public class PentagramOfDemonicForces : Item
    {
        public static Dictionary<Mobile, Timer> _Table = new Dictionary<Mobile, Timer>();
        public override int LabelNumber { get { return 1159796; } } // Pentagram of Demonic Forces

        [Constructable]
        public PentagramOfDemonicForces()
            : base(0xA797)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (!InRange(m, 2))
            {
                m.SendLocalizedMessage(500295);
                return;
            }

            if (m.BodyMod != 0 && m.HueMod != -1)
            {
                m.SendMessage("You may not use this while in a form or disguise.");
                return;
            }

            m.HueMod = 2128;
            _Table[m] = Timer.DelayCall<Mobile>(TimeSpan.FromSeconds(60), RemoveEffects, m);
            m.SendLocalizedMessage(1159792);
        }

        public static void RemoveEffects(Mobile m)
        {
            if (IsUnderEffects(m))
            {
                m.HueMod = -1;
                _Table.Remove(m);
                m.SendLocalizedMessage(1159793);
            }
        }

        public static bool IsUnderEffects(Mobile m)
        {
            if (m == null)
                return false;
            return _Table.ContainsKey(m);
        }

        public static void ForceRemoveEffects(Mobile m)
        {
            if (IsUnderEffects(m))
            {
                m.HueMod = -1;
                _Table.Remove(m);
                m.SendLocalizedMessage(1159791);
            }
        }

        public PentagramOfDemonicForces(Serial serial)
            : base(serial)
        {
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
