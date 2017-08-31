using System.Collections.Generic;
using ShtikLive.Models.Live;
using ShtikLive.Models.Questions;

namespace ShtikLive.Models.Present
{
    public class PresenterViewModel
    {
        public Show Show { get; set; }
        public List<Question> Questions { get; set; }
    }
}