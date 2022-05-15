using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace MeetPlayPal
{
    public class News : ConnManager
    {
        public int OptionID { get; set; }
        public double NewsId { get; set; }
        public double ImageId { get; set; }
        public string NewsDesc { get; set; }
        public string Title { get; set; }
        public double AddrId { get; set; }
        public string ExtURL { get; set; }
        public string Status_Code { get; set; }
        public double CreatedUser { get; set; }
        public double ModifiedUser { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }




        public bool SetCommandNews(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("News_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamNewsId = Cmd.Parameters.Add("@NewsId", SqlDbType.Float);
            SqlParameter ParamImageId = Cmd.Parameters.Add("@ImageId", SqlDbType.Float);
            SqlParameter ParamNewsDesc = Cmd.Parameters.Add("@NewsDesc", SqlDbType.VarChar);
            SqlParameter ParamTitle = Cmd.Parameters.Add("@Title", SqlDbType.VarChar);
            SqlParameter ParamAddrId = Cmd.Parameters.Add("@AddrId", SqlDbType.Float);
            SqlParameter ParamExtURL = Cmd.Parameters.Add("@ExtURL", SqlDbType.VarChar);
            SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
            SqlParameter ParamCreatedUser = Cmd.Parameters.Add("@CreatedUser", SqlDbType.Float);
            SqlParameter ParamModifiedUser = Cmd.Parameters.Add("@ModifiedUser", SqlDbType.Float);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = OptionID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamNewsId.Value = NewsId;
            ParamNewsId.Direction = ParameterDirection.Input;
            ParamImageId.Value = ImageId;
            ParamImageId.Direction = ParameterDirection.Input;
            ParamNewsDesc.Value = NewsDesc;
            ParamNewsDesc.Direction = ParameterDirection.Input;
            ParamTitle.Value = Title;
            ParamTitle.Direction = ParameterDirection.Input;
            ParamAddrId.Value = AddrId;
            ParamAddrId.Direction = ParameterDirection.Input;
            ParamExtURL.Value = ExtURL;
            ParamExtURL.Direction = ParameterDirection.Input;
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


        public bool CreateNews(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            SqlCommand CmdExecute = new SqlCommand();
            if (SetCommandNews(ref CmdExecute))
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
