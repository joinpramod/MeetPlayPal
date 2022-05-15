﻿using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace MeetPlayPal
{
    public class Post : ConnManager
    {
        public int OptionID { get; set; }
        public double PostId { get; set; }
        public double UserId { get; set; }
        public string Message { get; set; }
        public string Status_Code { get; set; }
        public double CreatedUser { get; set; }
        public double ModifiedUser { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }


        public bool SetCommandPost(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Post_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamPostId = Cmd.Parameters.Add("@PostId", SqlDbType.Float);
            SqlParameter ParamUserId = Cmd.Parameters.Add("@UserId", SqlDbType.Float);
            SqlParameter ParamMessage = Cmd.Parameters.Add("@Message", SqlDbType.VarChar);
            SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
            SqlParameter ParamCreatedUser = Cmd.Parameters.Add("@CreatedUser", SqlDbType.Float);
            SqlParameter ParamModifiedUser = Cmd.Parameters.Add("@ModifiedUser", SqlDbType.Float);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = OptionID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamPostId.Value = PostId;
            ParamPostId.Direction = ParameterDirection.Input;
            ParamUserId.Value = UserId;
            ParamUserId.Direction = ParameterDirection.Input;
            ParamMessage.Value = Message;
            ParamMessage.Direction = ParameterDirection.Input;
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


        public bool CreatePost(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            SqlCommand CmdExecute = new SqlCommand();
            if (SetCommandPost(ref CmdExecute))
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
