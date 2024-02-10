using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.ViewModels
{

    public class vMNPO
    {
        [Required(ErrorMessage = "Le nom de l'organisation est requis")]
        //[Required(ErrorMessageResourceName = "OrganizationNameRequired", ErrorMessageResourceType = typeof(NPO))]
        [StringLength(150, ErrorMessage = "Le champs doit faire moins de 150 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Organisation")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Address")]
        public string OrganizationStreet1 { get; set; }

        //[Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Address 2")]
        public string OrganizationStreet2 { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("City")]
        public string OrganizationCity { get; set; }

        //[Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(50, ErrorMessage = "Le champs doit faire moins de 50 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("State")]
        public string OrganizationState { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(16, ErrorMessage = "Le champs doit faire moins de 16 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Zip")]
        public string OrganizationZipCode { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Country")]
        public string OrganizationCountry { get; set; }

        [DisplayName("Email")]
        [StringLength(60, ErrorMessage = "Le champs doit faire moins de 60 caracteres.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                    ErrorMessage = "Le courriel n'est pas valide")]
        public string OrganizationEmail { get; set; }

        [Required]
        [DisplayName("Nom du site web")]
        [StringLength(60, ErrorMessage = "Le champs doit faire moins de 60 caracteres.")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Le nom du site n'est pas valide")]
        [Remote("ValidateOrganizationSiteName", "NPO")]
        public string OrganizationSiteName { get; set; }


        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -:/?=éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes -:/?= sont autorises.")]
        [DisplayName("Web site")]
        public string OrganizationWebSite { get; set; }

        [DisplayName("Facebook page")]
        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -:/?=éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes -:/?= sont autorises.")]
        public string OrganizationFacebook { get; set; }

        [DisplayName("Twitter")]
        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -:/?=éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes -:/?= sont autorises.")]
        public string OrganizationTwitter { get; set; }

        [DisplayName("First name")]
        [StringLength(30, ErrorMessage = "Le champs doit faire moins de 30 caracteres.")]
        [RegularExpression("([a-zA-Z ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        public string SndFirstname { get; set; }

        [DisplayName("Last name")]
        [StringLength(30, ErrorMessage = "Le champs doit faire moins de 30 caracteres.")]
        [RegularExpression("([a-zA-Z ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        public string SndLastName { get; set; }

        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 250 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        [DisplayName("Address 1")]
        public string SndAddress { get; set; }

        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 250 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        [DisplayName("Address 2")]
        public string SndAddress2 { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("City")]
        public string SndCity { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("State")]
        public string SndState { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Zip")]
        public string SndZipCode { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        [DisplayName("Country")]
        public string SndCountry { get; set; }



        [StringLength(20, ErrorMessage = "Le numero est limitee a 20 caracteres.")]
        [RegularExpression("([0-9 +()#-]+)", ErrorMessage = "Seul les chiffres et ces signes +()#- sont autorises.")]
        [Display(Name = "Telephone")]
        public string SndTelephone { get; set; }

        [DisplayName("Email")]
        [StringLength(60, ErrorMessage = "Le champs doit faire moins de 60 caracteres.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Le courriel n'est pas valide")]
        public string SndEmail { get; set; }
    }
}
