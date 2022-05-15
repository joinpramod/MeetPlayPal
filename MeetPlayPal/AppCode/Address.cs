using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace MeetPlayPal
{
    public class Address : ConnManager
    {
        public int OptionID { get; set; }
        public double AddrId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public double CityId { get; set; }
        public double CountryId { get; set; }
        public double ZipCodeId { get; set; }
        public string Status_Code { get; set; }
        public double CreatedUser { get; set; }
        public double ModifiedUser { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }




        public bool SetCommandAddress(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Address_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamAddrId = Cmd.Parameters.Add("@AddrId", SqlDbType.Float);
            SqlParameter ParamLine1 = Cmd.Parameters.Add("@Line1", SqlDbType.VarChar);
            SqlParameter ParamLine2 = Cmd.Parameters.Add("@Line2", SqlDbType.VarChar);
            SqlParameter ParamLine3 = Cmd.Parameters.Add("@Line3", SqlDbType.VarChar);
            SqlParameter ParamCityId = Cmd.Parameters.Add("@CityId", SqlDbType.Float);
            SqlParameter ParamCountryId = Cmd.Parameters.Add("@CountryId", SqlDbType.Float);
            SqlParameter ParamZipCodeId = Cmd.Parameters.Add("@ZipCodeId", SqlDbType.Float);
            SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
            SqlParameter ParamCreatedUser = Cmd.Parameters.Add("@CreatedUser", SqlDbType.Float);
            SqlParameter ParamModifiedUser = Cmd.Parameters.Add("@ModifiedUser", SqlDbType.Float);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = OptionID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamAddrId.Value = AddrId;
            ParamAddrId.Direction = ParameterDirection.Input;
            ParamLine1.Value = Line1;
            ParamLine1.Direction = ParameterDirection.Input;
            ParamLine2.Value = Line2;
            ParamLine2.Direction = ParameterDirection.Input;
            ParamLine3.Value = Line3;
            ParamLine3.Direction = ParameterDirection.Input;

            ParamCityId.Value = CityId;
            ParamCityId.Direction = ParameterDirection.Input;
            ParamCountryId.Value = CountryId;
            ParamCountryId.Direction = ParameterDirection.Input;
            ParamZipCodeId.Value = ZipCodeId;
            ParamZipCodeId.Direction = ParameterDirection.Input;

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


        public bool CreateAddress(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            SqlCommand CmdExecute = new SqlCommand();
            if (SetCommandAddress(ref CmdExecute))
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
