using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace MeetPlayPal
{
    public class Message : ConnManager
    {
        public int OptionID { get; set; }
        public double MessageId { get; set; }
        public string MessageText { get; set; }
        public double MsgFrom { get; set; }
        public double MsgTo { get; set; }
        public double MsgOrder { get; set; }
        public string Status_Code { get; set; }
        public double CreatedUser { get; set; }
        public double ModifiedUser { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }




        public bool SetCommandMessage(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Message_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamMessageId = Cmd.Parameters.Add("@MessageId", SqlDbType.Float);
            SqlParameter ParamMessageText = Cmd.Parameters.Add("@MessageText", SqlDbType.VarChar);
            SqlParameter ParamMsgFrom = Cmd.Parameters.Add("@MsgFrom", SqlDbType.Float);
            SqlParameter ParamMsgTo = Cmd.Parameters.Add("@MsgTo", SqlDbType.Float);
            SqlParameter ParamMsgOrder = Cmd.Parameters.Add("@MsgOrder", SqlDbType.Float);
            SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
            SqlParameter ParamCreatedUser = Cmd.Parameters.Add("@CreatedUser", SqlDbType.Float);
            SqlParameter ParamModifiedUser = Cmd.Parameters.Add("@ModifiedUser", SqlDbType.Float);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = OptionID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamMessageId.Value = MessageId;
            ParamMessageId.Direction = ParameterDirection.Input;
            ParamMessageText.Value = MessageText;
            ParamMessageText.Direction = ParameterDirection.Input;
            ParamMsgFrom.Value = CreatedUser;
            ParamMsgFrom.Direction = ParameterDirection.Input;
            ParamMsgTo.Value = CreatedUser;
            ParamMsgTo.Direction = ParameterDirection.Input;
            ParamMsgOrder.Value = CreatedUser;
            ParamMsgOrder.Direction = ParameterDirection.Input;
            ParamStatus.Value = Status_Code;
            ParamStatus.Direction = ParameterDirection.Input;
            ParamCreatedUser.Value = CreatedUser;
            ParamCreatedUser.Direction = ParameterDirection.Input;
            ParamModifiedUser.Value = ModifiedUser;
            ParamModifiedUser.Direction = ParameterDirection.Input;


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


        public bool CreateMessage(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            SqlCommand CmdExecute = new SqlCommand();
            if (SetCommandMessage(ref CmdExecute))
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
