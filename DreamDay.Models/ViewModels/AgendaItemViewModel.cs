using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class AgendaItemViewModel
    {
        public string Time { get; set; }
        public string Event { get; set; }
        public string Location { get; set; }
        public string Responsible { get; set; }
        public string Notes { get; set; }
    }
}
