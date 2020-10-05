using System;


using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace DesWeb
{
    public partial class Mail : System.Web.UI.Page
    {
        string email;
        protected void Page_Load(object sender, EventArgs e)
        {
            Uri uri = new Uri(Request.Url.ToString());
            email = HttpUtility.ParseQueryString(uri.Query).Get("email");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            MailMessage feedBack = new MailMessage();
            MailMessage companyFeedBack = new MailMessage();

            if (Session["reqinfocom"] != null)
            {
                feedBack.To.Add("world.mouse@outlook.com");
            }
            else
            {
                feedBack.To.Add(email);
            }

            feedBack.From = new MailAddress(txtEmail.Text);
            companyFeedBack.From = new MailAddress(txtEmail.Text);

            feedBack.Subject = txtSubject.Text;
            companyFeedBack.Subject = "No Reply "  + txtSubject.Text;

            feedBack.Body = "Sender Name: " + txtName.Text + "<br><br>Sender Email: " + txtEmail.Text + "<br><br>" + txtNote.Text + "<br><br>";
            feedBack.IsBodyHtml = true;
            companyFeedBack.Body = "Sender Name: " + txtName.Text + "<br><br>Sender Email: " + txtEmail.Text + "<br><br>" + txtNote.Text + "<br><br>";
            companyFeedBack.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address

            smtp.Port = 587;

            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
           
            smtp.Credentials = new System.Net.NetworkCredential("iwlamail@gmail.com", "IWLA1891a!");

            List<Company> reqInfoCompanies = new List<Company>();
            if (Session["reqinfocom"] != null)
            {
                reqInfoCompanies = (List<Company>)Session["reqinfocom"];
                foreach (var company in reqInfoCompanies)
                {
                    feedBack.Body = feedBack.Body + "<br>";
                    companyFeedBack.Body = feedBack.Body + "<br>";

                    if (company.Email != null)
                    {
                        companyFeedBack.To.Add(company.Email);
                    }
                    smtp.Send(companyFeedBack);
                    //feedBack.CC.Add(company.Email);
                }

            }
           
           // smtp.Credentials = new System.Net.NetworkCredential("ranasaleem621@gmail.com", "kameer113!A");

            //Or your Smtp Email ID and Password

            smtp.Send(feedBack);

            Label1.Text = "Thanks for contacting us";
        }
    }
}