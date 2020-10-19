using AM4.DATA;
using AM4.VIEW.CustomV;
using System;
using System.ComponentModel.DataAnnotations;

namespace AM4.VIEW.Partial
{
    public class FaleConoscoPartial : FaleConosco
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório", AllowEmptyStrings = false)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DtNascimento { get; set; }
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O campo CPF é obrigatório", AllowEmptyStrings = false)]
        [CustomValidator(ErrorMessage = "CPF Incorreto")]
        public string Cpf { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage ="Email inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Celular é obrigatório", AllowEmptyStrings = false)]
        public string Celular { get; set; }
        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O campo CEP é obrigatório", AllowEmptyStrings = false)]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O campo Rua é obrigatório", AllowEmptyStrings = false)]
        public string Rua { get; set; }
        [Required(ErrorMessage = "O campo Numero é obrigatório", AllowEmptyStrings = false)]
        public string Numero { get; set; }

        public string Complemento { get; set; }
        [Required(ErrorMessage = "O campo Bairro é obrigatório", AllowEmptyStrings = false)]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "O campo Cidade é obrigatório", AllowEmptyStrings = false)]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O campo Estado é obrigatório" )]
        [Display(Name = "Estado")]        
        public int Idestado { get; set; }

        [Required(ErrorMessage = "O campo Titulo/Motivo é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Titulo/Motivo")]
        public string TituloMensagem { get; set; }

        [Required(ErrorMessage = "O campo Mensagem é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Mensagem")]
        public string TextoMensagem { get; set; }

        public string DataEnvioStr { get; set; }
    }


}
