using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace T1.PR2._APIrestAPI.Models
{
    [Table("User")]
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IList<Game> VotedGames { get; set; }
    }
}
