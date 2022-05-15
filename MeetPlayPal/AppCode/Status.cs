using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace MeetPlayPal
{
    public class Status : ConnManager
    {
        public int OptionID { get; set; }
        public double StatusId { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        //public double CreatedUser { get; set; }
        //public double ModifiedUser { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }




        public bool SetCommandStatus(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Status_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamStatusId = Cmd.Parameters.Add("@StatusId", SqlDbType.Float);
            SqlParameter ParamName = Cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            SqlParameter ParamTableName = Cmd.Parameters.Add("@TableName", SqlDbType.VarChar);
            //SqlParameter ParamCreatedUser = Cmd.Parameters.Add("@CreatedUser", SqlDbType.Float);
            //SqlParameter ParamModifiedUser = Cmd.Parameters.Add("@ModifiedUser", SqlDbType.Float);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = OptionID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamStatusId.Value = StatusId;
            ParamStatusId.Direction = ParameterDirection.Input;
            ParamName.Value = Name;
            ParamName.Direction = ParameterDirection.Input;
            ParamTableName.Value = TableName;
            ParamTableName.Direction = ParameterDirection.Input;
            //ParamStatus.Value = Status_Code;
            //ParamStatus.Direction = ParameterDirection.Input;
            //ParamCreatedUser.Value = CreatedUser;
            //ParamCreatedUser.Direction = ParameterDirection.Input;
            //ParamModifiedUser.Value = ModifiedUser;
            //ParamModifiedUser.Direction = ParameterDirection.Input;


            if (CreatedDateTime < DateTime.Parse("1-1-2000"))
            {
                ParamCreatedDateTime.Value = DBNull.Value;
            }
            else
            {
                ParamCreatedDateTime.Value = CreatedDateTime;
            }
            ParamCreatedDateTime.Direction = ParameterDirection.Input;


            if (ModifiedDateTime < DateTime.Parse("1-1-2000"))
            {
                ParamModifiedDateTime.Value = DBNull.Value;
            }
            else
            {
                ParamModifiedDateTime.Value = ModifiedDateTime;
            }
            ParamModifiedDateTime.Direction = ParameterDirection.Input;

            CmdSent = Cmd;
            return true;
        }


        public bool CreateStatus(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            SqlCommand CmdExecute = new SqlCommand();
            if (SetCommandStatus(ref CmdExecute))
            {
                try
                {
                    if (TrTransaction != null)
                    {
                        CmdExecute.Transaction = TrTransaction;
                    }
                    SqlDataReader DATReader = CmdExecute.ExecuteReader();
                    DATReader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }


    }

}   
