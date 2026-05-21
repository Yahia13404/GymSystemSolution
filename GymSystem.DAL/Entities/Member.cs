namespace GymSystem.DAL.Entities
{
    public class Member : GymUser
    {
        public string Photo { get; set; } = null!;

        public HealthRecord HealthRecord { get; set; } =  null!;
        public ICollection<MemberShip> MemberShips { get; set; } = new HashSet<MemberShip>();

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}