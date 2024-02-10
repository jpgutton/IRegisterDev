using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;
using System.Text;
using System.Threading;
using Boilerplate.AspNetCore;
using Boilerplate.AspNetCore.Filters;
using Microsoft.Extensions.Options;
using DashBoardDev.Constants;
using DashBoardDev.Services;
using DashBoardDev.Settings;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using DashBoardDev.DAL;
using DashBoardDev.Models;
using DashBoardDev.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Data;

namespace DashBoardDev.Controllers
{
    [Authorize]
    public class NPOController : Controller
    {
        #region Fields

        private readonly IOptions<AppSettings> appSettings;
        private readonly IStringLocalizer<NPOController> _localizer;
        private readonly OrganizationContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly AzureTables _azureTables;
        private readonly AzureQueues _azureQueues;
        private readonly INPODBRepository _npoDBRepository;
        private readonly INPOATRepository _npoATRepository;

        private readonly IMapper _mapper;

        public CloudTable tableUserRights;
        public CloudTable tableNPORights;
        public CloudTable tableDefaultFields;

        private string _tableConnection;

        //public CloudTable table;
        //public CloudTableClient tableNPO;

        #endregion


        #region Constructors

        public NPOController(
            IStringLocalizer<NPOController> localizer,
            IOptions<AppSettings> appSettings,
            OrganizationContext context,
            IMemoryCache memoryCache,
            IOptions<AzureTablesOptions> tableConnection,
            IMapper mapper,
            AzureTables azureTables,
            AzureQueues azureQueues,
            INPODBRepository npoDBRepository,
            INPOATRepository npoATRepository)
        {
            _localizer = localizer;
            this.appSettings = appSettings;
            _context = context;
            _memoryCache = memoryCache;
            _tableConnection = tableConnection.Value.AzTableConnection;
            _mapper = mapper;
            _azureTables = azureTables;
            _azureQueues = azureQueues;
            _npoDBRepository = npoDBRepository;
            _npoATRepository = npoATRepository;

            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            CloudTableClient tables = tableStorageAccount.CreateCloudTableClient();

            tableUserRights = tables.GetTableReference("UserRights");
            tableNPORights = tables.GetTableReference("NPORights");
            tableDefaultFields = tables.GetTableReference("DefaultFields");
        }

        #endregion


        #region create NPO
        [HttpGet]
        //[ResponseCache(Duration = 86400)]
        //[HttpGet("index", Name = DashboardsControllerRoute.GetIndex)]
        public IActionResult Create()
        {
            List<Country> countryList = new List<Country>();

            // Get Data from DB
            countryList = (from country in _context.Countries
                           select country).ToList();

            // Insert data into list
            countryList.Insert(0, new Country { CountryName = null });

            // assign list to viewbag
            ViewBag.ListofCountry = countryList;

            return View();
        }


        /// <summary>
        /// Create a new NPO
        /// </summary>
        /// <param name="_vMnpo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Organization, OrganizationStreet1, OrganizationStreet2, OrganizationCity, OrganizationState, OrganizationZipCode, OrganizationCountry, OrganizationEmail, OrganizationSiteName, OrganizationWebSite, OrganizationFacebook, OrganizationTwitter, SndFirstname, SndLastName, SndAddress, SndAddress2, SndCity, SndState, SndZipCode, SndCountry, SndTelephone, SndEmail")] vMNPO _vMnpo)
        {
            if (ModelState.IsValid)
            {
                // With Automapper
                var model = _mapper.Map<vMNPO, NPO>(_vMnpo);
                model.NPOID = Guid.NewGuid().ToString();
                model.UserID = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
                model.UserName = User.Claims.FirstOrDefault(c => c.Type == "email").Value;
                model.CreationDate = DateTime.Now;
                model.OrganizationSiteName = _vMnpo.OrganizationSiteName.ToLower().RemoveDiacritics();
                model.GroupID = model.NPOID;


                //Call the BackEnd API to create the new NPO in DBs, Add the new sitename in Dbs etc...
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
                var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                HttpResponseMessage result = await client.PostAsync("http://localhost:45101/api/npo/", content);

                if (result.IsSuccessStatusCode)
                {

                }

                // Set the club Id in cookie
                HttpContext.Session.SetString("NPOID1", model.NPOID);
                HttpContext.Session.SetString("NPOID2", model.OrganizationSiteName.ToLower() + model.NPOID.Replace("-", ""));


                return RedirectToAction("Index", "Dashboards");
            }

            return View(_vMnpo);
        }

        #endregion


        #region Default form fields
        /// <summary>
        /// View to create the default fields for the NPO form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DefaultFields(string id, string state)
        {
            var npoDefaultFields = new List<NPOFields>();

            #region No Default fields present
            if (state == null)
            {
                //CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
                //CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
                //CloudTable azTableNPO = tableNPO.GetTableReference("DefaultFields");
                //CloudTable azTableNPO = this.tables.GetTableReference("DefaultFields");

                // Construct the query operation for all customer entities where PartitionKey="DefaultNPOFields".
                TableQuery<NPOAZFields> query = new TableQuery<NPOAZFields>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "DefaultNPOFields"));

                // Print the fields for each customer.
                TableContinuationToken token = null;
                do
                {
                    TableQuerySegment<NPOAZFields> resultSegment = await tableDefaultFields.ExecuteQuerySegmentedAsync(query, token);
                    token = resultSegment.ContinuationToken;

                    foreach (NPOAZFields entity in resultSegment.Results)
                    {
                        NPOFields npoFields = new NPOFields();
                        npoFields.NPOID = id;
                        npoFields.FieldType = entity.FieldType;
                        npoFields.FieldName = entity.RowKey;
                        npoFields.FieldLabel = entity.FieldLabel;
                        npoFields.FieldData = entity.FieldData;
                        npoFields.FieldPopup = entity.FieldPopup;
                        npoFields.FieldMandatory = entity.FieldMandatory;
                        npoFields.FieldActivate = entity.FieldActivate;
                        npoFields.FieldHidden = entity.FieldHiden;
                        npoFields.FieldOrder = entity.FieldOrder;
                        npoDefaultFields.Add(npoFields);
                    }
                } while (token != null);
            }
            #endregion

            #region Default fields present
            else if (state == "default" || state == "Yes")
            {
                string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

                bool gotRights = await _npoATRepository.NPOCheckRights(id, userid);

                if (gotRights == true)
                {
                    var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                    var client = new HttpClient();
                    client.SetBearerToken(accessToken);
                    var content = await client.GetStringAsync("http://localhost:45101/api/npo/defaultfields/" + id);

                    npoDefaultFields = JsonConvert.DeserializeObject<List<NPOFields>>(content);
                }
                ViewBag.Processed = "Yes";
            }
            #endregion

            npoDefaultFields = npoDefaultFields.OrderBy(o => o.FieldOrder).ToList();

            HttpContext.Session.SetString("NPOID1", id);

            return View(npoDefaultFields);
        }


        /// <summary>
        /// Post all default fields for the NPO form
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DefaultFields(List<NPOFields> list)
        {
            if (ModelState.IsValid)
            {
                string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

                string npoid = list[0].NPOID;

                //CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
                //CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
                //CloudTable azTableNPO = tableNPO.GetTableReference("NPORights");


                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(npoid, userid);
                //TableOperation retrieveOperation2 = TableOperation.Retrieve<NPORights>("org1101C53e5fc6cc33c4542986cd239807b3a2b", "ba340250-9b6f-49cd-ba92-b03d6c17f3e3");

                // Execute the retrieve operation.
                TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);
                //TableResult retrievedResult2 = await azTableNPO.ExecuteAsync(retrieveOperation2);

                // Print the phone number of the result.
                if (retrievedResult.Result != null)
                {
                    string blah = JsonConvert.SerializeObject(list);
                    //Call the BackEnd API to create the new NPO in DBs, Add the new sitename in Dbs etc...
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(list), System.Text.Encoding.UTF8, "application/json");
                    //HttpContent content = new StringContent(blah, System.Text.Encoding.UTF8, "application/json");
                    var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                    var client = new HttpClient();
                    client.SetBearerToken(accessToken);
                    HttpResponseMessage result = await client.PostAsync("http://localhost:45101/api/npo/defaultfields/", content);
                }
                return RedirectToAction("Index", "Dashboards");
            }
            return View(list);
        }
        #endregion


        #region Custom form fields

        /// <summary>
        /// Retrive all custom fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CustomFields(string id, string state)
        {
            // Retrive the user ID from the claim
            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            #region check for user's rights
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(id, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);
            #endregion

            if (retrievedResult.Result != null)
            {
                var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                var content = await client.GetStringAsync("http://localhost:45101/api/npo/customfields/" + id);

                vMNPOCustomFields model = new vMNPOCustomFields();
                model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(content);
                model.SelectedFields = null;

                HttpContext.Session.SetString("NPOID1", id);
                ViewBag.NPOID = id;

                if (state != null)
                {
                    ViewBag.Processed = "Yes";
                    HttpContext.Session.SetString("NPOCustomField", "Processed");
                }
                return View(model);
            }
            return RedirectToAction("Index", "Dashboards");
        }


        /// <summary>
        /// retrieve all custom field and show the new custom field form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewCustomFields()
        {
            string id = HttpContext.Session.GetString("NPOID1");

            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            //CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            //CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            //CloudTable azTableNPO = tableNPO.GetTableReference("NPORights");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(id, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");

                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                var content = await client.GetStringAsync("http://localhost:45101/api/npo/customfields/" + id);

                vMNPOCustomFields model = new vMNPOCustomFields();

                model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(content);

                model.SelectedFields = null;
                model.DisplayMode = "WriteOnly";

                return View("CustomFields", model);
            }
            return View();
        }


        /// <summary>
        /// post new custom field
        /// </summary>
        /// <param name="npoCustomField"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertCustomFields(NPOFields npoCustomField)
        {
            string id = HttpContext.Session.GetString("NPOID1");

            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            npoCustomField.FieldOrder = 0;
            npoCustomField.NPOID = id;

            //CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            //CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            //CloudTable azTableNPO = tableNPO.GetTableReference("NPORights");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(id, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                npoCustomField.FieldLabel = npoCustomField.FieldName;

                string fieldName = npoCustomField.FieldName;
                fieldName = fieldName.Replace("\"", "");
                fieldName = fieldName.Replace("\'", "");
                fieldName = fieldName.Replace("-", "");
                fieldName = fieldName.RemoveDiacritics();
                fieldName = fieldName.Replace(" ", "");

                npoCustomField.FieldName = fieldName;

                //string tableToCheck = id + "fieldname";

                //CloudTable azTableFieldsxxx = tables.GetTableReference(tableToCheck);

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation2 = TableOperation.Retrieve<NPOUniqueFieldName>(id, npoCustomField.FieldName);

                // Execute the retrieve operation.
                TableResult retrievedResult2 = await tableNPORights.ExecuteAsync(retrieveOperation2);

                if (retrievedResult2.Result != null)
                {
                    return View("CustomFields", id);
                }

                if (ModelState.IsValid)
                {
                    //Call the BackEnd API to create the new NPO in DBs, Add the new sitename in Dbs etc...
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(npoCustomField), System.Text.Encoding.UTF8, "application/json");
                    var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                    var client = new HttpClient();
                    client.SetBearerToken(accessToken);
                    HttpResponseMessage result = await client.PostAsync("http://localhost:45101/api/npo/customfields/", content);

                    if (result.IsSuccessStatusCode)
                    {
                        var accessTokenRead = await HttpContext.Authentication.GetTokenAsync("access_token");

                        var clientRead = new HttpClient();
                        clientRead.SetBearerToken(accessTokenRead);
                        var contentRead = await client.GetStringAsync("http://localhost:45101/api/npo/customfields/" + id);

                        vMNPOCustomFields model = new vMNPOCustomFields();
                        model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(contentRead);
                        model.SelectedFields = null;
                        model.DisplayMode = "ReadOnly";

                        return View("CustomFields", model);
                    }
                    return View("CustomFields", id);
                }
                return View("CustomFields", id);
            }
            return View("CustomFields", id);

        }






        /// <summary>
        /// Select a custom field and return its values
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SelectCustomField(string id)
        {
            // Retrive the user ID from the claim
            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            string npoID = HttpContext.Session.GetString("NPOID1");

            #region check for user's rights
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(npoID, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);
            #endregion

            if (retrievedResult.Result != null)
            {
                GetNPOCustomFieldByID myNPOCustomfield = new GetNPOCustomFieldByID();
                myNPOCustomfield.NPOID = npoID;
                myNPOCustomfield.FieldName = id;

                string blah = JsonConvert.SerializeObject(myNPOCustomfield);

                var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                var contentCustomField = await client.GetStringAsync("https://localhost:44349/api/npo/customfields/" + npoID + "/" + id);

                var content = await client.GetStringAsync("https://localhost:44349/api/npo/customfields/" + npoID);

                vMNPOCustomFields model = new vMNPOCustomFields();
                model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(content);
                model.SelectedFields = JsonConvert.DeserializeObject<NPOFields>(contentCustomField);
                model.DisplayMode = "ReadOnly";


                if (HttpContext.Session.GetString("NPOCustomField") == "Processed")
                {
                    ViewBag.Processed = "Yes";
                }

                return View("Customfields", model);
            }
            return RedirectToAction("Index", "Dashboards");
        }



        /// <summary>
        /// Select a custom field and return its values for edition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditCustomFields(string id)
        {
            // Retrive the user ID from the claim
            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            string npoID = HttpContext.Session.GetString("NPOID1");

            #region check for user's rights
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(npoID, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);
            #endregion

            if (retrievedResult.Result != null)
            {
                GetNPOCustomFieldByID myNPOCustomfield = new GetNPOCustomFieldByID();
                myNPOCustomfield.NPOID = npoID;
                myNPOCustomfield.FieldName = id;

                string blah = JsonConvert.SerializeObject(myNPOCustomfield);

                var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                var contentCustomField = await client.GetStringAsync("https://localhost:44349/api/npo/customfields/" + npoID + "/" + id);

                var content = await client.GetStringAsync("https://localhost:44349/api/npo/customfields/" + npoID);

                vMNPOCustomFields model = new vMNPOCustomFields();
                model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(content);
                model.SelectedFields = JsonConvert.DeserializeObject<NPOFields>(contentCustomField);
                model.DisplayMode = "ReadWrite";

                return View("Customfields", model);
            }
            return RedirectToAction("Index", "Dashboards");
        }



        /// <summary>
        /// post new custom field
        /// </summary>
        /// <param name="npoCustomField"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCustomFields(NPOFields npoCustomField)
        {
            string npoID = HttpContext.Session.GetString("NPOID1");

            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            npoCustomField.FieldOrder = 0;
            npoCustomField.NPOID = npoID;

            #region check for user's rights
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(npoID, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);
            #endregion

            if (retrievedResult.Result != null)
            {
                if (ModelState.IsValid)
                {
                    //Call the BackEnd API to create the new NPO in DBs, Add the new sitename in Dbs etc...
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(npoCustomField), System.Text.Encoding.UTF8, "application/json");
                    var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                    var client = new HttpClient();
                    client.SetBearerToken(accessToken);
                    HttpResponseMessage result = await client.PutAsync("https://localhost:44349/api/npo/customfields/", content);

                    if (result.IsSuccessStatusCode)
                    {
                        var accessTokenRead = await HttpContext.Authentication.GetTokenAsync("access_token");

                        var clientRead = new HttpClient();
                        clientRead.SetBearerToken(accessTokenRead);
                        var contentRead = await client.GetStringAsync("https://localhost:44349/api/npo/customfields/" + npoID);

                        vMNPOCustomFields model = new vMNPOCustomFields();
                        model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(contentRead);
                        model.SelectedFields = null;
                        model.DisplayMode = "ReadOnly";

                        return View("CustomFields", model);
                    }
                    return View("CustomFields", npoID);
                }
                return View("CustomFields", npoID);
            }
            return View("CustomFields", npoID);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="npoID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteCustomFields(string id)
        {
            string npoID = HttpContext.Session.GetString("NPOID1");

            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference("NPORights");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(npoID, userid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await azTableNPO.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                HttpResponseMessage result = await client.DeleteAsync("https://localhost:44349/api/npo/customfields/" + npoID + "/" + id);

                if (result.IsSuccessStatusCode)
                {
                    var accessTokenRead = await HttpContext.Authentication.GetTokenAsync("access_token");

                    var clientRead = new HttpClient();
                    clientRead.SetBearerToken(accessTokenRead);
                    var contentRead = await client.GetStringAsync("https://localhost:44349/api/npo/customfields/" + npoID);

                    vMNPOCustomFields model = new vMNPOCustomFields();
                    model.NPOFields = JsonConvert.DeserializeObject<List<NPOFields>>(contentRead);
                    model.SelectedFields = null;
                    model.DisplayMode = "ReadOnly";

                    return View("CustomFields", model);
                }


                return View("CustomFields", npoID);
            }
            return View("CustomFields", npoID);
        }



        #endregion



    }
}
