using Newtonsoft.Json;
using Probea.Helpers;
using Probea.Models;
using Probea.ViewModels;
using Sonda.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Probea.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        #region MVC
        public ActionResult MVC()
        {
            return View(GetProbeFormDay());
        }


        [HttpPost]
        [AnyIsChecked]
        [SetCookieProbe]
        public ActionResult MVC(ProbeViewModel model)
        {
            UpdateProbe(model);
            return RedirectToAction("MVC");
        }
        public ActionResult AddProbe()
        {
            ProbeViewModel model = new ProbeViewModel { Answers = new List<AnswerViewModel>() };
            for (int i = 0; i < 8; i++)
            {
                model.Answers.Add(new AnswerViewModel());
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddProbe(ProbeViewModel model)
        {
            Probe probe = new Probe
            {
                PublicationDate = model.PublicationDate,
                Question = model.Question,
                Answers = model.Answers.Where(x => x.Text != null).Select(x => new Answer { Text = x.Text, Value = "0" }).ToList()
            };
            XMLSerializerHelper.AddOrUpdate(probe);
            return RedirectToAction("Index");
        } 
        #endregion

        #region Ajax
        public ActionResult AjaxIndex()
        {
            return View();
        }
        public ActionResult AjaxGet()
        {
            return PartialView(GetProbeFormDay());
        }
        [HttpPost]
        [AnyIsChecked]
        [SetCookieProbe]
        public void AjaxPost([System.Web.Http.FromBody]ProbeViewModel model)
        {
            UpdateProbe(model);
        } 
        #endregion

        #region WCFAjax
        public ActionResult AjaxWcfIndex()
        {
            return View();
        }
        public ActionResult AjaxWcfGet()
        {
            Sonda.SondaWcfServ.Service1Client service = new Sonda.SondaWcfServ.Service1Client();
            var probe = service.GetProbeFromDay();
            var probeIsChecked = this.ControllerContext.HttpContext.Request.Cookies["ProbeChecked"];
            ProbeViewModel model = new ProbeViewModel
            {
                Answers = probe.Answers.Select(x => new AnswerViewModel
                {
                    Text = x.Text,
                    Value = int.Parse(x.Value),
                    ValueProgress = int.Parse(x.Value) != 0 ? (int)(int.Parse(x.Value) / (double)probe.Answers.Sum(y => int.Parse(y.Value)) * 100): 0
                }).ToList(),
                PublicationDate = probe.PublicationDate,
                Question = probe.Question,
                IsChecked = probeIsChecked != null && probe.PublicationDate == probeIsChecked.Value ? true : false
            };
            return PartialView(model);
        }
        [HttpPost]
        [AnyIsChecked]
        [SetCookieProbe]
        public void AjaxWcfPost([System.Web.Http.FromBody]ProbeViewModel model)
        {
            model.Answers.Single(m => m.IsChecked).Value += 1;
            Probe probe = new Probe
            {
                Question = model.Question,
                PublicationDate = model.PublicationDate,
                Answers = model.Answers.Select(x => new Answer { Text = x.Text, Value = x.Value.ToString() }).ToList()
            };
            Sonda.SondaWcfServ.Service1Client service = new Sonda.SondaWcfServ.Service1Client();
            service.PostProbe(probe);
        } 
        #endregion

        #region Helpers
        private ProbeViewModel GetProbeFormDay()
        {
            var probeIsChecked = this.ControllerContext.HttpContext.Request.Cookies["ProbeChecked"];
            ProbeViewModel Probe = XMLSerializerHelper.DeserializeProbeFromXML()
                .Select(x => new ProbeViewModel
                {
                    Question = x.Question,
                    PublicationDate = x.PublicationDate,
                    IsChecked = probeIsChecked != null && x.PublicationDate == probeIsChecked.Value ? true : false,
                    Answers = x.Answers.Select(z => new AnswerViewModel
                    {
                        Text = z.Text,
                        Value = int.Parse(z.Value),
                        ValueProgress = int.Parse(z.Value) != 0 ? (int)(int.Parse(z.Value) / (double)x.Answers.Sum(y => int.Parse(y.Value)) * 100): 0
                    }).ToList(),
                }).First(x => x.PublicationDate == DateTime.Now.ToShortDateString());
            return Probe;
        }

        private void UpdateProbe(ProbeViewModel model)
        {
            model.Answers.Single(m => m.IsChecked).Value += 1;
            XMLSerializerHelper.AddOrUpdate(new Probe
            {
                Question = model.Question,
                PublicationDate = model.PublicationDate,
                Answers = model.Answers.Select(x => new Answer { Text = x.Text, Value = x.Value.ToString() }).ToList()
            });
        } 
        #endregion

    }
}
