using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Probea.ViewModels
{
    public class AnswerViewModel
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public double ValueProgress { get; set; }
        public bool IsChecked { get; set; }
    }
}
