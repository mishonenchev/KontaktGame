using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Models
{
    public class WordToGuess
    {
        [Key]
        public int Id { get; set; }

        public string Word { get; set; }
        public Player PlayerWhoIsAsked { get; set; }
    }
}
