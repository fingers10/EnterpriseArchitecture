using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class BookForUpdateDto : BookManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description.")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
