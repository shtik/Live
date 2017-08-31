using System;
using System.Collections.Generic;

namespace ShtikLive.Models.Questions
{
    public class Question
    {
        public string Id { get; set; }
        public string Show { get; set; }
        public int Slide { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Time { get; set; }
        public List<Answer> Answers { get; set; }
    }
}