using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace StarIndiaHoliday.Models
{
    public class DataLayer
    {
        public static string constr
        {
            get
            {
                //return (System.Configuration.ConfigurationManager.AppSettings["ConString"].ToString());
                return (System.Configuration.ConfigurationManager.ConnectionStrings["Youme"].ToString());
            }
        }
        public static int executenonquery(string sp, SqlParameter[] para)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);

                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < para.Length; i++)
                {
                    cmd.Parameters.Add(para[i]);
                }
                if (con.State == ConnectionState.Closed)
                {
                    // SqlConnection.ClearAllPools();
                    con.Open();
                }
                int y = cmd.ExecuteNonQuery();
                return y;
            }
            //catch (Exception ex)
            //{
            //    return -1;
            //}
            catch (SqlException ex)
            {
                int s = ex.ErrorCode;
                if (s == -2146232060)
                {
                    return s;
                }
                else
                {
                    return -1;
                }

            }
        }

        public static int executenonqueryTask(string sp, SqlParameter[] para)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < para.Length; i++)
                {
                    cmd.Parameters.Add(para[i]);
                }
                if (con.State == ConnectionState.Closed)
                {
                    //SqlConnection.ClearAllPools();
                    con.Open();
                }
                int y = cmd.ExecuteNonQuery();
                return y;
            }
            catch
            {
                return -1;
            }
        }
        public static DataTable getdataTable(string stpro, SqlParameter[] para)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stpro, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < para.Length; i++)
            {
                cmd.Parameters.Add(para[i]);
            }
            if (con.State == ConnectionState.Closed)
            {
                //SqlConnection.ClearAllPools();
                con.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public static DataSet getdset(string stpro, SqlParameter[] para)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stpro, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < para.Length; i++)
            {
                cmd.Parameters.Add(para[i]);
            }
            if (con.State == ConnectionState.Closed)
            {
                //SqlConnection.ClearAllPools();
                con.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public static DataSet getdset(string stpro)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stpro, con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public static DataSet getdsetbyquery(string stpro)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stpro, con);
            cmd.CommandType = CommandType.Text;

            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public static int executenonquery(string sp)
        {
            SqlConnection con = new SqlConnection(constr);

            SqlCommand cmd = new SqlCommand(sp, con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            int y = cmd.ExecuteNonQuery();
            con.Close();
            return y;
        }
        public static SqlDataReader getreader(string stdprocedurenm)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stdprocedurenm, con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader dreader = cmd.ExecuteReader();
            con.Close();
            return dreader;

        }

        public static SqlDataReader getreaderByQuery(string stdprocedurenm)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stdprocedurenm, con);
            //  cmd.CommandType = CommandType.StoredProcedure;
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader dreader = cmd.ExecuteReader();
            return dreader;

        }

        public static SqlDataReader getreader(string stdprocedurenm, SqlParameter[] parameters)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stdprocedurenm, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < (parameters.Length); i++)
            {
                cmd.Parameters.Add(parameters[i]);
            }
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader dreader = cmd.ExecuteReader();
            return dreader;

        }
        public static bool IsExist(string strSql)
        {
            SqlConnection con = new SqlConnection(constr);
            // Set Command object properties.
            SqlCommand cmd = new SqlCommand(strSql, con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;
            cmd.CommandTimeout = 30;
            if (con.State == ConnectionState.Closed)
                con.Open();
            // Execute the method
            int row = cmd.ExecuteNonQuery(); // typecasting because ExecuteScalar return object.
            con.Close();

            // Check the result.
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string executescalar(string pro, SqlParameter[] parameter)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(pro, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < (parameter.Length); i++)
            {
                cmd.Parameters.Add(parameter[i]);
            }
            if (con.State == ConnectionState.Closed)
                con.Open();
            string str = "";
            if (cmd.ExecuteScalar() == null)
            {
                con.Close();
                return "";
            }
            else
            {
                str = cmd.ExecuteScalar().ToString();
                con.Close();
                return str;
            }
        }
        public static string executescalar(string query)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;

            if (con.State == ConnectionState.Closed)
                con.Open();
            string str = "";
            if (cmd.ExecuteScalar() == null)
            {
                con.Close();
                return "";
            }
            else
            {
                str = cmd.ExecuteScalar().ToString();
                con.Close();
                return str;
            }
        }

        public static int executescalarint(string query)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;

            if (con.State == ConnectionState.Closed)
                con.Open();
            int str = 0;
            if ((cmd.ExecuteScalar() == null) || (cmd.ExecuteScalar() == ""))
            {
                con.Close();
                return 0;
            }
            else
            {
                str = Convert.ToInt16(cmd.ExecuteScalar());
                con.Close();
                return str;
            }
        }

        public static DataSet getdataset(string stdprocedurenm, SqlParameter[] parameters, Int32 start_record, Int32 max_records, string src_table)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stdprocedurenm, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < (parameters.Length); i++)
            {
                cmd.Parameters.Add(parameters[i]);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataSet dset = new DataSet();
            da.Fill(dset, start_record, max_records, src_table);
            con.Close();
            return dset;

        }

        public static DataSet getdataset(string stdprocedurenm, Int32 start_record, Int32 max_records, string src_table)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stdprocedurenm, con);
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataSet dset = new DataSet();
            da.Fill(dset, start_record, max_records, src_table);
            con.Close();
            return dset;

        }
        //pw deepak786


        public static SqlDataReader getreaderTask(string stdprocedurenm, SqlParameter[] parameters)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(stdprocedurenm, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < (parameters.Length); i++)
            {
                cmd.Parameters.Add(parameters[i]);
            }
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader dreader = cmd.ExecuteReader();
            return dreader;

        }
        ///<summary>
        ///this is to use for Generate Random Password
        ///</summary>
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public void genID(string procname, HiddenField hdn, string format)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procname;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            string a = ds.Tables[0].Rows[0]["Registration_Id"].ToString();
            if (a == "0" || a == null || a == "")
            {

                hdn.Value = format + "1";
            }
            else
            {
                int aa = Convert.ToInt32(a);
                aa++;
                hdn.Value = format + Convert.ToString(aa);
            }
        }

        public static bool sendssemail(string from, string fromName, string to, string toName, string sub, string bodytext)
        {
            SmtpClient client = new SmtpClient("mail.indoresupermarket.com", 25);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            NetworkCredential oCredential = new NetworkCredential("support@indoresupermarket.com", "ADf@#@&jh3");
            client.UseDefaultCredentials = false;
            client.Credentials = oCredential;


            MailAddress sendfrom = new MailAddress(from, fromName);
            MailAddress sendto = new MailAddress(to, toName);
            MailMessage message = new MailMessage(sendfrom, sendto);
            message.IsBodyHtml = true;
            message.Subject = sub;

            message.Bcc.Add("shashank@aksindia.com");

            message.Body = bodytext;

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    public class randomno
    {
        public randomno()
        {
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}