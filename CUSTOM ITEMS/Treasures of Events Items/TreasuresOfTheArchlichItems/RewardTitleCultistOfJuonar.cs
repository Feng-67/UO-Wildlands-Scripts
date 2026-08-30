namespace Server.Items
{
    public class RewardTitleCultistOfJuonarDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1160485; //Cultist Of Juo'nar

        [Constructable]
        public RewardTitleCultistOfJuonarDeed()
        {
        }

        public RewardTitleCultistOfJuonarDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();
        }
    }
}