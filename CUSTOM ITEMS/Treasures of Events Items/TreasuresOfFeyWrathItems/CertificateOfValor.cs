using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	public class CertificateOfValor : Item
	{
        private Mobile Owner;

        [Constructable]
		public CertificateOfValor() : base(0x2258)
		{
            Name = "Certificate of Valor";
            Weight = 1.0;
            Hue = 2755;
        }
       
        public CertificateOfValor(Serial serial) : base(serial)
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
            if (Owner == null)
                list.Add("Double Click To Record Your Name");
        } 

        public override void OnDoubleClick(Mobile m)
        {
            if (Owner != null)
                return;

            if (m is PlayerMobile pm)
            {
                Owner = m;
                Name = $"Awarded To {Owner.Name} For Outstanding Valor During The Incursion Of Fey Wrath";
                InvalidateProperties();
            }
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
	}
}
