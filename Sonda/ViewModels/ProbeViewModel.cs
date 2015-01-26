using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Probea.ViewModels
{
    public class ProbeViewModel
    {
        public bool IsChecked { get; set; }
        public string PublicationDate { get; set; }
        public string Question { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
    }
}