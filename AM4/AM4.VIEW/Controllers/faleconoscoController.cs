using AM4.DATA;
using AM4.VIEW.Partial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AM4.VIEW.Views
{
    public class faleconoscoController : Controller
    {
        private readonly AM4_BDContext _context;
        

        public faleconoscoController(AM4_BDContext context)
        {
            _context = context;
        }      

        [Route("/fale-conosco")]
        // GET: faleconosco
        public IActionResult Index()
        {         
            ViewData["Idestado"] = new SelectList(_context.Estado, "EstadoId", "Nome");
            return View("Create");
        }
      
        public IActionResult Create()
        {
            return View();
        }

        // POST: faleconosco/Create      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FaleConoscoPartial faleConoscoPartial)
        {
            FaleConosco fc = new FaleConosco();
            Pessoa pe = new Pessoa();
            Mensagem msg = new Mensagem();

            if (ModelState.IsValid)
            {
                pe.Nome = faleConoscoPartial.Nome;
                pe.DtNascimento = faleConoscoPartial.DtNascimento;

                string CpfSemMascara = faleConoscoPartial.Cpf.Replace(".", "").Replace("-", "");
                //CpfSemMascara = faleConoscoPartial.Cpf.Replace("-", "");
                pe.Cpf = Convert.ToInt64(CpfSemMascara);
                pe.Email = faleConoscoPartial.Email;

                string CelularSemMascara = faleConoscoPartial.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                pe.Celular = Convert.ToInt64(CelularSemMascara);
                pe.Cep = Convert.ToInt32(faleConoscoPartial.Cep);
                pe.Rua = faleConoscoPartial.Rua;
                pe.Numero = faleConoscoPartial.Numero;
                pe.Complemento = faleConoscoPartial.Complemento;
                pe.Bairro = faleConoscoPartial.Bairro;
                pe.Cidade = faleConoscoPartial.Cidade;
                pe.Idestado = faleConoscoPartial.Idestado;

                _context.Pessoa.Add(pe);
                await _context.SaveChangesAsync();

                msg.TextoMensagem = faleConoscoPartial.TextoMensagem;
                msg.TituloMensagem = faleConoscoPartial.TituloMensagem;

                _context.Mensagem.Add(msg);
                await _context.SaveChangesAsync();

                fc.DataEnvio = DateTime.Today;
                fc.Idmensagem = msg.MensagemId;
                fc.Idpessoa = pe.PessoaId;

                _context.FaleConosco.Add(fc);

                await _context.SaveChangesAsync();

                EnviarEmail(fc, pe, msg);

                TempData["$AlertMessageFeedback$"] = "Feedbackok";

                //return RedirectToAction("Index");
                return RedirectToAction("Index");
            }
            return View(faleConoscoPartial);
        }


        public void EnviarEmail(FaleConosco fc, Pessoa pe, Mensagem msg)
        {

            Estado estado = _context.Estado.Find(pe.Idestado);
            MailMessage objEmail = new MailMessage();
            objEmail.From = new MailAddress(pe.Email);

            MailAddress oEmail = new MailAddress("am4faleconosco@gmail.com");

            objEmail.To.Add(oEmail);
            objEmail.Priority = MailPriority.Normal;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = msg.TituloMensagem;
            objEmail.Body = "<p>Nome: " + pe.Nome + "</p>";
            objEmail.Body += "<p>Estado: " + estado.Nome + "</p>";
            objEmail.Body += "<p>Cidade: " + pe.Cidade + "</p>";
            objEmail.Body += "<p>Mensagem: " + msg.TextoMensagem + "</p><br>";
            objEmail.Body += "<h4>Email: " + pe.Email + "</h4>";
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
            SmtpClient objSmtp = new SmtpClient();
            objSmtp.Host = "smtp.gmail.com";
            objSmtp.EnableSsl = true;
            objSmtp.Port = 587;
            objSmtp.Credentials = new NetworkCredential("am4faleconosco@gmail.com", "am4faleconosco1710");
            objSmtp.Send(objEmail);
        }
    }
}
