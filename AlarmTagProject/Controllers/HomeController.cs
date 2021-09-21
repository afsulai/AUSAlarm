using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using AlarmTagProject.Models;
using Microsoft.Ajax.Utilities;
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Packaging;
using System.Globalization;

namespace AlarmTagProject.Controllers
{
    public class HomeController : Controller
    {
        //Loads on page load of Book out.
        public ActionResult Index()
        {
            try
            {
                
               //Calls respective stored procedures mentioned below.
                ViewBag.ExceptionMsg = "";
                List<sp_GetSites_Result> SitesList = new List<sp_GetSites_Result>();
                List<sp_GetAllStatusList_Result> StatusList = new List<sp_GetAllStatusList_Result>();
                List<masterAlarmDB_sp_GetAllTagStatus_Result> TagStatusList = new List<masterAlarmDB_sp_GetAllTagStatus_Result>();
                using (MASTERALARMEntities myContext = new MASTERALARMEntities())
                {
                    SitesList = myContext.sp_GetSites().ToList();
                    StatusList = myContext.sp_GetAllStatusList().ToList();
                    TagStatusList = myContext.masterAlarmDB_sp_GetAllTagStatus().ToList();
                }
                ViewData["SitesList"] = SitesList;
                ViewData["StatusList"] = StatusList;
                ViewData["TagStatusList"] = TagStatusList;
                LogEvent("Book Out Page Opened");
            } catch (Exception ex) {
                LogException(ex);
                ViewBag.ExceptionMsg = "Unable to process your request. Please try again.";
            }

            //returns data to view.
            return View();
        }

        //Gets and returns data based on the search.
        public ActionResult AlarmTableData(int PageNum, string Site, string Status, string TagStatus, string SearchTerm, int CriteriaVal)
        {
            List<sp_AlarmSearch_Result> Model = new List<sp_AlarmSearch_Result>();

            //Gets page size from web.config file which is 100 currently.
            int bookoutGridPageSize = int.Parse(ConfigurationManager.AppSettings["BookoutGridPageSize"]);

            try {
                using (MASTERALARMEntities myContext = new MASTERALARMEntities())
                {
                    System.Data.Entity.Core.Objects.ObjectParameter TotalPages = new System.Data.Entity.Core.Objects.ObjectParameter("TotalPages", typeof(int));
                    System.Data.Entity.Core.Objects.ObjectParameter TotRows = new System.Data.Entity.Core.Objects.ObjectParameter("TotRows", typeof(int));
                    Model = myContext.sp_AlarmSearch(SearchTerm, Site, Status, TagStatus, bookoutGridPageSize, PageNum, CriteriaVal, TotalPages, TotRows, 1).OrderBy(m => m.Associated_Tagname).ToList();
                    int PageRange = int.Parse(ConfigurationManager.AppSettings["PageRange"]);
                    string strTotalRecords = TotRows.Value.ToString();
                    string strTotalPages = TotalPages.Value.ToString();


                    if (strTotalRecords != "" && strTotalPages != "")
                    {
                        ViewBag.PageIndex = PageNum;
                        ViewBag.PageRange = PageRange;

                        ViewBag.TotalRecords = int.Parse(strTotalRecords);
                        ViewBag.TotalPagesCount = int.Parse(strTotalPages);
                        GetFirstLastpages(PageNum);
                    }
                    else
                    {
                        ViewBag.TotalRecords = 0;
                        ViewBag.TotalPagesCount = 0;
                    }


                }
                LogEvent("Searched: Page Num - " + PageNum.ToString() + ", Site - " + Site + ", Status - " + Status + ", Tag Status - " + TagStatus + ", Searchterm - " + SearchTerm);
            }
            catch (Exception ex) {
                //Need to log the exception in DB
                LogException(ex);
            }

            //returns data to view.
            return PartialView("AlarmTableData", Model);
        }
        private void GetFirstLastpages(int PageNum)
        {
            int PageRange = int.Parse(ConfigurationManager.AppSettings["PageRange"]);
            if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "1")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum;
                    ViewBag.lastpage = PageNum + 9;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum;
                    ViewBag.lastpage = PageNum + 4;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "2")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 1;
                    ViewBag.lastpage = PageNum + 8;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 1;
                    ViewBag.lastpage = PageNum + 3;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "3")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 2;
                    ViewBag.lastpage = PageNum + 7;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 2;
                    ViewBag.lastpage = PageNum + 2;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "4")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 3;
                    ViewBag.lastpage = PageNum + 6;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 3;
                    ViewBag.lastpage = PageNum + 1;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "5")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 4;
                    ViewBag.lastpage = PageNum + 5;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 4;
                    ViewBag.lastpage = PageNum;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "6")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 5;
                    ViewBag.lastpage = PageNum + 4;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum;
                    ViewBag.lastpage = PageNum + 4;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "7")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 6;
                    ViewBag.lastpage = PageNum + 3;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 1;
                    ViewBag.lastpage = PageNum + 3;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "8")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 7;
                    ViewBag.lastpage = PageNum + 2;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 2;
                    ViewBag.lastpage = PageNum + 2;
                }
            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "9")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 8;
                    ViewBag.lastpage = PageNum + 1;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 3;
                    ViewBag.lastpage = PageNum + 1;
                }

            }
            else if (PageNum.ToString().Substring(PageNum.ToString().Length - 1) == "0")
            {
                if (PageRange == 10)
                {
                    ViewBag.FisrtPage = PageNum - 9;
                    ViewBag.lastpage = PageNum;
                }
                else
                {
                    ViewBag.FisrtPage = PageNum - 4;
                    ViewBag.lastpage = PageNum;
                }
            }
        }

        //Gets alarm tag details and display in Admin.
        public ActionResult AlarmDetailsData(string Tagname, string AlarmType, string Site)
        {
            
            sp_AlarmTagDetails_Result  Model = new sp_AlarmTagDetails_Result();
            try
            {
                using (MASTERALARMEntities myContext = new MASTERALARMEntities())
                {
                    Model = myContext.sp_AlarmTagDetails(Tagname, AlarmType, Site).FirstOrDefault();
                    
                    GetMasterDataFromDB();
                    
                    LogEvent("Alarm Viewed: Tage Name - " + Tagname + ", Alram Type - " + AlarmType + ", Site - " + Site);
                }
            }
            catch (Exception ex)
            {
                //Need to log the exception in DB
                LogException(ex);
            }

            //return edit page for Admin, and view page for viewer.
            if (User.IsInRole("NA\\MASTERALARM-ADMIN.UG"))
            {
                return PartialView("_AlarmDetailsData", Model);
            }
            else if (User.IsInRole("NA\\MASTERALARM-VIEWER.UG"))
            {
                return PartialView("_AlarmDetailsDataView", Model);
            }

            return PartialView("_AlarmDetailsDataView", Model);

        }

        public void GetMasterDataFromDB()
            {

            List<tblAlarmFieldsData> SystemList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> SourceList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> ProcessStatusList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> HwColorList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> HwIasList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> IasBoxList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> IasSlotList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> IasChannelList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> TagStatusList = new List<tblAlarmFieldsData>();
            List<tblAlarmFieldsData> AlarmProrityList = new List<tblAlarmFieldsData>();

            using (MASTERALARMEntities myContext = new MASTERALARMEntities())
            {
                SystemList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 3 && x.Retain == 1).ToList();
                ViewData["SystemList"] = SystemList;

                SourceList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 4 && x.Retain == 1).ToList();
                ViewData["SourceList"] = SourceList;

                ProcessStatusList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 5 && x.Retain == 1).ToList();
                ViewData["ProcessStatusList"] = ProcessStatusList;

                AlarmProrityList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 6 && x.Retain == 1).ToList();
                ViewData["AlarmProrityList"] = AlarmProrityList;

                HwColorList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 7 && x.Retain == 1).ToList();
                ViewData["HwColorList"] = HwColorList;

                HwIasList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 8 && x.Retain == 1).ToList();
                ViewData["HwIasList"] = HwIasList;

                IasBoxList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 9 && x.Retain == 1).ToList();
                ViewData["IasBoxList"] = IasBoxList;

                IasSlotList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 10 && x.Retain == 1).ToList();
                ViewData["IasSlotList"] = IasSlotList;

                IasChannelList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 11 && x.Retain == 1).ToList();
                ViewData["IasChannelList"] = IasChannelList;

                TagStatusList = myContext.tblAlarmFieldsDatas.Where(x => x.FieldID == 12 && x.Retain == 1).ToList();
                ViewData["TagStatusList"] = TagStatusList;
            }

        }

        
        //Called on click of Book out.
        public JsonResult BookOutprocess(string ProjectFCANumber, string Requestor, List<int> AlarmTagIDs)
        {
            string msgError = "Error: Please check your permissions with the Admin. You do not have enough rights to Book out alarm tags.";
            
            if (User.IsInRole("NA\\MASTERALARM-VIEWER.UG"))
            {
                
                return Json(msgError, JsonRequestBehavior.AllowGet);
            }
            else if (User.IsInRole("NA\\MASTERALARM-ADMIN.UG"))
            {



                List<tblAlarmsMaster> ListTblMaster = new List<tblAlarmsMaster>();
                string projectFCANumber = ProjectFCANumber;
                string requestor = Requestor;
                bool Upadted = true;
                string Message = "Selected Alarm Tags are Successfully Booked Out";
                using (var context = new MASTERALARMEntities())
                {
                    using (System.Data.Entity.DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //Save Book out data to DB, and send it to excel file.
                            for (var item = 0; item < AlarmTagIDs.Count; item++)
                            {
                                int alarmID = AlarmTagIDs[item];
                                var alarmTag = context.tblAlarmsMasters.Where(p => p.AlarmTagID.Equals(alarmID)).FirstOrDefault();
                                alarmTag.Status = "Booked Out";
                                alarmTag.ChangeDate = DateTime.Now;
                                alarmTag.Status_Revision_Date = DateTime.Now;
                                alarmTag.Project_or_FCA_Revision_Date = DateTime.Now;
                                alarmTag.Project_or_FCA_Number = projectFCANumber;
                                alarmTag.Custodian = requestor;
                                alarmTag.UserModified = User.Identity.Name;
                                context.Entry(alarmTag).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                ListTblMaster.Add(alarmTag);
                            }
                            try
                            {
                                List<sp_GetMasterFieldsData_Result> masterlist = context.sp_GetMasterFieldsData().ToList();

                                //Send data to excel file.
                                string DestinationFile = "EngineerDataProcess_" + projectFCANumber + DateTime.Now.ToString("_yyyy-MM-dd_hhmm") + ".xlsm";
                                string BookoutExcelPath = ConfigurationManager.AppSettings["BookoutExcelPath"].ToString();

                                if (BookoutExcelPath == "")
                                    GenrateExcelSheet(Server.MapPath("~/") + "AlarmTags.xlsx", "Exportdata", ListTblMaster, masterlist, Server.MapPath("~/Excel/") + DestinationFile);
                                else
                                    GenrateExcelSheet(Server.MapPath("~/") + "AlarmTags.xlsx", "Exportdata", ListTblMaster, masterlist, BookoutExcelPath + DestinationFile);
                                dbContextTransaction.Commit();
                                Message = "../Excel/" + DestinationFile;
                                LogEvent("Book Out Excel generated: " + DestinationFile);
                            }
                            catch (Exception ex)
                            {
                                Message = "Error: " + ex.Message;

                                //Log if excepton occured.
                                LogException(ex);
                            }


                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            Upadted = false;
                            Message = "Error:  Data update failed. Please try again.";

                            //Log if excepton occured.
                            LogException(ex);
                        }
                    }


                }

                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            return Json(msgError, JsonRequestBehavior.AllowGet);
        }
        public void GenrateExcelSheet(string excelFilePath,
      string sheetName, List<tblAlarmsMaster> BookOutList, List<sp_GetMasterFieldsData_Result> masterlist, string DestinationFile)
        {

            ExcelPackage package = new ExcelPackage(new FileInfo(Server.MapPath("~/Excel/") + "EngineerOfflineDataProcess.xlsm"));
            
            OfficeOpenXml.ExcelWorksheet ws = package.Workbook.Worksheets.Add("Book Out Data");
            OfficeOpenXml.ExcelWorksheet wsMaster = package.Workbook.Worksheets.Add("SiteSheet");
            wsMaster.Hidden = OfficeOpenXml.eWorkSheetHidden.Hidden;

            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1.
            DataTable dtBookOutList = BookOutList.AsDataTable();

            //Set columns order.
            dtBookOutList = dtBookOutList.SetColumnsOrderBookout();

            //Get master data.
            DataTable DTMasterData = masterlist.AsDataTable();            


            ws.Cells["A1"].LoadFromDataTable(dtBookOutList, true); 
            wsMaster.Cells["A1"].LoadFromDataTable(DTMasterData, true);
            ws.Cells.AutoFitColumns();
           
            ws.Column(20).Style.Numberformat.Format = "dd/MM/yyyy";
            ws.Column(23).Style.Numberformat.Format = "dd/MM/yyyy";
            ws.Column(35).Style.Numberformat.Format = "dd/MM/yyyy";
            ws.Column(36).Style.Numberformat.Format = "dd/MM/yyyy";


            //Write it back to the web folder-> Excel on the server.
            package.SaveAs(new System.IO.FileInfo(DestinationFile));

        }
        private void LogException(Exception ex)
        {
            try
            {
                tbl_AlarmMasterExceptionLogging tblExLog = new tbl_AlarmMasterExceptionLogging();
                tblExLog.ExceptionMsg = ex.Message.ToString();
                tblExLog.ExceptionType = ex.GetType().Name.ToString();
                tblExLog.ExceptionSource = ex.StackTrace.ToString();
                tblExLog.ExceptionURL = System.Web.HttpContext.Current.Request.Url.ToString();
                tblExLog.Logdate = DateTime.Now;
                using (var dbCtx = new MASTERALARMEntities())
                {
                    //Add Exception object into tbl_AlarmMasterExceptionLogging DBset
                    dbCtx.tbl_AlarmMasterExceptionLogging.Add(tblExLog);

                    // call SaveChanges method to save Exception into database
                    dbCtx.SaveChanges();
                }
            }
            catch(Exception exlog) { }

        }
        private void LogEvent(string Action)
        {
            try
            {
                tbl_AlarmMasterEventAuditLog tblEventLog = new tbl_AlarmMasterEventAuditLog();
                tblEventLog.Action = Action;
                tblEventLog.UserName = User.Identity.Name;
                tblEventLog.Eventdate = DateTime.Now;
                using (var dbCtx = new MASTERALARMEntities())
                {
                    //Add Event object into tbl_AlarmMasterEventAuditLog DBset
                    dbCtx.tbl_AlarmMasterEventAuditLog.Add(tblEventLog);

                    // call SaveChanges method to save Event into database
                    dbCtx.SaveChanges();
                }
            }
            catch (Exception exlog) { LogException(exlog); }

        }
        public void EntityToExcelSheet(string excelFilePath,
       string sheetName, List<tblAlarmsMaster> BookOutList, string DestinationFile)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel.Workbook oWB;
            Microsoft.Office.Interop.Excel.Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Sheets oSheets;
            object misValue = System.Reflection.Missing.Value;
            try
            {
                // Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();

                // Set some properties
                oXL.Visible = false;
                oXL.DisplayAlerts = false;

                // Get a new workbook. 
                
                oWB = oXL.Workbooks.Open(Server.MapPath("~/Excel/") + "EngineerOfflineDataProcess.xlsm", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                // Get the active sheet 
               
                oSheets = oWB.Worksheets;
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(sheetName);
                oSheet.Cells.Clear();
              
                // Process the DataTable
                // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE 
                DataTable dt = BookOutList.AsDataTable();

               

                //Get the header
                for (int i = 0; i < dt.Columns.Count; i++)
                    oSheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;

                
                //Create 2D Array. This increases the performance, instead of binding each row to  excel.
                string[,] Values = new string[dt.Rows.Count, dt.Columns.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Values[i, j] = dt.Rows[i][j].ToString();
                    }
                }

                //Fill Excel with an array of values
                String MaxRow = (dt.Rows.Count + 1).ToString();
                String MaxColumn = ((String)(Convert.ToChar(dt.Columns.Count / 26 + 64).ToString() + Convert.ToChar(dt.Columns.Count % 26 + 64))).Replace('@', ' ').Trim();
                String MaxCell = MaxColumn + MaxRow;


                oSheet.get_Range("A2", MaxCell).Value2 = Values;
                oSheet.get_Range("A2", MaxCell).Columns.AutoFit();
               
                 oWB.Close(true, misValue, misValue);
                oXL.Quit();
                oWB = null;
                oSheet = null;
               
                System.IO.File.Copy(Server.MapPath("~/Excel/") + "EngineerOfflineDataProcess.xlsm", DestinationFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Gets called on the click of Export report.
        public JsonResult ExportReport(string Site, string Status, string TagStatus, string SearchTerm, int CriteriaVal)
        {
            List<masterAlarmDB_sp_ExportReport_Result> Model = new List<masterAlarmDB_sp_ExportReport_Result>();


            using (MASTERALARMEntities myContext = new MASTERALARMEntities())
            {
                Model = myContext.masterAlarmDB_sp_ExportReport(SearchTerm, Site, Status, TagStatus,CriteriaVal).OrderBy(m => m.Associated_Tagname).ToList();

                LogEvent("Export report clicked.");
            }
            string fileName = "MasterAlarmReport_" + DateTime.Now.ToString("yyyy-MM-dd_hhmm") + ".xlsx";
            try
            {

                DataTable dt = Model.AsDataTable();
                //Set columns order as in Book out.It sets For Export report.
                dt = dt.SetColumnsOrderExport();
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("MasterALarmReport");
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    ws.Column(8).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(24).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(35).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(36).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Cells.AutoFitColumns();

                    pck.SaveAs(new System.IO.FileInfo(Server.MapPath("~/Excel/") + fileName));

                }
            }
            catch (Exception ex)
            {
                //Need to log the exception in DB
            }

            string strFileame = "../Excel/" + fileName;
            return Json(strFileame, JsonRequestBehavior.AllowGet);


        }

        //Gets called on click of Book in.
        public ActionResult Bookin()
        {
            if (User.IsInRole("NA\\MASTERALARM-VIEWER.UG"))
            {

                return View("Error");
            }
            else if (User.IsInRole("NA\\MASTERALARM-ADMIN.UG"))
                {
                //reset any old values for this logged in user in tblBookinImport to updateflag = old.
                using (MASTERALARMEntities context = new MASTERALARMEntities())
                {
                    int reset = context.masterAlarmDB_sp_BookinReset(User.Identity.Name);
                    LogEvent("Book In Page Opened");
                }

                return View();
            }

            return View("Error");
            
        }

        private DateTime GetFormatedDate(string currentformatdate)
        {
            string[] dateparts = currentformatdate.Split('/');
            DateTime expextedDate = new DateTime(Convert.ToInt32(dateparts[2]), Convert.ToInt32(dateparts[0]), Convert.ToInt32(dateparts[1]));
            return expextedDate;
        }


        //Gets called on drag and drop of the file.
        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {

            DataTable dt = new DataTable();

            List<masterAlarmDB_sp_BookinShowData_Result> list = new List<masterAlarmDB_sp_BookinShowData_Result>();
            foreach (var file in files)
            {
                string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), filePath);
                file.SaveAs(filePath);
                //Get data from Excel file and upload in database  table tblAlarmsMasterBookinImport
               
                bool isDateFormatInCorrect = false;
                try
                {
                    
                    bool hasHeader = true;
                    
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(filePath))
                        {
                            pck.Load(stream);
                        }
                        var ws = pck.Workbook.Worksheets["Book Out Data"];
                        
                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        {
                            dt.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? 2 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow row = dt.Rows.Add();
                            foreach (var cell in wsRow)
                            {

                                //Fix for Epplus error on conversion on date formats.Column numbers of excel file are taken below.
                                if (cell.Start.Column.Equals(20) || cell.Start.Column.Equals(23) || cell.Start.Column.Equals(35) || cell.Start.Column.Equals(36))
                                {
                                    if (cell.Text != "")
                                    {
                                       
                                        DateTime celldate;

                                        if (DateTime.TryParseExact(cell.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out celldate))
                                        {
                                            // Success
                                            row[cell.Start.Column - 1] = DateTime.ParseExact(cell.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else if (DateTime.TryParseExact(cell.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out celldate))
                                        {
                                            row[cell.Start.Column - 1] = DateTime.ParseExact(cell.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else if (DateTime.TryParseExact(cell.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out celldate))
                                        {
                                            // Success
                                            row[cell.Start.Column - 1] = DateTime.ParseExact(cell.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        }
                                        else if (DateTime.TryParseExact(cell.Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out celldate))
                                        {
                                            row[cell.Start.Column - 1] = DateTime.ParseExact(cell.Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        }
                                        else {
                                            isDateFormatInCorrect = true;
                                        }
                                    }

                                    else
                                        row[cell.Start.Column - 1] = null;
                                }
                                else
                                    row[cell.Start.Column - 1] = cell.Text;


                            }
                        }


                        
                    }

                    if (dt.Columns.Count <= 38)
                    {

                        dt.Columns.Add("UpdateFlag", typeof(System.String));

                    }




                    //Check if the excel file dragged has more than one record and make a DB call.
                    if (dt.Rows.Count > 0)
                    {


                        using (var context = new MASTERALARMEntities())
                        {
                            //load data in tblAlarmsMasterBookinImport.
                            SqlParameter parameter = new SqlParameter("@BookinMasterAlarm", SqlDbType.Structured);
                            parameter.TypeName = "dbo.BookinTYPE";
                            parameter.Value = dt;

                            SqlParameter userName = new SqlParameter("@Username", SqlDbType.NVarChar);
                            userName.Value = User.Identity.Name;


                            int get = context.Database.ExecuteSqlCommand("exec dbo.masterAlarmDB_sp_BookinLoadOfflineData @BookinMasterAlarm, @Username", parameter, userName);

                            //Get the data to be bind to grid 
                            int bookoutGridPageSize = int.Parse(ConfigurationManager.AppSettings["BookoutGridPageSize"]);
                            SqlParameter parameter1 = new SqlParameter("@RowsPerPage", SqlDbType.Int);
                            parameter1.Value = bookoutGridPageSize;

                            int i = 1;
                            SqlParameter parameter2 = new SqlParameter("@PageIndex", SqlDbType.Int);
                            parameter2.Value = i;


                            SqlParameter parameter3 = new SqlParameter("@Username", SqlDbType.NVarChar);
                            parameter3.Value = User.Identity.Name;                           


                            SqlParameter parameter4 = new SqlParameter("@TotalPages", SqlDbType.Int);
                            parameter4.Direction = ParameterDirection.Output;

                            SqlParameter parameter5 = new SqlParameter("@TotalRows", SqlDbType.Int);
                            parameter5.Direction = ParameterDirection.Output;

                           

                            SqlParameter parameter6 = new SqlParameter("@DataType", SqlDbType.NVarChar);
                            parameter6.Value = "Page";
                            
                            
                            list = context.Database.SqlQuery<masterAlarmDB_sp_BookinShowData_Result>("exec dbo.masterAlarmDB_sp_BookinShowData  @RowsPerPage,@PageIndex,@Username,@TotalPages out,@TotalRows out,@DataType", parameter1, parameter2, parameter3, parameter4, parameter5, parameter6).OrderBy(m => m.Associated_Tagname).ToList();

                            //Get total records and total pages from the database.
                            int PageRange = int.Parse(ConfigurationManager.AppSettings["PageRange"]);

                            string strTotalRecords = parameter5.Value.ToString();
                            string strTotalPages = parameter4.Value.ToString();
                            
                            string usrName = User.Identity.Name;
                            System.Data.Entity.Core.Objects.ObjectParameter IsTagBookedIn = new System.Data.Entity.Core.Objects.ObjectParameter("IsTagBookedIn", typeof(string));

                            //Check the DB whether the tags in the excel are already Booked in.
                            // IsTagBookedIn is returned to the front end, so on click of Import, show message with OK/Cancel.
                            int returnValue = context.masterAlarmDB_sp_BookinCheckData(usrName, IsTagBookedIn);
                            
                            
                            string strIsTagBookedIn = IsTagBookedIn.Value.ToString();


                            if (strTotalRecords != "" && strTotalPages != "")
                            {
                                ViewBag.PageIndex = 1;
                                ViewBag.PageRange = PageRange;

                                ViewBag.IsBookedIN = strIsTagBookedIn;
                                ViewBag.TotalRecords = int.Parse(strTotalRecords);
                                ViewBag.TotalPagesCount = int.Parse(strTotalPages);
                                GetFirstLastpages(1);
                            }
                            else
                            {
                                ViewBag.TotalRecords = 0;
                                ViewBag.TotalPagesCount = 0;
                            }

                            LogEvent("Book in file is dragged and dropped in the Book in page");
                        }


                    }

                }
                catch (Exception ex)
                {
                   
                    LogException(ex);

                    if(isDateFormatInCorrect)
                        ViewBag.Error = "Please recheck the date fields in the file. All the date fields need to be either in dd-mm-yyyy or dd/mm/yyyy format";
                    else
                        ViewBag.Error = "Error occurred while uploading the file. Please recheck the data in the file. ";
                }
                finally
                {

                    
                }

            }

            //return to Bookin page.
            return PartialView("BookinTags", list);

        }

        //Book in the file on the click of Import.
        [HttpPost]
        public ActionResult BookinFile()
        {
            //Book in file on Import.
            List<masterAlarmDB_sp_BookinAlarmTags_Result> Model = new List<masterAlarmDB_sp_BookinAlarmTags_Result>();
            List<masterAlarmDB_sp_BookinShowData_Result> listPage = new List<masterAlarmDB_sp_BookinShowData_Result>();
            string Message = "Success:  Bookin file imported succesfully.";
            try
            {

                using (var context = new MASTERALARMEntities())
                {
                    SqlParameter parameter = new SqlParameter("@Username", SqlDbType.NVarChar);
                    parameter.Value = User.Identity.Name;

                    
                    //Book in tags.
                    Model = context.Database.SqlQuery<masterAlarmDB_sp_BookinAlarmTags_Result>("exec dbo.masterAlarmDB_sp_BookinAlarmTags @Username", parameter).OrderBy(m => m.Associated_Tagname).ToList();


                    //If model has records , it means that Book in tags have some invalid data.
                    //We check if any Alarm tag /Alarm type/Site is empty.
                    //Export those records to Macro enabled excel sheet.              

                    if (Model != null && Model.Count > 0)
                        {
                            Message = "The Bookin excel file has invalid data.Please check the generated BookInFailureReport for invalid records.No record is Booked in. ";

                            string fileName = "EngineerDataProcess_BookInFailureReport " + DateTime.Now.ToString("yyyy-MM-dd_hhmm") + ".xlsm";
                            ////Load the datatable into the sheet, starting from cell A1. Print the column names on row 1


                            List<sp_GetMasterFieldsData_Result> masterlist = new List<sp_GetMasterFieldsData_Result>();
                           
                                masterlist = context.sp_GetMasterFieldsData().ToList();
                           

                            ExcelPackage package = new ExcelPackage(new FileInfo(Server.MapPath("~/Excel/") + "EngineerOfflineDataProcess.xlsm"));
                            
                            OfficeOpenXml.ExcelWorksheet ws = package.Workbook.Worksheets.Add("Book Out Data");
                            OfficeOpenXml.ExcelWorksheet wsMaster = package.Workbook.Worksheets.Add("SiteSheet");
                            wsMaster.Hidden = OfficeOpenXml.eWorkSheetHidden.Hidden;

                        //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1

                        DataTable dtBookOutList = new DataTable();

                        

                             dtBookOutList = Model.AsDataTable();
                        dtBookOutList = dtBookOutList.SetColumnsOrderBookinFailureReport();

                        //Load master data from DB.
                        DataTable DTMasterData = masterlist.AsDataTable();


                            ws.Cells["A1"].LoadFromDataTable(dtBookOutList, true);
                            wsMaster.Cells["A1"].LoadFromDataTable(DTMasterData, true);

                           //Give format of the date columns of the excel to be dd/MM/yyyy.
                            ws.Column(20).Style.Numberformat.Format = "dd/MM/yyyy";
                            ws.Column(23).Style.Numberformat.Format = "dd/MM/yyyy";
                            ws.Column(35).Style.Numberformat.Format = "dd/MM/yyyy";
                            ws.Column(36).Style.Numberformat.Format = "dd/MM/yyyy";

                        //Write it to the folder on server.
                        package.SaveAs(new System.IO.FileInfo(Server.MapPath("~/Excel/") + fileName));

                            //send file name to client side.
                            Message = "../Excel/" + fileName;

                            return Json(Message, JsonRequestBehavior.AllowGet);

                        }
                        
                    //}
                    else
                    {
                        //Reset the values of UpdateFlag to old of tblAlarmsMasterBookinImport for the logged in user.

                        int reset = context.masterAlarmDB_sp_BookinReset(User.Identity.Name);

                    }
                    //Log exception.
                    LogEvent("Clicked on Import in Book in page.");
                }

               

            }
            catch (Exception ex)
            {
                Message = "Error:  Data update failed. Please try again.";
                //Log exception.
                LogException(ex);
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetErrorBookin(string SelectOption)
        {
            List<masterAlarmDB_sp_BookinShowData_Result> listPage = new List<masterAlarmDB_sp_BookinShowData_Result>();
            try
            {
               //Get the records of file which has incorrect data, bind to the grid, and highlight the data which is incorrect.
                int bookoutGridPageSize = int.Parse(ConfigurationManager.AppSettings["BookoutGridPageSize"]);
                SqlParameter parameter1 = new SqlParameter("@RowsPerPage", SqlDbType.Int);
                parameter1.Value = bookoutGridPageSize;

                int i = 1;
                SqlParameter parameter2 = new SqlParameter("@PageIndex", SqlDbType.Int);
                parameter2.Value = i;


                SqlParameter parameter3 = new SqlParameter("@Username", SqlDbType.NVarChar);
                parameter3.Value = User.Identity.Name;

                SqlParameter parameter4 = new SqlParameter("@TotalPages", SqlDbType.Int);
                parameter4.Direction = ParameterDirection.Output;

                SqlParameter parameter5 = new SqlParameter("@TotalRows", SqlDbType.Int);
                parameter5.Direction = ParameterDirection.Output;

                SqlParameter parameter6 = new SqlParameter("@DataType", SqlDbType.NVarChar);

                if(SelectOption == "CheckStatus")
                    parameter6.Value = "CheckStatus";
                else
                    parameter6.Value = "";

                

                using (var context = new MASTERALARMEntities())
                {

                    listPage = context.Database.SqlQuery<masterAlarmDB_sp_BookinShowData_Result>("exec dbo.masterAlarmDB_sp_BookinShowData  @RowsPerPage,@PageIndex,@Username,@TotalPages out,@TotalRows out,@DataType", parameter1, parameter2, parameter3, parameter4, parameter5, parameter6).OrderBy(m => m.Associated_Tagname).ToList();


                    int reset = context.masterAlarmDB_sp_BookinReset(User.Identity.Name);

                    LogEvent("In Book in page duplicate tags are identified and sent to the Admin.");
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return PartialView("BookinTags", listPage);

        }


        //Gets called on the click of some page in Book in page.
        [HttpPost]
        public ActionResult BookinPaging(int PageNumber)
        {
            List<masterAlarmDB_sp_BookinShowData_Result> listPage = new List<masterAlarmDB_sp_BookinShowData_Result>();

            try
            {
                //Implemented paging for records of Uploaded file, i.e., before Import
                using (var context = new MASTERALARMEntities())
                {


                    int bookoutGridPageSize = int.Parse(ConfigurationManager.AppSettings["BookoutGridPageSize"]);
                    SqlParameter parameter1 = new SqlParameter("@RowsPerPage", SqlDbType.Int);
                    parameter1.Value = bookoutGridPageSize;


                    SqlParameter parameter2 = new SqlParameter("@PageIndex", SqlDbType.Int);
                    parameter2.Value = PageNumber;


                    SqlParameter parameter3 = new SqlParameter("@Username", SqlDbType.NVarChar);
                    parameter3.Value = User.Identity.Name;

                    SqlParameter parameter4 = new SqlParameter("@TotalPages", SqlDbType.Int);
                    parameter4.Direction = ParameterDirection.Output;

                    SqlParameter parameter5 = new SqlParameter("@TotalRows", SqlDbType.Int);
                    parameter5.Direction = ParameterDirection.Output;


                    SqlParameter parameter6 = new SqlParameter("@DataType", SqlDbType.NVarChar);
                    parameter6.Value = "Page";


                    listPage = context.Database.SqlQuery<masterAlarmDB_sp_BookinShowData_Result>("exec dbo.masterAlarmDB_sp_BookinShowData  @RowsPerPage,@PageIndex,@Username,@TotalPages out,@TotalRows out,@DataType", parameter1, parameter2, parameter3, parameter4, parameter5, parameter6).OrderBy(m => m.Associated_Tagname).ToList();

                    //Get total records and total pages from the database.
                    int PageRange = int.Parse(ConfigurationManager.AppSettings["PageRange"]);

                    string strTotalRecords = parameter5.Value.ToString();
                    string strTotalPages = parameter4.Value.ToString();

                    string usrName = User.Identity.Name;
                    System.Data.Entity.Core.Objects.ObjectParameter IsTagBookedIn = new System.Data.Entity.Core.Objects.ObjectParameter("IsTagBookedIn", typeof(string));
                    int returnValue = context.masterAlarmDB_sp_BookinCheckData(usrName, IsTagBookedIn);

                    string strIsTagBookedIn = IsTagBookedIn.Value.ToString();


                    if (strTotalRecords != "" && strTotalPages != "")
                    {
                        ViewBag.PageIndex = PageNumber;
                        ViewBag.PageRange = PageRange;

                        ViewBag.IsBookedIN = strIsTagBookedIn;
                        ViewBag.TotalRecords = int.Parse(strTotalRecords);
                        ViewBag.TotalPagesCount = int.Parse(strTotalPages);
                        GetFirstLastpages(PageNumber);
                    }
                    else
                    {
                        ViewBag.TotalRecords = 0;
                        ViewBag.TotalPagesCount = 0;
                    }

                    LogEvent("Searched: Page Num - " + PageNumber.ToString() + " in Bookin page. ");

                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return PartialView("BookinTags", listPage);
        }


        //Called when user clicks on Update in Admin page.
        [HttpPost]
        public ActionResult AlarmTagDataUpdate(sp_AlarmTagDetails_Result AlarmTagDetails)
        {
            
            try {
                if (ModelState.IsValid)
                {
                    using (var context = new MASTERALARMEntities())
                    {
                        using (System.Data.Entity.DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                
                                var alarmTag = context.tblAlarmsMasters.Where(p => p.AlarmTagID.Equals(AlarmTagDetails.AlarmTagID)).FirstOrDefault();
                                context.Entry(alarmTag).State = System.Data.Entity.EntityState.Modified;
                                //Save the details to DB.
                                alarmTag.System = AlarmTagDetails.System;
                                alarmTag.Source = AlarmTagDetails.Source;
                                alarmTag.Group = AlarmTagDetails.Group;
                                alarmTag.Alarm_Text_for_Operator = AlarmTagDetails.Alarm_Text_for_Operator;
                                alarmTag.AlarmSetting_Units = AlarmTagDetails.AlarmSetting_Units;
                                alarmTag.AlarmSetting_Status = AlarmTagDetails.AlarmSetting_Status;
                                alarmTag.Alarm_Priority= AlarmTagDetails.Alarm_Priority;
                                alarmTag.Design_Document_Reference = AlarmTagDetails.Design_Document_Reference;
                                alarmTag.Level_of_Consequences = AlarmTagDetails.Level_of_Consequences;
                                alarmTag.Urgency_of_Response = AlarmTagDetails.Urgency_of_Response;
                                alarmTag.Defined_Response = AlarmTagDetails.Defined_Response;
                                alarmTag.Design_Basis_Comment = AlarmTagDetails.Design_Basis_Comment;
                                alarmTag.Project_or_FCA_Number = AlarmTagDetails.Project_or_FCA_Number;
                                alarmTag.Project_or_FCA_Revision_Date = AlarmTagDetails.Project_or_FCA_Revision_Date;
                                alarmTag.Custodian = AlarmTagDetails.Custodian;
                                alarmTag.Status = AlarmTagDetails.Status; 
                                alarmTag.ChangeDate = AlarmTagDetails.ChangeDate;
                                alarmTag.Hard_wired_Colour = AlarmTagDetails.Hard_wired_Colour;
                                alarmTag.Hard_wired_Location = AlarmTagDetails.Hard_wired_Location;
                                alarmTag.Affiliated_Tag = AlarmTagDetails.Affiliated_Tag;
                                alarmTag.Hard_wired___IAS_Alarm_Type = AlarmTagDetails.Hard_wired___IAS_Alarm_Type;
                                alarmTag.IAS_Box = AlarmTagDetails.IAS_Box;
                                alarmTag.IAS_Channel = AlarmTagDetails.IAS_Channel;
                                alarmTag.IAS_Slot = AlarmTagDetails.IAS_Slot;
                                alarmTag.Review_team = AlarmTagDetails.Review_team;
                                alarmTag.Review_Date = AlarmTagDetails.Review_Date;
                                alarmTag.Development_Status = AlarmTagDetails.Development_Status;
                                alarmTag.Status_Revision_Date = AlarmTagDetails.Status_Revision_Date;
                                alarmTag.DateModified = DateTime.Now;
                                alarmTag.UserModified = AlarmTagDetails.UserModified;

                                context.SaveChanges();
                                dbContextTransaction.Commit();

                                LogEvent("Alarm tag updated: Tage Name - " + AlarmTagDetails.Associated_Tagname + ", Alram Type - " + AlarmTagDetails.Alarm_Type + ", Site - " + AlarmTagDetails.Site);
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                                //Log if any exception occured.
                                LogException(ex);
                            }
                        }
                    }

                    GetMasterDataFromDB();

                }

            }
            catch (Exception ex)
            {
                //Log if any exception occured.
                LogException(ex);
            }
            
            //return to same view.
            return View("_AlarmDetailsData", AlarmTagDetails);
        }


        //Gets called when user clicks on Permanent Delete in Admin page.
        [HttpPost]
        public ActionResult AlarmTagDataDelete(int AlarmTagID)
        {
            string Message = "Successfully deleted the Alarm tag details.";
            try
            {
               
                    using (var context = new MASTERALARMEntities())
                    {
                        using (System.Data.Entity.DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {                               
                                tblAlarmsMaster AlarmTagDetails = context.tblAlarmsMasters.Find(AlarmTagID);

                                context.tblAlarmsMasters.Remove(AlarmTagDetails);
                                context.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                                Message = "Error:  Data deletion failed. Please try again.";
                                //Log if any exception occured.
                                LogException(ex);
                            }
                        }
                    }

            }
            catch (Exception ex)
            {
                Message = "Error:  Data deletion failed. Please try again.";
                //Log if any exception occured.
                LogException(ex);
            }

            //return to Search page.
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }

}
 
