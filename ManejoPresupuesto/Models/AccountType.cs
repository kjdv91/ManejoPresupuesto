using ManejoPresupuesto.Validations;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class AccountType: IValidatableObject
    {
        public int AccountTypeId { get; set; }
        
        [Required(ErrorMessage = "El campo nombre es requirido")]
        [StringLength(maximumLength:50, MinimumLength =3, ErrorMessage ="La longuitud del campo debe estar entre 3 y 50 caracteres")]
        [Display(Name="Nombre del tipo de Cuenta")]
        //[FirtsLetterCapital]
        
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Order { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Name !=null && Name.Length > 0)
            {
                var firstLetter = Name[0].ToString();
                if(firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula", 
                        new[] {nameof(Name)});
                }
            }
        }
    }
}
