
namespace T1.PR2._APIrestAPI.DTO
{
    public class GetGameDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeveloperTeam { get; set; }
        public string ImgUrl { get; set; }
        public IList<string> VotingUsers { get; set; } = new List<string>();
    }
}
