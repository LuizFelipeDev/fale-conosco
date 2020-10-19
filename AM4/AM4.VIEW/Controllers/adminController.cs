using AM4.DATA;
using AM4.VIEW.Partial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AM4.VIEW.Views
{
    public class adminController : Controller
    {
        private readonly AM4_BDContext _context;

        public adminController(AM4_BDContext context)
        {
            _context = context;
        }

        [Route("admin/fale-conosco")]
        [Authorize]
        public async Task<IActionResult> faleconosco()
        {
            List<FaleConoscoPartial> listFCPartial = new List<FaleConoscoPartial>();
            var listFC = _context.FaleConosco;

            foreach (var i in listFC)
            {
                Pessoa pe = _context.Pessoa.Find(i.Idpessoa);
                Mensagem msg = _context.Mensagem.Find(i.Idmensagem);

                FaleConoscoPartial fc = new FaleConoscoPartial();
                fc.FaleCId = i.FaleCId;
                fc.Nome = pe.Nome;
                string celMask = "{0:(00)0 0000-0000}";
                fc.Celular = string.Format(celMask, pe.Celular);
                fc.Email = pe.Email;
                DateTime _data = i.DataEnvio;
                fc.DataEnvioStr = _data.ToString("dd/MM/yyyy");
                fc.TituloMensagem = msg.TituloMensagem;

                listFCPartial.Add(fc);
            }

            ViewData["ListaSubmissoes"] = listFCPartial;

            return View();
        }

        public async Task<IActionResult> VerExcluir()
        {
            string btn = Request.Form["VerSubmissao"].ToString();

            if (btn != "") // Ver
            {
                int idFC = Convert.ToInt32(btn);
                return RedirectToAction("ver", new { id = idFC });
            }
            else //Excluir
            {
                btn = Request.Form["ExcluirSubmissao"].ToString();

                FaleConosco fc = new FaleConosco();
                Pessoa pe = new Pessoa();
                Mensagem msg = new Mensagem();

                fc = _context.FaleConosco.Find(Convert.ToInt32(btn));
                pe = _context.Pessoa.Find(fc.Idpessoa);
                msg = _context.Mensagem.Find(fc.Idmensagem);

                _context.FaleConosco.Remove(fc);
                _context.Pessoa.Remove(pe);
                _context.Mensagem.Remove(msg);

                await _context.SaveChangesAsync();

                TempData["$AlertMessageExclusão$"] = "Exclusãook";

            }

            return RedirectToAction("faleconosco");
        }

        public async Task<IActionResult> RespostaEmail()
        {
            string id = Request.Form["EnviarResposta"].ToString();
            string mensagem = Request.Form["txtResposta"].ToString();

            FaleConosco fc = new FaleConosco();
            fc = _context.FaleConosco.Find(Convert.ToInt32(id));

            Pessoa pe = new Pessoa();
            pe = _context.Pessoa.Find(fc.Idpessoa);          

            EnviarEmail(fc, pe, mensagem);

            TempData["$AlertMessageEmailResposta$"] = "EmailRespostaok";

            return RedirectToAction("faleconosco");
        }

        public async Task<IActionResult> Ver(int id)
        {

            ViewData["Idestado"] = new SelectList(_context.Estado, "EstadoId", "Nome");

            FaleConoscoPartial fcP = new FaleConoscoPartial();
            FaleConosco fc = new FaleConosco();
            Pessoa pe = new Pessoa();
            Mensagem msg = new Mensagem();

            fc = _context.FaleConosco.Find(id);
            pe = _context.Pessoa.Find(fc.Idpessoa);
            msg = _context.Mensagem.Find(fc.Idmensagem);

            fcP.FaleCId = fc.FaleCId;
            fcP.Nome = pe.Nome;
            fcP.DtNascimento = pe.DtNascimento;
            fcP.Cpf = Convert.ToString(pe.Cpf);
            fcP.Email = Convert.ToString(pe.Email);
            fcP.Celular = Convert.ToString(pe.Celular);
            fcP.Cep = pe.Cep.ToString();
            fcP.Rua = pe.Rua;
            fcP.Numero = pe.Numero;
            fcP.Complemento = pe.Complemento;
            fcP.Bairro = pe.Bairro;
            fcP.Cidade = pe.Cidade;
            fcP.Idestado = pe.Idestado;
            fcP.TituloMensagem = msg.TituloMensagem;
            fcP.TextoMensagem = msg.TextoMensagem;

            return View(fcP);
        }

        public void EnviarEmail(FaleConosco fc, Pessoa pe, string msg)
        {
            MailMessage objEmail = new MailMessage();
            objEmail.From = new MailAddress("am4faleconosco@gmail.com");

            MailAddress oEmail = new MailAddress(pe.Email);

            objEmail.To.Add(oEmail);
            objEmail.Priority = MailPriority.Normal;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = "Resposta Feedback";
            objEmail.Body = "<p> Olá "+ pe.Nome +", </p>";
            objEmail.Body +=  msg;
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
