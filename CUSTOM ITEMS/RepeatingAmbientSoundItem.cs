/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;

namespace Server.Items
{
    public class RepeatingAmbientSoundItem : Item
    {
        private Timer m_Timer;

        private const double CycleDuration = 120.0;

        [Constructable]
        public RepeatingAmbientSoundItem() : base(0x1B71)
        {
            Movable = false;
            Visible = false;

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(CycleDuration), TimeSpan.FromSeconds(CycleDuration), OnSoundTick);
        }

        public RepeatingAmbientSoundItem(Serial serial) : base(serial)
        {
        }

        private void OnSoundTick()
        {
            if (Map == null || Map == Map.Internal)
                return;

            Effects.PlaySound(Location, Map, 0x682);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (m_Timer != null) m_Timer.Stop();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(CycleDuration), TimeSpan.FromSeconds(CycleDuration), OnSoundTick);
        }
    }
}
