using System.ComponentModel.DataAnnotations;

namespace IdentityDev.Models.ManageViewModels
{
    public class ChangeInfoViewModel
    {
        // Additional fields
        [Required(ErrorMessage = "Field required")]
        [StringLength(30, ErrorMessage = "30 max")]
        [RegularExpression("([a-zA-Z ,'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Field required")]
        [StringLength(40, ErrorMessage = "40 max")]
        [RegularExpression("([a-zA-Z ,'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Field required")]
        [StringLength(60, ErrorMessage = "60 max")]
        [RegularExpression("([a-zA-Z0-9 ,'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [StringLength(60, ErrorMessage = "60 max")]
        [RegularExpression("([a-zA-Z0-9 ,'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Field required")]
        [StringLength(40, ErrorMessage = "40 max")]
        [RegularExpression("([a-zA-Z ,'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "City")]
        public string City { get; set; }

        // [Required(ErrorMessage = "Field required")]
        [StringLength(30, ErrorMessage = "30 max")]
        [RegularExpression("([a-zA-Z ,'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Field required")]
        [StringLength(10, ErrorMessage = "10 max")]
        [RegularExpression("([a-zA-Z0-9 -]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Field required")]
        [StringLength(30, ErrorMessage = "30 max")]
        [RegularExpression("([a-zA-Z '-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        // [Required(ErrorMessage = "Field required")]
        [StringLength(30, ErrorMessage = "30 max")]
        [RegularExpression("([0-9 -+()]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        // [Required(ErrorMessage = "Field required")]
        [StringLength(30, ErrorMessage = "30 max")]
        [RegularExpression("([0-9 -+()]+)", ErrorMessage = "Seul les lettres et certains signes de ponctuation sont autorises.")]
        [Display(Name = "Cellphone")]
        public string Cellphone { get; set; }
    }
}
