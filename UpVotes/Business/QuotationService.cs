using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using UpVotes.Models;
using System;

namespace UpVotes.Business
{
    public class QuotationService
    {
        HttpClient _httpClient = null;
        public QuotationResponse GetQuoteForMobileApp(QuotationRequest quotationreq)
        {
            string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
            string apiMethod = "GetQuotationDetailsForMobileApp";
            string completeURL = WebAPIURL + apiMethod + '/';
            var response = Execute(completeURL,quotationreq, new QuotationResponse());
            return response.ResponseObject;          
        }

        public ServiceResponse<K> Execute<T,K>(string url,T requestType,K responseType)
        {
            try
            {
                using (_httpClient = new HttpClient())
                {
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(requestType), Encoding.UTF8, "application/json");

                    var response = _httpClient.PostAsJsonAsync(url, requestType).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<ServiceResponse<K>>().Result;
                    }
                    else
                    {
                        return null;
                    }
                    
                }
            }
            catch (Exception ex)
            { }
            return null;
        }
        
    }
}