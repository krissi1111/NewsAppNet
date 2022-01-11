﻿namespace NewsAppNet.Models.ViewModels
{
    public class NewsItemView
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}