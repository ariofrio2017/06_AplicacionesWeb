using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CalculadoraWeb.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public double Numero1 { get; set; }

        [BindProperty]
        public double Numero2 { get; set; }

        [BindProperty]
        public string Operacion { get; set; } = "sumar";

        public double? Resultado { get; set; }
        public string MensajeError { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            switch (Operacion)
            {
                case "sumar":
                    Resultado = Numero1 + Numero2;
                    break;
                case "restar":
                    Resultado = Numero1 - Numero2;
                    break;
                case "multiplicar":
                    Resultado = Numero1 * Numero2;
                    break;
                case "dividir":
                    if (Numero2 == 0)
                    {
                        MensajeError = "Error: No se puede dividir entre cero.";
                    }
                    else
                    {
                        Resultado = Numero1 / Numero2;
                    }
                    break;
                default:
                    MensajeError = "Operación no válida.";
                    break;
            }

            return Page();
        }
    }
}