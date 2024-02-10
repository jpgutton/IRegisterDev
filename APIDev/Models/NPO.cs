using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDev.Models
{
    public class NPO
    {
        [Key]
        public string ClubID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Le nom de l'organisation est requis")]
        //[Required(ErrorMessageResourceName = "OrganizationNameRequired", ErrorMessageResourceType = typeof(NPO))]
        [StringLength(150, ErrorMessage = "Le champs doit faire moins de 150 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string OrganizationStreet1 { get; set; }

        //[Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string OrganizationStreet2 { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string OrganizationCity { get; set; }

        // [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(50, ErrorMessage = "Le champs doit faire moins de 50 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string OrganizationState { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(16, ErrorMessage = "Le champs doit faire moins de 16 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string OrganizationZipCode { get; set; }

        [Required(ErrorMessage = "L'adresse de l'organisation est requise")]
        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string OrganizationCountry { get; set; }

        [StringLength(60, ErrorMessage = "Le champs doit faire moins de 60 caracteres.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                    ErrorMessage = "Le courriel n'est pas valide")]
        public string OrganizationEmail { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Le champs doit faire moins de 60 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -]+)", ErrorMessage = "Le nom du site n'est pas valide")]
        public string OrganizationSiteName { get; set; }

        [Required]
        public string NPOSiteID { get; set; }


        /// <summary>
        /// Additionnal information & second contact
        /// </summary>

        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -:/?=éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes -:/?= sont autorises.")]
        public string OrganizationWebSite { get; set; }

        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -:/?=éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes -:/?= sont autorises.")]
        public string OrganizationFacebook { get; set; }

        [StringLength(128, ErrorMessage = "Le champs doit faire moins de 128 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 -:/?=éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes -:/?= sont autorises.")]
        public string OrganizationTwitter { get; set; }

        [StringLength(30, ErrorMessage = "Le champs doit faire moins de 30 caracteres.")]
        [RegularExpression("([a-zA-Z ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        public string SndFirstname { get; set; }

        [StringLength(30, ErrorMessage = "Le champs doit faire moins de 30 caracteres.")]
        [RegularExpression("([a-zA-Z ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        public string SndLastName { get; set; }

        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 250 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        public string SndAddress { get; set; }

        [StringLength(250, ErrorMessage = "Le champs doit faire moins de 250 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes ,_.'- sont autorises.")]
        public string SndAddress2 { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string SndCity { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string SndState { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string SndZipCode { get; set; }

        [StringLength(200, ErrorMessage = "Le champs doit faire moins de 200 caracteres.")]
        [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
        public string SndCountry { get; set; }



        [StringLength(20, ErrorMessage = "Le numero est limitee a 20 caracteres.")]
        [RegularExpression("([0-9 +()#-]+)", ErrorMessage = "Seul les chiffres et ces signes +()#- sont autorises.")]
        [Display(Name = "Telephone")]
        public string SndTelephone { get; set; }

        [StringLength(60, ErrorMessage = "Le champs doit faire moins de 60 caracteres.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Le courriel n'est pas valide")]
        public string SndEmail { get; set; }



        public string Processed { get; set; }
        public string GroupID { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
