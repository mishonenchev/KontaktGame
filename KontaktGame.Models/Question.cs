using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Models
{
    class Question
    {
        [Key]
        public int Id { get; set; }

        public string question { get; set; }
        public Player PlayerWhoAsked { get; set; }
    }
}
