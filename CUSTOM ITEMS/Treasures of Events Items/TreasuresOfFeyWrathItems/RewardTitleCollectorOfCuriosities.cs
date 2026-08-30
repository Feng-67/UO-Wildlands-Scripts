namespace Server.Items
{
    public class RewardTitleCollectorOfCuriositiesDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1159913; //Collector of Curiosities

        [Constructable]
        public RewardTitleCollectorOfCuriositiesDeed()
        {
        }

        public RewardTitleCollectorOfCuriositiesDeed(Serial serial)
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