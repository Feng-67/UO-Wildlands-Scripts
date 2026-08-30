using System;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class TearOfTheIceDragon : Item
    {
		public override bool IsArtifact => true;

        [Constructable]
        public TearOfTheIceDragon()
            : base(0x9D7F)
        {
            Name = "Tear Of The Ice Dragon";
            Weight = 1.0;
            Hue = 1153;
        }

        public TearOfTheIceDragon(Serial serial)
            : base(serial)
        {
        }

         public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile && m.Alive && m.IsPlayer())
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        this.RunSequence(m, 1095160, false);
                        break; //You steadily walk over the slippery surface.
                    case 1:
                        this.RunSequence(m, 1095161, true);
                        break; //You skillfully manage to maintain your balance.
                    default:
                        this.RunSequence(m, 1095162, true);
                        break; //You lose your footing and ungracefully splatter on the ground.
                }
            }
            return base.OnMoveOver(m);
        }

        public virtual void RunSequence(Mobile m, int message, bool freeze)
        {
            object[] arg = null;

            if (freeze)
            {
                m.Frozen = freeze;
                Timer.DelayCall(TimeSpan.FromSeconds((message == 1095162) ? 2.0 : 1.25), new TimerStateCallback(EndFall_Callback), m);
            }

            m.SendLocalizedMessage(message);

            if (message == 1095162)
            {
                if (m.Mounted)
                {
                    m.Mount.Rider = null;
                }

                Point3D p = new Point3D(this.Location);

                if (SpellHelper.FindValidSpawnLocation(this.Map, ref p, true))
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(0), new TimerStateCallback(Relocate_Callback), new object[] { m, p });
                }

                arg = new object[] { m, (21 + Utility.Random(2)), !m.Female ? 0x426 : 0x317 };
            }
            else if (message == 1095161)
            {
                arg = new object[] { m, 17, !m.Female ? 0x429 : 0x319 };
            }
            if (arg != null)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(.4), new TimerStateCallback(BeginFall_Callback), arg);
            }
        }

        private static void Relocate_Callback(object state)
        {
            object[] states = (object[])state;
            Mobile m = (Mobile)states[0];
            Point3D to = (Point3D)states[1];

            m.MoveToWorld(to, m.Map);
        }

        private static void BeginFall_Callback(object state)
        {
            object[] states = (object[])state;

            Mobile m = (Mobile)states[0];
            int action = (int)states[1];
            int sound = (int)states[2];
            if (!m.Mounted)
            {
                m.Animate(action, 1, 1, false, true, 0);
            }
            m.PlaySound(sound);
        }

        private static void EndFall_Callback(object state)
        {
            ((Mobile)state).Frozen = false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}