﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GarageVParrot.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}
