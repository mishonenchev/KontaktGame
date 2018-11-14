using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsAsked { get; set; }
        public string CookieId { get; set; }
        public string ConID { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastActiveTime { get; set; }
    }
}
