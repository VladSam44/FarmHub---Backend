 namespace HDigital.Helper
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
    <head>
    </head>
   <body style=""margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif;"">
    <div style=""height: auto; background:blue linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width: 400px; padding: 30px;"">
           <div> 
            <div>
             <h1>Resetarea parolei</h1>
               <hr>
               <p>Ati primit acest e-mail pentru ca dumneavoastra ati solocitat resetarea parolei pentru contul dumneavoastra  de HubDigital!.</p>
               <p>Daca nu ati solicitat acest lucru, va rugam sa ignorati mail-ul si sa raportati problema la contul de gmail: adrianvladut2k2@gmail.com

               <p>Apasati butonul pentru a alege noua parola.</p>
                
               <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd; padding:10px;border:none;
               color:white;broder-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Resetare Parola</a><br>

             <p>Toate cele bune,<br><br>
             Echipa HubDigital! </p>
             </div>
            </div> 
          </div>
        </body>

            </html>";
        }
    }
}
