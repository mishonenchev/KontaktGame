using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Models
{
    class UsedWord
    {
        [Key]
        public int Id { get; set; }

        public string Word { get; set; }
        public Player PlayerWhoUsed { get; set; }
    }
}
