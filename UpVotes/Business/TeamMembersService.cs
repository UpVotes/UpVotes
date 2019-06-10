using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Business
{
    public class TeamMembersService
    {
        private HttpClient _httpClient = null;

        private static string WebApiUrl { get; } = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();


        public List<TeamMemebersEntity> GetAllTeamMembers(string companyOrsoftwareName, bool isService)
        {
            using (_httpClient = new HttpClient())
            {
                string apiMethod = isService ? "GetTeamMembersByCompany" : "GetTeamMembersBySoftware";
                string completeUrl = WebApiUrl + apiMethod + '/' + companyOrsoftwareName;

                string response = _httpClient.GetStringAsync(completeUrl).Result;
                return JsonConvert.DeserializeObject<List<TeamMemebersEntity>>(response);
            }
        }

        public int SaveTeamMembers(TeamMemebersEntity teamMembersInfo)
        {
            using (StringContent httpContent = new StringContent(JsonConvert.SerializeObject(teamMembersInfo), Encoding.UTF8, "application/json"))
            {
                using (_httpClient = new HttpClient())
                {
                    string apiMethod = teamMembersInfo.IsService ? "SaveCompanyTeamMembers" : "SaveSoftwareTeamMembers";
                    string completeUrl = WebApiUrl + apiMethod + '/';

                    using (HttpResponseMessage response = _httpClient.PostAsync(completeUrl, httpContent).Result)
                    {
                        return response.IsSuccessStatusCode ? Convert.ToInt32(response.Content.ReadAsStringAsync().Result) : 0;
                    }
                }
            }
        }

        public bool DeleteTeamMember(int teamMemberId, bool isService)
        {
            using (_httpClient = new HttpClient())
            {
                string apiMethod = isService ? "DeleteCompanyTeamMember" : "DeleteSoftwareTeamMember";
                string completeUrl = WebApiUrl + apiMethod + '/' + teamMemberId;
                using (var response = _httpClient.DeleteAsync(completeUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public TeamMemebersEntity GetTeamMember(int memberId, bool isService)
        {
            using (_httpClient = new HttpClient())
            {
                string apiMethod = isService ? "GetCompanyTeamMember" : "GetSoftwareTeamMember";
                string completeUrl = WebApiUrl + apiMethod + '/' + memberId;

                string response = _httpClient.GetStringAsync(completeUrl).Result;
                return JsonConvert.DeserializeObject<TeamMemebersEntity>(response);
            }
        }
    }
}