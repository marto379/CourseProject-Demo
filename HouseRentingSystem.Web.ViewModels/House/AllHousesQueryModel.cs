namespace HouseRentingSystem.Web.ViewModels.House
{
    using HouseRentingSystem.Web.ViewModels.House.Enums;
    using System.ComponentModel.DataAnnotations;

    public class AllHousesQueryModel
    {

        public AllHousesQueryModel()
        {
            this.CurrentPage = 1;
            this.HousesPerPage = 3;
        }
        public string? Category { get; set; }

        [Display(Name = "Search by word")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort Houses By")]
        public HouseSorting HouseSorting { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name="Houses On Page")]
        public int HousesPerPage { get; set; }

        public int TotalHouses { get; set; }

        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public IEnumerable<HouseAllViewModel> Houses { get; set; } = new List<HouseAllViewModel>();
    }
}
