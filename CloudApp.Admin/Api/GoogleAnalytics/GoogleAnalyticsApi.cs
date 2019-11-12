using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.ViewModel;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace CloudApp.Admin.Api.GoogleAnalytics
{
    public class GoogleAnalyticsApi
    {
        private AnalyticsService service;
        private string profileId;
        private string organizationName;
        public GoogleAnalyticsApi(string mappath, int OrganizationId)
        {
            service = GetService(mappath);
            DbDataContext db = new DbDataContext();
            var org = db.Organizations.FirstOrDefault(s => s.OrganizationId == OrganizationId && s.ActiveStatus == EActiveStatus.Active);
            if (org != null)
            {
                profileId = !String.IsNullOrEmpty(org.GoogleAnalyticsProfileId) ? org.GoogleAnalyticsProfileId : "";
                organizationName = !String.IsNullOrEmpty(org.Name) ? org.Name : "";
            }

        }

        public AnalyticsService GetService(string serverMapPath)
        {
            AnalyticsService Service;
            var certificate = new X509Certificate2(serverMapPath + "Api/GoogleAnalytics/ApiKey/ERDEN Yazilim-6d82de2b6b18.p12", "notasecret", X509KeyStorageFlags.Exportable);

            var credentials = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer("anaytics-api@erden-yazilim.iam.gserviceaccount.com")
               {
                   Scopes = new[] { AnalyticsService.Scope.AnalyticsReadonly }
               }.FromCertificate(certificate));

            Service = new AnalyticsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = organizationName
            });
            return Service;
        }
        public async Task<string> RealTimeVisitors()
        {
            if (!String.IsNullOrEmpty(profileId))
            {
                var request = service.Data.Realtime.Get(String.Format("ga:{0}", profileId), "rt:activeUsers");
                var qq = await request.ExecuteAsync();
                return qq.TotalsForAllResults.FirstOrDefault().Value;

            }
            else
            {
                return "";
            }
        }
        public async Task<List<GoogleVisitorsAndPageView>> GetMonthVisitorsAndPageviews()
        {
            if (!String.IsNullOrEmpty(profileId))
            {
                DataResource.GaResource.GetRequest request = service.Data.Ga.Get(String.Format("ga:{0}", profileId), "30daysAgo", "today", "ga:sessions,ga:pageviews,ga:users");
                request.Dimensions = "ga:date";
                var qq = await request.ExecuteAsync();
                List<GoogleVisitorsAndPageView> visitors = new List<GoogleVisitorsAndPageView>();
                foreach (var item in qq.Rows)
                {
                    GoogleVisitorsAndPageView vv = new GoogleVisitorsAndPageView();
                    string time = item[0].Substring(0, 4) + "/" + item[0].Substring(4, 2) + "/" + item[0].Substring(6, 2);
                    vv.Time = time;
                    vv.Session = item[1];
                    vv.PageViews = item[2];
                    vv.Users = item[3];
                    visitors.Add(vv);
                }
                return visitors;
                //return qq.TotalResults.ToString();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<GoogleSource>> GetSource()
        {
            if (!String.IsNullOrEmpty(profileId))
            {
                DataResource.GaResource.GetRequest request = service.Data.Ga.Get(String.Format("ga:{0}", profileId), "30daysAgo", "today", "ga:sessions,ga:users");
                request.Dimensions = "ga:medium";
                var qq = await request.ExecuteAsync();
                List<GoogleSource> visitors = new List<GoogleSource>();
                foreach (var item in qq.Rows)
                {
                    GoogleSource vv = new GoogleSource();
                    vv.Refferal = item[0];
                    vv.SessionCount = item[1];
                    vv.Users = item[2];
                    visitors.Add(vv);
                }
                return visitors;
                //return qq.TotalResults.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}