using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AlarmTagProject.Controllers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extenstion method for converting list to Data Table
        /// T is Generic Type.
        /// list is the input parmeter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable AsDataTable<T>(this IEnumerable<T> list)
            where T : class
        {
            DataTable dtOutput = new DataTable("tblOutput");

            //if the list is empty, return empty data table
            if (list.Count() == 0)
                return dtOutput;

            //get the list of  public properties and add them as columns to the
            //output table           
            PropertyInfo[] properties = list.FirstOrDefault().GetType().
                GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
                dtOutput.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType)?? propertyInfo.PropertyType);
            //(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            //populate rows
            DataRow dr;
            //iterate through all the objects in the list and add them
            //as rows to the table
            foreach (T t in list)
            {
                dr = dtOutput.NewRow();
                //iterate through all the properties of the current object
                //and set their values to data row
                foreach (PropertyInfo propertyInfo in properties)
                {
                   // dr[propertyInfo.Name] = propertyInfo.GetValue(t, null);
                    dr[propertyInfo.Name] = propertyInfo.GetValue(t) ?? DBNull.Value;
                }
                dtOutput.Rows.Add(dr);
            }
            return dtOutput;
        }

        /// <summary>
        /// Extension method to set coulmns order in Datatable.Useful when the order in Model is different from stored procedure select list.
        /// </summary>
        /// <param name="dtable"></param>
        /// <param name="columnNames"></param>
        public static DataTable SetColumnsOrderBookout(this DataTable dtable)
        {
           // DataTable dtable = new DataTable();

             String[] columnNames = { "AlarmTagID","Associated_Tagname", "Alarm_Type", "Site", "System" , "Source", "Group", "Alarm_Text_for_Operator", "Alarm_Setting",
                                        "AlarmSetting_Units","AlarmSetting_Status","Alarm_Priority","DCS_Priority","Design_Document_Reference","Alarm_Comment","Level_of_Consequences",
                                        "Urgency_of_Response","Defined_Response","Design_Basis_Comment","Project_or_FCA_Number","Project_or_FCA_Revision_Date","Custodian",
                                        "Status","ChangeDate","Hard_wired_Colour","Hard_wired_Location","Affiliated_Tag","Hard_wired___IAS_Alarm_Type","IAS_Box","IAS_Slot",
                                        "IAS_Channel","Review","Review_team","Review_Date","Development_Status","Status_Revision_Date","DateModified","UserModified",
                                        "AlarmSuppressionDetail","Status_Old" };
            List<string> listColNames = columnNames.ToList();

            String[] columnNamesToRemove = { "AlarmTagID", "Status_Old" };
            List<string> listColNamesRemove = columnNamesToRemove.ToList();

            //Remove invalid column names.
            foreach (string colNameRemove in listColNamesRemove)
            {
                if (dtable.Columns.Contains(colNameRemove))
                {
                    listColNames.Remove(colNameRemove);
                    dtable.Columns.Remove(colNameRemove);
                }
            }

            foreach (string colName in listColNames)
            {
                dtable.Columns[colName].SetOrdinal(listColNames.IndexOf(colName));
            }

            return dtable;
        }

        /// <summary>
        /// Extension method to set coulmns order in Datatable.Useful when the order in Model is different from stored procedure select list.
        /// </summary>
        /// <param name="dtable"></param>
        /// <param name="columnNames"></param>
        public static DataTable SetColumnsOrderBookinFailureReport(this DataTable dtable)
        {
            // DataTable dtable = new DataTable();

            String[] columnNames = { "Associated_Tagname", "Alarm_Type", "Site", "System" , "Source", "Group", "Alarm_Text_for_Operator", "Alarm_Setting",
                                        "AlarmSetting_Units","AlarmSetting_Status","Alarm_Priority","DCS_Priority","Design_Document_Reference","Alarm_Comment","Level_of_Consequences",
                                        "Urgency_of_Response","Defined_Response","Design_Basis_Comment","Project_or_FCA_Number","Project_or_FCA_Revision_Date","Custodian",
                                        "Status","ChangeDate","Hard_wired_Colour","Hard_wired_Location","Affiliated_Tag","Hard_wired___IAS_Alarm_Type","IAS_Box","IAS_Slot",
                                        "IAS_Channel","Review","Review_team","Review_Date","Development_Status","Status_Revision_Date","DateModified","UserModified",
                                        "AlarmSuppressionDetail","UpdateFlag"};
            List<string> listColNames = columnNames.ToList();

           
            foreach (string colName in listColNames)
            {
                dtable.Columns[colName].SetOrdinal(listColNames.IndexOf(colName));
            }

            return dtable;
        }
        /// <summary>
        /// Extension method to set coulmns order in Datatable.Useful when the order in Model is different from stored procedure select list.
        /// </summary>
        /// <param name="dtable"></param>
        /// <param name="columnNames"></param>
        public static DataTable SetColumnsOrderExport(this DataTable dtable)
        {
            // DataTable dtable = new DataTable();

            String[] columnNames = {"Associated_Tagname", "Alarm_Type", "Site", "System" , "Source", "Group", "Alarm_Text_for_Operator", "Alarm_Setting",
                                        "AlarmSetting_Units","AlarmSetting_Status","Alarm_Priority","DCS_Priority","Design_Document_Reference","Alarm_Comment","Level_of_Consequences",
                                        "Urgency_of_Response","Defined_Response","Design_Basis_Comment","Project_or_FCA_Number","Project_or_FCA_Revision_Date","Custodian",
                                        "Status","ChangeDate","Hard_wired_Colour","Hard_wired_Location","Affiliated_Tag","Hard_wired___IAS_Alarm_Type","IAS_Box","IAS_Slot",
                                        "IAS_Channel","Review","Review_team","Review_Date","Development_Status","Status_Revision_Date","DateModified","UserModified",
                                        "AlarmSuppressionDetail"};
            List<string> listColNames = columnNames.ToList();

           
            foreach (string colName in listColNames)
            {
                dtable.Columns[colName].SetOrdinal(listColNames.IndexOf(colName));
            }

            return dtable;
        }


    }

}