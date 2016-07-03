using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Certified_Label.Models;
using System.Net.Http.Formatting;

namespace Certified_Label.Controllers
{
    public class CertifiedLabelController : ApiController
    {
        string url = @"C:\Users\hlg87\Downloads\CertifiedLabe_pdf\";
        private Certified_LabelEntities db = new Certified_LabelEntities();

        private string GetRequest(HttpRequest request)
        {
            string urlAddress = "http://www.printcertifiedmail.com/generateCertified.aspx?";

            urlAddress += "UserID=" + request.UserID;

            urlAddress += "&UserEmail=" + request.UserEmail;

            urlAddress += "&AddresseeContact=" + request.Contact;

            urlAddress += "&AddresseeCompany=" + request.Company;

            urlAddress += "&AddresseeAddress=" + request.Address1;

            urlAddress += "&AddresseeAddress2=" + request.Address2;

            urlAddress += "&AddresseeCity=" + request.City;

            urlAddress += "&AddresseeState=" + request.State;

            urlAddress += "&AddresseeZip=" + request.Zip;

            urlAddress += "&InternalCode=x_internalcode";

            urlAddress += "&FileNumber=" + request.CaseNumber;

            urlAddress += "&InternalFileNumber=x_internalfilenumber";

            urlAddress += "&SenderContact=" + request.DepartmentName;

            urlAddress += "&SenderCompany=" + request.Sector;

            urlAddress += "&SenderAddress=" + request.SenderAddress;

            urlAddress += "&SenderAddress2=" + request.SenderAddress2;

            urlAddress += "&SenderCity=" + request.SenderCity;

            urlAddress += "&SenderState=" + request.SenderState;

            urlAddress += "&SenderZip=" + request.SenderZip;

            urlAddress += "&Weight=1";

            urlAddress += "&ReturnReceipt=";

            urlAddress += "&ERR=checkbox";

            urlAddress += "&RestrictedDelivery=";

            urlAddress += "&Optlbl=1";

            urlAddress += "&SenderID=" + request.SenderID;

            urlAddress += "&CustomerNumber=";

            urlAddress += "&DUNSNumber=900176001";

            urlAddress += "&ERRDeliveryMethod=FTP";

            urlAddress += "&ERRPaymentMethod=Meter/PC%20Postage";

            urlAddress += "&PackageType=Letters";

            urlAddress += "&FormType=15";
            return urlAddress;
        }

        private string GetName(string filepath)
        {
            char[] temp = new char[26];
            filepath.CopyTo(9, temp, 0, 26);
            return new string(temp);
        }
        // GET: 
        /* api/CertifiedLabel?UserID=1689&SenderID=1865&UserEmail=csalazar@miamigov.com&DepartmentName=Test2&AplicationName=Test2&Sector=Test&SenderAddress=Test&SenderAddress2=Test&SenderCity=Miami&SenderState=FL&SenderZip=33141&SenderPhone=12345678&CaseNumber=Test123456&SupervisorName=JuanCarlos&Description=Test&Subject=Test&Date=06/26/2016&Contact=Test&Company=Test&Address=Test&Address2=Test&City=Miami&State=FL&Zip=33181
         
         */
        public HttpResponseMessage Get(
         int UserID,
         int SenderID,
         string UserEmail,
         string DepartmentName,
         string AplicationName,
         string Sector,
         string SenderAddress,
         string SenderAddress2,
         string SenderCity,
         string SenderState,
         string SenderZip,
         string SenderPhone,
         string CaseNumber,
         string SupervisorName,
         string Description,
         string Subject,
         string Date,
         string Contact,
         string Company,
         string Address,
         string Address2,
         string City,
         string State,
         string Zip)
        {


            HttpRequest request = new HttpRequest()
            {
                UserID = UserID,
                SenderID = SenderID,
                UserEmail = UserEmail,
                DepartmentName = DepartmentName,
                AplicationName = AplicationName,
                Sector = Sector,
                SenderAddress = SenderAddress,
                SenderAddress2 = SenderAddress2,
                SenderCity = SenderCity,
                SenderState = SenderState,
                SenderZip = SenderZip,
                SenderPhone = SenderPhone,
                CaseNumber = CaseNumber,
                SupervisorName = SupervisorName,
                Description = Description,
                Subject = Subject,
                Date = Date,
                Contact = Contact,
                Company = Company,
                Address1 = Address,
                Address2 = Address2,
                City = City,
                State = State,
                Zip = Zip
            };

            Department department = new Department()
            {
                DepartmentName = DepartmentName,
                UserID = UserID,
                UserEmail = UserEmail,
                Sector = Sector,
                Address = Address,
                Address2 = Address2,
                City = City,
                State = State,
                Zip = Zip,
                Phone = SenderPhone
            };

            Case _case = new Case()
            {
                CaseNumber = CaseNumber,
                SupervisorName = SupervisorName,
                Description = Description,
                DepartmentName = DepartmentName
            };

            Letter letter = new Letter()
            {
                DepartmentName = DepartmentName,
                CaseNumber = CaseNumber,
                Subject = Subject,
                Date = Date,
                Contact = Contact,
                Company = Company,
                Address1 = Address,
                Address2 = Address2,
                City = City,
                State = State,
                Zip = Zip
            };
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            response = client.GetAsync(GetRequest(request)).Result;

            string filename = GetName(response.RequestMessage.RequestUri.LocalPath);
            request.Name = filename;
            letter.Name = filename;
            request.Url = url + filename + ".pdf";
            letter.Url = url + filename + ".pdf";
            department.Cases.Add(_case);
            department.Letters.Add(letter);
            _case.Department = department;
            _case.Letters.Add(letter);
            letter.Department = department;
            letter.Case = _case;

            db.Cases.Add(_case);
            db.Departments.Add(department);
            db.Letters.Add(letter);
            db.HttpRequests.Add(request);

            db.SaveChanges();
            return response;
        }
    }
}
