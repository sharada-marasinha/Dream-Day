using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DreamDay.Models.ViewModels
{
    public class WeddingViewModel
    {
        public WeddingViewModel()
        {
            GuestCategories = new List<GuestCategoryViewModel>();
            Services = new List<string>();
            Agenda = new List<AgendaItemViewModel>();
        }

        public string WeddingName { get; set; }
        public DateTime WeddingDate { get; set; } = DateTime.Today.AddMonths(6);
        public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(14);
        public string WeddingLocation { get; set; }
        public string WeddingTheme { get; set; }
        public string WeddingDescription { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile CoverImage { get; set; } // For file upload
        public int EstimatedGuests { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile GuestList { get; set; } // For CSV upload
        public bool EnableRSVP { get; set; }
        public List<GuestCategoryViewModel> GuestCategories { get; set; } = new List<GuestCategoryViewModel>();
        public decimal TotalBudget { get; set; }
        public List<string> Services { get; set; } = new List<string>();
        public List<AgendaItemViewModel> Agenda { get; set; } = new List<AgendaItemViewModel>();
    }
}
