using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace T1.PR2._APIrestAPI.Models
{
    [Table("Game")]
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeveloperTeam { get; set; }
        public string ImgUrl { get; set; }
        public IList<User> VotingUsers { get; set; }
    }
}
