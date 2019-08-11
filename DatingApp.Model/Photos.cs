﻿using System;

namespace DatingApp.Model
{
    public class Photos
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public int UserId { get; set; }
    }
}
