using System;

namespace ShtikLive.Models.Questions
{
    public class Answer
    {
        public string QuestionId { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}